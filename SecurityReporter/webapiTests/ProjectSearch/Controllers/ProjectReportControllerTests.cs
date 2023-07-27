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
            string filePath = @"C:\Users\User\source\repos\projects\2023\SecurityReporter\webapiTests\ProjectSearch\Controllers\dobre.zip";

            // Open the ZIP archive
            ZipArchive zipArchive = ZipFile.OpenRead(filePath);

            // Get the absolute path to the ZIP file
            string zipFilePath = @"C:\Users\User\source\repos\projects\2023\SecurityReporter\webapiTests\ProjectSearch\Controllers\dobre.zip";

            // Read the ZIP file's content into a byte array
            byte[] zipFileContent = File.ReadAllBytes(zipFilePath);
            var stream = new MemoryStream(zipFileContent);
            IFormFile zipFile;

            // Create a MemoryStream from the ZIP file content
            using (MemoryStream memoryStream = new MemoryStream(zipFileContent))
            {
                // Create an IFormFile instance using the MemoryStream
                /*zipFile = new FormFile(memoryStream, 0, zipFileContent.Length, zipFilePath, "dobre.zip");*/
                zipFile = new FormFile(memoryStream, 0, memoryStream.Length, null, "dobre.zip");

                // Now you can use the 'zipFile' instance as needed, such as passing it to your method for testing or further processing.
            }

            // Act
            var result = await projectReportController.addProjectReport(zipFile);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);


            /*// Check if the file exists
            if (File.Exists(filePath))
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);

                IFormFile formFile = new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, "formFile", Path.GetFileName(filePath));

                // Act
                var result = await projectReportController.addProjectReport(formFile);

                // Assert
                Assert.IsNotNull(result);
                Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);
                *//*Assert.AreEqual(200, result.StatusCode);*//*
            }
            else
            {
                Console.WriteLine("The file does not exist.");
            }*/
        }

        [Test]
        public void getProjectReportByIdTest()
        {
            Assert.Fail();
        }

        //  TODO
        [Test]
        public void getProjectReportsAsyncTest()
        {
            Assert.Fail();
        }
    }
}