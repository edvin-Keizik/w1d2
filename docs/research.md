# Research: Backend API Endpoints

**Date:** 2026-03-08  
**Scope:** How the backend API endpoints work — routing, controllers, services, data flow, and patterns.

---

## 1. High-Level Architecture

The backend is an **ASP.NET Core** web application (`AnagramSolver.WebApp`) structured in a layered fashion:

```
HTTP Request
  └─► Middleware Pipeline (CORS, Static Files, Auth, Routing)
        └─► Controllers / GraphQL / Minimal API
              └─► Decorator Chain (Logging → Timing → WordProcessor)
                    └─► Business Logic (AnagramSearchEngine, FrequencyAnalysisService)
                          └─► Data Layer (AnagramDbContext → SQL Server)
```

The solution follows a **Contracts → Business Logic → WebApp** dependency direction. `AnagramSolver.Contracts` defines all interfaces; `AnagramSolver.BusinessLogic` implements them; `AnagramSolver.WebApp` wires everything via DI and exposes endpoints.

---

## 2. Startup & DI Registration (`Program.cs`)

| Registration | Interface | Implementation | Lifetime |
|---|---|---|---|
| Search engine | `IAnagramSearchEngine` | `AnagramSearchEngine` | Singleton |
| Word processing | `IWordProcessor` / `IGetAnagrams` | `WordProcessor` | Scoped |
| Search decorator | `IGetAnagrams` (actual) | `AnagramSearchTimingDecorator` → `AnagramSearchLogDecorator` → `WordProcessor` | Scoped |
| AI chat | `IAiChatService` | `AiChatService` | Scoped |
| Chat history | `IChatHistoryService` | `ChatHistoryService` | Singleton |
| Frequency analysis | `IFrequencyAnalysisService` | `FrequencyAnalysisService` | Scoped |
| Stop words | `IStopWordProvider` | `StopWordProvider` | Singleton |
| DB context | `AnagramDbContext` | EF Core (SQL Server) | Scoped |
| Filter pipeline | `FilterPipeline` | Via `FilterPipelineFactory` | Scoped |
| Semantic Kernel | `Kernel` | OpenAI GPT-4O + Plugins | Scoped |

### Middleware Pipeline Order
1. Static files (`wwwroot/browser/`)  
2. CORS (allows `http://localhost:4200` for Angular dev)  
3. Authorization  
4. Controller routing  
5. GraphQL endpoint (`/graphql`)  
6. Fallback to `index.html` for SPA routing  

### Initialization at Startup
- Stop word dictionaries (Lithuanian + English) are loaded into memory
- Database is migrated (`EnsureCreated` / migrations)
- Dictionary file (`zodynas.txt`) is seeded into the database if no data exists

---

## 3. REST API Endpoints

### 3.1 Anagram Search — `GET /api/Anagrams/{word}`

**Controller:** `AnagramsController` (in `Api/`)  
**Route:** `api/Anagrams`  

| Aspect | Detail |
|---|---|
| **Input** | `{word}` route parameter + `CancellationToken` |
| **Output** | `IEnumerable<string>` (anagram words) |
| **Custom Headers** | `X-Anagram-Count`, `X-Search-Duration-Ms` |
| **Service call** | `IGetAnagrams.GetAnagramsAsync(word, MaxAnagramsToShow, MinWordLength, filter)` |

**Data flow:**
1. Controller receives `word`, starts a `Stopwatch`
2. Reads `MaxAnagramsToShow` and `MinWordLength` from `AnagramSettings`
3. Calls `IGetAnagrams.GetAnagramsAsync()` through the decorator chain
4. `AnagramSearchTimingDecorator` logs elapsed time
5. `AnagramSearchLogDecorator` logs search start/end
6. `WordProcessor.GetAnagramsAsync()`:
   - Checks internal `MemoryCache` (key: `"{input}_{minWordLength}"`)
   - Normalizes input to lowercase, removes spaces
   - Computes a **signature** (sorted unique letters) using `stackalloc` for ≤128 chars
   - Runs candidate signatures through the **FilterPipeline** (Chain of Responsibility)
   - Loads matching `WordGroupsEntity` records from the DB
   - Spawns **parallel tasks** (one per word-count: 1, 2, … up to `maxAnagramsToShow`)
   - Each task uses `AnagramSearchEngine.FindAllCombinations()` (recursive backtracking)
   - Merges results, applies caller-provided filter, caches, and returns
