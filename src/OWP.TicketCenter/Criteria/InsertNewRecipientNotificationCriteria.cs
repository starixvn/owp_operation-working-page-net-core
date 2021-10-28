using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class InsertNewRecipientNotificationCriteria : NonQueryCommand
    {
        public string NotificationContent { get; set; }
        public int ActorId { get; set; }
        public int ReceivedUserId { get; set; }
        public bool IsImportant { get; set; }

        public bool IsWatcher { get; set; }
        public string SessionId { get; set; }

        public override string GetSettingKey() => "OWP_TicketCenter_InsertNewRecipientNotification";

        public override bool IsValid() => true;
    }
}