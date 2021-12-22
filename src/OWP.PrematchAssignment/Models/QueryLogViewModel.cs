namespace OWP.PreMatchAssignment.Models
{
    using System.Collections.Generic;

    public class QueryLogViewModel
    {
        public IEnumerable<Log> Logs { get; set; }

        public bool IsLastPage { get; set; }
    }
}