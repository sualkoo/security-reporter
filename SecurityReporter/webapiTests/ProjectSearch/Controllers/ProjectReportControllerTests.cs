using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using webapi.ProjectSearch.Controllers;
using webapi.ProjectSearch.Models;
using webapi.ProjectSearch.Services;

namespace webapiTests.ProjectSearch.Controllers;

[TestFixture]
public class ProjectReportControllerTests
{
    [SetUp]
    public void SetUp()
    {
        mockLogger = new Mock<ILogger<ProjectReportController>>();
        mockLoggerBase = new Mock<ILogger<ExceptionHandlingControllerBase>>();
        mockProjectReportService = new Mock<IProjectReportService>();
        mockPdfBuilder = new Mock<IPdfBuilder>();
        projectReportController = new ProjectReportController(mockLogger.Object, mockLoggerBase.Object,
            mockProjectReportService.Object, mockPdfBuilder.Object);
    }

    private Mock<IProjectReportService> mockProjectReportService;
    private Mock<ILogger<ProjectReportController>> mockLogger;
    private Mock<ILogger<ExceptionHandlingControllerBase>> mockLoggerBase;
    private ProjectReportController projectReportController;
    private Mock<IPdfBuilder> mockPdfBuilder;

    [Test]
    public void addProjectReport_ValidFile_ReturnsOkResult()
    {
        // Arrange
        IFormFile formFile = new FormFile(Stream.Null, 0, 0, "file", "zip");
        var data = new ProjectReportData
        {
            Id = Guid.NewGuid()
        };

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

        var expectedException = new CustomException(StatusCodes.Status406NotAcceptable,
            "Invalid file type. Only .zip files are allowed.");

        mockProjectReportService.Setup(service => service.SaveReportFromZipAsync(formFile))
            .ThrowsAsync(expectedException);

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

        var expectedException = new CustomException(StatusCodes.Status500InternalServerError,
            "Failed to save ProjectReport to database.");

        mockProjectReportService.Setup(service => service.SaveReportFromZipAsync(formFile))
            .ThrowsAsync(expectedException);

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
        // Arrange
        var expectedId = Guid.NewGuid();
        var data = new ProjectReportData
        {
            Id = expectedId
        };

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
        // Arrange
        var expectedId = Guid.NewGuid();
        var expectedException = new CustomException(StatusCodes.Status404NotFound, "Report with searched ID not found");

        mockProjectReportService.Setup(service => service.GetReportByIdAsync(expectedId))
            .ThrowsAsync(expectedException);

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
        const string projectName = "Dummy Project 1";

        const string searchedValue = "Dumm";
        const int page = 1;

        var expectedFinding = new FindingResponse
        {
            ProjectReportId = Guid.NewGuid(),
            ProjectReportName = projectName
        };

        var expectedData = new List<FindingResponse>
        {
            expectedFinding
        };

        var expectedResponse = new PagedDBResults<List<FindingResponse>>(expectedData, 1);

        mockProjectReportService.Setup(service => service.GetReportFindingsAsync(searchedValue, searchedValue,
            searchedValue, searchedValue, searchedValue, searchedValue, page)).ReturnsAsync(expectedResponse);

        // Act
        var result = projectReportController.GetProjectReportFindings(searchedValue, searchedValue, searchedValue,
            searchedValue, searchedValue, searchedValue, page).Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(OkObjectResult), result.GetType());
        Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);
    }

    [Test]
    public void getProjectReportFindings_InValidFinding_BadRequest()
    {
        // Arrange
        var searchedValue = "Dummy";
        var page = 1;

        var expectedException =
            new CustomException(StatusCodes.Status400BadRequest, "At list one filter has to be selected");

        mockProjectReportService
            .Setup(service => service.GetReportFindingsAsync(null, null, null, null, null, null, page))
            .ThrowsAsync(expectedException);

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
        var expectedException = new CustomException(StatusCodes.Status400BadRequest, "Items with that id don't exist");
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
        var expectedException = new CustomException(StatusCodes.Status404NotFound, "Container doesn't exists");
        mockProjectReportService.Setup(service => service.DeleteReportAllAsync()).Throws(expectedException);

        // Act
        var result = projectReportController.DeleteProjectReportsAll().Result;

        // Assert

        Assert.IsNotNull(result);
        Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 404);
    }
}