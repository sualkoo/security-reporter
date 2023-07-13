using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture()]
    public class EmailListValidationAttributeTests
    {
        [Test()]
        public void NullListTest()
        {
            // Arrange
            var attribute = new EmailListValidationAttribute();
            List<string> emailList = null;

            // Act
            var isValid = attribute.IsValid(emailList);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test()]
        public void ValidEmailsTest()
        {
            // Arrange
            var attribute = new EmailListValidationAttribute();
            var emailList = new List<string> { "test1@example.com", "test2@example.com", "test3@example.com" };

            // Act
            var isValid = attribute.IsValid(emailList);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test()]
        public void EmptyListTest()
        {
            // Arrange
            var attribute = new EmailListValidationAttribute();
            var emailList = new List<string> {};

            // Act
            var isValid = attribute.IsValid(emailList);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test()]
        public void EmptyStringTest()
        {
            // Arrange
            var attribute = new EmailListValidationAttribute();
            var emailList = new List<string> { "test1@example.com", "", "test3@example.com" };

            // Act
            var isValid = attribute.IsValid(emailList);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test()]
        public void WhiteSpacesTest()
        {
            // Arrange
            var attribute = new EmailListValidationAttribute();
            var emailList = new List<string> { "test1@example.com", "       ", "test3@example.com" };

            // Act
            var isValid = attribute.IsValid(emailList);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test()]
        public void InvalidEmailFormatTest()
        {
            // Arrange
            var attribute = new EmailListValidationAttribute();
            var emailList = new List<string> { "test1@example.com", "test3.com" };

            // Act
            var isValid = attribute.IsValid(emailList);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test()]
        public void NullStringTest()
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
}