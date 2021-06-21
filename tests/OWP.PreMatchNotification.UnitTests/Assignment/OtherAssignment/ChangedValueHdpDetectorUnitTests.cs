using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Assignment.Services.OtherAssignment;
using OWP.PreMatchNotification.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.PreMatchNotification.UnitTests.Assignment.OtherAssignment
{
    public class ChangedValueHdpDetectorUnitTests
    {
        [Fact]
        public void DetectChangeMainAssignment_ReturnChangedValue()
        {
            var hdpDetector = new ChangedValueHdpDetectorImp();
            var oldSnapshot = new AssignmentSnapshot
            {
                LeagueName = "VLeague",
                IsDay = true,
                Hdp = "Sa",
                HdpUserId = 8
            };

            var newSnapshot = new AssignmentSnapshot
            {
                LeagueName = "VLeague",
                Hdp = "Starix15",
                HdpUserId = 121,
                IsDay = true
            };

            var actual = new List<ChangedValue>();
            var expected = new List<ChangedValue>
            {
                new ()
                {
                    League = "VLeague",
                    BetTypeName = "HDP",
                    OldValue = "Sa",
                    NewValue = "Starix15",
                    ReceivedUserIds = new List<int>(121),
                    MessageTypeId = 203
                }
            };
            hdpDetector.DetectMain(oldSnapshot, newSnapshot, actual, 121);
            DeepAssert.Equal(actual, expected);
        }

        [Fact]
        public void DetectChangeOtherAssignment_ReturnChangedValue()
        {
            var hdpDetector = new ChangedValueHdpDetectorImp();
            var oldSnapshot = new AssignmentSnapshot
            {
                LeagueName = "VLeague",
                IsDay = true,
                OtherHdp = "Sa",
                OtherHdpUserId = 8
            };

            var newSnapshot = new AssignmentSnapshot
            {
                LeagueName = "VLeague",
                OtherHdp = "Starix15",
                OtherHdpUserId = 121,
                IsDay = true
            };

            var actual = new List<ChangedValue>();
            var expected = new List<ChangedValue>
            {
                new ()
                {
                    League = "VLeague",
                    BetTypeName = "HDP",
                    OldValue = "Sa",
                    NewValue = "Starix15",
                    ReceivedUserIds = new List<int>(121),
                    MessageTypeId = 203
                }
            };
            hdpDetector.DetectOther(oldSnapshot, newSnapshot, actual, 121);
            DeepAssert.Equal(actual, expected);
        }
    }
}
