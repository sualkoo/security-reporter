using Microsoft.AspNetCore.Mvc;
using webapi.Models;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class TestReportsController : ControllerBase
{
    private readonly ILogger<TestReportsController> _logger;

    private static readonly string[] ProjectName =
    {
        "Alpha", "Beta", "Gamma", "Delta"
    };

    private static readonly string[] ProjectType =
    {
        "CT", "MR", "MRI", "X-Ray"
    };

    private static readonly string[] AssetType =
    {
        "png", "jpeg", "dicom"
    };

    public TestReportsController(ILogger<TestReportsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetTestReports")]
    public IEnumerable<TestReport> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new TestReport()
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            ProjectName = ProjectName[Random.Shared.Next(ProjectName.Length)],
            ProjectType = ProjectType[Random.Shared.Next(ProjectType.Length)],
            AssetType = AssetType[Random.Shared.Next(AssetType.Length)]
        })
        .ToArray();
    }
}
