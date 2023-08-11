using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Moq;
using NUnit.Framework;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;
using webapi.ProjectSearch.Services.Extractor;
using webapi.Service;

namespace webapiTests.ProjectSearch.Services
{
    [TestFixture()]
    [Ignore("")]
    public class ProjectReportServiceTests
    {
        private Mock<ICosmosService> mockCosmosService;
        private Mock<IProjectDataParser> mockParser;
        private Mock<IDBProjectDataParser> mockDBParser;
        private Mock<IPdfBuilder> mockPdfBuilder;
        private Mock<IProjectDataValidator> mockValidator;
        private Mock<IAzureBlobService> mockAzureBlobService;
        private ProjectReportService projectReportService;


        [SetUp]
        public void SetUp()
        {
            // Initialize mocks and service
            mockCosmosService = new Mock<ICosmosService>();
            mockParser = new Mock<IProjectDataParser>();
            mockDBParser = new Mock<IDBProjectDataParser>();
            mockValidator = new Mock<IProjectDataValidator>();
            mockPdfBuilder = new Mock<IPdfBuilder>();
            mockAzureBlobService = new Mock<IAzureBlobService>();
            projectReportService = new ProjectReportService(mockParser.Object, mockDBParser.Object,
                mockValidator.Object, mockCosmosService.Object, mockPdfBuilder.Object, mockAzureBlobService.Object);
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
            mockCosmosService.Setup((cosmos) => cosmos.GetProjectReport(id.ToString()))
                .ReturnsAsync(expectedProjectReport);

            // Act
            var result = await ((IProjectReportService)projectReportService).GetReportByIdAsync(id);

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
            mockCosmosService.Setup((cosmos) => cosmos.GetProjectReport(id.ToString())).ThrowsAsync(
                new Microsoft.Azure.Cosmos.CosmosException("Resource not found.", System.Net.HttpStatusCode.NotFound, 0,
                    "", 0));

            // Act & Assert
            Assert.ThrowsAsync<CosmosException>(async () =>
                await ((IProjectReportService)projectReportService).GetReportByIdAsync(id));
        }

        [Test]
        public void GetReportFindingsAsync_AtLeastOneParamSpecified_ReturnsPageOfFindings()
        {
            // Arange
            var searchedValue = "Dummy";

            var expectedFinding = new FindingResponse();
            expectedFinding.ProjectReportId = Guid.NewGuid();
            expectedFinding.ProjectReportName = "Dummy Project 1";

            var expectedData = new List<FindingResponse>
            {
                expectedFinding
            };

            var expectedResponse = new PagedDBResults<List<FindingResponse>>(expectedData, 1);


            // Searching by Project Report Name
            mockCosmosService
                .Setup(cosmos => cosmos.GetPagedProjectReportFindings(searchedValue, null, null, null, null, null, 1))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = ((IProjectReportService)projectReportService)
                .GetReportFindingsAsync(searchedValue, null, null, null, null, null, 1).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResponse, result);
        }

        [Test]
        public void GetReportFindingsAsync_NoFilterSpecified_ReturnsPageOfFindings()
        {
            // Arange
            var searchedValue = "Dummy";
            mockCosmosService
                .Setup(cosmos => cosmos.GetPagedProjectReportFindings(null, null, null, null, null, null, 1))
                .ThrowsAsync(new CustomException(StatusCodes.Status400BadRequest,
                    "At least one search filter must be specified"));

            // Act & Assert
            Assert.ThrowsAsync<CustomException>(async () =>
                await ((IProjectReportService)projectReportService).GetReportFindingsAsync(null, null, null, null, null,
                    null, 1));
        }

        [Test]
        public void GetReportFindingsAsync_NoSearchValueSpecified_ReturnsPageOfFindings()
        {
            // Arange
            var searchedValue = "Dummy";
            mockCosmosService
                .Setup(cosmos => cosmos.GetPagedProjectReportFindings(null, null, null, null, null, null, 1))
                .ThrowsAsync(new CustomException(StatusCodes.Status400BadRequest, "Search value cannot be null."));

            // Act & Assert
            Assert.ThrowsAsync<CustomException>(async () =>
                await ((IProjectReportService)projectReportService).GetReportFindingsAsync(null, null, null, null, null,
                    null, 1));
        }

