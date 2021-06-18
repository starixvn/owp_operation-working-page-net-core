using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Services.OtherAssignment
{
    public interface ChangedValueOtherHdpDetector
    {
        void Detect(
          AssignmentSnapshot oldSnapshot,
          AssignmentSnapshot newSnapshot,
          IList<ChangedValue> changedValues,
          int receivedUserId);
    }

    public class ChangedValueOtherHdpDetectorImp : ChangedValueOtherHdpDetector
    {
        public void Detect(
          AssignmentSnapshot oldSnapshot,
          AssignmentSnapshot newSnapshot,
          IList<ChangedValue> changedValues,
          int receivedUserId)
        {
            if (!string.Equals(oldSnapshot.OtherHdp, newSnapshot.OtherHdp))
            {
                changedValues.Add(new ChangedValue
                {
                    ReceivedUserIds = new List<int>(receivedUserId),
                    BetTypeName = Bettype.HDP,
                    OldValue = oldSnapshot.OtherHdp,
                    NewValue = newSnapshot.OtherHdp,
                    MessageTypeId = AssignmentHelper.DetectMessageType(newSnapshot.IsDay.Value)
                });
            }
        }
    }
}
