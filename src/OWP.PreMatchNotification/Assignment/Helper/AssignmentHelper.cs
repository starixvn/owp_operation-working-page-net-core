using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Helper
{
    public static class AssignmentHelper
    {
        public static ChangedValue GenerateChangedValue(MyAssignmentDetectorModel model)
        {
            if (!string.Equals(model.oldAssignment, model.newAssignment) &&
                !string.IsNullOrWhiteSpace(model.newAssignment))
            {
                return new ChangedValue
                {
                    BetTypeName = model.BettypeName,
                    ReceivedUserIds = new List<int>(model.ReceivedUserId),
                };
            }
            return new ChangedValue();
        }
    }
}
