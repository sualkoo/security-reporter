using Microsoft.Extensions.Logging;
using Moq;
using System.IO;
using System.Threading.Tasks;
using webapi.ProjectSearch.Controllers;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webapi.Models.ProjectReport;
using Microsoft.Azure.Cosmos.Linq;
using System.IO.Compression;
using NUnit.Framework.Constraints;
using Microsoft.AspNetCore.Http.HttpResults;


// namespace webapi.ProjectSearch.Controllers.Tests
// {
//     [TestFixture()]
//     public class ProjectReportControllerTests
//     {
//         // Helper method to create a stream from a string
//         private static Stream GetStreamFromString(string content)
//         {
//             var stream = new MemoryStream();
//             var writer = new StreamWriter(stream);
//             writer.Write(content);
//             writer.Flush();
//             stream.Position = 0;
//             return stream;
//         }

//         [Test]
//         public async Task AddProjectReport_ValidFile_ReturnsOkResult()
//         {
//             // Arrange
//             var mockProjectReportService = new Mock<IProjectReportService>();
//             var mockLogger = new Mock<ILoggerFactory>();
//             var mockLoggerInstance = new Mock<ILogger<ProjectReportController>>();

//             // Replace with your expected ProjectReportData instance
//             var expectedReportData = new ProjectReportData();

//             mockProjectReportService.Setup(s => s.SaveReportFromZip(It.IsAny<IFormFile>()))
//                                     .ReturnsAsync(expectedReportData);

//             // Setup the mock ILoggerFactory to return the mock ILogger instance
//             mockLogger.Setup(factory => factory.CreateLogger(It.IsAny<string>())).Returns(mockLoggerInstance.Object);

//             var controller = new ProjectReportController(mockLogger.Object, mockProjectReportService.Object);

//             // Create a mock form file using a stream
//             var formFileContent = "Mock file content"; // Replace with your file content
//             var formFile = new Mock<IFormFile>();
//             formFile.Setup(f => f.OpenReadStream()).Returns(GetStreamFromString(formFileContent));
//             formFile.Setup(f => f.Length).Returns(formFileContent.Length);
//             formFile.Setup(f => f.FileName).Returns("mock-file.zip"); // Replace with your desired file name

//             // Act
//             var result = await controller.addProjectReport(formFile.Object) as OkObjectResult;

//             // Assert
//             Assert.IsNotNull(result);
//             Assert.AreEqual(200, result.StatusCode);
//             Assert.AreEqual(expectedReportData, result.Value);
//         }
//     }
// }

namespace webapi.ProjectReportControllers.Tests
{
    [TestFixture()]
    public class ProjectReportControllerTests
    {
        private Mock<IProjectReportService> mockProjectReportService;
        private Mock<ILogger<ProjectReportController>> mockLogger;
        private ProjectReportController projectReportController;

        [SetUp]
        public void SetUp()
        {
            mockLogger = new Mock<ILogger<ProjectReportController>>();
            mockProjectReportService = new Mock<IProjectReportService>();
            projectReportController = new ProjectReportController(mockLogger.Object, mockProjectReportService.Object);
        }

        [Test]
        public void ProjectReportControllerTest()
        {
            Assert.Fail();
        }

        [Test]
        public void addProjectReportTest()
        {
            // Arrange

            // Mock the CosmosService's behavior to return the expected project report


            // Act


            // Assert

            Assert.Fail();
        }

        [Test]
        public async Task addProjectReport_ValidFile_ReturnsOkResult()
        {
            // Arrange
            IFormFile formFile = new FormFile(Stream.Null, 0, 0, "file", "jpg");
            ProjectReportData data = new ProjectReportData();
            data.Id = Guid.NewGuid();


            //mockProjectReportService.Setup(service => service.SaveReportFromZip(formFile)).ReturnsAsync(data);
            mockProjectReportService.Setup(service => service.SaveReportFromZip(formFile)).ReturnsAsync(data);

            // Act
            var result = await projectReportController.addProjectReport(formFile);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);


         
        }

        [Test]
        public void getProjectReportByIdTest()
        {
            // Arange
            Guid expectedId = Guid.NewGuid();
            ProjectReportData data = new ProjectReportData();
            data.Id = expectedId;

            mockProjectReportService.Setup(service => service.GetReportByIdAsync(expectedId)).ReturnsAsync(data);

            // Act
            var result = projectReportController.getProjectReportById(expectedId);
            var resultResponse = result.Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), resultResponse.GetType());

        }

        [Test]
        public async void getProjectReportById_InvalidId_ReturnsErrorResponse()
        {
            // Arange
            Guid expectedId = Guid.NewGuid();
            CustomException expectedException = new CustomException(StatusCodes.Status404NotFound, "Report with searched ID not found");

            mockProjectReportService.Setup(service => service.GetReportByIdAsync(expectedId)).ThrowsAsync(expectedException);

            // Act
            var result = await projectReportController.getProjectReportById(expectedId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ErrorResponse), result.GetType());
        }

        //  TODO
        [Test]
        public void getProjectReportsAsyncTest()
        {
            Assert.Fail();
        }
    }
}