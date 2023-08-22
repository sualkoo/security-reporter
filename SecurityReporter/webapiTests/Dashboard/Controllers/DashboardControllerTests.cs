using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using webapi.Dashboard.Controllers;
using webapi.Dashboard.Services;
using webapi.ProjectSearch.Controllers;
using webapi.ProjectSearch.Models;

namespace webapiTests.Dashboard.Controllers;

[TestFixture]
public class DashboardControllerTests
{
    [SetUp]
    public void SetUp()
    {
        mockLogger = new Mock<ILogger<DashboardController>>();
        mockLoggerBase = new Mock<ILogger<ExceptionHandlingControllerBase>>();
        mockDashboardService = new Mock<IDashboardService>();
        dashboardController =
            new DashboardController(mockLogger.Object, mockLoggerBase.Object, mockDashboardService.Object);
    }

    private Mock<IDashboardService> mockDashboardService;
    private Mock<ILogger<DashboardController>> mockLogger;
    private Mock<ILogger<ExceptionHandlingControllerBase>> mockLoggerBase;
    private DashboardController dashboardController;

    [Test]
    public void getCriticalityDataTest_ReturnsOkResult()
    {
        // Arrange
        var data = new List<Tuple<string, int, int>>();
        mockDashboardService.Setup(service => service.GetCriticalityData()).ReturnsAsync(data);

        // Act
        var result = dashboardController.GetCriticalityData().Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(OkObjectResult), result.GetType());
        Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);
    }


    [Test]
    public void getCriticalityDataTest_ReturnsErrorResult()
    {
        // Arange
        var expectedException = new CustomException(StatusCodes.Status500InternalServerError,
            "Unexpected error while searching for data");
        mockDashboardService.Setup(service => service.GetCriticalityData()).ThrowsAsync(expectedException);

        // Act
        var result = dashboardController.GetCriticalityData().Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(ObjectResult), result.GetType());
        Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 500);
    }

    [Test]
    public void getVulnerabilityDataTest_ReturnsOkResult()
    {
        // Arange
        var data = new List<Tuple<string, int, int>>();
        mockDashboardService.Setup(service => service.GetVulnerabilityData()).ReturnsAsync(data);

        // Act
        var result = dashboardController.GetVulnerabilityData().Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(OkObjectResult), result.GetType());
        Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);
    }


    [Test]
    public void getVulnerabilityDataTest_ReturnsErrorResult()
    {
        // Arange
        var expectedException = new CustomException(StatusCodes.Status500InternalServerError,
            "Unexpected error while searching for data");
        mockDashboardService.Setup(service => service.GetVulnerabilityData()).ThrowsAsync(expectedException);

        // Act
        var result = dashboardController.GetVulnerabilityData().Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(ObjectResult), result.GetType());
        Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 500);
    }


    [Test]
    public void getCWEDataTest_ReturnsOkResult()
    {
        // Arange
        var data = new List<Tuple<int, int>>();
        mockDashboardService.Setup(service => service.GetCweData()).ReturnsAsync(data);

        // Act
        var result = dashboardController.GetCweData().Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(OkObjectResult), result.GetType());
        Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);
    }


    [Test]
    public void getCWEDataTest_ReturnsErrorResult()
    {
        // Arange
        var expectedException = new CustomException(StatusCodes.Status500InternalServerError,
            "Unexpected error while searching for data");
        mockDashboardService.Setup(service => service.GetCweData()).ThrowsAsync(expectedException);

        // Act
        var result = dashboardController.GetCweData().Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(ObjectResult), result.GetType());
        Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 500);
    }

    [Test]
    public void getCVSSDataTest_ReturnsOkResult()
    {
        // Arange
        var data = new List<Tuple<float, string, string>>();
        mockDashboardService.Setup(service => service.GetCvssData()).ReturnsAsync(data);

        // Act
        var result = dashboardController.GetCvssData().Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(OkObjectResult), result.GetType());
        Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);
    }


    [Test]
    public void getCVSSDataTest_ReturnsErrorResult()
    {
        // Arange
        var expectedException = new CustomException(StatusCodes.Status500InternalServerError,
            "Unexpected error while searching for data");
        mockDashboardService.Setup(service => service.GetCvssData()).ThrowsAsync(expectedException);

        // Act
        var result = dashboardController.GetCvssData().Result;

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(typeof(ObjectResult), result.GetType());
        Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 500);
    }
}