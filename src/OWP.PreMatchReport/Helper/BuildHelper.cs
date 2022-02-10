namespace OWP.PreMatchReport.Helper
{
    using OWP.PreMatchReport.Models;
    public static class BuildHelper
    {
        public static void build(ReportBaseModel singleTraderOfLeague, QueryPrematchReportModel element)
        {
            switch (element.GroupId)
            {
                case 0:
                    singleTraderOfLeague.Unknown  = element.Amount;
                    break;
                case 1:
                    singleTraderOfLeague.MJ  = element.Amount;
                    break;
                case 2:
                    singleTraderOfLeague.M1  = element.Amount;
                    break;
                case 3:
                    singleTraderOfLeague.M2  = element.Amount;
                    break;
                case 4:
                    singleTraderOfLeague.M3  = element.Amount;
                    break;
                case 5:
                    singleTraderOfLeague.M4  = element.Amount;
                    break;
                case 6:
                    singleTraderOfLeague.Others  = element.Amount;
                    break;
                case 7:
                    singleTraderOfLeague.Others1  = element.Amount;
                    break;
                case 8:
                    singleTraderOfLeague.Others2  = element.Amount;
                    break;
                case 9:
                    singleTraderOfLeague.Others3  = element.Amount;
                    break;
                case 10:
                    singleTraderOfLeague.Others4  = element.Amount;
                    break;
                case 11:
                    singleTraderOfLeague.Others5  = element.Amount;
                    break;
            }
        }
    }
}