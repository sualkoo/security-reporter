using NUnit.Framework;
using webapi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace webapi.Utils.Tests
{
    [TestFixture]
    public class DateRangeValidationAttributeTests
    {
        public class TestModel
        {
            public DateOnly StartDate { get; set; }
            
            [DateRangeValidation(nameof(StartDate))]
            public DateOnly EndDate { get; set; }
        }

        [Test]
        public void ValidRangeTest()
        {
            // Arrange
            var model = new TestModel
            {
                StartDate = new DateOnly(2023, 1, 1),
                EndDate = new DateOnly(2023, 2, 1)
            };

            var validationContext = new ValidationContext(model);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void EqualDates()
        {
            // Arrange
            var model = new TestModel
            {
                StartDate = new DateOnly(2023, 1, 1),
                EndDate = new DateOnly(2023, 1, 1)
            };

            var validationContext = new ValidationContext(model);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void InvalidRange()
        {
            // Arrange
            var model = new TestModel
            {
                StartDate = new DateOnly(2023, 2, 1),
                EndDate = new DateOnly(2023, 1, 1)
            };

            var validationContext = new ValidationContext(model);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual(1, validationResults.Count);
            Assert.AreEqual("End Date must be greater than or equal to Start Date.", validationResults[0].ErrorMessage);
        }

        [Test]
        public void NullValues()
        {
            // Arrange
            var model = new TestModel
            {
                StartDate = new DateOnly(0001,1,1),
                EndDate = new DateOnly(0001, 1, 1)
            };

            var validationContext = new ValidationContext(model);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}