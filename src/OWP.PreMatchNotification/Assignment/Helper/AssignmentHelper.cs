using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Constants;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Helper
{
    public static class AssignmentHelper
    {
        public static IList<ChangedValue> GenerateChangedValue(MyAssignmentDetectorModel model)
        {
            var changedValueMyAssignment = new List<ChangedValue>();
            GenerateChangedValueNewAssignment(model, changedValueMyAssignment);
            GenerateChangedValueDeleteAssignment(model, changedValueMyAssignment);
            return changedValueMyAssignment;
        }

        public static void GenerateChangedValueNewAssignment(MyAssignmentDetectorModel model, List<ChangedValue> changedValues)
        {
            if (!string.Equals(model.oldAssignment, model.newAssignment) &&
                !string.IsNullOrWhiteSpace(model.newAssignment))
            {
                changedValues.Add(new ChangedValue
                {
                    League = model.LeagueName,
                    BetTypeName = model.BettypeName,
                    ReceivedUserIds = new List<int>(model.UserIdNewAssign),
                    MessageTypeId = model.IsDay.Value
                    ? (int)MsgTemplateCode.DayShiftAssignmentInsert
                    : (int)MsgTemplateCode.NightShiftAssignmentInsert
                });
            }
        }

        public static void GenerateChangedValueDeleteAssignment(MyAssignmentDetectorModel model, List<ChangedValue> changedValues)
        {
            if (!string.Equals(model.oldAssignment, model.newAssignment) &&
                !string.IsNullOrWhiteSpace(model.oldAssignment))
            {
                changedValues.Add(new ChangedValue
                {
                    League = model.LeagueName,
                    BetTypeName = model.BettypeName,
                    ReceivedUserIds = new List<int>(model.UserIdRemoved),
                    MessageTypeId = model.IsDay.Value
                    ? (int)MsgTemplateCode.DayShiftAssignmentDelete
                    : (int)MsgTemplateCode.NightShiftAssignmentDelete
                });
            }
        }
    }
}
