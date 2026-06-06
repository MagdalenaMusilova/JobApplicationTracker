export interface AuthServiceResultDto<T> {
  succeeded: boolean;
  value?: T | null;
  errorMessage?: string | null;
  statusCode: number;
}