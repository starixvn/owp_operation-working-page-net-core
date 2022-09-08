namespace OWP.LiveDetail.Assignment
{
    public class Assignment
    {
        public int AssignmentId { get; set; }

        public int OWPMatchId { get; set; }

        public int Bettype { get; set; }

        public int MinorBettypeId { get; set; }

        public byte AssignmentType { get; set; }

        public string DisplayName { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string AssignmentNote { get; set; }
    }
}