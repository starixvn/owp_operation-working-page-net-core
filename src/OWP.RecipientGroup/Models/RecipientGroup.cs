using System.Collections.Generic;

namespace OWP.RecipientGroup.Models
{
    public class RecipientGroup
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public List<User> Recipients { get; set; }
    }
}