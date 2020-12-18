using System.Collections.Generic;

namespace OWP.OtherSport.Validation.Models
{
    public class UploadingDataFromExcel
    {
        public List<AssignmentModelInExcel> Assignments { get; set; }

        public List<string> Traders { get; set; }
    }
}