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
using NUnit.Framework.Interfaces;

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
        public void addProjectReport_ValidFile_ReturnsOkResult()
        {
            // Arrange
            IFormFile formFile = new FormFile(Stream.Null, 0, 0, "file", "zip");
            ProjectReportData data = new ProjectReportData();
            data.Id = Guid.NewGuid();

            mockProjectReportService.Setup(service => service.SaveReportFromZip(formFile)).ReturnsAsync(data);

            // Act
            var result = projectReportController.addProjectReport(formFile).Result;

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

            mockProjectReportService.Setup(service => service.SaveReportFromZip(formFile)).ThrowsAsync(expectedException);

            // Act
            var result = projectReportController.addProjectReport(formFile).Result;

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

            mockProjectReportService.Setup(service => service.SaveReportFromZip(formFile)).ThrowsAsync(expectedException);

            // Act
            var result = projectReportController.addProjectReport(formFile).Result;

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
            var result = projectReportController.getProjectReportById(expectedId).Result;

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
            var result = projectReportController.getProjectReportById(expectedId).Result;

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
            var result = projectReportController.getProjectReportFindings(searchedValue, searchedValue, searchedValue, searchedValue, searchedValue, searchedValue, page).Result;

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
            var result = projectReportController.getProjectReportFindings(null, null, null, null, null, null, page).Result;

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
            var task = projectReportController.deleteProjectReports(ids);

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
            var result = projectReportController.deleteProjectReports(ids).Result;

            // Assert

            Assert.IsNotNull(result);
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 400);
        }
    }
}