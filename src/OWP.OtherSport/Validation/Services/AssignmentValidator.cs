using OWP.OtherSport.Validation.Exceptions;
using OWP.OtherSport.Validation.Helpers;
using OWP.OtherSport.Validation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace OWP.OtherSport.Validation.Services
{
    public interface AssignmentValidator
    {
        void Validate(UploadingDataFromExcel rawDataFromExcel, List<Trader> traders);

        void ValidateTraders(AssignmentModelInExcel assignment, List<ValidationResult> results, List<Trader> traders);

        void ValidateInvalidTraders(AssignmentModelInExcel assignment, List<ValidationResult> results, List<Trader> traders);

        void ValidateDuplicatedTraders(List<Trader> traders);
    }

    public class AssignmentValidatorImp : AssignmentValidator
    {
        public void Validate(UploadingDataFromExcel rawDataFromExcel, List<Trader> traders)
        {
            CustomValidateAssignment(rawDataFromExcel.Assignments, traders);
        }

        public void CustomValidateAssignment(List<AssignmentModelInExcel> assignments, List<Trader> traders)
        {
            var validationResults = new List<AssignmentValidationRowResult>();

            for (var i = 0; i < assignments.Count; i++)
            {
                var rowIndex = i;
                var validationResultsOfAssignment = CustomValidateAssignment(assignments[i], traders);
                ValidationHelper.BuildValidationRowResult(
                   validationResultsOfAssignment,
                   validationResults,
                   rowIndex);
            }

            var hasError = validationResults.Any();

            if (hasError)
            {
                throw new InvalidAssignmentRowException(validationResults);
            }
        }

        public List<ValidationResult> CustomValidateAssignment(AssignmentModelInExcel assignment, List<Trader> traders)
        {
            var results = new List<ValidationResult>();
            ValidateTraders(assignment, results, traders);
            ValidateDataAnotation(assignment, results);
            return results;
        }


        public void ValidateTraders(AssignmentModelInExcel assignment, List<ValidationResult> results, List<Trader> traders)
        {
            ValidateInvalidTraders(assignment, results, traders);         
            ValidateDuplicatedTraders(traders);
        }

        public void ValidateDuplicatedTraders(List<Trader> traders)
        {
            var duplicatedTraders = traders.Where(t => t.IsDuplicate).Select(t => t.Username).ToArray();

            if (duplicatedTraders.Length > 0)
            {
                var duplicatedTradersInString = string.Join("; ", duplicatedTraders);
                throw new DuplicatedTraderException(duplicatedTradersInString);
            }
        }

        public void ValidateInvalidTraders(AssignmentModelInExcel assignment, List<ValidationResult> results, List<Trader> traders)
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
        {
            return model.GetType().GetProperties().Single(p => p.Name == propertyName);
        }

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
    }
}

