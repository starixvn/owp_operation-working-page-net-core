using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Services.OtherAssignment
{
    public interface ChangedValueOtherOuTakeOverDetector
    {
        void Detect(
          AssignmentSnapshot oldSnapshot,
          AssignmentSnapshot newSnapshot,
          IList<ChangedValue> changedValues,
          int receivedUserId);
    }

    public class ChangedValueOtherOuTakeOverDetectorImp : ChangedValueOtherOuTakeOverDetector
    {
        public void Detect(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, IList<ChangedValue> changedValues, int receivedUserId)
        {
            if (!string.Equals(oldSnapshot.OtherOuTakeOver, newSnapshot.OtherOuTakeOver))
            {
                changedValues.Add(new ChangedValue
                {
                    ReceivedUserIds = new List<int>(receivedUserId),
                    BetTypeName = Bettype.OU_TAKEOVER,
                    OldValue = oldSnapshot.OtherOuTakeOver,
                    NewValue = newSnapshot.OtherOuTakeOver,
                    MessageTypeId = AssignmentHelper.DetectMessageType(newSnapshot.IsDay.Value)
                });
            }
        }
    }
}
