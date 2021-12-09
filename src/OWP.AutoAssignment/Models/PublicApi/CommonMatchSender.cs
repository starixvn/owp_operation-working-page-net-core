namespace OWP.AutoAssignment.Models.PublicApi
{
    public class CommonMatchSender
    {
        public int[] PickedBettypes { get; set; } = new[] { 0 };

        public string KOTime { get; set; }

        public int MatchId { get; set; }
    }
}
