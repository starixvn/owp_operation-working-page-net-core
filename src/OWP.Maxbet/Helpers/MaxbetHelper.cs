using System.Collections.Generic;
using System.Linq;
using OWP.Maxbet.Models;

namespace OWP.Helpers
{
    public static class MaxbetHelper
    {
        public static T FindTraderAssinmentByBettype<T>(List<T> assignment, int bettypeId) where T : TraderAssignment
            => assignment.FirstOrDefault(a => a.BettypeId == bettypeId);

        public static T FindSpreadByBettype<T>(List<T> spreads, int bettypeId) where T : SpreadModel
            => spreads.FirstOrDefault(a => a.BettypeId == bettypeId);

        public static T FindMaxbetByBettype<T>(List<T> maxbets, int bettypeId) where T : MaxbetModel
            => maxbets.FirstOrDefault(a => a.BettypeId == bettypeId);
    }
}