7. Controller adds custom response headers and returns the flat word list

### 3.2 Words — `api/Words`

**Controller:** `WordsController` (in `Api/`)

| Endpoint | Verb | Description |
|---|---|---|
| `GET /api/Words` | GET | Returns all dictionary words |
| `GET /api/Words/paginated?page=1&pageSize=90` | GET | Paginated word list (max 500/page). Returns `{ words, totalCount, page, pageSize }` |
| `GET /api/Words/{id}` | GET | Returns a single word by index (404 if out of bounds) |
| `POST /api/Words` | POST | Adds a new word. Validates uniqueness, creates signature, persists to DB |

The `AddWordAsync` method in `WordProcessor`:
- Checks the word doesn't already exist
- Computes the sorted-letter signature
- Inserts into `Words` table + updates `WordGroupsEntity`
- Updates the in-memory `_signatures` `HashSet`

### 3.3 Settings — `GET /api/Settings`

**Controller:** `SettingsController` (in `Api/`)  
Returns the current `AnagramSettings` values (`minWordLength`, `maxAnagramsToShow`) as JSON.

### 3.4 AI Chat — `api/ai`

**Controller:** `AiChatController` (in `Controllers/`)

| Endpoint | Verb | Description |
|---|---|---|
| `POST /api/ai/chat` | POST | Send a message → get AI response |
| `GET /api/ai/chat/{sessionId}/history` | GET | Retrieve chat history for a session |

**`POST /api/ai/chat` flow:**
1. Accepts `ChatRequest` (`Message`, optional `SessionId`)
2. Validates model state
3. Calls `AiChatService.GetResponseAsync()`
4. `AiChatService`:
   - Loads system prompt from `Prompts/AnagramAgentRole.md`
   - Gets or creates `ChatHistory` for the session (in-memory `ConcurrentDictionary`)
   - Sends to OpenAI GPT-4O via **Semantic Kernel** with `FunctionChoiceBehavior.Auto()` (max 10 tool iterations)
   - The AI can invoke **plugins**: `FindAnagrams`, `GetDictionaryInfo`, `GetCurrentTime`, `GetDayOfWeek`, `GetHoursSince`
5. Returns `ChatResponse` with the AI's message and the `SessionId`

### 3.5 Frequency Analysis — `POST /api/v1/Analysis/frequency`

**Controller:** `AnalysisController` (in `Controllers/`)

| Aspect | Detail |
|---|---|
| **Input** | `FrequencyAnalysisRequestModel` (`Text`, max 100K chars) |
| **Output** | Top 10 words by frequency, total/unique counts, longest word, timestamp |
| **Service** | `FrequencyAnalysisService.AnalyzeAsync()` |

**Processing flow:**
1. Text is tokenized character-by-character (`WordTokenizer`) — supports Lithuanian diacritics
2. Stop words (Lithuanian + English sets) are filtered out
3. Frequencies are counted in a `Dictionary<string, int>`
4. Results are sorted by frequency descending, then alphabetically
5. Top 10 words + statistics are returned

---

## 4. GraphQL Endpoint

**Path:** `/graphql` (via HotChocolate)

| Query | Returns | Service |
|---|---|---|
| `GetAnagrams(word: String!)` | `IEnumerable<string>` | `IGetAnagrams` with settings |
| `GetWords` | `List<string>` | `IWordProcessor.GetDictionary()` |

The GraphQL layer is a thin wrapper that calls the same services as the REST controllers.

---

## 5. Key Design Patterns

### 5.1 Decorator Pattern (Search Pipeline)
```
AnagramSearchTimingDecorator  ← measures Duration
  └─► AnagramSearchLogDecorator  ← logs input/output
        └─► WordProcessor  ← actual anagram logic
```
All implement `IGetAnagrams`. The DI container composes them in order so logging and timing are transparent to callers.

### 5.2 Chain of Responsibility (Filter Pipeline)
The `FilterPipeline` passes each candidate signature through ordered steps:
1. **`WordLengthStep`** — rejects signatures longer than the letter bank or shorter than `minWordLength`
2. **`AllowedCharactersStep`** — rejects signatures containing characters not in the input's character set

