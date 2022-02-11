namespace OWP.PreMatchReport.Helper
{
    using OWP.PreMatchReport.Models;
    using OWP.PreMatchReport.Constants;
    public static class BuildHelper
    {
        public static void build(ReportBaseModel singleTraderOfLeague, QueryPrematchReportModel element)
        {
            switch (element.GroupId)
            {
                case ReportConst.Unknown_Id:
                    singleTraderOfLeague.Unknown  = element.Amount;
                    break;
                case ReportConst.MJ_Id:
                    singleTraderOfLeague.MJ  = element.Amount;
                    break;
                case ReportConst.M1_Id:
                    singleTraderOfLeague.M1  = element.Amount;
                    break;
                case ReportConst.M2_Id:
                    singleTraderOfLeague.M2  = element.Amount;
                    break;
                case ReportConst.M3_Id:
                    singleTraderOfLeague.M3  = element.Amount;
                    break;
                case ReportConst.M4_Id:
                    singleTraderOfLeague.M4  = element.Amount;
                    break;
                case ReportConst.Others_Id:
                    singleTraderOfLeague.Others  = element.Amount;
                    break;
                case ReportConst.Others1_Id:
                    singleTraderOfLeague.Others1  = element.Amount;
                    break;
                case ReportConst.Others2_Id:
                    singleTraderOfLeague.Others2  = element.Amount;
                    break;
                case ReportConst.Others3_Id:
                    singleTraderOfLeague.Others3  = element.Amount;
                    break;
                case ReportConst.Others4_Id:
                    singleTraderOfLeague.Others4  = element.Amount;
                    break;
                case ReportConst.Others5_Id:
                    singleTraderOfLeague.Others5  = element.Amount;
                    break;
            }
        }
    }
}