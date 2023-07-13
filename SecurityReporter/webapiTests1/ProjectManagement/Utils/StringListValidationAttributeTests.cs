using NUnit.Framework;

namespace Tests
{
    [TestFixture()]
    public class StringListValidationAttributeTests
    {
    
        [Test]
        public void ValidStringList_WithNonEmptyStrings()
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
        public void InvalidStringList_WithEmptyList()
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
        public void InvalidStringList_WithEmptyStrings()
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
        public void ValidStringList_WithNullValues()
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
        public void ValidStringList_Null()
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
}