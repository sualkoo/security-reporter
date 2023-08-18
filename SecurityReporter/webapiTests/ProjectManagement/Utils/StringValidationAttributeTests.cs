using NUnit.Framework;
using webapi.Utils;

namespace webapiTests.ProjectManagement.Utils;

[TestFixture]
public class StringValidationAttributeTests
{
    [Test]
    public void IsValid_ValidValueIsNull_ShouldPassValidation()
    {
        // Arrange
        string value = null;
        var attribute = new StringValidationAttribute();

        // Act
        var isValid = attribute.IsValid(value);

        // Assert
        Assert.IsTrue(isValid);
    }

    [Test]
    public void IsValid_ValidValueIsNonEmptyString_ShouldPassValidation()
    {
        // Arrange
        const string value = "Test";
        var attribute = new StringValidationAttribute();

        // Act
        var isValid = attribute.IsValid(value);

        // Assert
        Assert.IsTrue(isValid);
    }

    [Test]
    public void IsValid_InvalidValueIsEmptyString_ShouldFailValidation()
    {
        // Arrange
        const string value = "";
        var attribute = new StringValidationAttribute();

        // Act
        var isValid = attribute.IsValid(value);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void IsValid_InvalidValueIsWhitespaceString_ShouldFailValidation()
    {
        // Arrange
        const string value = "   ";
        var attribute = new StringValidationAttribute();

        // Act
        var isValid = attribute.IsValid(value);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void IsValid_ValidValueIsNonString_ShouldPassValidation()
    {
        // Arrange
        object value = 42;
        var attribute = new StringValidationAttribute();

        // Act
        var isValid = attribute.IsValid(value);

        // Assert
        Assert.IsTrue(isValid);
    }

    [Test]
    public void IsValid_ValidValueIsValidString_ShouldPassValidation()
    {
        // Arrange
        const string value = "Name of this project";
        var attribute = new StringValidationAttribute();

        // Act
        var isValid = attribute.IsValid(value);

        // Assert
        Assert.IsTrue(isValid);
    }
}