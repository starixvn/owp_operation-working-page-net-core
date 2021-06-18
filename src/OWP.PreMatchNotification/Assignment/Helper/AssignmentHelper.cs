using OWP.PreMatchNotification.Assignment.Constant;
using OWP.PreMatchNotification.Assignment.Models;
using OWP.PreMatchNotification.Constants;
using OWP.PreMatchNotification.Models;
using System.Collections.Generic;

namespace OWP.PreMatchNotification.Assignment.Helper
{
    public static class AssignmentHelper
    {
        public static int DetectMessageType(bool isDay)
            => isDay
                    ? (int)MsgTemplateCode.DayShiftOtherAssignmentUpdate
                    : (int)MsgTemplateCode.NightShiftOtherAssignmentUpdate;
    }

}
