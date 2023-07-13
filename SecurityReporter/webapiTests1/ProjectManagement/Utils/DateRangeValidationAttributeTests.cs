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
        public void ValidDateRangeTest()
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
        public void EqualStartAndEndDates()
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
        public void EqualIKOAndEndDate()
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
        public void EqualTKOAndEndDate()
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
        public void InvalidDateRange()
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
        public void NullValues()
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
        public void ValidIKOTest()
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
        public void ValidTKOTest()
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
        public void InvalidIKOTest()
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
        public void InvalidTKOTest()
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
        public void ValidRequestDueTest()
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
        public void InvalidRequestDueTest()
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