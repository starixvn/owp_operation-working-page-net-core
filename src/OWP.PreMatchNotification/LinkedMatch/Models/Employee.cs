using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.LinkedMatch.Models
{
    public class Employee
    {
        public Dictionary<int, InfositePrematchLeague> PrematchInfosites { get; set; }

        public List<int> AllPrematchEmployees { get; set; }

        public List<int> AllLSTEmployees { get; set; }

        public Employee()
        {
            PrematchInfosites = new Dictionary<int, InfositePrematchLeague>();
            AllLSTEmployees = new List<int>();
            AllPrematchEmployees = new List<int>();
        }
    }
}
