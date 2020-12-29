namespace OWP.OtherSport.UnitTests.Validation.Helpers
{
    using OWP.OtherSport.Validation.Helpers;
    using OWP.OtherSport.Validation.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class ValidationHelperUnitTests
    {
       [Fact]

       public void BuildValidationRowResult_ValidationResult_ReturnValidation()
        {
            var assignmentValidationList = new List<AssignmentValidationRowResult>();          

            var validationResult = new List<ValidationResult>
            {
                new ValidationResult("Invalid Trader Baseball FT Run Line (HDP): Esther")
            };

            var expected = new List<AssignmentValidationRowResult>
            {
                new AssignmentValidationRowResult
                {
                    Row = 2,
                    Message = "Invalid Trader Baseball FT Run Line (HDP): Esther"
                }
            };

            ValidationHelper.BuildValidationRowResult(validationResult, assignmentValidationList, 2);

            DeepAssert.Equal(expected, assignmentValidationList);
        }

        [Fact]
        public void BuildValidationRowResult_EmptyValidation_ReturnValidationEqualZero()
        {
            var assignmentValidationList = new List<AssignmentValidationRowResult>();

            var validationResult = new List<ValidationResult>();

            var expected = new List<AssignmentValidationRowResult>();           

            ValidationHelper.BuildValidationRowResult(validationResult, assignmentValidationList, 0);

            DeepAssert.Equal(expected, assignmentValidationList);
        }
    }
}
