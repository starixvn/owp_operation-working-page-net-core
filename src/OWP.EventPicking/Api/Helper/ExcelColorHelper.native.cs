using System.Text;
using OWP.EventPicking.Api.Constant;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Helper;

public static class ExcelColorHelper
{
    public static void FillRowColor(IList<EventPickingModel> matches, ExcelWorksheet templateSheet)
    {
        for (int i = 0, row = 2; i < matches.Count; i++, row++)
        {
            var match = matches[i];
            for (var column = 1; column <= 76; column++)
            {
                if (match.RowColorHex == null || match.RowColorHex == Misc.Constant.ColorWhiteHex)
                {
                    continue;
                }

                var color = ColorTranslator.FromHtml(match.RowColorHex);
                templateSheet.Cells[row, column].Style.Fill.SetBackground(color);
            }
        }
    }

    public static void FillCellColor(IList<EventPickingModel> matches, ExcelWorksheet templateSheet)
    {
        for (int i = 0, row = Misc.Constant.START_ROW; i < matches.Count; i++, row++)
        {
            AddHomeRemark(matches[i], row, templateSheet);
            AddAwayRemark(matches[i], row, templateSheet);

            FillColorAndAddRemark(matches[i], row, templateSheet);
            FillColorTeamColumn(matches[i], row, templateSheet);
            FillColor(matches[i], row, templateSheet, Misc.Constant.TIME_COL_INDEX, RAndDColorConstant.UPDATE_KO_TIME);
            FillColor(matches[i], row, templateSheet, Misc.Constant.DATE_COL_INDEX, RAndDColorConstant.RENEW_MATCH);
            FillColor(matches[i], row, templateSheet, Misc.Constant.LEAGUE_COL_INDEX, RAndDColorConstant.UPDATE_LEAGUE);
            FillColor(matches[i], row, templateSheet, Misc.Constant.TV_COL_INDEX, RAndDColorConstant.UPDATE_TV, RAndDColorConstant.CANCEL_TV);
            FillColor(matches[i], row, templateSheet, Misc.Constant.IPTV_COL_INDEX, RAndDColorConstant.UPDATE_IPTV, RAndDColorConstant.CANCEL_IPTV);
            FillColor(matches[i], row, templateSheet, Misc.Constant.ROUND_COL_INDEX, RAndDColorConstant.UPDATE_ROUND, RAndDColorConstant.CANCEL_ROUND);
            FillColor(matches[i], row, templateSheet, Misc.Constant.REMARK_COL_INDEX, RAndDColorConstant.UPDATE_REMARK, RAndDColorConstant.CANCEL_REMARK);
            FillColor(matches[i], row, templateSheet, Misc.Constant.HOME_REMARK_COL_INDEX, RAndDColorConstant.UPDATE_HOME_REMARK, RAndDColorConstant.CANCEL_HOME_REMARK);
            FillColor(matches[i], row, templateSheet, Misc.Constant.AWAY_REMARK_COL_INDEX, RAndDColorConstant.UPDATE_AWAY_REMARK, RAndDColorConstant.CANCEL_AWAY_REMARK);
            FillColor(matches[i], row, templateSheet, Misc.Constant.OTHER_PROVIDER_COL_INDEX, RAndDColorConstant.UPDATE_OTHER_PROVIDER, RAndDColorConstant.CANCEL_OTHER_PROVIDER);
            FillColor(matches[i], row, templateSheet, Misc.Constant.DIVISION_COL_INDEX, RAndDColorConstant.UPDATE_DIVISION, RAndDColorConstant.CANCEL_DIVISION);
            FillColor(matches[i], row, templateSheet, Misc.Constant.ROUND_CONFIRM_COL_INDEX, RAndDColorConstant.UPDATE_ROUND_CONFIRM, RAndDColorConstant.CANCEL_ROUND_CONFIRM);
            FillColor(matches[i], row, templateSheet, Misc.Constant.ADMINSITE_ID_COL_INDEX, RAndDColorConstant.UPDATE_ADMINSITEID, RAndDColorConstant.CANCEL_ADMINSITEID);
            FillColor(matches[i], row, templateSheet, Misc.Constant.WEB1_COL_INDEX, RAndDColorConstant.UPDATE_WEB1, RAndDColorConstant.CANCEL_WEB1);
            FillColor(matches[i], row, templateSheet, Misc.Constant.WEB2_COL_INDEX, RAndDColorConstant.UPDATE_WEB2, RAndDColorConstant.CANCEL_WEB2);
            FillColor(matches[i], row, templateSheet, Misc.Constant.WEB3_COL_INDEX, RAndDColorConstant.UPDATE_WEB3, RAndDColorConstant.CANCEL_WEB3);
            FillColor(matches[i], row, templateSheet, Misc.Constant.WEB4_COL_INDEX, RAndDColorConstant.UPDATE_WEB4, RAndDColorConstant.CANCEL_WEB4);
            FillColor(matches[i], row, templateSheet, Misc.Constant.LINK_COL_INDEX, RAndDColorConstant.UPDATE_LINK, RAndDColorConstant.CANCEL_LINK);
            FillColor(matches[i], row, templateSheet, Misc.Constant.CHANNEL_COL_INDEX, RAndDColorConstant.UPDATE_CHANNEL, RAndDColorConstant.CANCEL_CHANNEL);
            FillColor(matches[i], row, templateSheet, Misc.Constant.AGENT_COL_INDEX, RAndDColorConstant.UPDATE_AGENT, RAndDColorConstant.CANCEL_AGENT);
            FillColor(matches[i], row, templateSheet, Misc.Constant.VENUE_COL_INDEX, RAndDColorConstant.UPDATE_VENUE, RAndDColorConstant.CANCEL_VENUE);
            FillColor(matches[i], row, templateSheet, Misc.Constant.ETPK_COL_INDEX, RAndDColorConstant.UPDATE_ETPK, RAndDColorConstant.CANCEL_ETPK);
            FillColor(matches[i], row, templateSheet, Misc.Constant.HOME_COL_INDEX, RAndDColorConstant.UPDATE_NEUTRAL, RAndDColorConstant.CANCEL_NEUTRAL);
        }
    }

