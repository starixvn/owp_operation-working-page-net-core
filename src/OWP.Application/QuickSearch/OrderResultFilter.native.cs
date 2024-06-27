namespace OWP.Application.QuickSearch;

using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;


public static class OrderResultFilter
{
    private static readonly DateTime currentTimeGmt8 = DateTime.Now.ToGmt8();

    private static readonly IList<MatchInfoModel> past = new List<MatchInfoModel>();

    private static IList<MatchInfoModel> current = new List<MatchInfoModel>();

    private static IList<MatchInfoModel> future = new List<MatchInfoModel>();

    public static IList<MatchInfoModel> Order(IEnumerable<MatchInfoModel> matchInfo)
    {
        ClearData();
        if (!matchInfo.Any())
        {
            return new List<MatchInfoModel>();
        }
        var startDate = currentTimeGmt8.Date.AddMinutes(-1); // MM/dd-1/yyyy 11:59:00 PM
        var endTimeMorning = startDate.AddDays(0.5);// MM/dd/yyyy 11:59:00 AM
        var endDate = currentTimeGmt8.Date.AddDays(1).AddMinutes(-1); // MM/dd/yyyy 11:59:00 PM
        if (startDate < currentTimeGmt8 && currentTimeGmt8 <= endTimeMorning)
        {
            // Filter morning
            return OrderResultFilterMorning(matchInfo.ToList());
        }

        if (endTimeMorning < currentTimeGmt8 && currentTimeGmt8 <= endDate)
        {
            // Filter afternoon
            return OrderResultAfternoon(matchInfo.ToList());
        }
        return matchInfo.ToList();
    }

    private static void ClearData()
    {
        past.Clear();
        current.Clear();
        future.Clear();
    }

    private static IList<MatchInfoModel> OrderResultFilterMorning(IList<MatchInfoModel> matchInfo)
    {
        var startDate = currentTimeGmt8.Date.AddMinutes(-1).AddDays(-0.5);// MM/dd-1/yyyy 11:59:00 AM
        var endDate = currentTimeGmt8.Date.AddHours(35).AddMinutes(59);// MM/dd+1/yyyy 11:59:00 AM

        return Execute(startDate, endDate, matchInfo);
    }

    private static IList<MatchInfoModel> OrderResultAfternoon(IList<MatchInfoModel> matchInfo)
    {
        var startDate = currentTimeGmt8.Date.AddHours(11).AddMinutes(59);// MM/dd/yyyy 11:59:00 AM
        var endDate = currentTimeGmt8.Date.AddDays(1.5).AddMinutes(-1); // MM/dd+1/yyyy 11:59:00 AM
        return Execute(startDate, endDate, matchInfo);
    }

    private static IList<MatchInfoModel> Execute(DateTime startDate, DateTime endDate, IList<MatchInfoModel> matchInfo)
    {
        foreach (var match in matchInfo)
        {
            if (match.EventDate <= startDate)
            {
                past.Add(match);
            }
            if (startDate < match.EventDate && match.EventDate <= endDate)
            {
                current.Add(match);
            }
            if (match.EventDate > endDate)
            {
                future.Add(match);
            }
        }
        current = current.OrderBy(m => m.EventDate).ToList();
        future = future.OrderBy(m => m.EventDate).ToList();
        var result = current.Concat(future).Concat(past);
        return result.ToList();
    }
}