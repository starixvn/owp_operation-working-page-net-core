namespace OWP.TicketCenter.Criteria
{
    public class InsertNewTicketMatchNotificationCriteria : InsertNotificationMessageCriteria
    {
        public override string GetSettingKey()
            => "OWP_TicketCenter_InsertNewTicketMatchNotification";
    }
}
