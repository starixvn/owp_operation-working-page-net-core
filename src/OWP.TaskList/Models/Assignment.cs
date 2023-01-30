using Newtonsoft.Json;

namespace OWP.TaskList.Models
{

    public class Assignment
    {
        [JsonIgnore]
        public int OWPMatchId { get; set; }

        [JsonIgnore]
        public int ParentMatchId { get; set; }

        [JsonIgnore]
        public int? MinorBettypeId { get; set; }

        public int BettypeId { get; set; }

        public int AssignmentType { get; set; }

        public string AssignmentNote { get; set; }

        public int UserId { get; set; }

        public bool IsCurrentUser { get; set; }

        public string Username { get; set; }

        public string Skype { get; set; }

        public string Extension { get; set; }

        public string Phone { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsHasContactInfo
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Skype) || !string.IsNullOrWhiteSpace(Phone) || !string.IsNullOrWhiteSpace(Extension);
            }
        }
    }
}
