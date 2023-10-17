using System.Drawing;
using OfficeOpenXml;
using OWP.EventPicking.Constant;
using OWP.EventPicking.Models;

namespace OWP.EventPicking.Helper;

public static class ExcelColorHelper
{
    public static void FillRowColor(IList<EventPickingModel> matches, ExcelWorksheet templateSheet)
    {
        for (int i = 0, row = 2; i < matches.Count; i++, row++)
        {
            var match = matches[i];
            for (var column = 1; column <= 90; column++)
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
            FillColorKoTime(matches[i], row, templateSheet);
            FillColorLeague(matches[i], row, templateSheet);
            FillColorTeam(matches[i], row, templateSheet);
            FillColorTv(matches[i], row, templateSheet);
            FillColorIpTv(matches[i], row, templateSheet);
            FillColorProvider(matches[i], row, templateSheet);
        }
    }

    private static void FillColorTeam(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        var updateTeam = match.RAndDInfos?.FirstOrDefault(r => r.UpdateTypeId == RAndDColorConstant.UPDATE_TEAM);
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

    private static void FillColorProvider(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_PROVIDER, match)
              || ExistsUpdateType(RAndDColorConstant.CANCEL_PROVIDER, match))
        {
            FillCellColor(templateSheet.Cells[row, Misc.Constant.PROVIDER_COL_INDEX]);
        }
    }

    private static void FillColorIpTv(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.UPDATE_IPTV, match)
                || ExistsUpdateType(RAndDColorConstant.UPDATE_IPTV, match))
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

    private static void FillColorNewMatch(EventPickingModel match, int row, ExcelWorksheet templateSheet)
    {
        if (ExistsUpdateType(RAndDColorConstant.NEW_MATCH, match))
        {
            var cell = templateSheet.Cells[row, Misc.Constant.REMARK_COL_INDEX];
            FillCellColor(cell);
            cell.Value = !string.IsNullOrWhiteSpace(cell.Value?.ToString()) ?
            $"[NEW ADD] {cell.Value}"
            : cell.Value;
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
    => match.RAndDInfos is not null && match.RAndDInfos.Exists(r => r.UpdateTypeId == updateType);
}