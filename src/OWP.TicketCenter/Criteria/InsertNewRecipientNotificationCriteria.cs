using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data.Repository;

namespace OWP.TicketCenter.Criteria
{
    public class InsertNewRecipientNotificationCriteria : InsertNotificationMessageCriteria
    {
        public int ReceivedUserId { get; set; }

        public override string GetSettingKey() => "OWP_TicketCenter_InsertNewRecipientNotification";

        public override bool IsValid()
            => NotificationContent.Length > 0 && ReceivedUserId > 0;
    }
}