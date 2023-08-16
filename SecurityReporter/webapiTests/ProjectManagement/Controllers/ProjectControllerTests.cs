﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using System.Collections;

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

        public ProjectList projectListData = new ProjectList
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
            Comments = new List<Comment>
                {
                    new Comment { Text = "Comment 1", Author = "User1", CreatedAt = DateTime.Now },
                    new Comment { Text = "Comment 2", Author = "User2", CreatedAt = DateTime.Now },
                },
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

        [Test]
        public async Task DeleteProject_Existing_Success()
        {
            // Arrange
            List<string> idList = new List<string>() { newProject.id.ToString() };

            // Act
            cosmosServiceMock.Setup(x => x.DeleteProjects(idList)).ReturnsAsync(new List<string>());
            IActionResult postResult = await projectController.DeleteProject(idList);

            // Assert
            Assert.AreEqual(200, ((IStatusCodeActionResult)postResult).StatusCode);
        }

        [Test]
        public async Task DeleteProject_NotExisting_Fail()
        {
            // Arrange
            List<string> idList = new List<string>() { newProject.id.ToString() };

            // Act
            cosmosServiceMock.Setup(x => x.DeleteProjects(idList))
                            .ReturnsAsync(new List<string> { $"{newProject.id}, Not found." }); // Mock failed deletion

            IActionResult postResult = await projectController.DeleteProject(idList);

            // Assert
            Assert.AreEqual(404, ((IStatusCodeActionResult)postResult).StatusCode);
        }

        [Test]
        public async Task GetNumberOfProjects_NonEmpty_Success()
        {
            // Arrange

            // Act
            cosmosServiceMock.Setup(x => x.GetNumberOfProjects()).ReturnsAsync(1);
            IActionResult postResult = await projectController.GetNumberOfProjects();

            // Assert
            Assert.AreEqual(200, ((IStatusCodeActionResult)postResult).StatusCode);
            
            var resultContent = (postResult as ObjectResult)?.Value?.ToString();
            Assert.AreEqual(resultContent, "1");
        }

        [Test]
        public async Task GetNumberOfProjects_Empty_Success()
        {
            // Arrange

            // Act
            cosmosServiceMock.Setup(x => x.GetNumberOfProjects()).ReturnsAsync(0);
            IActionResult postResult = await projectController.GetNumberOfProjects();

            // Assert
            Assert.AreEqual(200, ((IStatusCodeActionResult)postResult).StatusCode);

            var resultContent = (postResult as ObjectResult)?.Value?.ToString();
            Assert.AreEqual(resultContent, "0");
        }

        [Test]
        public async Task GetProjectById_Existing_Success()
        {
            // Arrange

            // Act
            cosmosServiceMock.Setup(x => x.GetProjectById(newProject.id.ToString())).ReturnsAsync(newProject);
            IActionResult postResult = await projectController.GetProjectById(newProject.id.ToString());

            // Assert
            Assert.AreEqual(200, ((IStatusCodeActionResult)postResult).StatusCode);

            var resultContent = (postResult as ObjectResult)?.Value?.ToString();
            Assert.AreEqual(resultContent, newProject.ToString());
        }

        [Test]
        public async Task GetProjectById_NotExisting_Fail()
        {
            // Arrange

            // Act
            cosmosServiceMock.Setup(x => x.GetProjectById(newProject.id.ToString())).ReturnsAsync((ProjectData)null);
            IActionResult postResult = await projectController.GetProjectById(newProject.id.ToString());

            // Assert
            Assert.AreEqual(404, ((IStatusCodeActionResult)postResult).StatusCode);

            var resultContent = (postResult as ObjectResult)?.Value?.ToString();
            Assert.AreEqual(resultContent, $"Project with ID '{newProject.id.ToString()}' not found.");
        }

        [Test]
        public async Task GetItems_WithData_Returns200AndData()
        {
            // Arrange
            var pageSize = 10;
            var pageNumber = 1;
            var filter = new FilterData();
            var sort = new SortData();

            var mockItems = new List<ProjectList>
            {
                projectListData
            };

            cosmosServiceMock.Setup(x => x.GetItems(pageSize, pageNumber, filter, sort))
                            .ReturnsAsync(mockItems);

            // Act
            IActionResult result = await projectController.GetItems(pageSize, pageNumber, filter, sort);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;

            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.AreEqual(mockItems, objectResult.Value);
        }

        [Test]
        public async Task GetItems_NoData_Returns204AndMessage()
        {
            // Arrange
            var pageSize = 10;
            var pageNumber = 1;
            var filter = new FilterData();
            var sort = new SortData();

            cosmosServiceMock.Setup(x => x.GetItems(pageSize, pageNumber, filter, sort))
                            .ReturnsAsync(new List<ProjectList>());

            // Act
            IActionResult result = await projectController.GetItems(pageSize, pageNumber, filter, sort);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;

            Assert.AreEqual(204, objectResult.StatusCode);
            Assert.AreEqual("No data available.", objectResult.Value);
        }

        [Test]
        public async Task GetItems_Exception_Returns400AndErrorMessage()
        {
            // Arrange
            var pageSize = 10;
            var pageNumber = 1;
            var filter = new FilterData();
            var sort = new SortData();

            var errorMessage = "An error occurred.";

            cosmosServiceMock.Setup(x => x.GetItems(pageSize, pageNumber, filter, sort))
                            .ThrowsAsync(new Exception(errorMessage));

            // Act
            IActionResult result = await projectController.GetItems(pageSize, pageNumber, filter, sort);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;

            Assert.AreEqual(400, objectResult.StatusCode);
            Assert.AreEqual($"Error retrieving data: {errorMessage}", objectResult.Value);
        }

        [Test]
        public async Task GetItems_Existing_Returns200AndAppliedFilter()
        {
            // Arrange
            var pageSize = 10;
            var pageNumber = 1;
            var filter = new FilterData();
            filter.FilteredProjectName = "Sample Project";
            var sort = new SortData();

            var mockItems = new List<ProjectList>
            {
                projectListData
            };

            cosmosServiceMock.Setup(x => x.GetItems(pageSize, pageNumber, filter, sort))
                            .ReturnsAsync(mockItems);

            // Act
            IActionResult result = await projectController.GetItems(pageSize, pageNumber, filter, sort);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;

            Assert.AreEqual(200, objectResult.StatusCode);
            Assert.AreEqual(mockItems, objectResult.Value);
        }

        [Test]
        public async Task GetItems_NonEmpty_Returns204AndMessageWithFilterApplied()
        {
            // Arrange
            var pageSize = 10;
            var pageNumber = 1;
            var filter = new FilterData();
            filter.FilteredProjectStatus = ProjectStatus.Finished;
            var sort = new SortData();

            var mockItems = new List<ProjectList>
            {
                projectListData
            };

            cosmosServiceMock.Setup(x => x.GetItems(pageSize, pageNumber, filter, sort))
                            .ReturnsAsync(new List<ProjectList>());

            // Act
            IActionResult result = await projectController.GetItems(pageSize, pageNumber, filter, sort);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;

            Assert.AreEqual(204, objectResult.StatusCode);
            Assert.AreEqual("No data available.", objectResult.Value);
        }
    }
}
