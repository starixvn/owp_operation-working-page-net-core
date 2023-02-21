namespace OWP.AssignmentUpload.Validation.Services;

using System.ComponentModel.DataAnnotations;
using Exceptions;
using Helper;
using Models;

public interface InfoSiteValidation
{
    void ValidateDuplicatedMatches(
        AssignmentModel owpMatch,
        List<AssignmentModel> owpMatches,
        List<ValidationResult> results);

    void ValidateDuplicatedInfoSiteId(
        AssignmentModel owpMatch,
        List<AssignmentModel> owpMatches,
        List<ValidationResult> results);

    void ValidateDuplicatedAdminSiteId(
        AssignmentModel assignment,
        List<AssignmentModel> assignments,
        List<ValidationResult> results);

    List<ValidationResult> CustomValidateInfoSiteMatch(
        AssignmentModel owpMatch,
        List<AssignmentModel> owpMatches);

    void Validate(List<AssignmentModel> owpMatches);
}

public class InfoSiteValidationImp : InfoSiteValidation
{
    public void Validate(List<AssignmentModel> owpMatches)
    {
        var validationResults = new List<AssignmentValidationRowResult>();

        var matchValidator = ValidationHelper.RemoveMatchGenerateBySystem(owpMatches);
        for (var i = 0; i < matchValidator.Count; i++)
        {
            var rowIndex = i;
            var validationResultsOfMatch = CustomValidateInfoSiteMatch(matchValidator[i], matchValidator);
            ValidationHelper.BuildValidationRowResult(
                validationResultsOfMatch,
                validationResults,
                rowIndex);
        }
        var hasError = validationResults.Any();
        if (hasError)
        {
            throw new InvalidAssignmentRowException(validationResults);
        }
    }

    public List<ValidationResult> CustomValidateInfoSiteMatch(
        AssignmentModel owpMatch,
        List<AssignmentModel> owpMatches)
    {
        var validationResults = new List<ValidationResult>();
        ValidateDuplicatedMatches(owpMatch, owpMatches, validationResults);
        ValidateDuplicatedInfoSiteId(owpMatch, owpMatches, validationResults);
        ValidateDuplicatedAdminSiteId(owpMatch, owpMatches, validationResults);
        return validationResults;
    }

    public void ValidateDuplicatedMatches(AssignmentModel owpMatch, List<AssignmentModel> owpMatches, List<ValidationResult> results)
    {
        var isDuplicated = owpMatch.IsSaba
            ? ValidationHelper.CountMatchSameLeagueHomeAwayKOTime(owpMatches, owpMatch) > 1
            : ValidationHelper.CountMatchSameLeagueHomeAway(owpMatches, owpMatch) > 1;

        if (isDuplicated)
        {
            results.Add(new ValidationResult("There is duplicated match(es) in the list. Please check again"));
        }
    }

    public void ValidateDuplicatedInfoSiteId(
        AssignmentModel owpMatch,
        List<AssignmentModel> owpMatches,
        List<ValidationResult> results)
    {
        var isDuplicated = owpMatch.InfoSiteMatchId > 0
                           && owpMatches.Count(a => a.InfoSiteMatchId == owpMatch.InfoSiteMatchId) > 1;

        if (isDuplicated)
        {
            results.Add(new ValidationResult($"Duplicate Infosite Id: '{owpMatch.InfoSiteMatchId}' of matches"));
        }
    }

    public void ValidateDuplicatedAdminSiteId(
        AssignmentModel assignment,
        List<AssignmentModel> assignments,
        List<ValidationResult> results)
    {
        var isDuplicated = assignment.AdminSiteId > 0
                           && assignments.Count(a => a.AdminSiteId == assignment.AdminSiteId) > 1;

        if (isDuplicated)
        {
            results.Add(new ValidationResult($"Duplicate AdminSite Id: '{assignment.AdminSiteId}' of matches"));
        }
    }
}