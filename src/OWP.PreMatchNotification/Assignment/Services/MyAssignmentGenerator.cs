using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Helper;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;
using System.Linq;

namespace OWP.PreMatchNotification.Assignment.Services
{
    public interface MyAssignmentGenerator
    {
        IEnumerable<ChangedValue> Create(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);
    }

    public class MyAssignmentGeneratorImp : MyAssignmentGenerator
    {
        public IEnumerable<ChangedValue> Create(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            var changedValue = new List<ChangedValue>();
            var changedAssignHdp = DetectChangeAssignmentHdp(oldSnapshot, newSnapshot);
            var changedAssignOu = DetectChangeAssignmentHdp(oldSnapshot, newSnapshot);
            var changedAssignHdpTover = DetectChangeAssignmentHdpTOver(oldSnapshot, newSnapshot);
            var changedAssignOuTOver = DetectChangeAssignmentOuTOver(oldSnapshot, newSnapshot);
            var changedAssignOtherHdp = DetectChangeAssignmentOtherHdp(oldSnapshot, newSnapshot);
            var changedAssignOtherOu = DetectChangeAssignmentOtherOu(oldSnapshot, newSnapshot);
            var changedAssignOtherHdpTOver = DetectChangeAssignmentOtherHdpTOver(oldSnapshot, newSnapshot);
            var changedAssignOtherOuTOver = DetectChangeAssignmentOtherOuTOver(oldSnapshot, newSnapshot);
            changedValue.Add(changedAssignHdp);
            changedValue.Add(changedAssignOu);
            changedValue.Add(changedAssignHdpTover);
            changedValue.Add(changedAssignOuTOver);
            changedValue.Add(changedAssignOtherHdp);
            changedValue.Add(changedAssignOtherOu);
            changedValue.Add(changedAssignOtherHdpTOver);
            changedValue.Add(changedAssignOtherOuTOver);
            return changedValue.Where(value => !string.IsNullOrWhiteSpace(value.League)) ?? new List<ChangedValue>();
        }

        private ChangedValue DetectChangeAssignmentHdp(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
            => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
            {
                oldAssignment = oldSnapshot.Hdp,
                newAssignment = newSnapshot.Hdp,
                BettypeName = Bettype.HDP,
                ReceivedUserId = newSnapshot.HdpUserId.Value,
            });

        private ChangedValue DetectChangeAssignmentOu(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
           => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
           {
               oldAssignment = oldSnapshot.Ou,
               newAssignment = newSnapshot.Ou,
               BettypeName = Bettype.OU,
               ReceivedUserId = newSnapshot.OuUserId.Value,
           });

        private ChangedValue DetectChangeAssignmentHdpTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              oldAssignment = oldSnapshot.HdpTakeOver,
              newAssignment = newSnapshot.HdpTakeOver,
              BettypeName = Bettype.HDP_TAKEOVER,
              ReceivedUserId = newSnapshot.HdpTakeOverUserId.Value,
          });

        private ChangedValue DetectChangeAssignmentOuTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              oldAssignment = oldSnapshot.OuTakeOver,
              newAssignment = newSnapshot.OuTakeOver,
              BettypeName = Bettype.OU_TAKEOVER,
              ReceivedUserId = newSnapshot.OuTakeOverUserId.Value,
          });

        private ChangedValue DetectChangeAssignmentOtherHdp(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              oldAssignment = oldSnapshot.OtherHdp,
              newAssignment = newSnapshot.OtherHdp,
              BettypeName = Bettype.HDP,
              ReceivedUserId = newSnapshot.OtherHdpUserId.Value,
          });

        private ChangedValue DetectChangeAssignmentOtherOu(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              oldAssignment = oldSnapshot.OtherOu,
              newAssignment = newSnapshot.OtherOu,
              BettypeName = Bettype.OU,
              ReceivedUserId = newSnapshot.OtherOuUserId.Value,
          });

        private ChangedValue DetectChangeAssignmentOtherHdpTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              oldAssignment = oldSnapshot.OtherHdpTakeOver,
              newAssignment = newSnapshot.Ou,
              BettypeName = Bettype.OU,
              ReceivedUserId = newSnapshot.OtherHdpTakeOverUserId.Value,
          });

        private ChangedValue DetectChangeAssignmentOtherOuTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
          => AssignmentHelper.GenerateChangedValue(new MyAssignmentDetectorModel
          {
              oldAssignment = oldSnapshot.OtherOuTakeOver,
              newAssignment = newSnapshot.Ou,
              BettypeName = Bettype.OU,
              ReceivedUserId = newSnapshot.OtherOuTakeOverUserId.Value,
          });
    }
}
