using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Moq;
using NUnit.Framework;
using System.IO.Compression;
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
        public async Task SaveReportFromZip_ValidZip_ReturnsProjectReportDataAsync()
        {
            //  Arrange
            var expectedReportData = new ProjectReportData
            {
                DocumentInfo = new DocumentInformation
                {
                    ProjectReportName = "Dummy Project 1"
                }
            };

            var fileName = "report.zip";
            byte[] zipFileContent;

            using (var zipStream = new MemoryStream())
            {
                zipFileContent = zipStream.ToArray();
            }

            var file = new FormFile(new MemoryStream(zipFileContent), 0, zipFileContent.Length, "reportFile", fileName);

            mockParser.Setup(parser => parser.Extract(It.IsAny<Stream>())).Returns(expectedReportData);
            mockValidator.Setup(validator => validator.Validate(expectedReportData)).Returns(true);
            mockCosmosService.Setup(cosmos => cosmos.AddProjectReport(expectedReportData)).ReturnsAsync(true);

            // Act
            var result = await projectReportService.SaveReportFromZip(file);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreSame(result, expectedReportData);
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
            var extractedData = new ProjectReportData
            {
                Id = Guid.NewGuid(),
                DocumentInfo = new DocumentInformation
                {
                    ProjectReportName = "Dummy Project 1"
                }
            };

            // Create sample zip file
            var fileName = "report.zip";
            byte[] zipFileContent;

            using (var zipStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    // Create a dummy file named "data.txt" with some content in the zip archive
                    var entry = archive.CreateEntry("data.txt");
                    using (var entryStream = entry.Open())
                    using (var writer = new StreamWriter(entryStream))
                    {
                        writer.Write("Sample content for testing.");
                    }
                }

                zipFileContent = zipStream.ToArray();
            }

            var file = new FormFile(new MemoryStream(zipFileContent), 0, zipFileContent.Length, "reportFile", fileName);

            // Mock
            mockParser.Setup(parser => parser.Extract(file.OpenReadStream())).Returns(extractedData);
            mockValidator.Setup(validator => validator.Validate(extractedData)).Returns(false);

            // Act & Assert
            Assert.ThrowsAsync<CustomException>(async () => await projectReportService.SaveReportFromZip(file));
        }

        [Test()]
        public void SaveReportFromZip_SavingToDatabaseFailed_ThrowsCustomException()
        {
            // Arrange

            var extractedData = new ProjectReportData
            {
                Id = Guid.NewGuid(),
                DocumentInfo = new DocumentInformation
                {
                    ProjectReportName = "Dummy Project 1"
                }
            };

            // Create sample zip file
            var fileName = "report.zip";
            byte[] zipFileContent;

            using (var zipStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                {
                    // Create a dummy file named "data.txt" with some content in the zip archive
                    var entry = archive.CreateEntry("data.txt");
                    using (var entryStream = entry.Open())
                    using (var writer = new StreamWriter(entryStream))
                    {
                        writer.Write("Sample content for testing.");
                    }
                }

                zipFileContent = zipStream.ToArray();
            }

            var file = new FormFile(new MemoryStream(zipFileContent), 0, zipFileContent.Length, "reportFile", fileName);

            // Mock sevices
            mockParser.Setup(parser => parser.Extract(file.OpenReadStream())).Returns(extractedData);
            mockValidator.Setup(validator => validator.Validate(extractedData)).Returns(true);
            mockCosmosService.Setup(cosmos => cosmos.AddProjectReport(extractedData)).Throws(new CustomException(StatusCodes.Status500InternalServerError, "Failed to save ProjectReport to database."));

            // Act & Assert
            Assert.ThrowsAsync<CustomException>(async () => await projectReportService.SaveReportFromZip(file));

            // Cleanup
            File.Delete(fileName);
        }
    }
}