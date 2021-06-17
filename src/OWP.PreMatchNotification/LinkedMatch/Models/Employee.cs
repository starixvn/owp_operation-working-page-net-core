using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.LinkedMatch.Models
{
    public class Employee
    {
        public Dictionary<int, InfositePrematchLeague> PrematchInfosites { get; set; }

        public IList<Assignee> AllPrematchEmployees { get; set; }

        public IList<Assignee> AllLSTEmployees { get; set; }

        public Employee()
        {
            PrematchInfosites = new Dictionary<int, InfositePrematchLeague>();
            AllLSTEmployees = new List<Assignee>();
            AllPrematchEmployees = new List<Assignee>();
        }
    }
}
