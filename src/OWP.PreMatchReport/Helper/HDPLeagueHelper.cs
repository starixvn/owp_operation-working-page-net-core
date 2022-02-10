namespace OWP.PreMatchReport.Helper
{
    using OWP.PreMatchReport.Models;
    public static class HDPLeagueHelper
    {
        public static void buildHDPLeague(ReportLeagueModel singleTraderOfLeague, QueryPrematchReportModel element)
        {
            switch (element.GroupId)
            {
                case 0:
                    singleTraderOfLeague.Unknown_HDPLeague = element.Amount;
                    break;
                case 1:
                    singleTraderOfLeague.MJ_HDPLeague = element.Amount;
                    break;
                case 2:
                    singleTraderOfLeague.M1_HDPLeague = element.Amount;
                    break;
                case 3:
                    singleTraderOfLeague.M2_HDPLeague = element.Amount;
                    break;
                case 4:
                    singleTraderOfLeague.M3_HDPLeague = element.Amount;
                    break;
                case 5:
                    singleTraderOfLeague.M4_HDPLeague = element.Amount;
                    break;
                case 6:
                    singleTraderOfLeague.Others_HDPLeague = element.Amount;
                    break;
                case 7:
                    singleTraderOfLeague.Others1_HDPLeague = element.Amount;
                    break;
                case 8:
                    singleTraderOfLeague.Others2_HDPLeague = element.Amount;
                    break;
                case 9:
                    singleTraderOfLeague.Others3_HDPLeague = element.Amount;
                    break;
                case 10:
                    singleTraderOfLeague.Others4_HDPLeague = element.Amount;
                    break;
                case 11:
                    singleTraderOfLeague.Others5_HDPLeague = element.Amount;
                    break;
            }
        }
        public static void buildOULeague(ReportLeagueModel singleTraderOfLeague, QueryPrematchReportModel element)
        {
            switch (element.GroupId)
            {
                case 0:
                    singleTraderOfLeague.Unknown_OULeague = element.Amount;
                    break;
                case 1:
                    singleTraderOfLeague.MJ_OULeague = element.Amount;
                    break;
                case 2:
                    singleTraderOfLeague.M1_OULeague = element.Amount;
                    break;
                case 3:
                    singleTraderOfLeague.M2_OULeague = element.Amount;
                    break;
                case 4:
                    singleTraderOfLeague.M3_OULeague = element.Amount;
                    break;
                case 5:
                    singleTraderOfLeague.M4_OULeague = element.Amount;
                    break;
                case 6:
                    singleTraderOfLeague.Others_OULeague = element.Amount;
                    break;
                case 7:
                    singleTraderOfLeague.Others1_OULeague = element.Amount;
                    break;
                case 8:
                    singleTraderOfLeague.Others2_OULeague = element.Amount;
                    break;
                case 9:
                    singleTraderOfLeague.Others3_OULeague = element.Amount;
                    break;
                case 10:
                    singleTraderOfLeague.Others4_OULeague = element.Amount;
                    break;
                case 11:
                    singleTraderOfLeague.Others5_OULeague = element.Amount;
                    break;
            }
        }
    }
}