        // [Test]
        // public async Task SaveReportFromZip_ValidZip_ReturnsProjectReportDataAsync()
        // {
        //     //  Arrange
        //     var expectedReportData = new ProjectReportData
        //     {
        //         DocumentInfo = new DocumentInformation
        //         {
        //             ProjectReportName = "Dummy Project 1"
        //         }
        //     };
        //
        //     var fileName = "report.zip";
        //     byte[] zipFileContent;
        //
        //     using (var zipStream = new MemoryStream())
        //     {
        //         zipFileContent = zipStream.ToArray();
        //     }
        //
        //     var file = new FormFile(new MemoryStream(zipFileContent), 0, zipFileContent.Length, "reportFile", fileName);
        //
        //     mockParser.Setup(parser => parser.Extract(It.IsAny<Stream>())).Returns(expectedReportData);
        //     mockValidator.Setup(validator => validator.Validate(expectedReportData)).Returns(true);
        //     mockCosmosService.Setup(cosmos => cosmos.AddProjectReport(expectedReportData)).ReturnsAsync(true);
        //
        //     // Act
        //     var result = await ((IProjectReportService)projectReportService).SaveReportFromZip(file);
        //
        //     // Assert
        //     Assert.IsNotNull(result);
        //     Assert.AreSame(result, expectedReportData);
        // }
        //
        // [Test()]
        // public void SaveReportFromZip_ParserFail_MissingInformation_ThrowsCustomException()
        // {
        //     // Arrange
        //     // Load sample zip file
        //     var file = new Mock<IFormFile>();
        //     var fileStream = new MemoryStream();
        //
        //     // Mock behaviour
        //     file.Setup(f => f.OpenReadStream()).Returns(fileStream);
        //     mockParser.Setup(parser => parser.Extract(fileStream)).Throws(new ArgumentNullException());
        //
        //     // Act
        //     var result = ((IProjectReportService)projectReportService).SaveReportFromZip(file.Object);
        //
        //     // Assert
        //     Assert.ThrowsAsync<CustomException>(async () => await ((IProjectReportService)projectReportService).SaveReportFromZip(file.Object));
        // }
        //
        // [Test()]
        // public void SaveReportFromZip_ValidationFail_ThrowsCustomException()
        // {
        //     var extractedData = new ProjectReportData
        //     {
        //         Id = Guid.NewGuid(),
        //         DocumentInfo = new DocumentInformation
        //         {
        //             ProjectReportName = "Dummy Project 1"
        //         }
        //     };
        //
        //     // Create sample zip file
        //     var fileName = "report.zip";
        //     byte[] zipFileContent;
        //
        //     using (var zipStream = new MemoryStream())
        //     {
        //         using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        //         {
        //             // Create a dummy file named "data.txt" with some content in the zip archive
        //             var entry = archive.CreateEntry("data.txt");
        //             using (var entryStream = entry.Open())
        //             using (var writer = new StreamWriter(entryStream))
        //             {
        //                 writer.Write("Sample content for testing.");
        //             }
        //         }
        //
        //         zipFileContent = zipStream.ToArray();
        //     }
        //
        //     var file = new FormFile(new MemoryStream(zipFileContent), 0, zipFileContent.Length, "reportFile", fileName);
        //
        //     // Mock
        //     mockParser.Setup(parser => parser.Extract(file.OpenReadStream())).Returns(extractedData);
        //     mockValidator.Setup(validator => validator.Validate(extractedData)).Returns(false);
        //
        //     // Act & Assert
        //     Assert.ThrowsAsync<CustomException>(async () => await ((IProjectReportService)projectReportService).SaveReportFromZip(file));
        // }
        //
        // [Test()]
        // public void SaveReportFromZip_SavingToDatabaseFailed_ThrowsCustomException()
        // {
        //     // Arrange
        //
        //     var extractedData = new ProjectReportData
        //     {
        //         Id = Guid.NewGuid(),
        //         DocumentInfo = new DocumentInformation
        //         {
        //             ProjectReportName = "Dummy Project 1"
        //         }
        //     };
        //
        //     // Create sample zip file
        //     var fileName = "report.zip";
        //     byte[] zipFileContent;
        //
        //     using (var zipStream = new MemoryStream())
        //     {
        //         using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        //         {
        //             // Create a dummy file named "data.txt" with some content in the zip archive
        //             var entry = archive.CreateEntry("data.txt");
        //             using (var entryStream = entry.Open())
        //             using (var writer = new StreamWriter(entryStream))
        //             {
        //                 writer.Write("Sample content for testing.");
        //             }
        //         }
        //
        //         zipFileContent = zipStream.ToArray();
        //     }
        //
        //     var file = new FormFile(new MemoryStream(zipFileContent), 0, zipFileContent.Length, "reportFile", fileName);
        //
        //     // Mock sevices
        //     mockParser.Setup(parser => parser.Extract(file.OpenReadStream())).Returns(extractedData);
        //     mockValidator.Setup(validator => validator.Validate(extractedData)).Returns(true);
        //     mockCosmosService.Setup(cosmos => cosmos.AddProjectReport(extractedData)).Throws(new CustomException(StatusCodes.Status500InternalServerError, "Failed to save ProjectReport to database."));
        //
        //     // Act & Assert
        //     Assert.ThrowsAsync<CustomException>(async () => await ((IProjectReportService)projectReportService).SaveReportFromZip(file));
        //
        //     // Cleanup
        //     File.Delete(fileName);
        // }
    }
}