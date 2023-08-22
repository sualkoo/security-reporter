using System;
using System.Collections.Generic;
using NUnit.Framework;
using webapi.Models;
using webapi.Utils;

namespace webapiTests.ProjectManagement.Utils;

[TestFixture]
public class CommentValidationAttributeTests
{
    [Test]
    public void IsValid_CommentListWithNullComment_ValidationFails()
    {
        // Arrange
        var attribute = new CommentValidationAttribute();
        var commentList = new List<Comment>
        {
            new()
            {
                Author = "John Doe",
                Text = "This is a random comment.",
                CreatedAt = DateTime.Now
            },
            null
        };

        // Act
        var isValid = attribute.IsValid(commentList);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void IsValid_EmptyCommentList_ValidationFails()
    {
        // Arrange
        var attribute = new CommentValidationAttribute();
        var commentList = new List<Comment>();

        // Act
        var isValid = attribute.IsValid(commentList);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void IsValid_ValidCommentList_ValidationPasses()
    {
        // Arrange
        var attribute = new CommentValidationAttribute();
        var commentList = new List<Comment>
        {
            new()
            {
                Author = "John Doe",
                Text = "This is a random comment.",
                CreatedAt = DateTime.Now
            },
            new()
            {
                Author = "Martin",
                Text = "This is a random comment.",
                CreatedAt = DateTime.Now
            }
        };

        // Act
        var isValid = attribute.IsValid(commentList);

        // Assert
        Assert.IsTrue(isValid);
    }

    [Test]
    public void IsValid_NullCommentList_ValidationPasses()
    {
        // Arrange
        var attribute = new CommentValidationAttribute();
        List<Comment> commentList = null;

        // Act
        var isValid = attribute.IsValid(commentList);

        // Assert
        Assert.IsTrue(isValid);
    }
}