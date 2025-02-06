using OWP.EventPicking.Api.Constant;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Helper;

public static class ExcelFilterHelper
{
    public static List<EventPickingModel> ExcludeChildMatch(EventPickingResult matches)
    {
        var matchesData = matches.FilteredMatches;

        var filters = new List<Func<EventPickingModel, bool>>
        {
            IsChildMatch,
            Is15Mins,
            Is10Mins,
            IsCorners,
            IsNextCorners,
            Is15MinsCorners,
            IsBookings,
            IsNextBookings,
            Is15MinsBookings,
            IsFirstPenalty,
            IsBronzeMedal,
            IsGoldMedal,
            IsWinner,
            IsAdvanceNextRound,
            IsLastCorners,
            IsLastBookings,
            IsSingleTeamOU,
            IsHomeSingleTeamOUOfCorners,
            Is1stPenaltyScore,
            Is1stPenaltyAwarded,
            IsPenaltyShootOut,
            IsNextGoalKick,
            IsLastGoalKick,
            Is2H1stGoalKick,
            IsLastOffside,
            Is2H1stOffside,
            IsLastThrowIn,
            Is2H1stThrowIn,
            IsLastFreeKick,
            Is2H1stFreeKick,
            IsSpecialGoal
        };

        foreach (var filter in filters)
        {
            matchesData.RemoveAll(match => !match.IsSaba && filter(match));
        }

        return matchesData;
    }

    public static IList<EventPickingModel> ExcludeMatchesDeletedToday(IList<EventPickingModel> matches, DateTime requestDate)
    {
        var isRemoveDeletedMatch = requestDate == DateTime.Now.Date;
        return isRemoveDeletedMatch ? matches.Where(match => !match.IsDeleted).ToList() : matches;
    }

    public static IList<EventPickingModel> FilterLeagueName(IList<EventPickingModel> matches, string includeText)
    {
        return matches.Where(match => !match.LeagueName.ToLowerInvariant().Contains(includeText.ToLowerInvariant())).ToList();
    }

    private static bool IsChildMatch(EventPickingModel match)
    {
        return match.IsChildMatch;
    }

