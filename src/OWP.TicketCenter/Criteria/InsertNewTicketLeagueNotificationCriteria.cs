namespace OWP.TicketCenter.Criteria
{
    public class InsertNewTicketLeagueNotificationCriteria: InsertNotificationMessageCriteria
    {
        public override string GetSettingKey()
            => "OWP_TicketCenter_InsertNewTicketLeagueNotification";
    }
}
