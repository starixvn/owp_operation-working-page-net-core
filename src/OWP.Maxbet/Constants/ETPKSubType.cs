using System.Collections.Immutable;

namespace OWP.Maxbet.Constants;

public static class SubTypeETPK
{
    public const int MainMatchET = 100;

    public const int MainMatchPK = 200;

    public const int FutsalET = 100;

    public const int FutsalPK = 200;

    public const int SaBaPK = 200;

    public const int NoOfCornersET = 2500;

    public const int NextCornersET = 2600;

    public const int TotalBookingET = 3300;

    public const int NextBookingET = 3400;

    public const int NextGoalKickET = 4300;

    public const int FirstPenatyScoreET = 6000;

    public const int FirstPenatyAwardedET = 5900;

    public const int SpecialGoalET = 500;

    public const int HomeSingleTeamET = 1701;

    public const int AwaySingleTeamET = 1702;

    public const int NoOfCorners = 2000;

    public const int NextCorners = 2100;

    public const int TotalBooking = 2800;

    public const int NextBooking = 2900;

    public const int NextGoalKick = 3900;

    public const int FirstPenaltyScore = 5800;

    public const int FirstPenaltyAwarded = 5700;

    public const int SpecialGoal = 400;

    public const int HomeSingleTeam = 1601;

    public const int AwaySingleTeam = 1602;

    public static readonly int[] SubTypeETPKs = {
        MainMatchET,
        MainMatchPK,
        FutsalET,
        FutsalPK,
        SaBaPK,
        NoOfCornersET,
        NextCornersET,
        TotalBookingET,
        NextBookingET,
        NextGoalKickET,
        FirstPenatyScoreET,
        FirstPenatyAwardedET,
        SpecialGoalET,
        HomeSingleTeamET,
        AwaySingleTeamET
    };

    public static readonly ImmutableDictionary<int, int> SpecialSubType = new Dictionary<int, int>
    {
        {NoOfCornersET, NoOfCorners },
        {NextCornersET, NextCorners },
        {TotalBookingET, TotalBooking },
        {NextBookingET, NextBooking },
        {NextGoalKickET, NextGoalKick },
        {FirstPenatyScoreET, FirstPenaltyScore },
        {FirstPenatyAwardedET, FirstPenaltyAwarded },
        {SpecialGoalET, SpecialGoal },
        {HomeSingleTeamET, HomeSingleTeam },
        {AwaySingleTeamET, AwaySingleTeam }
    }.ToImmutableDictionary();
}