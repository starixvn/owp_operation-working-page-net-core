namespace OWP.AssigneeSuggestionList.Models
{
    public class ScheduleUpdateModel
    {
        public DateTime EventDate { get; set; }

        public ETPKUpdateSchedule EtPk { get; set; } = new ETPKUpdateSchedule();

        public IList<string> PropertiesName { get; set; }

        public AssignmentCell AssignmentCell { get; set; }

        public (DateTime startDate, DateTime endDate) TimeRange { get; set; }
    }
}