using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Constants;
using OWP.PreMatchNotification.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.PreMatchNotification.Assignment.Services.OtherAssignment
{
    public interface OtherAssignmentDetector
    {
        IList<ChangedValue> DetectChangeOtherAssignmentExceptHdp(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOu(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentHdpTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOuTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOtherHdp(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOtherOu(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOtherHdpTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);

        IList<ChangedValue> DetectChangeAssignmentOtherOuTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);
    }

    public class OtherAssignmentDetectoImp : OtherAssignmentDetector
    {
        private readonly ChangedValueHdpDetector changedValueHdpDetector;

        private readonly ChangedValueHdpTakeOverDetector changedValueHdpTakeOverDetector;

        private readonly ChangedValueOtherHdpDetector changedValueOtherHdpDetector;

        private readonly ChangedValueOtherHdpTakeOverDetector changedValueOtherHdpTakeOverDetector;

        private readonly ChangedValueOtherOuDetector changedValueOtherOuDetector;

        private readonly ChangedValueOtherOuTakeOverDetector changedValueOtherOuTakeOverDetector;

        private readonly ChangedValueOuDetector changedValueOuDetector;

        private readonly ChangedValueOuTakeOverDetector changedValueOuTakeOverDetector;

        public OtherAssignmentDetectoImp(ChangedValueHdpDetector changedValueHdpDetector,
        ChangedValueHdpTakeOverDetector changedValueHdpTakeOverDetector,
        ChangedValueOtherHdpDetector changedValueOtherHdpDetector,
        ChangedValueOtherHdpTakeOverDetector changedValueOtherHdpTakeOverDetector,
        ChangedValueOtherOuDetector changedValueOtherOuDetector,
        ChangedValueOtherOuTakeOverDetector changedValueOtherOuTakeOverDetector,
        ChangedValueOuDetector changedValueOuDetector,
        ChangedValueOuTakeOverDetector changedValueOuTakeOverDetector)
        {
            this.changedValueHdpDetector = changedValueHdpDetector;
            this.changedValueHdpTakeOverDetector = changedValueHdpTakeOverDetector;
            this.changedValueOtherHdpDetector = changedValueOtherHdpDetector;
            this.changedValueOtherHdpTakeOverDetector = changedValueOtherHdpTakeOverDetector;
            this.changedValueOtherOuDetector = changedValueOtherOuDetector;
            this.changedValueOtherOuTakeOverDetector = changedValueOtherOuTakeOverDetector;
            this.changedValueOuDetector = changedValueOuDetector;
            this.changedValueOuTakeOverDetector = changedValueOuTakeOverDetector;
        }
        public IList<ChangedValue> DetectChangeOtherAssignmentExceptHdp(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            var changedValue = new List<ChangedValue>();
            if (string.Equals(oldSnapshot.Hdp, newSnapshot.Hdp)
                && !string.IsNullOrWhiteSpace(newSnapshot.Hdp))
            {
                changedValueHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot,changedValue, newSnapshot.HdpUserId.Value);
                changedValueOtherHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpUserId.Value); ;
                changedValueOtherHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpUserId.Value); ;
                changedValueOtherOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpUserId.Value); ;
                changedValueOtherOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpUserId.Value); ;
                changedValueOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpUserId.Value); ;
                changedValueOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpUserId.Value); ;
            }
            return changedValue;
        }

        public IList<ChangedValue> DetectChangeAssignmentHdpTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            var changedValue = new List<ChangedValue>();
            if (string.Equals(oldSnapshot.HdpTakeOver, newSnapshot.HdpTakeOver)
                && !string.IsNullOrWhiteSpace(newSnapshot.HdpTakeOver))
            {
                changedValueHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpTakeOverUserId.Value);
                changedValueOtherHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpTakeOverUserId.Value); ;
                changedValueOtherHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpTakeOverUserId.Value); ;
                changedValueOtherOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpTakeOverUserId.Value); ;
                changedValueOtherOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpTakeOverUserId.Value); ;
                changedValueOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpTakeOverUserId.Value); ;
                changedValueOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.HdpTakeOverUserId.Value);
            }
            return changedValue;
        }

        public IList<ChangedValue> DetectChangeAssignmentOtherHdp(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            var changedValue = new List<ChangedValue>();
            if (string.Equals(oldSnapshot.OtherHdp, newSnapshot.OtherHdp)
                && !string.IsNullOrWhiteSpace(newSnapshot.OtherHdp))
            {
                changedValueHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.OtherHdpUserId.Value);
                changedValueHdpTakeOverDetector .Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.OtherHdpUserId.Value); ;
                changedValueOtherHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.OtherHdpUserId.Value); ;
                changedValueOtherOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.OtherHdpUserId.Value); ;
                changedValueOtherOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.OtherHdpUserId.Value); ;
                changedValueOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.OtherHdpUserId.Value); ;
                changedValueOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, newSnapshot.OtherHdpUserId.Value);
            }
            return changedValue;
        }

        public IList<ChangedValue> DetectChangeAssignmentOtherHdpTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            var changedValue = new List<ChangedValue>();
            if (string.Equals(oldSnapshot.OtherHdpTakeOver, newSnapshot.OtherHdpTakeOver)
                && !string.IsNullOrWhiteSpace(newSnapshot.OtherHdpTakeOver))
            {
                var userId = newSnapshot.OtherHdpTakeOverUserId.Value;
                changedValueHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId); ;
                changedValueOtherHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
            }
            return changedValue;
        }

        public IList<ChangedValue> DetectChangeAssignmentOtherOu(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            var changedValue = new List<ChangedValue>();
            if (string.Equals(oldSnapshot.OtherOu, newSnapshot.OtherOu)
                && !string.IsNullOrWhiteSpace(newSnapshot.OtherOu))
            {
                var userId = newSnapshot.OtherOuUserId.Value;
                changedValueHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId); ;
                changedValueOtherHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
            }
            return changedValue;
        }

        public IList<ChangedValue> DetectChangeAssignmentOtherOuTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            var changedValue = new List<ChangedValue>();
            if (string.Equals(oldSnapshot.OtherOuTakeOver, newSnapshot.OtherOuTakeOver)
                && !string.IsNullOrWhiteSpace(newSnapshot.OtherOuTakeOver))
            {
                var userId = newSnapshot.OtherOuTakeOverUserId.Value;
                changedValueHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId); ;
                changedValueOtherHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
            }
            return changedValue;
        }

        public IList<ChangedValue> DetectChangeAssignmentOu(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            var changedValue = new List<ChangedValue>();
            if (string.Equals(oldSnapshot.Ou, newSnapshot.Ou)
                && !string.IsNullOrWhiteSpace(newSnapshot.Ou))
            {
                var userId = newSnapshot.OuUserId.Value;
                changedValueHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId); ;
                changedValueOtherHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
            }
            return changedValue;
        }

        public IList<ChangedValue> DetectChangeAssignmentOuTOver(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            var changedValue = new List<ChangedValue>();
            if (string.Equals(oldSnapshot.OuTakeOver, newSnapshot.OuTakeOver)
                && !string.IsNullOrWhiteSpace(newSnapshot.OuTakeOver))
            {
                var userId = newSnapshot.OuTakeOverUserId.Value;
                changedValueHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId); ;
                changedValueOtherHdpTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherOuTakeOverDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOuDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
                changedValueOtherHdpDetector.Detect(oldSnapshot, newSnapshot, changedValue, userId);
            }
            return changedValue;
        }
    }
}
