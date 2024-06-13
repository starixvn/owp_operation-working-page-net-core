namespace OWP.WorkingShift.WeeklyWorkingShift.Models
{
    using System.Collections.Generic;

    public class ValidateImportResult
    {
        public IList<WorkingShiftImportEntity> Shifts { get; set; }

        public IList<ImportInvalidCell> InvalidCells { get; set; }
    }
}
