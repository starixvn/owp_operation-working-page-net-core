using OWP.AssigneeSuggestionList.Constant;

namespace OWP.AssigneeSuggestionList.Helper
{
    public class DetectETPKHelper
    {
        public string DetectExtraTime(string remark)
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
            if (IsEtMatch(remark))
            {
                extraTime = ETPk.Et;
            }
            return extraTime;
        }

        private static bool IsEtPkMatch(string remark)
            => (remark.Contains(ETPk.Et, StringComparison.OrdinalIgnoreCase) && remark.Contains(ETPk.Pk, StringComparison.OrdinalIgnoreCase))
               || string.Equals(remark, ETPk.EtPkConfirm, StringComparison.OrdinalIgnoreCase);

        private static bool IsPkMatch(string remark)
            => !remark.Contains(ETPk.Et, StringComparison.OrdinalIgnoreCase) && remark.Contains(ETPk.Pk, StringComparison.OrdinalIgnoreCase);

        private static bool IsEtMatch(string remark)
        => remark.Contains(ETPk.Et, StringComparison.OrdinalIgnoreCase) && !remark.Contains(ETPk.Pk, StringComparison.OrdinalIgnoreCase);
    }
}