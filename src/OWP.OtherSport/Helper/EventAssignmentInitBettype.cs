using System.Collections.Generic;
using System.Linq;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.Helper
{
    public static class EventAssignmentInitBettype
    {
        #region Init bettype

        public static void InitBettype(List<EventAssignmentModel> matchs,
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment,
            Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet)
        {
            matchs.ForEach(match =>
            {
                var bettypes = BuildBettypeForMatch(match.OSMatchId, dicMatchAssignment, dicMatchMaxBet);
                foreach (var bettype in bettypes)
                {
                    match.TraderMaxbetModels.Add(new TraderMaxBetModel
                    {
                        BetTypeName = bettype,
                        TraderAssignment = new TraderAssignment
                        {
                            OSMatchId= match.OSMatchId,
                            BetTypeName = bettype
                        },
                        Maxbet = new MatchMaxBet
                        {
                            OSMatchId = match.OSMatchId,
                            BetTypeName = bettype
                        }
                    });
                }
            });
        }

        public static IList<string> BuildBettypeForMatch(
            int osId,
            Dictionary<int, List<MatchAssignment>> dicMatchAssignment,
            Dictionary<int, List<MatchMaxBet>> dicMatchMaxBet)
        {
            var assignments = DicHelper.GetValueFromDicBykey(dicMatchAssignment, osId);
            var maxbets = DicHelper.GetValueFromDicBykey(dicMatchMaxBet, osId);
            var bettypeAssignment = assignments.Select(a => a.BetTypeName).Distinct();
            var bettypeMaxbet = maxbets.Select(m => m.BetTypeName).Distinct();
            var bettypeSelected = bettypeAssignment.Count() > bettypeMaxbet.Count()
                ? bettypeAssignment
                : bettypeMaxbet;
            return bettypeSelected.ToList();
        }

       

        #endregion Init bettype
    }
}
