namespace OWP.Constants;

using System.Collections.Generic;

public static class MenuItemExtensions
{
    private static Dictionary<string, string> MenuItemIcons()
    {
        var dictionary = new Dictionary<string, string>
        {
            { MenuItems.TaskList, "icon-home" },
            { MenuItems.LiveGameAssignment, "icon-calendar-clock" },
            { MenuItems.LiveGameAssignmentNew, "icon-calendar-clock" },
            { MenuItems.ProviderArrangement, "icon-provider-arrangement" },
            { MenuItems.EventUpload, "icon-cloud-upload" },
            { MenuItems.EventPicking, "icon-calendar-check" },
            { MenuItems.EventPickingNew, "icon-calendar-check" },
            { MenuItems.LiveGameList, "icon-video" },
            { MenuItems.UserManagement, "icon-account-multiple" },
            { MenuItems.AssignmentConfiguration, "icon-account-multiple" },
            { MenuItems.WorkingShiftManagement, "icon-calendar-today" },
            { MenuItems.LeagueManagement, "icon-book" },
            { MenuItems.OddsCompiler, "icon-dice-2" },
            { MenuItems.DailyWorkingShift, "icon-calendar-today" },
            { MenuItems.TraderReport,"icon-file-chart" },
            { MenuItems.LAReport,"icon-Path" },
            { MenuItems.LAKPI, "icon-kpi"},
            { MenuItems.OvertimeReport,"icon-overtime-report" },
            { MenuItems.MatchCountLiveReport,"icon-Path" },
            { MenuItems.AutoAssignment, "icon-auto-assignment" },
            { MenuItems.DayShiftAssignment, "icon-day" },
            { MenuItems.NightShiftAssignment,"icon-night" },
            { MenuItems.PrematchReport, "icon-calendar-clock" },
            { MenuItems.EventTickets, "icon-event-checking" },
            { MenuItems.MyTickets, "icon-event-checking" },
            { MenuItems.RecipientGroup, "icon-recipient-group" },
            { MenuItems.TicketTypeSettings, "icon-ticket-type-recipient" },
            { MenuItems.OSLiveGameAssignment, "icon-event-checking" },
            { MenuItems.OSBasketballMaxBet, "icon-event-checking" },
        };
        return dictionary;
    }

    public static string GetIconCssClass(string menuName)
    {
        var dictionary = MenuItemIcons();
        return dictionary.ContainsKey(menuName) ? dictionary[menuName] : string.Empty;
    }
}