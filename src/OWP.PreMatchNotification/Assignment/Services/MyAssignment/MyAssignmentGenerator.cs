using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;
using System.Linq;

namespace OWP.PreMatchNotification.Assignment.Services.MyAssignment
{
    public interface MyAssignmentGenerator
    {
        IEnumerable<ChangedValue> Create(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot);
    }

    public class MyAssignmentGeneratorImp : MyAssignmentGenerator
    {
        private readonly MyAssignmentDetector myAssignmentDetector;

        public MyAssignmentGeneratorImp(MyAssignmentDetector myAssignmentDetector)
        {
            this.myAssignmentDetector = myAssignmentDetector;
        }

        public IEnumerable<ChangedValue> Create(AssignmentSnapshot oldSnapshot, AssignmentSnapshot newSnapshot)
        {
            var changedValue = new List<ChangedValue>();
            var changedAssignHdp = myAssignmentDetector.DetectChangeAssignmentHdp(oldSnapshot, newSnapshot);
            var changedAssignOu = myAssignmentDetector.DetectChangeAssignmentOu(oldSnapshot, newSnapshot);
            var changedAssignHdpTover = myAssignmentDetector.DetectChangeAssignmentHdpTOver(oldSnapshot, newSnapshot);
            var changedAssignOuTOver = myAssignmentDetector.DetectChangeAssignmentOuTOver(oldSnapshot, newSnapshot);
            var changedAssignOtherHdp = myAssignmentDetector.DetectChangeAssignmentOtherHdp(oldSnapshot, newSnapshot);
            var changedAssignOtherOu = myAssignmentDetector.DetectChangeAssignmentOtherOu(oldSnapshot, newSnapshot);
            var changedAssignOtherHdpTOver = myAssignmentDetector.DetectChangeAssignmentOtherHdpTOver(oldSnapshot, newSnapshot);
            var changedAssignOtherOuTOver = myAssignmentDetector.DetectChangeAssignmentOtherOuTOver(oldSnapshot, newSnapshot);
            changedValue.AddRange(changedAssignHdp);
            changedValue.AddRange(changedAssignOu);
            changedValue.AddRange(changedAssignHdpTover);
            changedValue.AddRange(changedAssignOuTOver);
            changedValue.AddRange(changedAssignOtherHdp);
            changedValue.AddRange(changedAssignOtherOu);
            changedValue.AddRange(changedAssignOtherHdpTOver);
            changedValue.AddRange(changedAssignOtherOuTOver);
            return changedValue.Where(value => !string.IsNullOrWhiteSpace(value.League))
                ?? new List<ChangedValue>();
        }
    }
}
