using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace webapi.ProjectSearch.Models
{
    [Serializable]
    public class EntityValidationResult
    {
        public IList<ValidationResult> ValidationErrors { get; private set; }
        public bool HasError
        {
            get { return ValidationErrors.Count > 0; }
        }

        public EntityValidationResult(IList<ValidationResult> violations = null)
        {
            ValidationErrors = violations ?? new List<ValidationResult>();
        }
    }

    public class DataAnnotation
    {
        public static EntityValidationResult ValidateEntity<T>(T entity) where T : IEntity
        {
            return new EntityValidator<T>().Validate(entity);
        }
        public static EntityValidationResult ValidateList<T>(IEnumerable<T> entities) where T : IEntity
        {
            return new EntityValidator<T>().ValidateList(entities);
        }
        public static EntityValidationResult ValidateTimeFrames<T>(T entity) where T : IEntity
        {
            return new EntityValidator<T>().ValidateTimeFrames(entity);
        }
    }

    public class EntityValidator<T> where T : IEntity
    {
        public EntityValidationResult Validate(T entity)
        {
            var validationResults = new List<ValidationResult>();
            var vc = new ValidationContext(entity, null, null);
            var isValid = Validator.TryValidateObject(entity, vc, validationResults, true);

            return new EntityValidationResult(validationResults);
        }
        public EntityValidationResult ValidateList(IEnumerable<T> entities)
        {
            var validationResults = new List<ValidationResult>();
            if (entities == null)
            {

            }
            else
            {
                foreach (var entity in entities)
                {
                    var entityValidationResult = Validate(entity);
                    validationResults.AddRange(entityValidationResult.ValidationErrors);
                }
            }
            return new EntityValidationResult(validationResults);
        }

        public EntityValidationResult ValidateTimeFrames(T entity)
        {
            var validationResults = new List<ValidationResult>();

            var timeFrameStartProperty = typeof(T).GetProperty("TimeFrameStart");
            var timeFrameEndProperty = typeof(T).GetProperty("TimeFrameEnd");
            var timeFrameReportDueProperty = typeof(T).GetProperty("TimeFrameReportDue");

            if (timeFrameStartProperty == null || timeFrameEndProperty == null || timeFrameReportDueProperty == null)
            {
                throw new ArgumentException("The entity does not have the required properties for time frames validation.");
            }

            var timeFrameStartValue = (DateTime)timeFrameStartProperty.GetValue(entity);
            var timeFrameEndValue = (DateTime)timeFrameEndProperty.GetValue(entity);
            var timeFrameReportDueValue = (DateTime)timeFrameReportDueProperty.GetValue(entity);

            if (timeFrameStartValue > timeFrameEndValue || timeFrameStartValue > timeFrameReportDueValue)
            {
                validationResults.Add(new ValidationResult("Invalid time frames."));
            }

            return new EntityValidationResult(validationResults);
        }
    }
}
