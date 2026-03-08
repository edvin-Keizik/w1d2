export interface FrequencyAnalysisRequest {
  text: string;
}

export interface WordFrequency {
  word: string;
  frequency: number;
}

export interface FrequencyAnalysisResult {
  topWords: WordFrequency[];
  totalWordCount: number;
  uniqueWordCount: number;
  longestWord: string;
  analyzedAt: string;
}
