using OWP.EventPicking.Models;

namespace OWP.EventPicking;

public static class Utils
{
    public static string GetMiscMatchTemplate(string currentDirectory)
    {
        const string templatePath = @"App_Data/Templates/EventPickingNewFormat.xlsx";
        return $"{currentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
    }

    public static void CalculateValueTurnover_WinLoss(EventPickingModel match, TurnOverWinLossModel turnover_winLoss)
    {
        if (Math.Abs(turnover_winLoss.Turnover) >= double.Epsilon)
        {
            match.Turnover.Value = $"{Math.Round(turnover_winLoss.Turnover, 2)}";
        }

        if (Math.Abs(turnover_winLoss.WinLoss) < double.Epsilon)
        {
            match.Margin.Value = "-";
        }
        else if (Math.Abs(turnover_winLoss.Turnover) >= double.Epsilon)
        {
            match.Margin.Value = $"{Math.Round(turnover_winLoss.WinLoss / turnover_winLoss.Turnover, 2)}";
        }
    }
    public static void CalculateColorTurnover_WinLoss(EventPickingModel match, TurnOverWinLossModel turnover_winLoss)
    {
        if (turnover_winLoss.Turnover < 0)
        {
            match.Turnover.Color = "red";
        }

        if (Math.Abs(turnover_winLoss.Turnover) >= double.Epsilon && (turnover_winLoss.WinLoss / turnover_winLoss.Turnover) < 0)
        {
            match.Margin.Color = "red";
        }
    }
}