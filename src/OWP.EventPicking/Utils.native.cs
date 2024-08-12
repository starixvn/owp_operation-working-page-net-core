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

    public static void CalculateValueTurnover_WinLoss(TurnOverWinLossModel turnover_winLoss,
    EventPickingModel match,
    string turnoverProperty,
    string marginProperty
    )
    {
        if (turnover_winLoss is null)
        {
            return;
        }

        var turnoverInfoProperty1 = match.GetType().GetProperty(turnoverProperty);
        var marginInfoProperty = match.GetType().GetProperty(marginProperty);

        if (Math.Abs(turnover_winLoss.Turnover) >= double.Epsilon)
        {
            var value = Math.Round(turnover_winLoss.Turnover, 2);
            var turnoverInfo = (TurnOverInfo)turnoverInfoProperty1.GetValue(match);
            var turnoverInfoType = turnoverInfo.GetType();
            var valueDisplayProperty = turnoverInfoType.GetProperty("ValueDisplay");
            var valueProperty = turnoverInfoType.GetProperty("Value");
            valueDisplayProperty.SetValue(turnoverInfo, $"{value:#,0.00}");
            valueProperty.SetValue(turnoverInfo, value);
        }

        if (Math.Abs(turnover_winLoss.WinLoss) < double.Epsilon)
        {
            var marginInfo = (MarginInfo)marginInfoProperty.GetValue(match);
            var marginInfoType = marginInfo.GetType();
            var valueDisplayProperty = marginInfoType.GetProperty("ValueDisplay");
            valueDisplayProperty.SetValue(marginInfo, "-");
        }
        else
        {
            var value = Math.Round(turnover_winLoss.WinLoss / turnover_winLoss.Turnover, 2);
            var marginInfo = (MarginInfo)marginInfoProperty.GetValue(match);
            var marginInfoType = marginInfo.GetType();
            var valueDisplayProperty = marginInfoType.GetProperty("ValueDisplay");
            var valueProperty = marginInfoType.GetProperty("Value");
            valueDisplayProperty.SetValue(marginInfo, $"{value:#,0.00}");
            valueProperty.SetValue(marginInfo, value);
        }
    }
    public static void CalculateColorTurnover_WinLoss(TurnOverWinLossModel turnover_winLoss,
    EventPickingModel match,
    string turnoverProperty,
    string marginProperty)
    {
        if (turnover_winLoss is null)
        {
            return;
        }
        var turnoverInfoProperty = match.GetType().GetProperty(turnoverProperty);
        var marginInfoProperty = match.GetType().GetProperty(marginProperty);

        if (turnover_winLoss.Turnover < 0)
        {
            var turnOverInfo = (TurnOverInfo)turnoverInfoProperty.GetValue(match);
            var marginInfoType = turnOverInfo.GetType();
            var colorProperty = marginInfoType.GetProperty("Color");
            colorProperty.SetValue(turnOverInfo, "red");
        }

        if (Math.Abs(turnover_winLoss.Turnover) >= double.Epsilon && turnover_winLoss.WinLoss / turnover_winLoss.Turnover < 0)
        {
            var marginInfo = (MarginInfo)marginInfoProperty.GetValue(match);
            var marginInfoType = marginInfo.GetType();
            var colorProperty = marginInfoType.GetProperty("Color");
            colorProperty.SetValue(marginInfo, "red");
        }
    }
}