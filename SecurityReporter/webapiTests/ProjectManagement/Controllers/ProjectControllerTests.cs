using Microsoft.VisualStudio.TestTools.UnitTesting;
using webapi.ProjectManagement.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using webapi.Service;
using NUnit.Framework;
using webapi.Models;
using webapi.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace webapi.ProjectManagement.Controllers.Tests
{
    [TestFixture]
    public class ProjectControllerTests
    {
        private ProjectController _projectController;
        private Mock<ICosmosService> _cosmosServiceMock;

        [SetUp]
        public void Setup()
        {
            _cosmosServiceMock = new Mock<ICosmosService>();
            _projectController = new ProjectController(_cosmosServiceMock.Object);
        }

        [Test]
        public async Task GetProjectById_ExistingId_ReturnsProjectData()
        {
            // Arrange
            var projectData = new ProjectData
            {
                id = Guid.NewGuid(),
                ProjectName = "Sample Project",
                ProjectStatus = ProjectStatus.InProgress,
                ProjectQuestionare = ProjectQuestionare.TBS,
                ProjectScope = ProjectScope.TBS,
                PentestDuration = 5,
                StartDate = new DateOnly(2023, 8, 1),
                EndDate = new DateOnly(2023, 8, 10),
                IKO = new DateOnly(2023, 7, 20),
                TKO = new DateOnly(2023, 8, 5),
                ReportDueDate = new DateOnly(2023, 8, 15),
                RequestCreated = DateTime.Now,
                Comments = new List<Comment>
                {
                    new Comment { Text = "Comment 1", Author = "User1", CreatedAt = DateTime.Now },
                    new Comment { Text = "Comment 2", Author = "User2", CreatedAt = DateTime.Now },
                },
                CatsNumber = "12345",
                ProjectOfferStatus = ProjectOfferStatus.OfferSentForSignatue,
                PentestAspects = "Aspect1, Aspect2, Aspect3",
                WorkingTeam = new List<string> { "John Doe", "Jane Smith" },
                ProjectLead = "John Doe",
                ReportStatus = "In Progress",
                ContactForClients = new List<string> { "client1@example.com", "client2@example.com" },
            };

            // Act
            _cosmosServiceMock.Setup(x => x.AddProject(projectData)).ReturnsAsync(true);
            IActionResult postResult = await _projectController.PostProject(projectData);

            _cosmosServiceMock.Setup(x => x.GetProjectById(projectData.id.ToString())).ReturnsAsync(projectData);
            var getResult = await _projectController.GetProjectById(projectData.id.ToString());

            //Assert
            NUnit.Framework.Assert.IsNotNull(getResult);
            NUnit.Framework.Assert.IsInstanceOf<ObjectResult>(getResult);

            var objectResult = getResult as ObjectResult;
            NUnit.Framework.Assert.AreEqual(StatusCodes.Status200OK, objectResult.StatusCode);
            NUnit.Framework.Assert.IsNotNull(objectResult.Value);
            NUnit.Framework.Assert.IsInstanceOf<ProjectData>(objectResult.Value);

            var returnedProject = objectResult.Value as ProjectData;
        }

        [Test]
        public async Task GetProjectById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var projectId = "05658ebe-0ddd-8975-83cf-g4561b843598"; 
            ProjectData nullProject = null;

            _cosmosServiceMock.Setup(x => x.GetProjectById(projectId)).ReturnsAsync(nullProject);

            // Act
            var actionResult = await _projectController.GetProjectById(projectId);

            // Assert
            NUnit.Framework.Assert.IsNotNull(actionResult);
            NUnit.Framework.Assert.IsInstanceOf<ObjectResult>(actionResult);

            var objectResult = actionResult as ObjectResult;
            NUnit.Framework.Assert.AreEqual(StatusCodes.Status404NotFound, objectResult.StatusCode);
            NUnit.Framework.Assert.AreEqual($"Project with ID '{projectId}' not found.", objectResult.Value);
        }
    }
}
