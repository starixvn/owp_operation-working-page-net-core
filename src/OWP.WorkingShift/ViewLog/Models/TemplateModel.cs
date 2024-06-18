namespace OWP.WorkingShift.ViewLog.Models;

using System.Collections.Generic;

public class TemplateModel
{
    public List<UserLogModel> UserLogs { get; set; }

    public Dictionary<int, string> Shifts { get; set; }
}
