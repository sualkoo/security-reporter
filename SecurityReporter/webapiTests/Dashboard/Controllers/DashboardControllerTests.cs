using NUnit.Framework;
using webapi.Dashboard.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using webapi.Dashboard.Services;
using Microsoft.Extensions.Logging;
using webapi.ProjectSearch.Controllers;
using webapi.ProjectSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Serialization.HybridRow;
using webapi.ProjectSearch.Models;
using Microsoft.AspNetCore.Http;

namespace webapi.Dashboard.Controllers.Tests
{
    [TestFixture()]
    public class DashboardControllerTests
    {
        private Mock<IDashboardService> mockDashboardService;
        private Mock<ILogger<DashboardController>> mockLogger;
        private Mock<ILogger<ExceptionHandlingControllerBase>> mockLoggerBase;
        private DashboardController dashboardController;

        [SetUp]
        public void SetUp()
        {
            mockLogger = new Mock<ILogger<DashboardController>>();
            mockLoggerBase = new Mock<ILogger<ExceptionHandlingControllerBase>>();
            mockDashboardService = new Mock<IDashboardService>();
            dashboardController = new DashboardController(mockLogger.Object, mockLoggerBase.Object, mockDashboardService.Object);
        }

        [Test]
        public void getCriticalityDataTest_ReturnsOkResult()
        {
            // Arange
            List<Tuple<int, int>> data = new List<Tuple<int, int>>();
            mockDashboardService.Setup(service => service.GetCriticalityData()).ReturnsAsync(data);

            // Act
            var result = dashboardController.getCriticalityData().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);

        }


        [Test]
        public void getCriticalityDataTest_ReturnsErrorResult()
        {
            // Arange
            CustomException expectedException = new CustomException(StatusCodes.Status500InternalServerError, "Unexpected error while searching for data");
            mockDashboardService.Setup(service => service.GetCriticalityData()).ThrowsAsync(expectedException);

            // Act
            var result = dashboardController.getCriticalityData().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ObjectResult), result.GetType());
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 500);

        }

        [Test]
        public void getVulnerabilityDataTest_ReturnsOkResult()
        {
            // Arange
            List<Tuple<int, int>> data = new List<Tuple<int, int>>();
            mockDashboardService.Setup(service => service.GetVulnerabilityData()).ReturnsAsync(data);

            // Act
            var result = dashboardController.getVulnerabilityData().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(OkObjectResult), result.GetType());
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 200);

        }


        [Test]
        public void getVulnerabilityDataTest_ReturnsErrorResult()
        {
            // Arange
            CustomException expectedException = new CustomException(StatusCodes.Status500InternalServerError, "Unexpected error while searching for data");
            mockDashboardService.Setup(service => service.GetVulnerabilityData()).ThrowsAsync(expectedException);

            // Act
            var result = dashboardController.getVulnerabilityData().Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ObjectResult), result.GetType());
            Assert.IsTrue(result is ObjectResult objectResult && objectResult.StatusCode == 500);

        }
    }
}