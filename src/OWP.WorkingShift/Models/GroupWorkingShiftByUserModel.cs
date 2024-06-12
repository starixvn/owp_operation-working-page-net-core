namespace OWP.WorkingShift.Models
{
    using System.Collections.Generic;

    public class GroupWorkingShiftByUserModel
    {
        public int UserId { get; set; }

        public IList<WorkingShiftModel> WorkingShift { get; set; }
    }
}