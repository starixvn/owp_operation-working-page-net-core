using OWP.EventPicking.Api.Models;


namespace OWP.EventPicking;

public static class Utils
{
    public static string GetMiscMatchTemplate(string currentDirectory)
    {
        const string templatePath = @"App_Data/Templates/EventPickingNewFormat.xlsx";
        return $"{currentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
    }

    public static string GetLSTFormatTemplate(string currentDirectory)
    {
        const string templatePath = @"App_Data/Templates/Mismatch-LST format.xlsx";
        return $"{currentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
    }

    public static void CalculateValueTurnover(TurnOverWinLossModel turnover_winLoss, TurnOverInfo turnoverInfo)
    {
        if (turnover_winLoss is null)
        {
            return;
        }

        if (Math.Abs(turnover_winLoss.Turnover) >= double.Epsilon)
        {
            var value = Math.Round(turnover_winLoss.Turnover, 2);
            value = Math.Abs(value).Equals(0) ? 0 : value;

            turnoverInfo.ValueDisplay = $"{value:#,0.00}";
            turnoverInfo.Value = value;

            if (value < 0)
            {
                turnoverInfo.Color = "red";
            }
        }
    }

    public static void CalculateValueWinLoss(TurnOverWinLossModel turnover_winLoss, MarginInfo marginInfo)
    {
        if (turnover_winLoss is null)
        {
            return;
        }

        if (Math.Abs(turnover_winLoss.WinLoss) < double.Epsilon)
        {
            marginInfo.ValueDisplay = "-";
        }
        else
        {
            var value = Math.Round(turnover_winLoss.WinLoss / turnover_winLoss.Turnover, 3);
            value = Math.Abs(value).Equals(0) ? 0 : value;

            marginInfo.ValueDisplay = $"{value:#,0.000}";
            marginInfo.Value = value;

            if (value < 0)
            {
                marginInfo.Color = "red";
            }
        }
    }
}