namespace webapi.ProjectSearch.Models;

public class PagedDbResults<T>
{
    public PagedDbResults(T data, int pageNumber)
    {
        PageNumber = pageNumber;
        Data = data;
    }

    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public Uri NextPage { get; set; }
    public T Data { get; set; }
}