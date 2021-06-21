using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Constants;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Helper
{
    public static class AssignmentHelper
    {
        public static int DetectMessageType(bool isDay)
            => isDay
                    ? (int)MsgTemplateCode.DayShiftOtherAssignmentUpdate
                    : (int)MsgTemplateCode.NightShiftOtherAssignmentUpdate;

        public static void DetectChangedValue(MyAssignmentDetectorModel model, List<ChangedValue> changedValues)
        {
            DetectChangedValueNewAssignment(model, changedValues);
            DetectChangedValueDeleteAssignment(model, changedValues);
        }

        public static void DetectChangedValueNewAssignment(MyAssignmentDetectorModel model, List<ChangedValue> changedValues)
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

        public static void DetectChangedValueDeleteAssignment(MyAssignmentDetectorModel model, List<ChangedValue> changedValues)
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
