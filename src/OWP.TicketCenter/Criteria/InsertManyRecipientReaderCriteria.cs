using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class InsertManyRecipientReaderCriteria: NonQueryCommand
    {
        public int TicketId { get; set; }

        public string GroupIds { get; set; }

        public string IndividualIds { get; set; }

        public bool IsRead { get; set; }

        public override string GetSettingKey()
            => "OWP_TicketCenter_InsertManyReader";


        public override bool IsValid()
            => TicketId > 0;
    }
}
