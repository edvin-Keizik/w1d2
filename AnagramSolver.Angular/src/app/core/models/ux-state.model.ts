export type UxState<T> =
  | { stage: 'idle' }
  | { stage: 'loading' }
  | { stage: 'loaded'; data: T }
  | { stage: 'error'; error: string };
