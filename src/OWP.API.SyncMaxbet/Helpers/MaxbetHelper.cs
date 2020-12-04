using System;
using System.Collections.Generic;
using System.Linq;
using OWP.API.SyncMaxbet.Models;

namespace OWP.Helpers
{
    public static class MaxbetHelper
    {
        public static T FindTraderAssinmentByBettype<T>(List<T> assignment, int bettypeId) where T : TraderAssignment
       => assignment.FirstOrDefault(a => a.BettypeId == bettypeId);

        public static T FindSpreadByBettype<T>(List<T> assignment, int bettypeId) where T : SpreadModel
       => assignment.FirstOrDefault(a => a.BettypeId == bettypeId);
    }
}
