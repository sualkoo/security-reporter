export interface PagedResponse<T> {
  pageNumber: number;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  data: T[];
}
