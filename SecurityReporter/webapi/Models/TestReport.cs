namespace webapi.Models;

public class TestReport
{
    public DateOnly Date { get; set; }
    public string ProjectType { get; set; }
    public string ProjectName { get; set; }
    public string ApplicationManager { get; set; }
    public string Department { get; set; }
    public string AssetType { get; set; }
}
