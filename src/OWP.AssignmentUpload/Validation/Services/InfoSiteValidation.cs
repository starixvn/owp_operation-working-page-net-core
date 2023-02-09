using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using OWP.AssignmentUpload.Validation.Exceptions;
using OWP.AssignmentUpload.Validation.Helper;
using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload.Validation.Services;

public interface InfoSiteValidation
{
    void ValidateDuplicatedMatches(
        AssignmentModel owpMatch,
        List<AssignmentModel> owpMatchs,
        List<ValidationResult> results);

    void ValidateDuplicatedInfoSiteId(
        AssignmentModel owpMatch,
        List<AssignmentModel> owpMatchs,
        List<ValidationResult> results);

    void ValidateDuplicatedAdminSiteId(
        AssignmentModel assignment,
        List<AssignmentModel> assignments,
        List<ValidationResult> results);

    List<ValidationResult> CustomValidateInfoSiteMatch(
        AssignmentModel owpMatch,
        List<AssignmentModel> owpMatchs);

    void Validate(List<AssignmentModel> owpMatchs);
}

public class InfoSiteValidationImp : InfoSiteValidation
{
    public void Validate(List<AssignmentModel> owpMatchs)
    {
        var validationResults = new List<AssignmentValidationRowResult>();

        var matchValidator = ValidationHelper.RemoveMatchGenerateBySystem(owpMatchs);
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
        List<AssignmentModel> owpMatchs)
    {
        var validationResults = new List<ValidationResult>();
        ValidateDuplicatedMatches(owpMatch, owpMatchs, validationResults);
        ValidateDuplicatedInfoSiteId(owpMatch, owpMatchs, validationResults);
        ValidateDuplicatedAdminSiteId(owpMatch, owpMatchs, validationResults);
        return validationResults;
    }

    public void ValidateDuplicatedMatches(AssignmentModel owpMatch, List<AssignmentModel> owpMatchs, List<ValidationResult> results)
    {
        var isDuplicated = owpMatch.IsSaba
            ? ValidationHelper.CountMatchSameLeagueHomeAwayKOTime(owpMatchs, owpMatch) > 1
            : ValidationHelper.CountMatchSameLeagueHomeAway(owpMatchs, owpMatch) > 1;

        if (isDuplicated)
        {
            results.Add(new ValidationResult("There is duplicated match(es) in the list. Please check again"));
        }
    }

    public void ValidateDuplicatedInfoSiteId(
        AssignmentModel owpMatch,
        List<AssignmentModel> owpMatchs,
        List<ValidationResult> results)
    {
        var isDuplicated = owpMatch.InfoSiteMatchId > 0
                           && owpMatchs.Count(a => a.InfoSiteMatchId == owpMatch.InfoSiteMatchId) > 1;

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