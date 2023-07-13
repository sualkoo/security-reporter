using NUnit.Framework;
using webapi.Utils;

[TestFixture]
public class StringValidationAttributeTests
{
    [Test]
    public void ValidValue_Null()
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
    public void ValidValue_NonEmptyString()
    {
        // Arrange
        string value = "Test";
        var attribute = new StringValidationAttribute();

        // Act
        var isValid = attribute.IsValid(value);

        // Assert
        Assert.IsTrue(isValid);
    }

    [Test]
    public void InvalidValue_EmptyString()
    {
        // Arrange
        string value = "";
        var attribute = new StringValidationAttribute();

        // Act
        var isValid = attribute.IsValid(value);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void InvalidValue_WhitespaceString()
    {
        // Arrange
        string value = "   ";
        var attribute = new StringValidationAttribute();

        // Act
        var isValid = attribute.IsValid(value);

        // Assert
        Assert.IsFalse(isValid);
    }

    [Test]
    public void ValidValue_NonString()
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
    public void ValidValue_ValidString()
    {
        // Arrange
        string value = "Name of this project";
        var attribute = new StringValidationAttribute();

        // Act
        var isValid = attribute.IsValid(value);

        // Assert
        Assert.IsTrue(isValid);
    }
}
