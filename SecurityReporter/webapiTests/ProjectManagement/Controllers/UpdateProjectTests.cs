using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Moq;
using webapi.Models;
using webapi.Service;
using webapi.Enums;

namespace webapi.ProjectManagement.Controllers.Tests
{
    [TestFixture]
    public class UpdateProjectTests
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
    }
}
