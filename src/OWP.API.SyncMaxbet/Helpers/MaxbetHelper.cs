using System.Collections.Generic;
using System.Linq;
using OWP.API.SyncMaxbet.Models;

namespace OWP.Helpers
{
    public static class MaxbetHelper
    {
        public static T FindTraderAssinmentByBettype<T>(List<T> assignment, int bettypeId) where T : TraderAssignment
            => assignment.FirstOrDefault(a => a.BettypeId == bettypeId);

        public static T FindSpreadByBettype<T>(List<T> spread, int bettypeId) where T : SpreadModel
            => spread.FirstOrDefault(a => a.BettypeId == bettypeId);

        public static T FindMaxbetByBettype<T>(List<T> maxbet, int bettypeId) where T : Maxbet
            => maxbet.FirstOrDefault(a => a.BettypeId == bettypeId);
    }
}
