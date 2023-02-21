namespace OWP.AutoAssignment.Services.PublicApi;

using Constant;

public interface ExtraTimeDetectorService
{
    string DetectExtraTime(string remark);
}

public class ExtraTimeDetectorServiceImp : ExtraTimeDetectorService
{
    public string DetectExtraTime(string remark)
    {
        var extraTime = AutoConstant.Normal;
        if (IsEtPkMatch(remark))
        {
            extraTime = AutoConstant.EtPk;
        }
        if (IsPkMatch(remark))
        {
            extraTime = AutoConstant.Pk;
        }
        return extraTime;
    }

    private static bool IsEtPkMatch(string remark)
        => (remark.Contains(AutoConstant.Et, StringComparison.OrdinalIgnoreCase) && remark.Contains(AutoConstant.Pk, StringComparison.OrdinalIgnoreCase))
           || string.Equals(remark, AutoConstant.EtPkConfirm, StringComparison.OrdinalIgnoreCase);

    private static bool IsPkMatch(string remark)
        => !remark.Contains(AutoConstant.Et, StringComparison.OrdinalIgnoreCase) && remark.Contains(AutoConstant.Pk, StringComparison.OrdinalIgnoreCase);
}