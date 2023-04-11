using System.ComponentModel.DataAnnotations;

namespace APIMe.Utilities.Validators
{
    public class CustomValidationResult : ValidationResult
    {
        public IEnumerable<ValidationResult> ValidationResults { get; }

        public CustomValidationResult(string errorMessage, IEnumerable<ValidationResult> validationResults)
            : base(errorMessage)
        {
            ValidationResults = validationResults;
        }
    }

}
