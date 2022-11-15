namespace OWP.AssignmentUpload.Validation.Models
{
    using System;

    public class Session
    {
        public string SessionId { get; set; }

        public int UserId { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public string Status { get; set; }

        public bool IsUploadFMAndAIOAssignment { get; set; }
    }
}
