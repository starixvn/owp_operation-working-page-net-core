using System.Collections.Immutable;
using OWP.AssignmentUpload.Validation.Models;

namespace OWP.AssignmentUpload.Validation.Constants;

public static class ExplicitMinorMatchRuleConstants
{
    private const string corners = "CORNERS";

    private const string ERROR_MESSAGE_MISSING_S = "[Column: {0}] - The {1} name is missing 's' at the end";
    private const string ERROR_MESSAGE_REDUNDANT_S = "[Column: {0}] - The {1} name is redundant 's' at the end";

    public static readonly IDictionary<string, string> MinorMatchedWrongGrammar = new Dictionary<string, string>(){
            {"No.of Corner",ERROR_MESSAGE_MISSING_S},
            {"Total Booking",ERROR_MESSAGE_MISSING_S},
            {"Next Corner",ERROR_MESSAGE_MISSING_S},
            {"Next Bookings",ERROR_MESSAGE_REDUNDANT_S}
    };

    public static readonly IDictionary<string, string> MinorLeagueWrongGrammar = new Dictionary<string, string>(){
            {"CORNER",ERROR_MESSAGE_MISSING_S},
            {"BOOKINGS",ERROR_MESSAGE_REDUNDANT_S}
    };

    public static readonly ImmutableList<MinorMatch> MinorMatchesRules1 = ImmutableList<MinorMatch>.Empty.AddRange(new[]
    {
        new MinorMatch
        {
            MinorBettypeId = -3,
            LeagueLike = corners,
            LeagueNotLike = "SPECIFIC 15 MINS NUMBER OF CORNERS",
            HomeLike = "No.of Corners",
            HomeNotLike = " Over",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 14,
            AwayChars = 14
        },
        new MinorMatch
        {
            MinorBettypeId = -4,
            LeagueLike = corners,
            LeagueNotLike = "",
            HomeLike = "Next Corner",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 12,
            AwayChars = 12
        },
        new MinorMatch
        {
            MinorBettypeId = -5,
            LeagueLike = "SPECIFIC 15 MINS NUMBER OF CORNERS",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 36,
            HomeChars = 32,
            AwayChars = 32
        },
        new MinorMatch
        {
            MinorBettypeId = -6,
            LeagueLike = "BOOKING",
            LeagueNotLike = "SPECIFIC 15 MINS TOTAL BOOKINGS",
            HomeLike = "Total Bookings",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 14,
            AwayChars = 14
        },
        new MinorMatch
        {
            MinorBettypeId = -7,
            LeagueLike = "BOOKING",
            LeagueNotLike = "",
            HomeLike = "Next Booking",
            HomeNotLike = "Total Bookings",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 12,
            AwayChars = 12
        },
        new MinorMatch
        {
            MinorBettypeId = -8,
            LeagueLike = "SPECIFIC 15 MINS TOTAL BOOKINGS",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 33,
            HomeChars = 33,
            AwayChars = 33
        },
        new MinorMatch
        {
            MinorBettypeId = -11,
            LeagueLike = "WHICH TEAM TO TAKE THE FIRST PENALTY",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 58,
            HomeChars = 0,
            AwayChars = 0
        },
        new MinorMatch
        {
            MinorBettypeId = -12,
            LeagueLike = "BRONZE MEDAL WINNER",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 21,
            HomeChars = 0,
            AwayChars = 0
        },
        new MinorMatch
        {
            MinorBettypeId = -13,
            LeagueLike = "GOLD MEDAL WINNER",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 19,
            HomeChars = 0,
            AwayChars = 0
        },
        new MinorMatch
        {
            MinorBettypeId = -14,
            LeagueLike = "WINNER",
            LeagueNotLike = "MEDAL WINNER",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 8,
            HomeChars = 0,
            AwayChars = 0
        },
        new MinorMatch
        {
            MinorBettypeId = -15,
            LeagueLike = "WHICH TEAM WILL ADVANCE TO NEXT ROUND",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 39,
            HomeChars = 0,
            AwayChars = 0
        },
        new MinorMatch
        {
            MinorBettypeId = -90,
            LeagueLike = "CORNERS",
            LeagueNotLike = "",
            HomeLike = "Last Corner",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 11,
            AwayChars = 11
        },
        new MinorMatch
        {
            MinorBettypeId = -91,
            LeagueLike = "BOOKING",
            LeagueNotLike = "",
            HomeLike = "Last Booking",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 12,
            AwayChars = 12
        },
        new MinorMatch
        {
            MinorBettypeId = -96,
            LeagueLike = "PENALTY",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "Score",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 0,
            AwayChars = 26
        },
        new MinorMatch
        {
            MinorBettypeId = -97,
            LeagueLike = "PENALTY",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "Awarded",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 0,
            AwayChars = 25
        },
        new MinorMatch
        {
            MinorBettypeId = -100,
            LeagueLike = "GOAL KICK",
            LeagueNotLike = "",
            HomeLike = "Next",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 11,
            HomeChars = 14,
            AwayChars = 14
        },
        new MinorMatch
        {
            MinorBettypeId = -101,
            LeagueLike = "GOAL KICK",
            LeagueNotLike = "",
            HomeLike = "Last",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 11,
            HomeChars = 14,
            AwayChars = 14
        },
        new MinorMatch
        {
            MinorBettypeId = -102,
            LeagueLike = "GOAL KICK",
            LeagueNotLike = "",
            HomeLike = "2nd",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 11,
            HomeChars = 22,
            AwayChars = 22
        },
        new MinorMatch
        {
            MinorBettypeId = -103,
            LeagueLike = "OFFSIDE",
            LeagueNotLike = "",
            HomeLike = "Last",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 12,
            AwayChars = 12
        },
        new MinorMatch
        {
            MinorBettypeId = -104,
            LeagueLike = "OFFSIDE",
            LeagueNotLike = "",
            HomeLike = "2nd",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 20,
            AwayChars = 20
        },
        new MinorMatch
        {
            MinorBettypeId = -105,
            LeagueLike = "THROW IN",
            LeagueNotLike = "",
            HomeLike = "Last",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 10,
            HomeChars = 13,
            AwayChars = 13
        },
        new MinorMatch
        {
            MinorBettypeId = -106,
            LeagueLike = "THROW IN",
            LeagueNotLike = "",
            HomeLike = "2nd",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 10,
            HomeChars = 21,
            AwayChars = 21
        },
        new MinorMatch
        {
            MinorBettypeId = -107,
            LeagueLike = "FREE KICK",
            LeagueNotLike = "",
            HomeLike = "Last",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 11,
            HomeChars = 14,
            AwayChars = 14
        },
        new MinorMatch
        {
            MinorBettypeId = -108,
            LeagueLike = "FREE KICK",
            LeagueNotLike = "",
            HomeLike = "2nd",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 11,
            HomeChars = 22,
            AwayChars = 22
        },
        new MinorMatch
        {
            MinorBettypeId = -109,
            LeagueLike = "SPECIALS GOAL",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 15,
            HomeChars = 9,
            AwayChars = 9
        }
    });

