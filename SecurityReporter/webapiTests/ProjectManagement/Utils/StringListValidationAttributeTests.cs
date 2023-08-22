using System.Collections.Generic;
using NUnit.Framework;

namespace webapiTests.ProjectManagement.Utils;

[TestFixture]
public class StringListValidationAttributeTests
{
    [Test]
    public void IsValid_ValidStringListWithNonEmptyStrings_ShouldPassValidation()
    {
        // Arrange
        var stringList = new List<string> { "Test", "Example" };
        var attribute = new StringListValidationAttribute();

        // Act
        var isValid = attribute.IsValid(stringList);

        // Assert
        Assert.IsTrue(isValid);
    }

    [Test]
    public void IsValid_InvalidStringListWithEmptyList_ShouldFailValidation()
    {
        // Arrange
        var stringList = new List<string>();
        var attribute = new StringListValidationAttribute();

        // Act
        var isValid = attribute.IsValid(stringList);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void IsValid_InvalidStringListWithEmptyStrings_ShouldFailValidation()
    {
        // Arrange
        var stringList = new List<string> { "", "   ", "Test" };
        var attribute = new StringListValidationAttribute();

        // Act
        var isValid = attribute.IsValid(stringList);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void IsValid_ValidStringListWithNullValues_ShouldFailValidation()
    {
        // Arrange
        var stringList = new List<string> { "Test", null, "Example" };
        var attribute = new StringListValidationAttribute();

        // Act
        var isValid = attribute.IsValid(stringList);

        // Assert
        Assert.IsFalse(isValid);
    }


    [Test]
    public void IsValid_ValidStringListIsNull_ShouldPassValidation()
    {
        // Arrange
        List<string> stringList = null;
        var attribute = new StringListValidationAttribute();

        // Act
        var isValid = attribute.IsValid(stringList);

        // Assert
        Assert.IsTrue(isValid);
    }
}