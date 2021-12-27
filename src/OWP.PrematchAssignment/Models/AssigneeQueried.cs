namespace OWP.PrematchAssignment.Models
{
    public class AssigneeQueried
    {
        public string NickName
        { get; set; }

        public int? RecipientId
        { get; set; }

        public int RecipientType => (int)Enum.RecipientType.Individual;

        public string UserName
        { get; set; }
    }
}
