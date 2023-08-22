using System.Collections.Generic;
using NUnit.Framework;

namespace webapiTests.ProjectManagement.Utils;

[TestFixture]
public class EmailListValidationAttributeTests
{
    [Test]
    public void IsValid_NullList_ShouldPassValidation()
    {
        // Arrange
        var attribute = new EmailListValidationAttribute();
        List<string> emailList = null;

        // Act
        var isValid = attribute.IsValid(emailList);

        // Assert
        Assert.IsTrue(isValid);
    }

    [Test]
    public void IsValid_ValidEmails_ShouldPassValidation()
    {
        // Arrange
        var attribute = new EmailListValidationAttribute();
        var emailList = new List<string> { "test1@example.com", "test2@example.com", "test3@example.com" };

        // Act
        var isValid = attribute.IsValid(emailList);

        // Assert
        Assert.IsTrue(isValid);
    }

    [Test]
    public void IsValid_EmptyList_ShouldFailValidation()
    {
        // Arrange
        var attribute = new EmailListValidationAttribute();
        var emailList = new List<string>();

        // Act
        var isValid = attribute.IsValid(emailList);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void IsValid_EmptyString_ShouldFailValidation()
    {
        // Arrange
        var attribute = new EmailListValidationAttribute();
        var emailList = new List<string> { "test1@example.com", "", "test3@example.com" };

        // Act
        var isValid = attribute.IsValid(emailList);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void IsValid_WhiteSpaces_ShouldFailValidation()
    {
        // Arrange
        var attribute = new EmailListValidationAttribute();
        var emailList = new List<string> { "test1@example.com", "       ", "test3@example.com" };

        // Act
        var isValid = attribute.IsValid(emailList);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void IsValid_InvalidEmailFormat_ShouldFailValidation()
    {
        // Arrange
        var attribute = new EmailListValidationAttribute();
        var emailList = new List<string> { "test1@example.com", "test3.com" };

        // Act
        var isValid = attribute.IsValid(emailList);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void IsValid_NullString_ShouldFailValidation()
    {
        // Arrange
        var attribute = new EmailListValidationAttribute();
        var emailList = new List<string> { "test1@example.com", null };

        // Act
        var isValid = attribute.IsValid(emailList);

        // Assert
        Assert.IsFalse(isValid);
    }
}