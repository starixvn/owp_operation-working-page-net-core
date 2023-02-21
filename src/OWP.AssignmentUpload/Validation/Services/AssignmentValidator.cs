using OWP.Models;

namespace OWP.AssignmentUpload.Validation.Services
{
    using Exceptions;
    using Helper;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    public interface AssignmentValidator
    {
        void Validate(DateTime eventDate, UploadingDataFromExcel uploadingDataFromExcel, List<Trader> traders, bool IsUploadFMAndAIO, List<ReferenceResource> matchOffers);

        void ValidateEventDate(DateTime eventDate, UploadingDataFromExcel uploadingDataFromExcel);

        void ValidateTraders(AssignmentModelInExcel assignment, List<ValidationResult> results, List<Trader> traders);

        void ValidateInvalidTraders(AssignmentModelInExcel assignment, List<ValidationResult> results, List<Trader> traders);

        void ValidateDuplicatedTraders(List<Trader> traders);

        void ValidateDataAnotation(AssignmentModelInExcel assignment, List<ValidationResult> results);

        void ValidateOtherAssignment(AssignmentModelInExcel assignment, List<ValidationResult> results, bool IsUploadFMAndAIO);

        void ValidateDuplicatedMatchNo(AssignmentModelInExcel assignment, List<AssignmentModelInExcel> assignments, List<ValidationResult> results);
    }

    public class AssignmentValidatorImp : AssignmentValidator
    {
        public void Validate(DateTime eventDate, UploadingDataFromExcel uploadingDataFromExcel, List<Trader> traders, bool IsUploadFMAndAIO, List<ReferenceResource> matchOffers)
        {
            ValidateEventDate(eventDate, uploadingDataFromExcel);
            CustomValidateAssignment(uploadingDataFromExcel.Assignments, IsUploadFMAndAIO, traders, matchOffers);
        }

        public void ValidateEventDate(DateTime eventDate, UploadingDataFromExcel uploadingDataFromExcel)
        {
            if (eventDate.Date != uploadingDataFromExcel.EventDate.Date)
            {
                throw new EventDateMismatchException();
            }
        }

        public static void ValidateInvalidTime(AssignmentModelInExcel assignment, List<ValidationResult> results)
        {
            if (string.IsNullOrWhiteSpace(assignment.Time))
            {
                results.Add(new ValidationResult("Invalid time"));
            }
        }

