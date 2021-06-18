using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Services
{
    public interface MyAssignmentDetector
    {
        IList<ChangedValue> DetectChangeAssignmentHdp(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOu(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentHdpTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOuTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOtherHdp(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOtherOu(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOtherHdpTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOtherOuTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);
    }

    public class MyAssignmentDetectorImp: MyAssignmentDetector
    {
        public IList<ChangedValue> DetectChangeAssignmentHdp(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
           => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
           {
               LeagueName= oldSnapshot.LeagueName,
               IsDay= oldSnapshot.IsDay,
               oldAssignment = oldSnapshot.Hdp,
               newAssignment = newSnapshot.Hdp,
               BettypeName = Bettype.HDP,
               UserIdNewAssign = newSnapshot.HdpUserId.Value,
               UserIdRemoved= oldSnapshot.HdpUserId.Value
           });

        public IList<ChangedValue> DetectChangeAssignmentOu(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
           => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
           {
               LeagueName = oldSnapshot.LeagueName,
               IsDay = oldSnapshot.IsDay,
               oldAssignment = oldSnapshot.Ou,
               newAssignment = newSnapshot.Ou,
               BettypeName = Bettype.OU,
               UserIdNewAssign = newSnapshot.OuUserId.Value,
               UserIdRemoved = oldSnapshot.OuUserId.Value
           });

        public IList<ChangedValue> DetectChangeAssignmentHdpTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              LeagueName = oldSnapshot.LeagueName,
              IsDay = oldSnapshot.IsDay,
              oldAssignment = oldSnapshot.HdpTakeOver,
              newAssignment = newSnapshot.HdpTakeOver,
              BettypeName = Bettype.HDP_TAKEOVER,
              UserIdNewAssign = newSnapshot.HdpTakeOverUserId.Value,
              UserIdRemoved = oldSnapshot.HdpTakeOverUserId.Value
          });

        public IList<ChangedValue> DetectChangeAssignmentOuTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              LeagueName = oldSnapshot.LeagueName,
              IsDay = oldSnapshot.IsDay,
              oldAssignment = oldSnapshot.OuTakeOver,
              newAssignment = newSnapshot.OuTakeOver,
              BettypeName = Bettype.OU_TAKEOVER,
              UserIdNewAssign = newSnapshot.OuTakeOverUserId.Value,
              UserIdRemoved = oldSnapshot.OuTakeOverUserId.Value
          });

        public IList<ChangedValue> DetectChangeAssignmentOtherHdp(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              LeagueName = oldSnapshot.LeagueName,
              IsDay = oldSnapshot.IsDay,
              oldAssignment = oldSnapshot.OtherHdp,
              newAssignment = newSnapshot.OtherHdp,
              BettypeName = Bettype.HDP,
              UserIdNewAssign = newSnapshot.OtherHdpUserId.Value,
              UserIdRemoved = oldSnapshot.OtherHdpUserId.Value
          });

        public IList<ChangedValue> DetectChangeAssignmentOtherOu(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              LeagueName = oldSnapshot.LeagueName,
              IsDay = oldSnapshot.IsDay,
              oldAssignment = oldSnapshot.OtherOu,
              newAssignment = newSnapshot.OtherOu,
              BettypeName = Bettype.OU,
              UserIdNewAssign = newSnapshot.OtherOuUserId.Value,
              UserIdRemoved = oldSnapshot.OtherOuUserId.Value
          });

        public IList<ChangedValue> DetectChangeAssignmentOtherHdpTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              LeagueName = oldSnapshot.LeagueName,
              IsDay = oldSnapshot.IsDay,
              oldAssignment = oldSnapshot.OtherHdpTakeOver,
              newAssignment = newSnapshot.Ou,
              BettypeName = Bettype.OU,
              UserIdNewAssign = newSnapshot.OtherHdpTakeOverUserId.Value,
              UserIdRemoved = oldSnapshot.OtherHdpTakeOverUserId.Value
          });

        public IList<ChangedValue> DetectChangeAssignmentOtherOuTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              LeagueName = oldSnapshot.LeagueName,
              IsDay = oldSnapshot.IsDay,
              oldAssignment = oldSnapshot.OtherOuTakeOver,
              newAssignment = newSnapshot.Ou,
              BettypeName = Bettype.OU,
              UserIdNewAssign = newSnapshot.OtherOuTakeOverUserId.Value,
              UserIdRemoved = oldSnapshot.OtherOuTakeOverUserId.Value
          });
    }
}
