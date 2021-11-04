using OWP.RecipientGroup.Const;

namespace OWP.RecipientGroup.Models
{
    public class Recipient
    {
        public int RecipientGroupId { get; set; } = GroupConst.DefaultGroupIdForNewRecipientIndividual;

        public int RecipientId { get; set; }

        public string RecipientName { get; set; }

        public int RecipientType { get; set; }
    }
}
