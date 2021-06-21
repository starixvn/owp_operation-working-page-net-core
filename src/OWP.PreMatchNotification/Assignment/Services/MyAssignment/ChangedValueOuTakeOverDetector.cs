using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Services.MyAssignment
{
    public interface ChangedValueOuTakeOverDetector
    {
        void DetectMain(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot,
            List<ChangedValue> changedValues);

        void DetectOther(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot,
            List<ChangedValue> changedValues);
    }

    public class ChangedValueOuTakeOverDetectorImp : ChangedValueOuTakeOverDetector
    {
        public void DetectMain(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues)
            => AssignmentHelper.DetectChangedValue(new MyAssignmentDetectorModel
            {
                LeagueName = oldSnapshot.LeagueName,
                IsDay = oldSnapshot.IsDay,
                oldAssignment = oldSnapshot.OuTakeOver,
                newAssignment = newSnapshot.OuTakeOver,
                BettypeName = Bettype.OU_TAKEOVER,
                UserIdNewAssign = newSnapshot.OuTakeOverUserId.Value,
                UserIdRemoved = oldSnapshot.OuTakeOverUserId.Value
            }, changedValues);

        public void DetectOther(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues)
            => AssignmentHelper.DetectChangedValue(new MyAssignmentDetectorModel
            {
                LeagueName = oldSnapshot.LeagueName,
                IsDay = oldSnapshot.IsDay,
                oldAssignment = oldSnapshot.OtherOuTakeOver,
                newAssignment = newSnapshot.OtherOuTakeOver,
                BettypeName = Bettype.OU_TAKEOVER,
                UserIdNewAssign = newSnapshot.OtherOuTakeOverUserId.Value,
                UserIdRemoved = oldSnapshot.OtherOuTakeOverUserId.Value
            }, changedValues);
    }
}
