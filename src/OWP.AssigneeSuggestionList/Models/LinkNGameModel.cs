namespace OWP.AssigneeSuggestionList.Models
{
    public class LinkNGameModel
    {
        public IList<AssignmentModel> Assignments { get; set; }

        public IList<MaxbetModel> MaxbetModels { get; set; }

        public int NumberGame { get; set; }

        public string ColorCode { get; set; }
    }
}