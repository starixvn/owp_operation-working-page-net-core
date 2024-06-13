namespace OWP.WorkingShift.WeeklyWorkingShift.Models
{
    using System.Collections.Generic;
    using OWP.Models;

    public class WorkingShiftExportModel
    {
        public User User { get; set; }

        public TimezoneSettingModel TimezoneSetting { get; set; }

        public IList<WorkingShiftUserModel> WorkingShiftUsers { get; set; }

        public IList<WorkingShiftColorModel> WorkingShiftColors { get; set; }

        public IList<WorkingShiftModel> WorkingShifts { get; set; }
    }
}