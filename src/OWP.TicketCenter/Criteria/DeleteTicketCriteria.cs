using System;
namespace OWP.TicketCenter.Criteria
{
    using System;
    using Fanex.Data.Repository;
    public class DeleteTicketCriteria: NonQueryCommand
    {
        public int UserId { get; set; }
        public int TicketId { get; set; }
        public DateTime EventDate { get; set;}
        public override string GetSettingKey() => "OWP_TicketCenter_DeleteTicket";
        public override bool IsValid() => true;
    }
}