using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Services.MyAssignment
{
    public interface ChangedValueOuDetector
    {
        void DetectMain(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot,
            List<ChangedValue> changedValues);

        void DetectOther(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot,
            List<ChangedValue> changedValues);
    }

    public class ChangedValueOuDetectorImp : ChangedValueOuDetector
    {
        public void DetectMain(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot,
            List<ChangedValue> changedValues)
        {
            AssignmentHelper.DetectChangedValue(new MyAssignmentDetectorModel
            {
                LeagueName = oldSnapshot.LeagueName,
                IsDay = oldSnapshot.IsDay,
                oldAssignment = oldSnapshot.Ou,
                newAssignment = newSnapshot.Ou,
                BettypeName = Bettype.OU,
                UserIdNewAssign = newSnapshot.OuUserId.Value,
                UserIdRemoved = oldSnapshot.OuUserId.Value
            }, changedValues);
        }

        public void DetectOther(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot, List<ChangedValue> changedValues)
            => AssignmentHelper.DetectChangedValue(new MyAssignmentDetectorModel
            {
                LeagueName = oldSnapshot.LeagueName,
                IsDay = oldSnapshot.IsDay,
                oldAssignment = oldSnapshot.OtherOu,
                newAssignment = newSnapshot.OtherOu,
                BettypeName = Bettype.OU,
                UserIdNewAssign = newSnapshot.OtherOuUserId.Value,
                UserIdRemoved = oldSnapshot.OtherOuUserId.Value
            }, changedValues);
    }
}
