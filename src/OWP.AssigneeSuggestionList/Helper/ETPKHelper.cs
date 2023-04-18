using OWP.AssigneeSuggestionList.Constant;

namespace OWP.AssigneeSuggestionList.Helper;

public static class ETPKHelper
{
    public static string DetectExtraTime(string remark)
    {
        var extraTime = string.Empty;
        if (IsEtPkMatch(remark))
        {
            extraTime = ETPk.EtPk;
        }
        if (IsPkMatch(remark))
        {
            extraTime = ETPk.Pk;
        }
        return extraTime;
    }

    public static string DetectExtraTimeCorner(string parentRemark, string remark)
    {
        var extraTimeCorner = DetectExtraTime(remark);
        if (string.IsNullOrEmpty(extraTimeCorner)
            && IsEtPkMatch(parentRemark))
        {
            extraTimeCorner = ETPk.Et;
        }
        return extraTimeCorner;
    }

    public static string BuildEtPkTextDisplay(IList<string> remark)
    {
        var etpkValues = new List<string>();
        foreach (var item in remark)
        {
            var value = DetectExtraTime(item);
            if (!string.IsNullOrEmpty(value))
            {
                etpkValues.Add(value);
            }
        }
        return string.Join("+", etpkValues);
    }

    private static bool IsEtPkMatch(string remark)
        => remark is not null && remark.Contains(ETPk.Et, StringComparison.OrdinalIgnoreCase) && remark.Contains(ETPk.Pk, StringComparison.OrdinalIgnoreCase)
           || string.Equals(remark, ETPk.EtPkConfirm, StringComparison.OrdinalIgnoreCase);

    private static bool IsPkMatch(string remark)
        => remark is not null && !remark.Contains(ETPk.Et, StringComparison.OrdinalIgnoreCase) && remark.Contains(ETPk.Pk, StringComparison.OrdinalIgnoreCase);
}