    public static readonly ImmutableList<MinorMatch> MinorMatchRules2Home = ImmutableList<MinorMatch>.Empty.AddRange(new[]
    {
        new MinorMatch
        {
            MinorBettypeId = -92,
            LeagueLike = "SINGLE TEAM OVER/UNDER",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 24,
            HomeChars = 6,
            AwayChars = 7
        },
        new MinorMatch
        {
            MinorBettypeId = -94,
            LeagueLike = corners,
            LeagueNotLike = "",
            HomeLike = "No.of Corners - Over",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 20,
            AwayChars = 21
        },
        new MinorMatch
        {
            MinorBettypeId = -98,
            LeagueLike = "PENALTY SHOOT-OUT",
            LeagueNotLike = "WHICH TEAM TO TAKE THE FIRST PENALTY",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 19,
            HomeChars = 12,
            AwayChars = 12
        }
    });

    public static readonly ImmutableList<MinorMatch> MinorMatchRules2Away = ImmutableList<MinorMatch>.Empty.AddRange(new[]
    {
        new MinorMatch
        {
            MinorBettypeId = -93,
            LeagueLike = "SINGLE TEAM OVER/UNDER",
            LeagueNotLike = "",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 24,
            HomeChars = 6,
            AwayChars = 7
        },
        new MinorMatch
        {
            MinorBettypeId = -95,
            LeagueLike = "",
            LeagueNotLike = "",
            HomeLike = "No.of Corners - Over",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 9,
            HomeChars = 20,
            AwayChars = 21
        },
        new MinorMatch
        {
            MinorBettypeId = -99,
            LeagueLike = "PENALTY SHOOT-OUT",
            LeagueNotLike = "WHICH TEAM TO TAKE THE FIRST PENALTY",
            HomeLike = "",
            HomeNotLike = "",
            AwayLike = "",
            AwayNotLike = "",
            LeagueChars = 19,
            HomeChars = 12,
            AwayChars = 12
        }
    });
}