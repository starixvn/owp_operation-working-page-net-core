using System.Collections.Generic;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.EventAssignment.Helper
{
    public static class TransformModelHelper
    {
        public static IList<EventAssignmentModel> TransformToAssignmentModelIgnoreTraderMaxbet(
         IList<MatchInfo> matches)
        {
            var events = new List<EventAssignmentModel>();
            foreach (var match in matches)
            {
                var eventInstance = new EventAssignmentModel
                {
                    OSMatchId = match.OSMatchId,
                    MatchStatus = match.MatchStatus,
                    Sport = match.Sport,
                    DateTime = match.DateTime,
                    Home = match.Home,
                    Away = match.Away,
                    League = match.League,
                    Category = match.Category,
                    NoHDP = match.NoHDP,
                    MatchOffer = match.MatchOffer,
                    AutoLink = match.AutoLink,
                    IPTV = match.IPTV,
                    Service = match.Service,
                    OtherProvider = match.OtherProvider,
                    WB = match.WB,
                    Provider = match.Provider,
                    Remark = match.Remark,
                };

                events.Add(eventInstance);
            }
            return events;
        }
    }
}
