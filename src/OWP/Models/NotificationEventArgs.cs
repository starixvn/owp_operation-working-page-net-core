namespace OWP.Models
{
    using System.Collections.Generic;

    public class NotificationEventArgs
    {
        public string UserId { get; set; }

        public IList<Message> Messages { get; set; }
    }
}
