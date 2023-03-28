using OWP.AssigneeSuggestionList.Constant;

namespace OWP.AssigneeSuggestionList.Helper;
public static class AssigneeSuggestionHelper
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
}
