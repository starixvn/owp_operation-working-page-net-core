using OWP.AssigneeSuggestionList.Models;
using OWP.Extensions;

namespace OWP.AssigneeSuggestionList.Helper;

public static class ETPKHelper
{
    public static string DetectExtraTime(string remark, string assignmentNote, int? minorBettypeId = null)
        => (IsEtPK(remark, assignmentNote),
            Utils.IsValidSpecialProduct(minorBettypeId),
            IsPK(remark, assignmentNote, minorBettypeId)) switch
        {
            (true, true, _) => "ET",
            (false, _, true) => "PK",
            (true, _, _) => "ET/PK",
            _ => string.Empty
        };

    public static string BuildEtPkTextDisplay(IList<AssignmentModel> assignments)
    {
        var etpkValues = assignments.Select(x => DetectExtraTime(x.Remark, x.AssignmentNote, x.MinorBettypeId))
            .Where(value => !string.IsNullOrEmpty(value))
            .ToList();
        return string.Join("+", etpkValues);
    }

    private static bool IsEtPK(string remark, string assignmentNote)
    => remark.IsContains(" ET")
        && remark.IsContains("PK")
        && IsEtPKConditionTwo(assignmentNote);

    private static bool IsEtPKConditionTwo(string assignmentNote)
    => assignmentNote is null
            || (assignmentNote.IsContains("ET")
                && assignmentNote.IsContains("PK")
                && !assignmentNote.IsContains("No"));

    private static bool IsPK(string remark, string assignmentNote, int? minorBettypeId)
        => !minorBettypeId.HasValue && IsPKRemark(remark) && IsPKAssignmentNote(assignmentNote);

    private static bool IsPKRemark(string remark)
        => !remark.IsContains(" ET")
           && remark.IsContains("PK");

    private static bool IsPKAssignmentNote(string assignmentNote)
        => assignmentNote is null
            || (!assignmentNote.IsContains("ET")
                && assignmentNote.IsContains("PK")
                && !assignmentNote.IsContains("No"));
}