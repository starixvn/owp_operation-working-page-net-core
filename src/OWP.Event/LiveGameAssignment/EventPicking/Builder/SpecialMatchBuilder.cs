namespace OWP.Event.LiveGameAssignment.EventPicking.Builder;

using Olympus.InfoSiteClient.Models;
using OWP.Event.LiveGameAssignment.Download.Misc;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public static class SpecialMatchBuilder
{
    public static IList<ChildMatch> ReduceSpecialMatch(IList<ChildMatch> childMatches)
    {
        var childMatchDict = childMatches
            .GroupBy(x => new { type = GetSpecialMatchType(x), x.match_id, x.game_date })
            .ToDictionary(x => x.Key, x => x.ToList());

        var match = new List<ChildMatch>();

        foreach (var key in childMatchDict.Keys)
        {
            var firstMatch = childMatchDict[key].FirstOrDefault();
            if (key.type != 0)
            {
                match.Add(GetPresentationMatch(firstMatch, key.type));
            }
            else
            {
                match.AddRange(childMatchDict[key]);
            }
        }

        return match;
    }

    private static ChildMatch GetPresentationMatch(ChildMatch childMatch, int specialType)
    {
        var awayName = childMatch.away_name;
        var homeName = childMatch.home_name;
        Constant.SpecialOwpNameDict.TryGetValue(specialType, out var nameExtension);

        return new ChildMatch
        {
            match_id = childMatch.match_id,
            create_match_id = 0,
            league_name = childMatch.league_name,
            home_name = GetOwpMatchName(homeName, nameExtension),
            away_name = GetOwpMatchName(awayName, nameExtension),
            game_date = childMatch.game_date
        };
    }

    private static string GetOwpMatchName(string matchName, string nameExtension)
    {
        const string pattern = @"(((\d+\w{2})\s(\w+))|(\d{2}:\d{2}-\d{2}:\d{2}))$";
        return Regex.Replace(matchName, pattern, nameExtension);
    }

    private static bool IsConditionOfCorner(ChildMatch match)
    {
        return CheckContainHomeAway(match, "Corner") &&
               char.IsNumber(char.Parse(match.away_name.Substring(match.away_name.Length - 10, 1)));
    }

    private static bool IsConditionOfBooking(ChildMatch match)
    {
        return CheckContainHomeAway(match, "Booking") &&
               char.IsNumber(char.Parse(match.away_name.Substring(match.away_name.Length - 11, 1)));
    }

    private static bool IsConditionOf15Min(ChildMatch match)
    {
        return !CheckContainHomeAway(match, "No.of Corners") && !CheckContainHomeAway(match, "Total Bookings") &&
               Constant.Special15Min.Any(x => CheckContainHomeAway(match, x));
    }

    private static bool CheckContainHomeAway(ChildMatch match, string typeSpecial)
    {
        return match.away_name.IsContains(typeSpecial) && match.home_name.IsContains(typeSpecial);
    }

    private static int GetSpecialMatchType(ChildMatch childMatch)
    {
        if (IsConditionOfCorner(childMatch))
        {
            return Constant.CornerType;
        }

        if (IsConditionOfBooking(childMatch))
        {
            return Constant.BookingType;
        }

        if (IsConditionOf15Min(childMatch))
        {
            return Constant.Special15MinsType;
        }

        return 0;
    }
}