using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Moq;
using NUnit.Framework;
using webapi.Dashboard.Services;
using webapi.Service;

namespace webapiTests.Dashboard.Services;

[TestFixture]
public class DashboardServiceTests
{
    [SetUp]
    public void SetUp()
    {
        // Initialize mocks and service
        mockCosmosService = new Mock<ICosmosService>();
        dashboardService = new DashboardService(mockCosmosService.Object);
    }

    private Mock<ICosmosService> mockCosmosService;
    private DashboardService dashboardService;


    [Test]
    public async Task GetCriticalityDataTest_ReturnsData()
    {
        //Arrange
        var data = new List<Tuple<string, int, int>>();

        // Mock the CosmosService's behavior to return the expected project report
        mockCosmosService.Setup(cosmos => cosmos.GetCriticalityData()).ReturnsAsync(data);

        // Act
        var result = await dashboardService.GetCriticalityData();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreSame(result, data);
    }

    [Test]
    public void GetCriticalityDataTest_ReturnsError()
    {
        //Arrange
        // Mock the CosmosService's behavior to return the expected project report
        mockCosmosService.Setup(cosmos => cosmos.GetCriticalityData())
            .ThrowsAsync(new CosmosException("Resource not found.", HttpStatusCode.NotFound, 0, "", 0));


        // Assert
        Assert.ThrowsAsync<CosmosException>(async () => await dashboardService.GetCriticalityData());
    }

    [Test]
    public async Task GetVulnerabilityDataTest_ReturnsData()
    {
        //Arrange
        var data = new List<Tuple<string, int, int>>();

        // Mock the CosmosService's behavior to return the expected project report
        mockCosmosService.Setup(cosmos => cosmos.GetVulnerabilityData()).ReturnsAsync(data);

        // Act
        var result = await dashboardService.GetVulnerabilityData();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreSame(result, data);
    }

    [Test]
    public void GetVulnerabilityDataTest_ReturnsError()
    {
        //Arrange
        var data = new List<Tuple<string, int, int>>();

        // Mock the CosmosService's behavior to return the expected project report
        mockCosmosService.Setup(cosmos => cosmos.GetVulnerabilityData())
            .ThrowsAsync(new CosmosException("Resource not found.", HttpStatusCode.NotFound, 0, "", 0));


        // Assert
        Assert.ThrowsAsync<CosmosException>(async () => await dashboardService.GetVulnerabilityData());
    }

    [Test]
    public async Task GetCWEDataTest_ReturnsData()
    {
        //Arrange
        var data = new List<Tuple<int, int>>();

        // Mock the CosmosService's behavior to return the expected project report
        mockCosmosService.Setup(cosmos => cosmos.GetCWEData()).ReturnsAsync(data);

        // Act
        var result = await dashboardService.GetCweData();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreSame(result, data);
    }

    [Test]
    public void GetCWEDataTest_ReturnsError()
    {
        //Arrange
        var data = new List<Tuple<int, int>>();

        // Mock the CosmosService's behavior to return the expected project report
        mockCosmosService.Setup(cosmos => cosmos.GetCWEData())
            .ThrowsAsync(new CosmosException("Resource not found.", HttpStatusCode.NotFound, 0, "", 0));


        // Assert
        Assert.ThrowsAsync<CosmosException>(async () => await dashboardService.GetCweData());
    }

    [Test]
    public async Task GetCVSSDataTest_ReturnsData()
    {
        //Arrange
        var data = new List<Tuple<float, string, string>>();

        // Mock the CosmosService's behavior to return the expected project report
        mockCosmosService.Setup(cosmos => cosmos.GetCVSSData()).ReturnsAsync(data);

        // Act
        var result = await dashboardService.GetCvssData();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreSame(result, data);
    }

    [Test]
    public void GetCVSSDataTest_ReturnsError()
    {
        //Arrange
        var data = new List<Tuple<float, string, string>>();

        // Mock the CosmosService's behavior to return the expected project report
        mockCosmosService.Setup(cosmos => cosmos.GetCVSSData())
            .ThrowsAsync(new CosmosException("Resource not found.", HttpStatusCode.NotFound, 0, "", 0));


        // Assert
        Assert.ThrowsAsync<CosmosException>(async () => await dashboardService.GetCvssData());
    }
}