Uses a closure-based "next" trampoline: each step calls `next()` to pass responsibility down the chain.

### 5.3 Repository / DbContext
`AnagramDbContext` manages two tables:
- **`Words`** — individual words (`WordEntity`: Id, Value)
- **`WordGroupsEntity`** — anagram groups keyed by signature (sorted letters → comma-separated words)

Grouping by signature is the core optimization: words that are anagrams of each other share the same sorted-letter key.

### 5.4 In-Memory Caching
`MemoryCache<T>` is a simple `Dictionary<string, T>` wrapper used inside `WordProcessor` to avoid re-computing anagrams for the same input within a single request scope.

### 5.5 Semantic Kernel + Plugins
The AI chat feature uses Microsoft Semantic Kernel with auto function-calling. The `AnagramPlugin` and `TimePlugin` expose `[KernelFunction]` methods that the AI can invoke autonomously during a conversation.

---

## 6. Technologies & Libraries

| Technology | Purpose |
|---|---|
| **ASP.NET Core** | Web framework, controllers, routing, middleware |
| **Entity Framework Core** | ORM, SQL Server provider, migrations |
| **HotChocolate** | GraphQL server |
| **Microsoft Semantic Kernel** | AI orchestration, OpenAI GPT-4O integration |
| **SQL Server Express** | Persistent word storage |
| **ConcurrentDictionary** | Thread-safe session storage for chat history |
| **Stopwatch / custom headers** | Performance measurement (X-Search-Duration-Ms) |
| **stackalloc / ReadOnlySpan** | Performance-optimized string operations (per project conventions) |

---

## 7. Configuration

From `appsettings.json`:

```json
{
  "AnagramSettings": {
    "MinWordLength": 3,
    "MaxAnagramsToShow": 3,
    "FilePath": "./Data/zodynas.txt",
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=AnagramSolver_Main;..."
  },
  "OpenAI": {
    "Model": "gpt-4o",
    "ApiKey": ""
  }
}
```

The dictionary file (`zodynas.txt`) contains Lithuanian words, seeded into the database on first run.

---

## 8. Risks & Edge Cases

| Risk | Detail |
|---|---|
| **Large input words** | `FindAllCombinations` is exponential in the worst case (many candidate signatures). Parallel tasks help but no hard timeout exists in the business logic itself. |
| **Memory pressure** | `ChatHistoryService` stores all sessions in-memory with no eviction. Long-running servers accumulate unbounded history. |
| **Missing API key** | If `OpenAI:ApiKey` is empty, the AI chat endpoint will fail at runtime. No startup validation exists. |
| **Concurrency in MemoryCache** | `MemoryCache<T>` uses a plain `Dictionary` — safe only because `WordProcessor` is scoped (one instance per request). |
| **Pagination edge cases** | `GET /api/Words/paginated` caps at 500 per page but has no lower-bound validation on `pageSize`. |
| **Stop word loading** | `StopWordProvider.InitializeAsync()` reads embedded resources — if assemblies are trimmed, these could be missing. |
| **Signature collision** | Words with identical sorted letters are grouped together, which is correct for anagrams but means the DB stores comma-separated word lists (non-normalized). |

---

## 9. Request Lifecycle Summary

```
Client (Angular / curl / GraphQL)
  │
  ▼
ASP.NET Core Middleware (CORS → Auth → Routing)
  │
  ├──► REST Controller (api/Anagrams, api/Words, api/Settings, api/ai, api/v1/Analysis)
  │         │
  │         ▼
  │    Service Layer
  │    ├─ IGetAnagrams (via Decorator chain → WordProcessor)
  │    ├─ IAiChatService → Semantic Kernel → OpenAI + Plugins
  │    └─ IFrequencyAnalysisService → WordTokenizer + StopWordProvider
  │         │
  │         ▼
  │    AnagramDbContext → SQL Server
  │
  └──► GraphQL (/graphql) → same IGetAnagrams / IWordProcessor services
```

Every endpoint ultimately resolves to one of three core capabilities:
1. **Anagram search** — recursive combination finding with signature-based filtering
2. **Word management** — CRUD against the dictionary DB
3. **Text analysis** — tokenization + frequency counting (with stop-word filtering)

The AI chat endpoint is an orchestration layer that can call capabilities 1 & 2 autonomously via Semantic Kernel plugins.
