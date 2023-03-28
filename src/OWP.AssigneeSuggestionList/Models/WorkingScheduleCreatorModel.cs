namespace OWP.AssigneeSuggestionList.Models
{
    public record WorkingScheduleCreatorModel
    {
        public IList<string> PropertiesName { get; set; }

        public required AssignmentCell AssignmentCell { get; set; }

        public required int UserId { get; set; }
    }
}