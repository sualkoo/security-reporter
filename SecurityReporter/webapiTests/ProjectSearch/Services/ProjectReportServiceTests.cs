using NUnit.Framework;
using webapi.ProjectSearch.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using webapi.Service;
using Microsoft.Extensions.Configuration;
using Moq;
using webapi.ProjectSearch.Models;
using System.Configuration;

namespace webapi.ProjectSearch.Services.Tests
{
    [TestFixture()]
    public class ProjectReportServiceTests
    {
        private Mock<ICosmosService> mockCosmosService;
        private Mock<ProjectDataParser> mockParser;
        private Mock<ProjectDataValidator> mockValidator;
        private ProjectReportService projectReportService;


        [SetUp] 
        public void SetUp() {
            // Initialize mocks and service
            mockCosmosService = new Mock<ICosmosService>();
            mockParser = new Mock<ProjectDataParser>();
            mockValidator = new Mock<ProjectDataValidator>();
            projectReportService = new ProjectReportService(mockParser.Object, mockValidator.Object, mockCosmosService.Object);
        }

        [Test()]
        public void ProjectReportServiceTest()
        {
            Assert.Fail();
        }

        [Test]
        public async Task GetReportByIdAsync_ValidId_ReturnsProjectReportData()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedId = id;
            var expectedProjectReport = new ProjectReportData();
            expectedProjectReport.Id = id;

            // Mock the CosmosService's behavior to return the expected project report
            mockCosmosService.Setup((cosmos) => cosmos.GetProjectReport(id.ToString())).ReturnsAsync(expectedProjectReport);

            // Act
            var result = await projectReportService.GetReportByIdAsync(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, expectedId);
        }

        [Test()]
        public void GetReportByIdAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void GetReportsAsyncTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void SaveReportFromZipTest()
        {
            Assert.Fail();
        }
    }
}