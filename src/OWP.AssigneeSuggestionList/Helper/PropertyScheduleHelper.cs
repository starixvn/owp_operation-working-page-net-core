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

    public static void UpdateWorkingSchedule(this WorkingSchedule schedule, AssignmentModel assignment, AssignmentCell assignmentCell)
    {
        var propertyName = GetPropertyName(assignment.EventDateTime, assignment.EventDate);
        typeof(WorkingSchedule).GetProperty(propertyName)?.SetValue(schedule, assignmentCell);
    }

    public static string GetPropertyName(DateTime eventDateTime, DateTime eventDate)
    {
        var assignmentDateTime = eventDate.Date.AddHours(12);
        string prefix;
        if (eventDateTime >= assignmentDateTime)
        {
            prefix = eventDateTime < assignmentDateTime.AddDays(1) ? "Today" : "Tomorrow";
        }
        else
        {
            prefix = "Yesterday";
        }

        return $"{prefix}_{eventDateTime.Hour:HH}h";
    }
}
