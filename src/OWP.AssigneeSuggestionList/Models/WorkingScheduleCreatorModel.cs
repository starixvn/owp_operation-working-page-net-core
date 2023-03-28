namespace OWP.AssigneeSuggestionList.Models
{
    public record WorkingScheduleUpdateModel
    {
        public IList<string> PropertiesName { get; set; }

        public AssignmentCell AssignmentCell { get; set; }
    }
}