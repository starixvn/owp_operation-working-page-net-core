using OWP.EventPicking.Middleware.Models;
using OWP.EventPicking.Models;

namespace OWP.EventPicking;

public static class Utils
{
    public static string GetMiscMatchTemplate(string currentDirectory)
    {
        const string templatePath = @"App_Data/Templates/EventPickingNewFormat.xlsx";
        return $"{currentDirectory}{Path.DirectorySeparatorChar}{templatePath}";
    }

    public static void CalculateValueTurnover_WinLoss(TurnOverWinLossModel turnover_winLoss,
        TurnOverInfo turnover,
        MarginInfo margin)
    {
        if (turnover_winLoss is null)
        {
            return;
        }
        if (Math.Abs(turnover_winLoss.Turnover) >= double.Epsilon)
        {
            var value = Math.Round(turnover_winLoss.Turnover, 2);
            //match.Turnover.ValueDisplay = string.Format("{0:#,0.00}", value);
            //match.Turnover.Value = value;

            var valueDisplayField = typeof(TurnOverInfo).GetField("ValueDisplay");
            var valueField = typeof(TurnOverInfo).GetField("Value");

            valueDisplayField.SetValue(turnover, string.Format("{0:#,0.00}", value));
            valueField.SetValue(turnover, value);

        }

        if (Math.Abs(turnover_winLoss.WinLoss) < double.Epsilon)
        {
            var valueDisplayField = typeof(MarginInfo).GetField("ValueDisplay");
            valueDisplayField.SetValue(margin, "-");
            //match.Margin.ValueDisplay = "-";
        }
        else
        {
            var value = Math.Round(turnover_winLoss.WinLoss / turnover_winLoss.Turnover, 2);
            //match.Margin.ValueDisplay = string.Format("{0:#,0.00}", value);
            //match.Margin.Value = value;

            var valueDisplayField = typeof(MarginInfo).GetField("ValueDisplay");
            var valueField = typeof(MarginInfo).GetField("Value");

            valueDisplayField.SetValue(margin, string.Format("{0:#,0.00}", value));
            valueField.SetValue(margin, value);
        }
    }
    public static void CalculateColorTurnover_WinLoss(TurnOverWinLossModel turnover_winLoss,
     TurnOverInfo turnover,
        MarginInfo margin)
    {
        if (turnover_winLoss.Turnover < 0)
        {
            //match.Turnover.Color = "red";
            var colorField = typeof(TurnOverInfo).GetField("Color");
            colorField.SetValue(turnover, "red");
        }

        if (Math.Abs(turnover_winLoss.Turnover) >= double.Epsilon && (turnover_winLoss.WinLoss / turnover_winLoss.Turnover) < 0)
        {
            //match.Margin.Color = "red";
            var colorField = typeof(MarginInfo).GetField("Color");
            colorField.SetValue(margin, "red");
        }
    }
}