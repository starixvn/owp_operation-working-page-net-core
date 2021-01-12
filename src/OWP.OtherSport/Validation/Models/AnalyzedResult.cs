using System.Collections.Generic;

namespace OWP.OtherSport.Validation.Models
{
    public class AnalyzedResult
    {
        public List<AssignmentModel> Assignments { get; set; }

        public int OriginalRecordCount { get; set; }
    }
}