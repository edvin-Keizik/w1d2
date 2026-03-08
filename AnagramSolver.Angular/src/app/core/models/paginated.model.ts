export interface PaginatedResult<T> {
  words: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}
