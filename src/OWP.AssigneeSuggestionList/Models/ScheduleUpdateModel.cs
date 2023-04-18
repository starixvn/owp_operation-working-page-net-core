namespace OWP.AssigneeSuggestionList.Models;

public class ScheduleUpdateModel
{
    public DateTime FilterDate { get; set; }

    public ETPKUpdateSchedule EtPk { get; set; } = new();

    public IList<string> PropertiesName { get; set; }

    public AssignmentCell AssignmentCell { get; set; }

    public (DateTime startDate, DateTime endDate) TimeRange { get; set; }
}