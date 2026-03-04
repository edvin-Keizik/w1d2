# AnagramSolver Project Instructions

1. **Performance First:** Always prefer `ReadOnlySpan<char>` and `stackalloc` for string manipulations to minimize GC pressure.
2. **Naming Convention:** Use PascalCase for all methods and private fields must start with an underscore (e.g., `_context`).
3. **Async Standard:** All I/O operations (Database, File) must use `async/await` with `CancellationToken` support.
4. **Data Structures:** When suggested, prioritize `HashSet<T>` for lookups and `Dictionary<TKey, TValue>` for grouping over LINQ `.Contains()`.
5. **Language:** Always respond in English, but acknowledge that the domain data (words) may be in Lithuanian.