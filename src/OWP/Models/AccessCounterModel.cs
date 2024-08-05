using Microsoft.Extensions.Options;

namespace OWP.Models;

public class AccessCounterModel
{
    public string ExportMiscMatch { get; set; }

    public string OsExportExcel { get; set; }

    public string OsUpload { get; set; }

    public string EventPicking { get; set; }

    public string EventPickingNew { get; set; }

    public string TaskListNew { get; set; }

    public string UserManagement { get; set; }

    public string WorkingShiftManagement { get; set; }

    public string DailyWorkingShift { get; set; }

    public string AssignmentConfiguration { get; set; }

    public string ProviderArrangement { get; set; }

    public string EventUploadNew { get; set; }

    public string LiveGameAssignment { get; set; }

    public string LiveGameList { get; set; }

    public string LiveGameListNew { get; set; }

    public string TraderReport { get; set; }

    public string LAReport { get; set; }

    public string OvertimeReport { get; set; }

    public string MatchCountLiveReport { get; set; }

    public string AutoAssignment { get; set; }

    public string MyTickets { get; set; }

    public string TicketTypeSettings { get; set; }

    public string RecipientGroup { get; set; }

    public string DayShiftAssignment { get; set; }

    public string NightShiftAssignment { get; set; }

    public string PrematchReport { get; set; }

    public string LeagueManagement { get; set; }

    public string OddsCompiler { get; set; }

    public string OSLiveGameAssignment { get; set; }

    public string OSBasketballMaxBet { get; set; }

    public string EventPicking_Export_Excel { get; set; }

    public string LiveGameAssignment_Export { get; set; }

    public string LiveGameAssignment_Export_Event_Picking { get; set; }

    public string AssigneeSuggestionList { get; set; }

    public string AddNewLeagueSetting { get; set; }

    public IOptions<AccessCounterModel> Value { get; set; }

    public override string ToString()
    => $"EventUploadNew: {EventUploadNew} + ExportMiscMatch:{ExportMiscMatch} + OsExportExcel:{OsExportExcel} + OsUpload:{OsUpload}";
}