    private static bool Is15Mins(EventPickingModel match)
    {
        return match.LeagueName.Contains("SPECIFIC 15 MINS", StringComparison.CurrentCultureIgnoreCase)
                && !match.LeagueName.Contains("Corner", StringComparison.CurrentCultureIgnoreCase)
                && !match.LeagueName.Contains("Booking", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool Is10Mins(EventPickingModel match)
    {
        return match.LeagueName.Contains("SPECIFIC 10 MINS", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsCorners(EventPickingModel match)
    {
        return match.LeagueName.Contains(ReportConstant.CORNERS, StringComparison.CurrentCultureIgnoreCase)
               && !match.LeagueName.Contains("SPECIFIC 15 MINS NUMBER OF CORNERS", StringComparison.CurrentCultureIgnoreCase)
               && match.HomeName.Contains("No.of Corners", StringComparison.CurrentCultureIgnoreCase)
               && !match.HomeName.Contains(" Over", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsNextCorners(EventPickingModel match)
    {
        return match.LeagueName.Contains(ReportConstant.CORNERS, StringComparison.CurrentCultureIgnoreCase)
               && match.HomeName.Contains("Next Corner", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool Is15MinsCorners(EventPickingModel match)
    {
        return match.LeagueName.Contains("SPECIFIC 15 MINS NUMBER OF CORNERS", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsBookings(EventPickingModel match)
    {
        return match.LeagueName.Contains("BOOKING", StringComparison.CurrentCultureIgnoreCase)
                 && !match.LeagueName.Contains("SPECIFIC 15 MINS TOTAL BOOKINGS", StringComparison.CurrentCultureIgnoreCase)
                 && match.HomeName.Contains("Total Bookings", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsNextBookings(EventPickingModel match)
    {
        return match.LeagueName.Contains("BOOKING", StringComparison.CurrentCultureIgnoreCase)
                 && match.HomeName.Contains("Next Booking", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool Is15MinsBookings(EventPickingModel match)
    {
        return match.LeagueName.Contains("SPECIFIC 15 MINS TOTAL BOOKINGS", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsFirstPenalty(EventPickingModel match)
    {
        return match.LeagueName.Contains("PENALTY SHOOT-OUT - WHICH TEAM TO TAKE THE FIRST PENALTY", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsBronzeMedal(EventPickingModel match)
    {
        return match.LeagueName.Contains("BRONZE MEDAL WINNER", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsGoldMedal(EventPickingModel match)
    {
        return match.LeagueName.Contains("GOLD MEDAL WINNER", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsWinner(EventPickingModel match)
    {
        return match.LeagueName.Contains("WINNER", StringComparison.CurrentCultureIgnoreCase)
                && !match.LeagueName.Contains("MEDAL WINNER", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsAdvanceNextRound(EventPickingModel match)
    {
        return match.LeagueName.Contains("WHICH TEAM WILL ADVANCE TO NEXT ROUND", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsLastCorners(EventPickingModel match)
    {
        return match.LeagueName.Contains(ReportConstant.CORNERS, StringComparison.CurrentCultureIgnoreCase)
                 && match.HomeName.Contains("Last Corner", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsLastBookings(EventPickingModel match)
    {
        return match.LeagueName.Contains("BOOKING", StringComparison.CurrentCultureIgnoreCase)
                 && match.HomeName.Contains("Last Booking", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsSingleTeamOU(EventPickingModel match)
    {
        return match.LeagueName.Contains("SINGLE TEAM OVER/UNDER", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsHomeSingleTeamOUOfCorners(EventPickingModel match)
    {
        return match.LeagueName.Contains(ReportConstant.CORNERS, StringComparison.CurrentCultureIgnoreCase)
                && match.HomeName.Contains("No.of Corners - Over", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool Is1stPenaltyScore(EventPickingModel match)
    {
        return match.LeagueName.Contains("PENALTY", StringComparison.CurrentCultureIgnoreCase)
                && match.AwayName.Contains("Score", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool Is1stPenaltyAwarded(EventPickingModel match)
    {
        return match.LeagueName.Contains("PENALTY", StringComparison.CurrentCultureIgnoreCase)
                && match.AwayName.Contains("Awarded", StringComparison.CurrentCultureIgnoreCase);
    }
    private static bool IsPenaltyShootOut(EventPickingModel match)
    {
        return match.LeagueName.Contains("PENALTY SHOOT-OUT", StringComparison.CurrentCultureIgnoreCase)
                && !match.LeagueName.Contains("WHICH TEAM TO TAKE THE FIRST PENALTY", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsNextGoalKick(EventPickingModel match)
    {
        return match.LeagueName.Contains("Goal Kick", StringComparison.CurrentCultureIgnoreCase)
                && match.HomeName.Contains("Next", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsLastGoalKick(EventPickingModel match)
    {
        return match.LeagueName.Contains("Goal Kick", StringComparison.CurrentCultureIgnoreCase)
                && match.HomeName.Contains("Last", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool Is2H1stGoalKick(EventPickingModel match)
    {
        return match.LeagueName.Contains("Goal Kick", StringComparison.CurrentCultureIgnoreCase)
                && match.HomeName.Contains("2nd", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsLastOffside(EventPickingModel match)
    {
        return match.LeagueName.Contains("Offside", StringComparison.CurrentCultureIgnoreCase)
                && match.HomeName.Contains("Last", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool Is2H1stOffside(EventPickingModel match)
    {
        return match.LeagueName.Contains("Offside", StringComparison.CurrentCultureIgnoreCase)
                && match.HomeName.Contains("2nd", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsLastThrowIn(EventPickingModel match)
    {
        return match.LeagueName.Contains("Throw In", StringComparison.CurrentCultureIgnoreCase)
                && match.HomeName.Contains("Last", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool Is2H1stThrowIn(EventPickingModel match)
    {
        return match.LeagueName.Contains("Throw In", StringComparison.CurrentCultureIgnoreCase)
                && match.HomeName.Contains("2nd", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsLastFreeKick(EventPickingModel match)
    {
        return match.LeagueName.Contains("Free Kick", StringComparison.CurrentCultureIgnoreCase)
                && match.HomeName.Contains("Last", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool Is2H1stFreeKick(EventPickingModel match)
    {
        return match.LeagueName.Contains("Free Kick", StringComparison.CurrentCultureIgnoreCase)
                && match.HomeName.Contains("2nd", StringComparison.CurrentCultureIgnoreCase);
    }

    private static bool IsSpecialGoal(EventPickingModel match)
    {
        return match.LeagueName.Contains("Specials Goal", StringComparison.CurrentCultureIgnoreCase);
    }
}