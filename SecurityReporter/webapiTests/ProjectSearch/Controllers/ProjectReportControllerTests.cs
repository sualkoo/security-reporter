using Microsoft.Extensions.Logging;
using Moq;
using webapi.ProjectSearch.Controllers;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webapi.ProjectReportControllers.Tests
{
    [TestFixture()]
    public class ProjectReportControllerTests
    {
        private Mock<IProjectReportService> mockProjectReportService;
        private Mock<ILogger<ProjectReportController>> mockLogger;
        private Mock<ILogger<ExceptionHandlingControllerBase>> mockLoggerBase;
        private ProjectReportController projectReportController;
        private Mock<IPdfBuilder> mockPdfBuilder;

        [SetUp]
        public void SetUp()
        {
            mockLogger = new Mock<ILogger<ProjectReportController>>();
            mockLoggerBase = new Mock<ILogger<ExceptionHandlingControllerBase>>();
            mockProjectReportService = new Mock<IProjectReportService>();
            mockPdfBuilder = new Mock<IPdfBuilder>();
            projectReportController = new ProjectReportController(mockLogger.Object, mockLoggerBase.Object, mockProjectReportService.Object, mockPdfBuilder.Object);
        }

        [Test]
        public void addProjectReport_ValidFile_ReturnsOkResult()
        {
            // Arrange
            IFormFile formFile = new FormFile(Stream.Null, 0, 0, "file", "zip");
            ProjectReportData data = new ProjectReportData();
            data.Id = Guid.NewGuid();

            mockProjectReportService.Setup(service => service.SaveReportFromZipAsync(formFile)).ReturnsAsync(data);

            // Act
            var result = projectReportController.AddProjectReport(formFile).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);
        }

        [Test]
        public void addProjectReport_InvalidFile_ReturnsNotAcceptableResult()
        {
            // Arrange
            IFormFile formFile = new FormFile(Stream.Null, 0, 0, "file", "pdf");
            ProjectReportData data = new ProjectReportData();
            data.Id = Guid.NewGuid();

            CustomException expectedException = new CustomException(StatusCodes.Status406NotAcceptable, "Invalid file type. Only .zip files are allowed.");

            mockProjectReportService.Setup(service => service.SaveReportFromZipAsync(formFile)).ThrowsAsync(expectedException);

            // Act
            var result = projectReportController.AddProjectReport(formFile).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ObjectResult), result.GetType());
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 406);
        }

        [Test]
        public void addProjectReport_InvalidFile_ReturnsInternalServerErrorResult()
        {
            // Arrange
            IFormFile formFile = new FormFile(Stream.Null, 0, 0, "file", "zip");
            ProjectReportData data = new ProjectReportData();
            data.Id = Guid.NewGuid();

            CustomException expectedException = new CustomException(StatusCodes.Status500InternalServerError, "Failed to save ProjectReport to database.");

            mockProjectReportService.Setup(service => service.SaveReportFromZipAsync(formFile)).ThrowsAsync(expectedException);

            // Act
            var result = projectReportController.AddProjectReport(formFile).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ObjectResult), result.GetType());
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 500);
        }

        [Test]
        public void getProjectReportByIdTest_ValidId_ReturnsOkResult()
        {
            // Arange
            Guid expectedId = Guid.NewGuid();
            ProjectReportData data = new ProjectReportData();
            data.Id = expectedId;

            mockProjectReportService.Setup(service => service.GetReportByIdAsync(expectedId)).ReturnsAsync(data);

            // Act
            var result = projectReportController.GetProjectReportById(expectedId).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);
        }

        [Test]
        public void getProjectReportById_InvalidId_ReturnsErrorResponse()
        {
            // Arange
            Guid expectedId = Guid.NewGuid();
            CustomException expectedException = new CustomException(StatusCodes.Status404NotFound, "Report with searched ID not found");

            mockProjectReportService.Setup(service => service.GetReportByIdAsync(expectedId)).ThrowsAsync(expectedException);

            // Act
            var result = projectReportController.GetProjectReportById(expectedId).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ObjectResult), result.GetType());
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 404);
        }

        [Test]
        public void getProjectReportFindings_ValidFinding_ReturnsOkResult()
        {
            // Arrange
            string projectName = "Dummy Project 1";
            string details = "The \\texttt{dummyapplication.apk} is signed with a debug certificate.";
            string impact = "Debug certificates do not meet security standards of the release certificates.";
            string repeatability = "The \\texttt{dummyapplication.apk} is signed with a debug certificate (\\texttt{CERT.RSA}), which can be found in the \\texttt{META-INF} folder.The certificate properties are shown in \\cref{figure:DebugCert}.";
            string references = " \\href{https://doku-center.med.siemens.de/regelwerke/L4U-Intranet/GD/GD-41/GD-41-03-E.pdf}{Siemens Healthineers Guidance for Secure Software Architecture, Design andDevelopment: 8.4 Code-Signing}";
            string cWE = "328";

            string searchedValue = "Dumm";
            int page = 1;

            var expectedFinding = new FindingResponse();
            expectedFinding.ProjectReportId = Guid.NewGuid();
            expectedFinding.ProjectReportName = projectName;

            var expectedData = new List<FindingResponse>
            {
                expectedFinding
            };

            var expectedResponse = new PagedDBResults<List<FindingResponse>>(expectedData, 1);

            mockProjectReportService.Setup(service => service.GetReportFindingsAsync(searchedValue, searchedValue, searchedValue, searchedValue, searchedValue, searchedValue, page)).ReturnsAsync(expectedResponse);

            // Act
            var result = projectReportController.GetProjectReportFindings(searchedValue, searchedValue, searchedValue, searchedValue, searchedValue, searchedValue, page).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);
        }

        [Test]
        public void getProjectReportFindings_InValidFinding_BadRequest()
        {
            // Arrange
            string searchedValue = "Dummy";
            int page = 1;

            CustomException expectedException = new CustomException(StatusCodes.Status400BadRequest, "At list one filter has to be selected");

            mockProjectReportService.Setup(service => service.GetReportFindingsAsync(null, null, null, null, null, null, page)).ThrowsAsync(expectedException);

            // Act
            var result = projectReportController.GetProjectReportFindings(null, null, null, null, null, null, page).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 400);
        }

        [Test]
        public void deleteProjectReports_WithValidIds_ReturnsOkResult()
        {
            // Arrange
            var ids = new List<string> { "1", "2", "3" };
            mockProjectReportService.Setup(service => service.DeleteReportAsync(ids)).ReturnsAsync(true);

            // Act
            var task = projectReportController.DeleteProjectReports(ids);

            var result = task.Result as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(true, result.Value);

        }

        [Test]
        public void deleteProjectReports_WithException_ReturnsBadRequestResult()
        {
            var ids = new List<string> { "1", "2", "3" };
            CustomException expectedException = new CustomException(StatusCodes.Status400BadRequest, "Items with that id don't exist");
            mockProjectReportService.Setup(service => service.DeleteReportAsync(ids)).Throws(expectedException);

            // Act
            var result = projectReportController.DeleteProjectReports(ids).Result;

            // Assert

            Assert.IsNotNull(result);
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 400);
        }

        [Test]
        public void DeleteProjectReportsALL_ReturnsOkResult()
        {
            // Arrange
            mockProjectReportService.Setup(service => service.DeleteReportAllAsync()).ReturnsAsync(true);

            // Act
            var task = projectReportController.DeleteProjectReportsAll();

            var result = task.Result as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(true, result.Value);
        }

        [Test]
        public void DeleteProjectReportsALL_WithException_ReturnsNotFoundResult()
        {
            //Arrange
            CustomException expectedException = new CustomException(StatusCodes.Status404NotFound, "Container doesn't exists");
            mockProjectReportService.Setup(service => service.DeleteReportAllAsync()).Throws(expectedException);

            // Act
            var result = projectReportController.DeleteProjectReportsAll().Result;

            // Assert

            Assert.IsNotNull(result);
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 404);
        }
    }
}