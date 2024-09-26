using OWP.Constants;
using System.Collections.Immutable;

namespace OWP.EventPicking.Api.Helper;

public static class MatchNoHelper
{
    private static readonly ImmutableDictionary<int, string> ChildMatchNo = new Dictionary<int, string>
    {
        { Bettype.NoOfCorner, "C"},
        { Bettype.NextCorner, "C1"},
        { Bettype.TotalBooking, "TB"},
        { Bettype.NextBooking, "TB1"}
    }.ToImmutableDictionary();

    public static string BuildMainMatchNo(int no) => $"{no:0000}";

    public static string BuildChildMatchNo(int no, int? minorBettypeId)
    {
        var childMatchSuffix = minorBettypeId is < 0
            ? ChildMatchNo.GetValueOrDefault(minorBettypeId.Value, string.Empty)
            : string.Empty;

        return $"{no:0000}{childMatchSuffix}";
    }
}
