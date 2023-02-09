using OWP.Extensions;

namespace OWP.Utils;

public static class OneManyMapping
{
    public const int NextCorners = -4;
    public const int NextBooking = -7;
    public const int HomeSingleTeamPenaltyShootout = -98;
    public const int AwaySingleTeamPenaltyShootout = -99;
    public const int NextGoal = -109;

    private static readonly Dictionary<int, Tuple<int, int>> OneManyList = new Dictionary<int, Tuple<int, int>>
    {
        { HomeSingleTeamPenaltyShootout, new Tuple<int, int>(1, 49) },
        { AwaySingleTeamPenaltyShootout, new Tuple<int, int>(1, 49) },
        { NextGoal, new Tuple<int, int>(1, 99) },
        { NextCorners, new Tuple<int, int>(1, 99) },
        { NextBooking, new Tuple<int, int>(1, 99) }
    };

    public static Tuple<int, int> GetRangeOneManyMatch(int? betTypeId)
    {
        var betTypeIdValue = betTypeId ?? 0;
        return OneManyList.GetValue(betTypeIdValue, new Tuple<int, int>(0, 0));
    }
}