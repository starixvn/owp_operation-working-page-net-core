using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.OtherSport.EventAssignment.Models
{
    public class Assignment
    {
        public int OSMatchId { get; set; }

        public string BetTypeName { get; set; }

        public int AssignmentType { get; set; }

        public int UserId { get; set; }

        public string Username { get; set; }

        public string AssignmentNote { get; set; }
    }
}
