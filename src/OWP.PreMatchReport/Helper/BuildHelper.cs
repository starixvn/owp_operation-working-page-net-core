namespace OWP.PreMatchReport.Helper
{
    using System.Reflection;
    using OWP.Extensions;
    using OWP.PreMatchReport.Models;
    using OWP.PreMatchReport.Constants;

    public static class LeagueGroupHelper
    {
        private static readonly Dictionary<int, string> leagueGroups = new Dictionary<int, string>
        {
            { LeagueGroupConst.Unknown , "Unknown"},
            { LeagueGroupConst.MJ , "MJ"},
            { LeagueGroupConst.M1 , "M1"},
            { LeagueGroupConst.M2 , "M2"},
            { LeagueGroupConst.M2 , "Unknown"},
            { LeagueGroupConst.M4 , "M4"},
            { LeagueGroupConst.Others , "Others"},
            { LeagueGroupConst.Others1 , "Others1"},
            { LeagueGroupConst.Others2 , "Others2"},
            { LeagueGroupConst.Others3 , "Others3"},
            { LeagueGroupConst.Others4 , "Others4"},
            { LeagueGroupConst.Others5 , "Others5"},
        };

        public static void Build(ReportBaseModel singleTraderOfLeague, QueryPrematchReportModel element)
        {
            var leagueGroup = leagueGroups.GetValue(element.GroupId, string.Empty);
            PropertyInfo prop = singleTraderOfLeague.GetType().GetProperty(leagueGroup, BindingFlags.Public | BindingFlags.Instance);

            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(singleTraderOfLeague, element.Amount, null);
            }
        }
    }
}