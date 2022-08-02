namespace OWP.Models
{
    public class Message
    {
        public int UserId { get; set; }

        public bool IsImportant { get; set; }

        public int MessageType { get; set; }

        public string MessageContent { get; set; }
    }
}
