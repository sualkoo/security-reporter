using Microsoft.VisualStudio.TestTools.UnitTesting;
using webapi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using webapi.ProjectSearch.Models;

namespace webapi.Service.Tests
{
    [TestClass()]
    public class CosmosServiceTests
    {
        [TestMethod()]
        public void AddProjectReportTest()
        {
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            CosmosService testService = new CosmosService(config);
            ProjectReportData projectReportData = new ProjectReportData();
            projectReportData.Id = Guid.NewGuid();
            projectReportData.DocumentInfo = new Models.ProjectReport.DocumentInformation();
            projectReportData.DocumentInfo.AssetType = "test String";
            var task = testService.AddProjectReport(projectReportData);
            bool isValid = task.Result;
            Assert.IsTrue(isValid);
        }
    }
}