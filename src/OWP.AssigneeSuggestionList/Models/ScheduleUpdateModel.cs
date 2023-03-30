namespace OWP.AssigneeSuggestionList.Models
{
    public class ScheduleUpdateModel
    {
        public DateTime EventDate { get; set; }

        public IList<int> BettypeIds { get; set; }

        public ETPKUpdateSchedule EtPk { get; set; } = new ETPKUpdateSchedule();

        public bool Is15Mins { get; set; }

        public IList<string> PropertiesName { get; set; }

        public AssignmentCell AssignmentCell { get; set; }

    }
}