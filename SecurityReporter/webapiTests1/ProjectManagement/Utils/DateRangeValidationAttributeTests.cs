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
            [TKOValidation(nameof(IKO))]
            [IKOValidation(nameof(TKO))]
            public DateOnly EndDate { get; set; }

            public DateOnly IKO { get; set; }

            public DateOnly TKO { get; set; }

            [DateRangeValidation(nameof(EndDate))]
            public DateOnly RequestDue { get; set; }
        }

        [Test]
        public void IsValid_ValidDateRange_ValidationPasses()
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
        public void IsValid_EqualStartAndEndDates_ValidationPasses()
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
        public void IsValid_EqualIKOAndEndDate_ValidationPasses()
        {
            // Arrange
            var model = new TestModel
            {
                EndDate = new DateOnly(2023, 1, 1),
                IKO = new DateOnly(2023, 1, 1)
            };

            var validationContext = new ValidationContext(model);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
        }


        [Test]
        public void IsValid_EqualTKOAndEndDate_ValidationPasses()
        {
            // Arrange
            var model = new TestModel
            {
                EndDate = new DateOnly(2023, 1, 1),
                TKO = new DateOnly(2023, 1, 1)
            };

            var validationContext = new ValidationContext(model);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsValid_InvalidDateRange_ValidationFails()
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
        }

        [Test]
        public void IsValid_NullValues_ValidationPasses()
        {
            // Arrange
            var model = new TestModel
            {
                StartDate = new DateOnly(0001, 1, 1),
                EndDate = new DateOnly(0001, 1, 1),
                IKO = new DateOnly(0001, 1, 1),
                TKO = new DateOnly(0001, 1, 1),
                RequestDue = new DateOnly(0001, 1, 1)

            };

            var validationContext = new ValidationContext(model);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void IsValid_ValidIKO_ValidationPasses()
        {
            // Arrange
            var model = new TestModel
            {
                IKO = new DateOnly(2023, 1, 1),
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
        public void IsValid_ValidTKO_ValidationPasses()
        {
            // Arrange
            var model = new TestModel
            {
                TKO = new DateOnly(2023, 1, 1),
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
        public void IsValid_InvalidIKO_ValidationFails()
        {
            // Arrange
            var model = new TestModel
            {
                IKO = new DateOnly(2023, 2, 1),
                EndDate = new DateOnly(2023, 1, 1)
            };

            var validationContext = new ValidationContext(model);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
          
        }

        [Test]
        public void IsValid_InvalidTKO_ValidationFails()
        {
            // Arrange
            var model = new TestModel
            {
                TKO = new DateOnly(2023, 2, 1),
                EndDate = new DateOnly(2023, 1, 1)
            };

            var validationContext = new ValidationContext(model);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);
           
        }

        [Test]
        public void IsValid_ValidRequestDue_ValidationPasses()
        {
            // Arrange
            var model = new TestModel
            {
                RequestDue = new DateOnly(2023, 2, 1),
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
        public void IsValid_InvalidRequestDue_ValidationFails()
        {
            // Arrange
            var model = new TestModel
            {
                RequestDue = new DateOnly(2023, 1, 1),
                EndDate = new DateOnly(2023, 2, 1)
            };

            var validationContext = new ValidationContext(model);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(model, validationContext, validationResults, true);

            // Assert
            Assert.IsFalse(isValid);

        }
    }

}