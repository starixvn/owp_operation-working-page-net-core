
namespace OWP.AssigneeSuggestionList.Models
{
    public record MaxbetModel
    {
        public int OWPMatchId { get; set; }

        public decimal Maxbet { get; set; }

        public int BettypeId { get; set; }
    }
}