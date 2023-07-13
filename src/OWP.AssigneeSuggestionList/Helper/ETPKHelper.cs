using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;
using OWP.Constants;
using OWP.Extensions;

namespace OWP.AssigneeSuggestionList.Helper;

public static class ETPKHelper
{
    public static string DetectExtraTime(AssignmentModel assignment)
        => (IsEtPK(assignment),
            IsEt(assignment),
            IsPK(assignment)) switch
        {
            (true, true, _) => ETPKConstant.ET,
            (false, _, true) => ETPKConstant.PK,
            (true, _, _) => ETPKConstant.ETPK,
            _ => string.Empty
        };

    public static string BuildEtPkTextDisplay(IList<AssignmentModel> assignments)
    {
        var etpkValues = assignments.Select(DetectExtraTime)
            .Where(value => !string.IsNullOrEmpty(value))
            .ToList();
        return string.Join("+", etpkValues);
    }

    private static bool IsEt(AssignmentModel assignment)
        => assignment.MinorBettypeId.HasValue && BetTypeSpecial.BetTypes.Contains(assignment.MinorBettypeId.Value) &&
            IsEtPK(assignment);

    private static bool IsEtPK(AssignmentModel assignment)
    => assignment.Remark.IsContains(ETPKConstant.ET)
        && assignment.Remark.IsContains(ETPKConstant.PK)
        && (assignment.AssignmentNote is null
            || (assignment.AssignmentNote.IsContains(ETPKConstant.ET)
                && assignment.AssignmentNote.IsContains(ETPKConstant.PK)
                && !assignment.AssignmentNote.IsContains("No")));

    private static bool IsPK(AssignmentModel assignment)
    => !assignment.Remark.IsContains(ETPKConstant.ET)
        && assignment.Remark.IsContains(ETPKConstant.PK)
        && (assignment.AssignmentNote is null
            || (!assignment.AssignmentNote.IsContains(ETPKConstant.ET)
                && assignment.AssignmentNote.IsContains(ETPKConstant.PK)
                && !assignment.AssignmentNote.IsContains("No")));
}