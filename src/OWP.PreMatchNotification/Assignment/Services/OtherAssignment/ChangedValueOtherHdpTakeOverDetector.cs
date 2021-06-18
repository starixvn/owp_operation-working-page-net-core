using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Services.OtherAssignment
{
    public interface ChangedValueOtherHdpTakeOverDetector
    {
        void Detect(
          AssignmentSnapshot oldSnapshot,
          AssignmentSnapshot newSnapshot,
          IList<ChangedValue> changedValues,
          int receivedUserId);
    }

    public class ChangedValueOtherHdpTakeOverDetectorImp : ChangedValueOtherHdpTakeOverDetector
    {
        public void Detect(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, IList<ChangedValue> changedValues, int receivedUserId)
        {
            if (!string.Equals(oldSnapshot.OtherHdpTakeOver, newSnapshot.OtherHdpTakeOver))
            {
                changedValues.Add(new ChangedValue
                {
                    ReceivedUserIds = new List<int>(receivedUserId),
                    BetTypeName = Bettype.HDP_TAKEOVER,
                    OldValue = oldSnapshot.OtherHdpTakeOver,
                    NewValue = newSnapshot.OtherHdpTakeOver,
                    MessageTypeId = AssignmentHelper.DetectMessageType(newSnapshot.IsDay.Value)
                });
            }
        }
    }
}
