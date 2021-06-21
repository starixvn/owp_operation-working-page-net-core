using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Services.MyAssignment
{
    public interface ChangedValueHdpDetector
    {
        void DetectMain(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues);

        void DetectOther(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues);
    }

    public class ChangedValueHdpDetectorImp : ChangedValueHdpDetector
    {
        public void DetectMain(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues)
            => AssignmentHelper.DetectChangedValue(new MyAssignmentDetectorModel
            {
                LeagueName = oldSnapshot.LeagueName,
                IsDay = oldSnapshot.IsDay,
                OldAssignment = oldSnapshot.Hdp,
                NewAssignment = newSnapshot.Hdp,
                BettypeName = Bettype.HDP,
                UserIdNewAssign = newSnapshot.HdpUserId.Value,
                UserIdRemoved = oldSnapshot.HdpUserId.Value
            }, changedValues);

        public void DetectOther(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues)
            => AssignmentHelper.DetectChangedValue(new MyAssignmentDetectorModel
            {
                LeagueName = oldSnapshot.LeagueName,
                IsDay = oldSnapshot.IsDay,
                OldAssignment = oldSnapshot.OtherHdp,
                NewAssignment = newSnapshot.OtherHdp,
                BettypeName = Bettype.HDP,
                UserIdNewAssign = newSnapshot.OtherHdpUserId.Value,
                UserIdRemoved = oldSnapshot.OtherHdpUserId.Value
            }, changedValues);
    }
}
