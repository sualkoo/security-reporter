using NUnit.Framework;
using webapi.Service;
using Moq;
using webapi.ProjectSearch.Models;
using Microsoft.Azure.Cosmos;

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

        [Test]
        public void GetReportByIdAsync_InvalidId_ThrowsException()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Mock the CosmosService's behavior to return the expected project report
            mockCosmosService.Setup((cosmos) => cosmos.GetProjectReport(id.ToString())).ThrowsAsync(new Microsoft.Azure.Cosmos.CosmosException("Resource not found.", System.Net.HttpStatusCode.NotFound, 0, "", 0));

            // Act & Assert
            Assert.ThrowsAsync<CosmosException>(async () => await projectReportService.GetReportByIdAsync(id));
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