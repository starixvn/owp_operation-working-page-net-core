using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Olympus.OWP.Constants;
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
                if (match.RowColorHex is null or Misc.Constant.ColorWhiteHex)
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
            FillColor(matches[i], row, templateSheet, Misc.Constant.TIME_COL_INDEX, UpdateTypeConstant.UPDATE_KO_TIME);
            FillColor(matches[i], row, templateSheet, Misc.Constant.DATE_COL_INDEX, UpdateTypeConstant.RENEW_MATCH);
            FillColor(matches[i], row, templateSheet, Misc.Constant.LEAGUE_COL_INDEX, UpdateTypeConstant.UPDATE_LEAGUE);
            FillColor(matches[i], row, templateSheet, Misc.Constant.TV_COL_INDEX, UpdateTypeConstant.UPDATE_TV, UpdateTypeConstant.CANCEL_TV);
            FillColor(matches[i], row, templateSheet, Misc.Constant.IPTV_COL_INDEX, UpdateTypeConstant.UPDATE_IPTV, UpdateTypeConstant.CANCEL_IPTV);
            FillColor(matches[i], row, templateSheet, Misc.Constant.TOTAL_ROUND_COL_INDEX, UpdateTypeConstant.UPDATE_TOTAL_ROUND, UpdateTypeConstant.CANCEL_TOTAL_ROUND);
            FillColor(matches[i], row, templateSheet, Misc.Constant.ROUND_COL_INDEX, UpdateTypeConstant.UPDATE_ROUND, UpdateTypeConstant.CANCEL_ROUND);
            FillColor(matches[i], row, templateSheet, Misc.Constant.REMARK_COL_INDEX, UpdateTypeConstant.UPDATE_REMARK, UpdateTypeConstant.CANCEL_REMARK);
            FillColor(matches[i], row, templateSheet, Misc.Constant.REMARK2_COL_INDEX, UpdateTypeConstant.UPDATE_REMARK2, UpdateTypeConstant.CANCEL_REMARK2);
            FillColor(matches[i], row, templateSheet, Misc.Constant.HOME_REMARK_COL_INDEX,
                UpdateTypeConstant.UPDATE_HOME_REMARK, UpdateTypeConstant.CANCEL_HOME_REMARK,
                UpdateTypeConstant.UPDATE_HOME_RELEGATION, UpdateTypeConstant.UPDATE_HOME_BAD_RANKING);
            FillColor(matches[i], row, templateSheet, Misc.Constant.AWAY_REMARK_COL_INDEX,
                UpdateTypeConstant.UPDATE_AWAY_REMARK, UpdateTypeConstant.CANCEL_AWAY_REMARK,
                UpdateTypeConstant.UPDATE_AWAY_RELEGATION, UpdateTypeConstant.UPDATE_AWAY_BAD_RANKING);
            FillColor(matches[i], row, templateSheet, Misc.Constant.PROVIDER_COL_INDEX, UpdateTypeConstant.UPDATE_PROVIDER, UpdateTypeConstant.CANCEL_PROVIDER);
            FillColor(matches[i], row, templateSheet, Misc.Constant.OTHER_PROVIDER_COL_INDEX, UpdateTypeConstant.UPDATE_OTHER_PROVIDER, UpdateTypeConstant.CANCEL_OTHER_PROVIDER);
            FillColor(matches[i], row, templateSheet, Misc.Constant.DIVISION_COL_INDEX, UpdateTypeConstant.UPDATE_DIVISION, UpdateTypeConstant.CANCEL_DIVISION);
            FillColor(matches[i], row, templateSheet, Misc.Constant.ROUND_CONFIRM_COL_INDEX, UpdateTypeConstant.UPDATE_ROUND_CONFIRM, UpdateTypeConstant.CANCEL_ROUND_CONFIRM);
            FillColor(matches[i], row, templateSheet, Misc.Constant.ADMINSITE_ID_COL_INDEX, UpdateTypeConstant.UPDATE_ADMINSITEID, UpdateTypeConstant.CANCEL_ADMINSITEID);
            FillColor(matches[i], row, templateSheet, Misc.Constant.WEB1_COL_INDEX, UpdateTypeConstant.UPDATE_WEB1, UpdateTypeConstant.CANCEL_WEB1);
            FillColor(matches[i], row, templateSheet, Misc.Constant.WEB2_COL_INDEX, UpdateTypeConstant.UPDATE_WEB2, UpdateTypeConstant.CANCEL_WEB2);
            FillColor(matches[i], row, templateSheet, Misc.Constant.WEB3_COL_INDEX, UpdateTypeConstant.UPDATE_WEB3, UpdateTypeConstant.CANCEL_WEB3);
            FillColor(matches[i], row, templateSheet, Misc.Constant.WEB4_COL_INDEX, UpdateTypeConstant.UPDATE_WEB4, UpdateTypeConstant.CANCEL_WEB4);
            FillColor(matches[i], row, templateSheet, Misc.Constant.LINK_COL_INDEX, UpdateTypeConstant.UPDATE_LINK, UpdateTypeConstant.CANCEL_LINK);
            FillColor(matches[i], row, templateSheet, Misc.Constant.CHANNEL_COL_INDEX, UpdateTypeConstant.UPDATE_CHANNEL, UpdateTypeConstant.CANCEL_CHANNEL);
            FillColor(matches[i], row, templateSheet, Misc.Constant.AGENT_COL_INDEX, UpdateTypeConstant.UPDATE_AGENT, UpdateTypeConstant.CANCEL_AGENT);
            FillColor(matches[i], row, templateSheet, Misc.Constant.VENUE_COL_INDEX, UpdateTypeConstant.UPDATE_VENUE, UpdateTypeConstant.CANCEL_VENUE);
            FillColor(matches[i], row, templateSheet, Misc.Constant.ETPK_COL_INDEX, UpdateTypeConstant.UPDATE_ETPK, UpdateTypeConstant.CANCEL_ETPK);
            FillColor(matches[i], row, templateSheet, Misc.Constant.HOME_COL_INDEX, UpdateTypeConstant.UPDATE_NEUTRAL, UpdateTypeConstant.CANCEL_NEUTRAL);

            FillNoOfferTeamCell(matches[i], row, templateSheet);
        }
    }

    private static void FillNoOfferTeamCell(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (match.IsClubFriendlyLeague)
        {
            if (match.IsHomeMainLeagueNoOfferLive)
            {
                var homeCell = templateSheet.Cells[row, Misc.Constant.HOME_COL_INDEX];
                var color = ColorTranslator.FromHtml(Misc.Constant.ColorYellowHex);
                homeCell.Style.Fill.SetBackground(color);
            }
            if (match.IsAwayMainLeagueNoOfferLive)
            {
                var awayCell = templateSheet.Cells[row, Misc.Constant.AWAY_COL_INDEX];
                var color = ColorTranslator.FromHtml(Misc.Constant.ColorYellowHex);
                awayCell.Style.Fill.SetBackground(color);
            }
        }
    }
    public static void FillTextColor(IList<EventPickingModel> matches, ExcelWorksheet templateSheet)
    {
        for (int i = 0, row = Misc.Constant.START_ROW; i < matches.Count; i++, row++)
        {
            FillTextColor(matches[i], templateSheet, row, Misc.Constant.MARGIN_COL_INDEX);
        }
    }

    private static void FillTextColor(EventPickingModel match, ExcelWorksheet templateSheet, int row, int column)
    {
        if (match.MarginExport is < 0)
        {
            SetTextColor(templateSheet.Cells[row, column],Color.Red);
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
        var remarkBuilder = BuildRemark(match);

        if (remarkBuilder.Length > 0)
        {
            PinkColorAndAddRemark(row, templateSheet, remarkBuilder.ToString());
        }
    }

    private static StringBuilder BuildRemark(EventPickingModel match)
    {
        var remarkBuilder = new StringBuilder();

        AddNewMatchRemark(match, remarkBuilder);
        AddRenewMatchRemarks(match, remarkBuilder);
        AddStatusRemarks(match, remarkBuilder);

        return remarkBuilder;
    }

    private static void AddNewMatchRemark(EventPickingModel match, StringBuilder remarkBuilder)
    {
        if (ExistsUpdateType(match, UpdateTypeConstant.NEW_MATCH))
        {
            remarkBuilder.Insert(0, "[NEW ADD]");
        }
    }

    private static void AddRenewMatchRemarks(EventPickingModel match, StringBuilder remarkBuilder)
    {
        var rAndDInfos = match.RAndDInfo?.Where(info => info.UpdateTypeId == UpdateTypeConstant.RENEW_MATCH);

        if (rAndDInfos == null)
            return;

        foreach (var info in rAndDInfos)
        {
            remarkBuilder.Insert(0, info.OldValue switch
            {
                "Postponed" => "[RENEW MATCH] [POSTPONED] ",
                "Cancelled" => "[RENEW MATCH] [CANCELLED] ",
                "Suspended" => "[RENEW MATCH] [SUSPENDED] ",
                _ => "[RENEW MATCH] [MOVE KO DATE] "
            });
        }
    }

    private static void AddStatusRemarks(EventPickingModel match, StringBuilder remarkBuilder)
    {
        var statusRemark = match switch
        {
            { IsDeleted: true } => "[DELETED MATCH] ",
            _ when IsPostponed(match) => "[POSTPONED] ",
            _ when IsCancelled(match) => "[CANCELLED] ",
            _ when IsSuspended(match) => "[SUSPENDED] ",
            { IsMovedEventDate: true } => "[MOVE KO DATE] ",
            _ => string.Empty
        };

        if (!string.IsNullOrEmpty(statusRemark))
        {
            remarkBuilder.Insert(0, statusRemark);
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
        var updateTeam = match.RAndDInfo?.Find(r => r.UpdateTypeId == UpdateTypeConstant.UPDATE_TEAM);
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

    private static void SetTextColor(ExcelRange excelRange, Color color)
    {
        excelRange.Style.Font.Color.SetColor(color);
        
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

    private static bool IsSuspended(EventPickingModel match)
    {
        return match.InfoSiteMatchStatus == InfoSiteMatchStatus.Suspended;
    }
}