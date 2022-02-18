
namespace OWP.PreMatchAssignment.Models
{
    

    public class Note
    {
        public int PreMatchNoteId { get; set; }

        public int PreMatchNoteMessageId { get; set; }

        public string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Username { get; set; }
    }
}
