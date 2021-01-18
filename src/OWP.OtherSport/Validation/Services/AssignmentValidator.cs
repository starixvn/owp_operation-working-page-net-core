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
            List<Trader> traders);

        void ValidateTraders(AssignmentModelInExcel assignment, List<ValidationResult> results, List<Trader> traders);

        void ValidateDateTime(AssignmentModelInExcel assignment, List<ValidationResult> results);

        void ValidateMaxbetInString(AssignmentModelInExcel assignment, List<ValidationResult> results);
    }

    public class AssignmentValidatorImp : AssignmentValidator
    {
        public void Validate(UploadingDataFromExcel rawDataFromExcel, List<Trader> traders)
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

            validationResults = GroupByDuplicateMatchMessage(validationResults);

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
            ValidateDateTime(assignment, results);
            ValidateMaxbetInString(assignment, results);

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

        public void ValidateDateTime(AssignmentModelInExcel assignment, List<ValidationResult> results)
        {
            const string regex = @"(?i:^(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\s*\d{2}\,\s*\d{4}\s*(1[0-2]|0?[1-9]):([0-5][0-9])\s*([AP][M])$)";
            var match = Regex.Match(assignment.DateTime, regex, RegexOptions.IgnoreCase);

            if (!match.Success)
            {
                results.Add(new ValidationResult("[Column: Date Time] - Error: Invalid date time format"));
            }
            else
            {
                try
                {
                    DateTime.Parse(assignment.DateTime);
                }

                catch (FormatException ex)
                {
                    results.Add(new ValidationResult($"[Column: Date Time] - Error: {ex.Message}"));
                }
            }
        }

        public void ValidateMaxbetInString(AssignmentModelInExcel assignment, List<ValidationResult> results)
        {
            const string regex = @"^(?:|[0-9]\d{0,9}(\.\d{1,9})?%?)$";

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            var destinationProperties = typeof(AssignmentModelInExcel).GetProperties(flags);

            foreach (var property in destinationProperties)
            {
                if (property.Name.StartsWith("MaxBet", StringComparison.CurrentCultureIgnoreCase))
                {
                    var value = property.GetValue(assignment).ToString() ?? string.Empty;
                    var match = Regex.Match(value, regex, RegexOptions.IgnoreCase);

                    if (!match.Success)
                    {
                        results.Add(new ValidationResult($"[Column: {PropertyHelper.GetDisplayName(property)}] - Error: Invalid Maxbet format"));
                    }
                }
            }
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

        public static List<AssignmentValidationRowResult> GroupByDuplicateMatchMessage(List<AssignmentValidationRowResult> validationResults)
        {
            var duplicateMatches = validationResults.Where(x => x.Message.StartsWith("Duplicate Match"))
                                     .GroupBy(x => x.Message)
                                     .Where(x => x.Count() > 1)
                                     .ToList();
            foreach (var duplicateMatch in duplicateMatches)
            {
                var duplicateRow = duplicateMatch.Select(x => x.Row).ToList();
                var rowNumber = duplicateMatch.FirstOrDefault().Row;
                duplicateRow.RemoveAt(0);
                validationResults.Add(new ()
                {
                    Row = rowNumber,
                    Message = $"{duplicateMatch.FirstOrDefault().Message} (with row {string.Join(',', duplicateRow)})"
                });

                validationResults.RemoveAll(x => duplicateMatch.Any(y => y.Message == x.Message));
            }

            return validationResults.OrderBy(x => x.Row).ToList();
        }
    }
}

