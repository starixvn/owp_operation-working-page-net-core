using System.Collections.Generic;
using OWP.Extensions;

namespace OWP.OtherSport.Notification.Constants
{
    public static class MessageType
    {
        private static readonly Dictionary<string, int> MessageTypeMap = new Dictionary<string, int>
        {
            { "MatchNo", (int)MsgTemplateCode.EditMatchNo },
            { "MatchStatus", (int)MsgTemplateCode.EditMatchStatus },
            { "Category", (int)MsgTemplateCode.EditCategory },
            { "MatchOffer", (int)MsgTemplateCode.EditMatchOffer },
            { "AutoLink", (int)MsgTemplateCode.EditAutoLink },
            { "OtherProvider", (int)MsgTemplateCode.EditOtherProvider },
            { "IPTV", (int)MsgTemplateCode.EditIPTV },
            { "WB", (int)MsgTemplateCode.EditWB },
            { "Remark", (int)MsgTemplateCode.EditRemark },
            { "MaxBet", (int)MsgTemplateCode.EditMaxbet },
            { "Reporter", (int)MsgTemplateCode.EditReporter },
            { "Provider", (int)MsgTemplateCode.EditProvider },
        };

        public static int GetMessageTypeIdForChangedMatchInfo(string property)
            => MessageTypeMap.GetValue(property, 0);
    }
}
