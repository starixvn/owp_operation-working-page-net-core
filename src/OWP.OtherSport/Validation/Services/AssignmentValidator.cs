using OWP.OtherSport.Validation.Exceptions;
using OWP.OtherSport.Validation.Helpers;
using OWP.OtherSport.Validation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
            throw new System.NotImplementedException();
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

        public static List<string> ValidateInvalidOtherTrader(AssignmentModelInExcel assignment, List<string> invalidTraders)
        {
            var errorList = new List<string>();           
            var properties = typeof(AssignmentModelInExcel).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.StartsWith("Other Trader", StringComparison.CurrentCultureIgnoreCase))
                {
                    CheckTrader(errorList, invalidTraders, prop.Name, PropertyHelper.GetDisplayName(prop));
                }
            }

            return errorList;
        }

        public static List<string> ValidateInvalidTrader(AssignmentModelInExcel assignment, List<string> invalidTraders)
        {
            var errorList = new List<string>();
            var properties = typeof(AssignmentModelInExcel).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name.StartsWith("Trader", StringComparison.CurrentCultureIgnoreCase))
                {
                    CheckTrader(errorList, invalidTraders, prop.Name, PropertyHelper.GetDisplayName(prop));
                }
            }

            return errorList;
        }
    
        public static void CheckTrader(List<string> errorList, List<string> invalidTraders, string traderName, string name)
        {
            if (invalidTraders.Contains(traderName))
            {
                errorList.Add($"({name}): {traderName}");
            }
        }
    }
}
