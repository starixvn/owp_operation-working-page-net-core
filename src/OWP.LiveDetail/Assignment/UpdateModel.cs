namespace OWP.LiveDetail.Assignment
{
    public class UpdateModel
    {
        public int AssignmentId { get; set; }

        public int? UserId { get; set; }

        public string Username { get; set; }

        public int OWPMatchId { get; set; }

        public int BetType { get; set; }

        public short AssignmentType { get; set; }

        public string AssignmentNote { get; set; }
    }
}