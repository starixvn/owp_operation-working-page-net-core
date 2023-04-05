using OWP.AssigneeSuggestionList.Constant;
using OWP.AssigneeSuggestionList.Models;

namespace OWP.AssigneeSuggestionList.Helper;

public static class PropertyScheduleHelper
{
    public static List<string> GetValidProperties(DateTime eventTime, DateTime fromTime, DateTime toTime)
    {
        var startTime = eventTime.AddDays(-1).Date.Add(new TimeSpan(02, 00, 00));
        var fromRange = (int)(fromTime - startTime).TotalHours;
        var toRange = (int)(toTime - startTime).TotalHours;

        return CalculateValidProperties(fromRange, toRange);
    }

    public static List<string> CalculateValidProperties(int fromRange, int toRange)
    {
        var properties = new List<string>();
        while (fromRange <= toRange)
        {
            properties.Add(WorkingShiftConst.PropertyDict.GetValueOrDefault(fromRange, string.Empty));
            fromRange++;
        }

        return properties
            .Where(x => !string.IsNullOrEmpty(x))
            .ToList();
    }

    public static void UpdateWorkingSchedule(this WorkingSchedule schedule, AssignmentModel assignment, AssignmentCell assignmentCell, DateTime eventDate)
    {
        var propertyName = GetPropertyName(assignment.EventDateTime, eventDate);
        typeof(WorkingSchedule).GetProperty(propertyName)?.SetValue(schedule, assignmentCell);
    }

    public static string GetPropertyName(DateTime eventDateTime, DateTime eventDate)
    {
        var todayFromDateTime = eventDate.Date.AddHours(12);
        var todayToDateTime = todayFromDateTime.AddDays(1);

        string prefix;
        if (eventDateTime.CompareTo(todayFromDateTime) < 0)
        {
            prefix = "Yesterday";
        }
        else if (eventDateTime.CompareTo(todayToDateTime) < 0)
        {
            prefix = "Today";
        }
        else
        {
            prefix = "Tomorrow";
        }

        return $"{prefix}_{eventDateTime:HH}h";
    }
}
