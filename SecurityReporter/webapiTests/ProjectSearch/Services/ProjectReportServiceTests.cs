using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Moq;
using NUnit.Framework;
using System.Net;
using webapi.Models.ProjectReport;
using webapi.ProjectSearch.Models;
using webapi.Service;

namespace webapi.ProjectSearch.Services.Tests
{
    [TestFixture()]
    public class ProjectReportServiceTests
    {
        private Mock<ICosmosService> mockCosmosService;
        private Mock<IProjectDataParser> mockParser;
        private Mock<IProjectDataValidator> mockValidator;
        private ProjectReportService projectReportService;


        [SetUp]
        public void SetUp()
        {
            // Initialize mocks and service
            mockCosmosService = new Mock<ICosmosService>();
            mockParser = new Mock<IProjectDataParser>();
            mockValidator = new Mock<IProjectDataValidator>();
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
        public void SaveReportFromZip_ValidZip_ReturnsProjectReportData()
        {
            // Arrange
            var file = new Mock<IFormFile>();
            var fileStream = new MemoryStream();
            file.Setup(f => f.OpenReadStream()).Returns(fileStream);

            var expectedReportData = new ProjectReportData
            {
                Id = Guid.NewGuid(),
                DocumentInfo = new DocumentInformation
                {
                    ProjectReportName = "Dummy Project 1"
                }
            };

            // Mock
            mockParser.Setup(parser => parser.Extract(file.Object.OpenReadStream())).Returns(expectedReportData);
            mockValidator.Setup(validator => validator.Validate(expectedReportData)).Returns(true);
            mockCosmosService.Setup(cosmos => cosmos.AddProjectReport(expectedReportData)).ReturnsAsync(true);

            // Act
            var result = projectReportService.SaveReportFromZip(file.Object);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(result.Result, expectedReportData);
        }

        [Test()]
        public void SaveReportFromZip_ParserFail_MissingInformation_ThrowsCustomException()
        {
            // Arrange
            // Load sample zip file
            var file = new Mock<IFormFile>();
            var fileStream = new MemoryStream();

            // Mock behaviour
            file.Setup(f => f.OpenReadStream()).Returns(fileStream);
            mockParser.Setup(parser => parser.Extract(fileStream)).Throws(new ArgumentNullException());

            // Act
            var result = projectReportService.SaveReportFromZip(file.Object);

            // Assert
            Assert.ThrowsAsync<CustomException>(async () => await projectReportService.SaveReportFromZip(file.Object));
        }

        [Test()]
        public void SaveReportFromZip_ValidationFail_ThrowsCustomException()
        {
            // Arrange

            // Load sample zip file
            var file = new Mock<IFormFile>();
            var fileStream = new MemoryStream();

            var extractedData = new ProjectReportData
            {
                Id = Guid.NewGuid(),
                DocumentInfo = new DocumentInformation
                {
                    ProjectReportName = "Dummy Project 1"
                }
            };

            // Mock behaviour
            file.Setup(f => f.OpenReadStream()).Returns(fileStream);
            mockParser.Setup(parser => parser.Extract(fileStream)).Returns(extractedData);
            mockValidator.Setup(validator => validator.Validate(extractedData)).Returns(false);

            // Act & Assert
            Assert.ThrowsAsync<CustomException>(async () => await projectReportService.SaveReportFromZip(file.Object));
        }

        [Test()]
        public void SaveReportFromZip_SavingToDatabaseFailed_ThrowsCustomException()
        {
            // Arrange

            // Load sample zip file
            var file = new Mock<IFormFile>();
            var fileStream = new MemoryStream();

            var extractedData = new ProjectReportData
            {
                Id = Guid.NewGuid(),
                DocumentInfo = new DocumentInformation
                {
                    ProjectReportName = "Dummy Project 1"
                }
            };

            // Mock behaviour
            file.Setup(f => f.OpenReadStream()).Returns(fileStream);
            mockParser.Setup(parser => parser.Extract(fileStream)).Returns(extractedData);
            mockValidator.Setup(validator => validator.Validate(extractedData)).Returns(true);
            mockCosmosService.Setup(cosmos => cosmos.AddProjectReport(extractedData)).Throws(new CustomException(StatusCodes.Status500InternalServerError, "Failed to save ProjectReport to database."));

            // Act & Assert
            Assert.ThrowsAsync<CustomException>(async () => await projectReportService.SaveReportFromZip(file.Object));
        }
    }
}