using System.Collections.Immutable;

namespace OWP.PrematchAssignment.Constants;

public static class AutoAssignmentConstants
{
    public static readonly ImmutableList<string> MajorLeagueGroups =
    [
        "MJ", "MJ.T1", "MJ.T2", "CL.MJ.Q", "INT.MJ", "World Cup", "EURO", "Others 1"
    ];

    public static readonly ImmutableList<string> SupportedSabaLeagues =
    [
        "SABA CLUB FRIENDLY Virtual FC 24 - 16 Mins Play",
        "SABA INTERNATIONAL FRIENDLY Virtual PES 21 - 20 Mins Play",
        "SABA CLUB FRIENDLY Virtual PES 21 - 15 Mins Play"
    ];

    public static readonly ImmutableList<string> ChildLeagueRules =
    [
        "- SPECIFIC 15 MINS",
        "- SPECIFIC 10 MINS",
        "- CORNERS",
        "- BOOKING",
        "- WINNER",
        "- PENALTY",
        "- GOAL KICK",
        "- OFFSIDE",
        "- THROW IN",
        "- FREE KICK",
        "- SPECIALS GOAL",
        "- SPECIFIC 15 MINS NUMBER OF CORNERS",
        "- SPECIFIC 15 MINS TOTAL BOOKINGS",
        "- BRONZE MEDAL WINNER",
        "- GOLD MEDAL WINNER",
        "- WHICH TEAM WILL ADVANCE TO NEXT ROUND",
        "- SINGLE TEAM OVER/UNDER",
        "- PENALTY SHOOT-OUT",
        "- PENALTY SHOOT-OUT - WHICH TEAM TO TAKE THE FIRST PENALTY",
        "- HOME TEAM/AWAY TEAM"
    ];
}