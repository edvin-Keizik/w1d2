export interface ChatRequest {
  message: string;
  sessionId?: string;
}

export interface ChatResponse {
  response: string;
  sessionId: string;
}

export interface ChatMessage {
  role: string;
  content: string;
  timestamp: number;
}

export interface ChatHistory {
  sessionId: string;
  messages: ChatMessage[];
  messageCount: number;
  createdAt: number;
}

export interface ActiveSessions {
  sessions: string[];
  count: number;
}
