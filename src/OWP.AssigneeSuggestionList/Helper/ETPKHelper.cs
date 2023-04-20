using OWP.AssigneeSuggestionList.Constant;
using OWP.Constants;
using OWP.Extensions;

namespace OWP.AssigneeSuggestionList.Helper;

public static class ETPKHelper
{
    public static string DetectExtraTime(string remark, int? minorMatchId = null)
    {
        var containsET = remark.IsContains(ETPKConstant.ET);
        var containsPK = remark.IsContains(ETPKConstant.PK);
        var isEqualETPK = string.Equals(remark, ETPKConstant.ETPK_Confirm, StringComparison.OrdinalIgnoreCase);
        var isETPK = containsET && containsPK || isEqualETPK;
        var canConsiderET = minorMatchId is Bettype.NextCorner or Bettype.NextBooking or Bettype.Corner or Bettype.TotalBooking;

        if (isETPK)
        {
            return canConsiderET ? ETPKConstant.ET : ETPKConstant.ETPK;
        }
        if (!containsET && containsPK)
        {
            return ETPKConstant.PK;
        }

        return string.Empty;
    }

    public static string BuildEtPkTextDisplay(IList<string> remark)
    {
        var etpkValues = remark
            .Select(item => DetectExtraTime(item))
            .Where(value => !string.IsNullOrEmpty(value))
            .ToList();
        return string.Join("+", etpkValues);
    }
}