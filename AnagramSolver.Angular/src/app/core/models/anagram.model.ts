export interface AnagramResult {
  words: string[];
  searchDurationMs?: number;
  count?: number;
}

export interface SearchHistoryItem {
  word: string;
  searchedAt: string;
  resultCount: number;
}
