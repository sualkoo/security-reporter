namespace webapi.ProjectSearch.Services
{
    public interface IDashboardService
    {
        public Task<List<Tuple<int, int>>> GetCriticalityData();
    }
}
