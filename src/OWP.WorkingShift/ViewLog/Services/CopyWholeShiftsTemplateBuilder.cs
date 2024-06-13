namespace OWP.WorkingShift.ViewLog.Services
{
    using System.Collections.Generic;
    using System.Text;
    using Models;

    public class CopyWholeShiftsTemplateBuilder : TemplateBuilder
    {
        public CopyWholeShiftsTemplateBuilder(LogService logService, LogData logData)
            : base(logService, logData)
        { }

        public override string BuildNewColumn()
        {
            if (string.IsNullOrEmpty(logData.UserLog.Data))
            {
                return string.Empty;
            }
            var data = logService.Parse<CopyWholeShiftsModel>(logData.UserLog.Data);
            return BuildColumn(data);
        }

        public override string BuildOldColumn()
        {
            if (string.IsNullOrEmpty(logData.UserLog.DataOld))
            {
                return string.Empty;
            }
            var data = logService.Parse<CopyWholeShiftsModel>(logData.UserLog.DataOld);
            return BuildColumn(data);
        }

        private string BuildColumn(IEnumerable<CopyWholeShiftsModel> data)
        {
            var html = new StringBuilder();
            foreach (var row in data)
            {
                var shiftCode = GetShiftCode(row.ShiftId);
                var refShift = !string.IsNullOrEmpty(row.RefShiftId) ? $"/{GetShiftCode(row.RefShiftId)}" : string.Empty;
                var logRow = string.IsNullOrEmpty(shiftCode) && string.IsNullOrEmpty(refShift) ? string.Empty : $"{row.ShiftDate.ToString("MM/dd/yyyy")}: {shiftCode}{refShift}<br/>";
                html.Append(logRow);
            }
            return html.ToString();
        }
    }
}