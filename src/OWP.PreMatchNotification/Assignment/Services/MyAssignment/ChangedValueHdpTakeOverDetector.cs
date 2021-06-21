using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Services.MyAssignment
{
    public interface ChangedValueHdpTakeOverDetector
    {
        void DetectMain(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues);

        void DetectOther(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues);
    }

    public class ChangedValueHdpTakeOverDetectorImp : ChangedValueHdpTakeOverDetector
    {
        public void DetectMain(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues)
            => AssignmentHelper.DetectChangedValue(new MyAssignmentDetectorModel
            {
                LeagueName = oldSnapshot.LeagueName,
                IsDay = oldSnapshot.IsDay,
                oldAssignment = oldSnapshot.HdpTakeOver,
                newAssignment = newSnapshot.HdpTakeOver,
                BettypeName = Bettype.HDP_TAKEOVER,
                UserIdNewAssign = newSnapshot.HdpTakeOverUserId.Value,
                UserIdRemoved = oldSnapshot.HdpTakeOverUserId.Value
            }, changedValues);

        public void DetectOther(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues)
            => AssignmentHelper.DetectChangedValue(new MyAssignmentDetectorModel
            {
                LeagueName = oldSnapshot.LeagueName,
                IsDay = oldSnapshot.IsDay,
                oldAssignment = oldSnapshot.OtherHdpTakeOver,
                newAssignment = newSnapshot.OtherHdpTakeOver,
                BettypeName = Bettype.HDP_TAKEOVER,
                UserIdNewAssign = newSnapshot.OtherHdpTakeOverUserId.Value,
                UserIdRemoved = oldSnapshot.OtherHdpTakeOverUserId.Value
            }, changedValues);
    }
}
