namespace OWP.AssigneeSuggestionList.Models
{
    public record WorkingScheduleUpdateModel
    {
        public IList<string> PropertiesName { get; set; }

        public required AssignmentCell AssignmentCell { get; set; }
    }
}