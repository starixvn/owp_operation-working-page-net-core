using OWP.EventPicking.Models;

namespace OWP.EventPicking.Helper;
public static class LengthPropertyBuilder
{
    public static void Build(List<EventPickingModel> matches)
        => matches.ForEach(x =>
        {
            x.LengthStreamingCell = LengthOfProperty(x.TV) + LengthOfProperty(x.Iptv) + LengthOfProperty(x.Channel) + LengthOfProperty(x.Wb);
            x.LengthProviderCell = LengthOfProperty(x.Provider) + LengthOfProperty(x.OtherProvider) + LengthOfProperty(x.Agent) + LengthOfProperty(x.Venue);
            x.LengthRoundCell = LengthOfProperty(x.Division) + LengthOfProperty(x.Round) + LengthOfProperty(x.ETPK);
            x.LengthRemarkCell = LengthOfProperty(x.LstNote) + LengthOfProperty(x.Remark) + LengthOfProperty(x.HomeRemark) + LengthOfProperty(x.AwayRemark);
        });

    private static int LengthOfProperty(string value) => value?.Length ?? 0;
}
