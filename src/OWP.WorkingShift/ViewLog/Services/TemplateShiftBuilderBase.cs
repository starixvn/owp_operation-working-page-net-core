namespace OWP.WorkingShift.ViewLog.Services
{
    using System.Collections.Generic;
    using Models;
    using OWP.WorkingShift.ViewLog.Models;

    public class TemplateShiftBuilderBase : TemplateBuilder
    {
        public TemplateShiftBuilderBase(LogService logService, LogData logData) : base(logService, logData)
        {
        }

        public override string BuildOldColumn()
        {
            return BuildColumn(logData.UserLog.DataOld);
        }

        public override string BuildNewColumn()
        {
            return BuildColumn(logData.UserLog.Data);
        }

        private string BuildColumn(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return string.Empty;
            }
            var result = new List<string>();
            var shiftModel = logService.Parse<ShiftModelBase>(data);
            foreach (var model in shiftModel)
            {
                var shiftCodeFormat = !string.IsNullOrWhiteSpace(model.RefShiftId) ?
                    $"{GetShiftCode(model.ShiftId)}/{GetShiftCode(model.RefShiftId)}" :
                    $"{GetShiftCode(model.ShiftId)}";
                if (!string.IsNullOrWhiteSpace(shiftCodeFormat))
                {
                    result.Add($"{model.Date.ToString("MM/dd/yyyy")}: {shiftCodeFormat}");
                }
            }
            return string.Join("<br>", result);
        }
    }
}