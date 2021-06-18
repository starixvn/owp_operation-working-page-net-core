using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Services.OtherAssignment
{
    public interface ChangedValueHdpTakeOverDetector
    {
        void Detect(
          AssignmentSnapshot oldSnapshot,
          AssignmentSnapshot newSnapshot,
          IList<ChangedValue> changedValues,
          int receivedUserId);
    }

    public class ChangedValueHdpTakeOverDetectorImp : ChangedValueHdpTakeOverDetector
    {
        public void Detect(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, IList<ChangedValue> changedValues, int receivedUserId)
        {
            if (!string.Equals(oldSnapshot.HdpTakeOver, newSnapshot.HdpTakeOver))
            {
                changedValues.Add(new ChangedValue
                {
                    ReceivedUserIds = new List<int>(receivedUserId),
                    BetTypeName = Bettype.HDP_TAKEOVER,
                    OldValue = oldSnapshot.HdpTakeOver,
                    NewValue = newSnapshot.HdpTakeOver,
                    MessageTypeId = AssignmentHelper.DetectMessageType(newSnapshot.IsDay.Value)
                });
            }
        }
    }
}
