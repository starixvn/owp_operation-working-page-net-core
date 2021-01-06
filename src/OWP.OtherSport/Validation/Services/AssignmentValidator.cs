namespace OWP.OtherSport.Validation.Services
{
    using OWP.OtherSport.Validation.Exceptions;
    using OWP.OtherSport.Validation.Helpers;
    using OWP.OtherSport.Validation.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    public interface AssignmentValidator
    {
        void Validate(UploadingDataFromExcel rawDataFromExcel,
            List<Trader> traders, 
            List<AssignmentValidationRowResult> dateTimeValidationResults);

        void ValidateTraders(AssignmentModelInExcel assignment, List<ValidationResult> results, List<Trader> traders);
    }

    public class AssignmentValidatorImp : AssignmentValidator
    {
        public void Validate(UploadingDataFromExcel rawDataFromExcel, List<Trader> traders, List<AssignmentValidationRowResult> dateTimeValidationResults)
        {
            var validationResults = new List<AssignmentValidationRowResult>();

            for (var i = 0; i < rawDataFromExcel.Assignments.Count; i++)
            {
                var rowIndex = i;
                var assignments = rawDataFromExcel.Assignments.ToList();
                assignments.RemoveAt(i); //Remove match at i for duplicate matches validation

                var validationResultsOfAssignment = CustomValidateAssignment(rawDataFromExcel.Assignments[i], assignments, traders);

                ValidationHelper.BuildValidationRowResult(
                   validationResultsOfAssignment,
                   validationResults,
                   rowIndex);
            }

            validationResults.AddRange(dateTimeValidationResults);

            var hasError = validationResults.Any();

            if (hasError)
            {
                throw new InvalidAssignmentRowException(validationResults);
            }
        }

        public List<ValidationResult> CustomValidateAssignment(AssignmentModelInExcel assignment, List<AssignmentModelInExcel> assigments, List<Trader> traders)
        {
            var results = new List<ValidationResult>();
            ValidateTraders(assignment, results, traders);
            ValidateDataAnotation(assignment, results);
            ValidateDuplicatedMatch(assignment, assigments, results);

            return results;
        }

        public void ValidateTraders(AssignmentModelInExcel assignment, List<ValidationResult> results, List<Trader> traders)
        {
            var invalidTraders = traders.Where(t => t.UserId == 0).Select(t => t.Username).ToList();

            if (invalidTraders.Count > 0)
            {
                var error = ValidateInvalidTrader(assignment, invalidTraders);

                if (error.Count > 0)
                {
                    var invalidTradersInString = string.Join("; ", error);
                    results.Add(new ValidationResult("Invalid " + invalidTradersInString));
                }
            }
        }

        public static List<string> ValidateInvalidTrader(AssignmentModelInExcel assignment, List<string> invalidTraders)
        {
            var errorList = new List<string>();
            var properties = typeof(AssignmentModelInExcel).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.StartsWith("Trader", StringComparison.CurrentCultureIgnoreCase))
                {
                    var property = GetProperty(assignment, prop.Name);
                    var trader = property.GetValue(assignment)?.ToString() ?? string.Empty;
                    CheckTrader(errorList, invalidTraders, trader, PropertyHelper.GetDisplayName(prop));
                }

                if (prop.Name.StartsWith("OtherTrader", StringComparison.CurrentCultureIgnoreCase))
                {
                    var property = GetProperty(assignment, prop.Name);
                    var trader = property.GetValue(assignment)?.ToString() ?? string.Empty;
                    CheckOtherTrader(errorList, invalidTraders, trader, PropertyHelper.GetDisplayName(prop));
                }
            }

            return errorList;
        }

        private static PropertyInfo GetProperty(AssignmentModelInExcel model, string propertyName)
            => model.GetType().GetProperties().Single(p => p.Name == propertyName);

        public static void CheckTrader(List<string> errorList, List<string> invalidTraders, string trader, string name)
        {
            if (invalidTraders.Contains(trader))
            {
                errorList.Add($"{name}: {trader}");
            }
        }

        public static void CheckOtherTrader(List<string> errorList, List<string> invalidTraders, string trader, string name)
        {
            var regex = new Regex(@"#(.*?)(\s|$)");
            var traderName = regex.Match(trader).Groups[1].Value;
            if (invalidTraders.Contains(traderName))
            {
                errorList.Add($"{name}: {traderName}");
            }
        }

        public static void ValidateDataAnotation(AssignmentModelInExcel assignment, List<ValidationResult> results)
        {
            var context = new ValidationContext(assignment, null, null);
            Validator.TryValidateObject(assignment, context, results, true);
        }

        private static void ValidateDuplicatedMatch(AssignmentModelInExcel assignment, List<AssignmentModelInExcel> assignments, List<ValidationResult> results)
        {
            var isDuplicated = assignments.Any(x => IsEquals(x.DateTime, assignment.DateTime) &&
                                                    IsEquals(x.League, assignment.League) &&
                                                    IsEquals(x.Home, assignment.Home) &&
                                                    IsEquals(x.Away, assignment.Away));

            if (isDuplicated)
            {
                results.Add(new ValidationResult($"Duplicate Match: {assignment.League} | {assignment.Home} -vs- {assignment.Away}"));
            }
        }

        private static bool IsEquals(string source, string value)
        {
            return !string.IsNullOrWhiteSpace(source) &&
                source.Equals(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}

