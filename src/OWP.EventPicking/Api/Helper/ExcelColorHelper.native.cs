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
            for (var column = 1; column <= 75; column++)
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

    public static void FillRowLSTFileColor(IList<ExportLSTFormat> matches, ExcelWorksheet templateSheet)
    {
        for (int i = 0, row = 2; i < matches.Count; i++, row++)
        {
            var match = matches[i];
            for (var column = 1; column <= 35; column++)
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
            FillColorNewMatch(matches[i], row, templateSheet);
            FillColorDeleteMatch(matches[i], row, templateSheet);
            FillColorKoTime(matches[i], row, templateSheet);
            FillColorLeague(matches[i], row, templateSheet);
            FillColorTeam(matches[i], row, templateSheet);
            FillColorTv(matches[i], row, templateSheet);
            FillColorIpTv(matches[i], row, templateSheet);
            FillColorOtherProvider(matches[i], row, templateSheet);
            FillColorRound(matches[i], row, templateSheet);
            FillColorRemark(matches[i], row, templateSheet);
            FillColorHomeRemark(matches[i], row, templateSheet);
            FillColorAwayRemark(matches[i], row, templateSheet);
            FillColorDivision(matches[i], row, templateSheet);
        }
    }

    private static void FillColorDivision(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_DIVISION, match)
            || ExistsUpdateType(RAndDColorConstant.CANCEL_DIVISION, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.DIVISION_COL_INDEX]);
        }
    }

    private static void FillColorAwayRemark(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        AddTeamRemarkBadRanking(templateSheet, row, Misc.Constant.AWAY_REMARK_COL_INDEX, match.AwayRemark, match.IsAwayBadRanking);
        AddTeamRemarkRegelation(templateSheet, row, Misc.Constant.AWAY_REMARK_COL_INDEX, match.IsAwayDowngrade);
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_AWAY_REMARK, match)
            || ExistsUpdateType(RAndDColorConstant.CANCEL_AWAY_REMARK, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.AWAY_REMARK_COL_INDEX]);
        }
    }

    private static void FillColorHomeRemark(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        AddTeamRemarkBadRanking(templateSheet, row, Misc.Constant.HOME_REMARK_COL_INDEX, match.HomeRemark, match.IsHomeBadRanking);
        AddTeamRemarkRegelation(templateSheet, row, Misc.Constant.HOME_REMARK_COL_INDEX, match.IsHomeDowngrade);
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_HOME_REMARK, match)
            || ExistsUpdateType(RAndDColorConstant.CANCEL_HOME_REMARK, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.HOME_REMARK_COL_INDEX]);
        }
    }

    private static void FillColorRemark(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_REMARK, match)
            || ExistsUpdateType(RAndDColorConstant.CANCEL_REMARK, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.REMARK_COL_INDEX]);
        }
    }

    private static void FillColorRound(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_ROUND, match)
            || ExistsUpdateType(RAndDColorConstant.CANCEL_ROUND, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.ROUND_COL_INDEX]);
        }
    }

    private static void FillColorTeam(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        var updateTeam = match.RAndDInfo?.Find(r => r.UpdateTypeId == RAndDColorConstant.UPDATE_TEAM);
        if (updateTeam is not null)
        {
            if (updateTeam.HomeUpdated)
            {
                FillCellColor(templateSheet.Cells[row, Misc.Constant.HOME_COL_INDEX]);
            }
            if (updateTeam.AwayUpdated)
            {
                FillCellColor(templateSheet.Cells[row, Misc.Constant.AWAY_COL_INDEX]);
            }
        }
    }

    private static void FillColorOtherProvider(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_OTHER_PROVIDER, match)
              || ExistsUpdateType(RAndDColorConstant.CANCEL_OTHER_PROVIDER, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.OTHER_PROVIDER_COL_INDEX]);
        }
    }

    private static void FillColorIpTv(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_IPTV, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.IPTV_COL_INDEX]);
        }
    }

    private static void FillColorTv(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_TV, match)
            || ExistsUpdateType(RAndDColorConstant.CANCEL_TV, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.TV_COL_INDEX]);
        }
    }

    private static void FillColorLeague(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_LEAGUE, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.LEAGUE_COL_INDEX]);
        }
    }

    private static void FillColorDeleteMatch(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.DELETE_MATCH, match))
        {
            var cell = templateSheet.Cells[row, Misc.Constant.REMARK_COL_INDEX];
            cell.Value = $"[DELETED] {cell.Value}";
            FillCellColor(cell);
            return;
        }

        if (ExistsUpdateType(RAndDColorConstant.MATCH_POSTPONED, match))
        {
            var cell = templateSheet.Cells[row, Misc.Constant.REMARK_COL_INDEX];
            cell.Value = $"[POSTPONED] {cell.Value}";
            FillCellColor(cell);
            return;
        }

        if (ExistsUpdateType(RAndDColorConstant.MATCH_CANCELLED, match))
        {
            var cell = templateSheet.Cells[row, Misc.Constant.REMARK_COL_INDEX];
            cell.Value = $"[CANCELLED] {cell.Value}";
            FillCellColor(cell);
        }
    }

    private static void FillColorNewMatch(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.NEW_MATCH, match))
        {
            var cell = templateSheet.Cells[row, Misc.Constant.REMARK_COL_INDEX];
            cell.Value = $"[NEW ADD] {cell.Value}";
            FillCellColor(cell);
        }
    }

    private static void FillColorKoTime(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_KO_TIME, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.TIME_COL_INDEX]);
        }
    }

    private static void FillCellColor(ExcelRange excelRange)
    {
        var color = ColorTranslator.FromHtml(Misc.Constant.ColorPinkHex);
        excelRange.Style.Fill.SetBackground(color);
    }

    private static bool ExistsUpdateType(int updateType, EventPickingModel match)
        => match.RAndDInfo is not null && match.RAndDInfo.Exists(r => r.UpdateTypeId == updateType);

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
}