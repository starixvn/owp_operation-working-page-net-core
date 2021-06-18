using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Services.OtherAssignment
{
    public interface ChangedValueHdpDetector
    {
        void Detect(
          AssignmentSnapshot oldSnapshot,
          AssignmentSnapshot newSnapshot,
          IList<ChangedValue> changedValues,
          int receivedUserId);
    }

    public class ChangedValueHdpDetectorImp : ChangedValueHdpDetector
    {
        public void Detect(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, IList<ChangedValue> changedValues, int receivedUserId)
        {
            if (!string.Equals(oldSnapshot.Hdp, newSnapshot.Hdp))
            {
                changedValues.Add(new ChangedValue
                {
                    ReceivedUserIds = new List<int>(receivedUserId),
                    BetTypeName = Bettype.HDP,
                    OldValue = oldSnapshot.Hdp,
                    NewValue = newSnapshot.Hdp,
                    MessageTypeId = AssignmentHelper.DetectMessageType(newSnapshot.IsDay.Value)
                });
            }
        }
    }
}
