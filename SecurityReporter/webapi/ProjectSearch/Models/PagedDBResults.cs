namespace webapi.ProjectSearch.Models
{
    public class PagedDBResults<T>
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public T Data { get; set; }

        public PagedDBResults(T data, int pageNumber)
        {
            this.PageNumber = pageNumber;
            this.Data = data;
        }
    }
}
