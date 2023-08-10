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
using Assert = NUnit.Framework.Assert;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace webapi.ProjectManagement.Controllers.Tests
{
    [TestFixture]
    public class ProjectControllerTests
    {
        private ProjectController projectController;
        private Mock<ICosmosService> cosmosServiceMock;

        public ProjectData newProject = new ProjectData
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

        [SetUp]
        public void Setup()
        {
            cosmosServiceMock = new Mock<ICosmosService>();
            projectController = new ProjectController(cosmosServiceMock.Object);
        }

        [Test]
        public async Task GetProjectById_ExistingId_ReturnsProjectData()
        {
            // Arrange
            var projectData = newProject;

            // Act
            cosmosServiceMock.Setup(x => x.AddProject(projectData)).ReturnsAsync(true);
            IActionResult postResult = await projectController.PostProject(projectData);

            cosmosServiceMock.Setup(x => x.GetProjectById(projectData.id.ToString())).ReturnsAsync(projectData);
            var getResult = await projectController.GetProjectById(projectData.id.ToString());

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

            cosmosServiceMock.Setup(x => x.GetProjectById(projectId)).ReturnsAsync(nullProject);

            // Act
            var actionResult = await projectController.GetProjectById(projectId);

            // Assert
            NUnit.Framework.Assert.IsNotNull(actionResult);
            NUnit.Framework.Assert.IsInstanceOf<ObjectResult>(actionResult);

            var objectResult = actionResult as ObjectResult;
            NUnit.Framework.Assert.AreEqual(StatusCodes.Status404NotFound, objectResult.StatusCode);
            NUnit.Framework.Assert.AreEqual($"Project with ID '{projectId}' not found.", objectResult.Value);
        }

        [Test]
        public async Task UpdateProject_UpdateName_Success()
        {
            // Arrange
            var newName = "updated";
            var updatedProject = newProject;
            updatedProject.ProjectName = newName;

            // Act
            cosmosServiceMock.Setup(x => x.AddProject(newProject)).ReturnsAsync(true);
            IActionResult postResult = await projectController.PostProject(newProject);

            cosmosServiceMock.Setup(x => x.UpdateProject(updatedProject)).ReturnsAsync(true);
            IActionResult updateResult = await projectController.UpdateProject(updatedProject);

            cosmosServiceMock.Setup(x => x.GetProjectById(newProject.id.ToString())).ReturnsAsync(updatedProject);
            var getResult = await projectController.GetProjectById(newProject.id.ToString());

            var objectResult = getResult as ObjectResult;
            var project = objectResult.Value as ProjectData;
            string projectName = project.ProjectName;

            // Assert
            Assert.AreEqual(newName, projectName);
        }

        [Test]
        public async Task UpdateProject_NotExistingProject_Fail()
        {
            // Act
            cosmosServiceMock.Setup(x => x.UpdateProject(newProject)).ReturnsAsync(false);
            var updateResult = await projectController.UpdateProject(newProject) as ObjectResult;

            // Assert
            Assert.AreEqual(400, updateResult.StatusCode);
        }

        [Test]
        public async Task AddProject_Success()
        {
            // Arrange

            // Act
            cosmosServiceMock.Setup(x => x.AddProject(newProject)).ReturnsAsync(true);
            IActionResult postResult = await projectController.PostProject(newProject);


            // Assert
            Assert.AreEqual(201, ((IStatusCodeActionResult)postResult).StatusCode);
        }
    }
}
