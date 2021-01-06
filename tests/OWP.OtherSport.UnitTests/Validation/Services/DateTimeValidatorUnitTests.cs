namespace OWP.OtherSport.UnitTests.Validation.Services
{
    using OWP.OtherSport.Validation.Models;
    using OWP.OtherSport.Validation.Services;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class DateTimeValidatorUnitTests
    {
        private readonly DateTimeValidatorImp dateTimeValidator;

        public DateTimeValidatorUnitTests()
        {
            dateTimeValidator = new DateTimeValidatorImp();
        }

        [Fact]
        public void Validate_ValidDateTime_ReturnAssignmentValidationRowResultIsEmpty()
        {
           var assignments = new List<AssignmentModelInExcel>
           {
              new AssignmentModelInExcel
              {
                  Away = "OKC",
                  Home = "Toronto Raptor",
                  League = "NBA",
                  Sport = "tennis",
                  DateTime = "Feb 21, 2020 02:00 PM"
              }             
           };
          
            var expected = new List<AssignmentValidationRowResult>();
           
            var actual =  dateTimeValidator.Validate(assignments);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void Validate_InValidDateTime_ReturnAssignmentValidationRowResult()
        {
            var assignments = new List<AssignmentModelInExcel>
           {
              new AssignmentModelInExcel
              {
                  Away = "OKC",
                  Home = "Toronto Raptor",
                  League = "NBA",
                  Sport = "tennis",
                  DateTime = "Feb 32, 2020 02:00 PM"
              }
           };

            var expected = new List<AssignmentValidationRowResult>
            {
                new AssignmentValidationRowResult
                {
                    Message = "[Column: Date Time] - Error: String 'Feb 32, 2020 02:00 PM' was not recognized as a valid DateTime.",
                    Row = 2
                }
            };

            var actual = dateTimeValidator.Validate(assignments);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void Validate_InValidDateTimeFormat_ReturnAssignmentValidationRowResult()
        {
            var assignments = new List<AssignmentModelInExcel>
           {
              new AssignmentModelInExcel
              {
                  Away = "OKC",
                  Home = "Toronto Raptor",
                  League = "NBA",
                  Sport = "tennis",
                  DateTime = "02 32, 2020 02:00 PM"
              }
           };

            var expected = new List<AssignmentValidationRowResult>
            {
                new AssignmentValidationRowResult
                {
                    Message = "[Column: Date Time] - Error: Invalid date time format",
                    Row = 2
                }
            };

            var actual = dateTimeValidator.Validate(assignments);
            DeepAssert.Equal(expected, actual);
        }
    }
}