        public void ValidateTraders(AssignmentModelInExcel assignment, List<ValidationResult> results, List<Trader> traders)
        {
            ValidateInvalidTraders(assignment, results, traders);
            ValidateDuplicatedTraders(traders);
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
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTradersLA, "OtherLA");
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTraders10Mins, "10mins");
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTraders15Mins, "15mins");
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTraders1H1X2, "1H 1X2");
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTraders1HHDP, "1H HDP");
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTraders1HOU, "1H O/U");
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTraders1X2, "1X2");
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTradersAIO, "AIO");
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTradersFM, "FM");
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTradersHDP, "HDP");
            CheckOtherTrader(errorList, invalidTraders, assignment.OtherTradersOU, "O/U");

            CheckTrader(errorList, invalidTraders, assignment.TraderLAName, "LA");
            CheckTrader(errorList, invalidTraders, assignment.Trader10MinsName, "10mins");
            CheckTrader(errorList, invalidTraders, assignment.Trader15MinsName, "15mins");
            CheckTrader(errorList, invalidTraders, assignment.Trader1H1X2Name, "1H 1X2");
            CheckTrader(errorList, invalidTraders, assignment.Trader1HHDPName, "1H HDP");
            CheckTrader(errorList, invalidTraders, assignment.Trader1HOUName, "1H O/U");
            CheckTrader(errorList, invalidTraders, assignment.Trader1X2Name, "1X2");
            CheckTrader(errorList, invalidTraders, assignment.TraderAIOName, "AIO");
            CheckTrader(errorList, invalidTraders, assignment.TraderFMName, "FM");
            CheckTrader(errorList, invalidTraders, assignment.TraderHDPName, "HDP");
            CheckTrader(errorList, invalidTraders, assignment.TraderOUName, "O/U");

            return errorList;
        }

        public static void CheckOtherTrader(List<string> errorList, List<string> invalidTraders, string traders, string name)
        {
            var traderList = traders.Split(',').ToList();
            var regex = new Regex(@"#(.*?)(\s|$)");
            foreach (var trader in traderList)
            {
                var traderName = regex.Match(trader).Groups[1].Value;
                if (invalidTraders.Contains(traderName))
                {
                    errorList.Add(name.Equals("OtherLA")
                        ? $"{name}: {traderName}"
                        : $"OtherTraders ({name}): {traderName}");
                }
            }
        }

        public static void CheckTrader(List<string> errorList, List<string> invalidTraders, string traderName, string name)
        {
            if (invalidTraders.Contains(traderName))
            {
                errorList.Add(name.Equals("LA")
                    ? $"{name}: {traderName}"
                    : $"Trader ({name}): {traderName}");
            }
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

        public void CustomValidateAssignment(List<AssignmentModelInExcel> assignments, bool IsUploadFMAndAIO, List<Trader> traders, List<ReferenceResource> matchOffers)
        {
            var validationResults = new List<AssignmentValidationRowResult>();

            for (var i = 0; i < assignments.Count; i++)
            {
                var rowIndex = i;
                var validationResultsOfAssignment = CustomValidateAssignment(assignments[i], assignments, IsUploadFMAndAIO, traders, matchOffers);
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

        public List<ValidationResult> CustomValidateAssignment(
            AssignmentModelInExcel assignment,
            List<AssignmentModelInExcel> assignments,
            bool isUploadFmAndAio,
            List<Trader> traders,
            List<ReferenceResource> matchOffers)
        {
            var results = new List<ValidationResult>();

            ValidateTraders(assignment, results, traders);
            ValidateMatchOffers(assignment, matchOffers, results);
            ValidateOtherAssignment(assignment, results, isUploadFmAndAio);
            ValidateDataAnotation(assignment, results);
            ValidateDuplicatedMatchNo(assignment, assignments, results);
            ValidateInvalidTime(assignment, results);

            return results;
        }

        public void ValidateOtherAssignment(AssignmentModelInExcel assignment, List<ValidationResult> results, bool isUploadFmAndAio)
        {
            var listTraderError = new List<string>
            {
                ValidateOtherAssignment(assignment.OtherTraders10Mins, assignment.Trader10MinsName, "10 Mins"),
                ValidateOtherAssignment(assignment.OtherTraders15Mins, assignment.Trader15MinsName, "15 Mins"),
                ValidateOtherAssignment(assignment.OtherTraders1H1X2, assignment.Trader1H1X2Name, "1H 1X2"),
                ValidateOtherAssignment(assignment.OtherTraders1HHDP, assignment.Trader1HHDPName, "1H HDP"),
                ValidateOtherAssignment(assignment.OtherTraders1HOU, assignment.Trader1HOUName, "1H O/U"),
                ValidateOtherAssignment(assignment.OtherTraders1X2, assignment.Trader1X2Name, "1X2")
            };
            if (isUploadFmAndAio)
            {
                listTraderError.Add(ValidateOtherAssignment(assignment.OtherTradersAIO, assignment.TraderAIOName, "AIO"));
                listTraderError.Add(ValidateOtherAssignment(assignment.OtherTradersFM, assignment.TraderFMName, "FM"));
            }
            listTraderError.Add(ValidateOtherAssignment(assignment.OtherTradersHDP, assignment.TraderHDPName, "HDP"));
            listTraderError.Add(ValidateOtherAssignment(assignment.OtherTradersLA, assignment.TraderLAName, "LA"));
            listTraderError.Add(ValidateOtherAssignment(assignment.OtherTradersOU, assignment.TraderOUName, "O/U"));

            var traderErrorList = listTraderError.Where(x => x != null).ToList();
            if (traderErrorList.Count > 0)
            {
                var traderErrorInString = string.Join("; ", traderErrorList);
                results.Add(new ValidationResult("Missing main assignment of " + traderErrorInString));
            }
        }

        public static string ValidateOtherAssignment(string otherTrader, string mainTrader, string errName)
        {
            if (string.IsNullOrWhiteSpace(otherTrader))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(mainTrader))
            {
                return errName;
            }

            return null;
        }

        public void ValidateDataAnotation(AssignmentModelInExcel assignment, List<ValidationResult> results)
        {
            var context = new ValidationContext(assignment, null, null);
            Validator.TryValidateObject(assignment, context, results, true);
        }

        public void ValidateDuplicatedMatchNo(AssignmentModelInExcel assignment, List<AssignmentModelInExcel> assignments, List<ValidationResult> results)
        {
            var isDuplicated = !string.IsNullOrWhiteSpace(assignment.MatchNo)
                && assignments.Count(a => a.MatchNo == assignment.MatchNo) > 1;

            if (isDuplicated)
            {
                results.Add(new ValidationResult($"Duplicate Match No: '{assignment.MatchNo}' of matches"));
            }
        }

        public void ValidateMatchOffers(AssignmentModelInExcel assignment, List<ReferenceResource> matchOffers, List<ValidationResult> results)
        {
            var excelMatchOffers = assignment.MatchOffer
                .Split(',')
                .Where((x => !string.IsNullOrEmpty(x))).ToList();
            var matchOfferValues = matchOffers.Select(y => y.Value.ToLower(CultureInfo.InvariantCulture));
            var isInValid = excelMatchOffers.Any(x => !matchOfferValues.Contains(x.Trim().ToLower(CultureInfo.InvariantCulture)));

            if (isInValid)
            {
                var matchOffersInString = string.Join(", ", matchOffers.Select(x => x.Value));
                results.Add(new ValidationResult($"The Match Offer must be {matchOffersInString}"));
            }
        }
    }
}
