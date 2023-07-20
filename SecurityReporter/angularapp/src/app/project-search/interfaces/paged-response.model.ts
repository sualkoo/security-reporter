export interface PagedResponse {
  pageNumber: number;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  data: any[];
}