    private static void FillColor(EventPickingModel match, int row, ExcelWorksheet templateSheet, int columnIndex, params int[] updateTypes)
    {
        if (ExistsUpdateType(match, updateTypes))
        {
            PinkColor(templateSheet.Cells[row, columnIndex]);
        }
    }

    private static void FillColorAndAddRemark(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        var remarkBuilder = new StringBuilder();

        if (ExistsUpdateType(match, RAndDColorConstant.NEW_MATCH))
        {
            remarkBuilder.Insert(0, "[NEW ADD]");
        }

        if (match.IsDeleted)
        {
            remarkBuilder.Insert(0, "[DELETED MATCH] ");
        }
        else if (IsPostponed(match))
        {
            remarkBuilder.Insert(0, "[POSTPONED] ");
        }
        else if (IsCancelled(match))
        {
            remarkBuilder.Insert(0, "[CANCELLED] ");
        }

        if (remarkBuilder.Length > 0)
        {
            PinkColorAndAddRemark(row, templateSheet, remarkBuilder.ToString());
        }
    }

    private static void AddAwayRemark(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        AddTeamRemarkBlackList(templateSheet, row, Misc.Constant.AWAY_REMARK_COL_INDEX, match.IsAwayBlackList);
        AddTeamRemarkBadRanking(templateSheet, row, Misc.Constant.AWAY_REMARK_COL_INDEX, match.AwayRemark, match.IsAwayBadRanking);
        AddTeamRemarkRegelation(templateSheet, row, Misc.Constant.AWAY_REMARK_COL_INDEX, match.IsAwayDowngrade);
    }

    private static void AddHomeRemark(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        AddTeamRemarkBlackList(templateSheet, row, Misc.Constant.HOME_REMARK_COL_INDEX, match.IsHomeBlackList);
        AddTeamRemarkBadRanking(templateSheet, row, Misc.Constant.HOME_REMARK_COL_INDEX, match.HomeRemark, match.IsHomeBadRanking);
        AddTeamRemarkRegelation(templateSheet, row, Misc.Constant.HOME_REMARK_COL_INDEX, match.IsHomeDowngrade);
    }

    private static void FillColorTeamColumn(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        var updateTeam = match.RAndDInfo?.Find(r => r.UpdateTypeId == RAndDColorConstant.UPDATE_TEAM);
        if (updateTeam is not null)
        {
            PinkColor(templateSheet.Cells[row, updateTeam.HomeUpdated
                ? Misc.Constant.HOME_COL_INDEX
                : Misc.Constant.AWAY_COL_INDEX]);
        }
    }

    private static void PinkColorAndAddRemark(int row, ExcelWorksheet templateSheet, string remark)
    {
        var cell = templateSheet.Cells[row, Misc.Constant.REMARK_COL_INDEX];
        cell.Value = $"{remark} {cell.Value}";
        PinkColor(cell);
    }

    private static void PinkColor(ExcelRange excelRange)
    {
        var color = ColorTranslator.FromHtml(Misc.Constant.ColorPinkHex);
        excelRange.Style.Fill.SetBackground(color);
    }

    private static bool ExistsUpdateType(EventPickingModel match, params int[] updateTypes)
    => match.RAndDInfo is not null && match.RAndDInfo.Any(r => updateTypes.Contains(r.UpdateTypeId));


    private static void AddTeamRemarkBlackList(ExcelWorksheet templateSheet, int row, int col, bool isBlackList)
    {
        if (isBlackList)
        {
            var cell = templateSheet.Cells[row, col];
            cell.Value = $"{cell.Value}{(!string.IsNullOrWhiteSpace(cell.Value.ToString()) ? Environment.NewLine : string.Empty)}Black List";
        }
    }

    private static void AddTeamRemarkBadRanking(ExcelWorksheet templateSheet, int row, int col, string remark, bool isBadRanking)
    {
        if ((string.IsNullOrEmpty(remark) || !remark.Contains("Bad ranking strike out", StringComparison.OrdinalIgnoreCase)) && isBadRanking)
        {
            var cell = templateSheet.Cells[row, col];
            cell.Value = $"{cell.Value}{(!string.IsNullOrWhiteSpace(cell.Value.ToString()) ? Environment.NewLine : string.Empty)}Bad Ranking";
        }
    }

    private static void AddTeamRemarkRegelation(ExcelWorksheet templateSheet, int row, int col, bool isDowngrade)
    {
        if (isDowngrade)
        {
            var cell = templateSheet.Cells[row, col];
            cell.Value = $"{cell.Value}{(!string.IsNullOrWhiteSpace(cell.Value.ToString()) ? Environment.NewLine : string.Empty)}Relegation";
        }
    }

    private static bool IsPostponed(EventPickingModel match)
    {
        return (!string.IsNullOrEmpty(match.Remark2) &&
                 match.Remark2.Contains("Postponed", StringComparison.OrdinalIgnoreCase))
                || match.InfoSiteMatchStatus == InfoSiteMatchStatus.Postponed;
    }

    private static bool IsCancelled(EventPickingModel match)
    {
        return (!string.IsNullOrEmpty(match.Remark2) &&
                 match.Remark2.Contains("Cancel", StringComparison.OrdinalIgnoreCase))
                || match.InfoSiteMatchStatus == InfoSiteMatchStatus.Cancelled;
    }
}