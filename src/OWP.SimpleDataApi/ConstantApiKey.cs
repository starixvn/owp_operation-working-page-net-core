namespace OWP.SimpleDataApi;

public static class ConstantApiKey
{
    #region TicketCenter

    public const string TicketCenterGetRecipient = "ticket-center-get-recipient";

    public const string TicketCenterGetDefaultRecipient = "ticket-center-get-default-recipient";

    public const string TicketSettingGetAllRecipients = "ticket-setting-get-all-recipients";

    #endregion

    #region OSLeagueSetting

    public const string OsLeagueSettingGetCategoryOption = "os-league-setting-get-category-option";

    public const string OsLeagueSettingGetRoundOption = "os-league-setting-get-round-option";

    public const string OsLeagueSettingGetDetail = "os-league-setting-get-detail";

    public const string OsLeagueSettingGetBetTypesOption = "os-league-setting-get-bet-types-option";

    public const string OsLeagueSettingGetBetDelayOption = "os-league-setting-get-bet-delay-option";

    public const string OsLeagueSettingGetOfferLiveOption = "os-league-setting-get-offer-line-option";

    public const string OsLeagueSettingGetProviderOption = "os-league-setting-get-provider-option";

    public const string OsLeagueSettingGetRegionOption = "os-league-setting-get-region-option";

    public const string OsLeagueSettingGetWeekMatchOption = "os-league-setting-get-week-match-option";

    #endregion

    #region LeagueManagement

    public const string LeagueManagementGetAllLeagueInfo = "league-management-get-all-league";

    public const string LeagueManagementGetLeagueSettingById = "league-management-get-league-setting-by-id";

    public const string LeagueManagementGetTVList = "league-management-get-tv-list";

    public const string LeagueManagementUpdateLeagueIsActive = "league-management-update-league-is-active";

    public const string LeagueManagementUpdateLeagueIsBlacklist = "league-management-update-league-is-blacklist";

    public const string LeagueManagementUpdateLeagueGroup = "league-management-update-league-group";

    public const string LeagueManagementInsertLeagueSetting = "league-management-insert-league-setting";

    public const string LeagueManagementUpdateLeagueSetting = "league-management-update-league-setting";

    public const string LeagueManagementDeleteLeagueSetting = "league-management-delete-league-setting";

    public const string LeagueManagementGetAllSpecialProduct = "league-management-get-all-special-product";

    public const string LeagueManagementUpdateSpecialProduct = "league-management-update-special-product";

    public const string LeagueManagementDeleteSpecialProduct = "league-management-delete-special-product";

    public const string LeagueManagementGetTeamInfo = "league-management-get-team-info";

    public const string GetAgentSuggestion = "get-agent-suggestion";

    public const string GetMatchSettingByMatchIdCriteria = "live-detail-get-matchsetting-by-id";

    public const string GetOtherProviderSuggestion = "get-other-provider-suggestion";

    public const string GetProviderSuggestion = "get-provider-suggestion";

    public const string GetTelevisionSuggestion = "get-television-suggestion";

    #endregion

    #region PreMatchAssignment 

    public const string GetLastUpdatedPreMatchAssignment = "pre-match-assignment-get-last-updated";
    public const string GetStartTime = "pre-match-assignment-get-start-time";
    public const string GetDeletedLeaguesPreMatchAssignment = "pre-match-assignment-get-deleted-leagues";
    public const string PreMatchAssignmentRecover = "pre-match-assignment-recover";
    public const string CheckLeagueContainsAssignment = "pre-match-assignment-check-league-contains-assignment";
    public const string PreMatchAssignmentValidateAssignment = "pre-match-assignment-validate-assignment";
    public const string PreMatchAssignmentGetNoteMessages = "pre-match-assignment-get-note-messages";
    public const string PreMatchAssignmentDeleteNoteMessages = "pre-match-assignment-delete-note-messages";

    #endregion

    #region LiveGameAssignment
    public const string GetBettypeForTradingInfo = "live-game-assignment-get-bettype-for-trading-info";

    public const string GetBettypeForSpecialEvent = "live-game-assignment-get-bettype-for-special-event";

    public const string GetSpecialEventTemplate = "live-game-assignment-get-special-event-template";
    #endregion

    public const string GetUserChangeFromAdminSite = "get-user-change-from-admin-site";
    public const string SyncUserChangeFromAdminSite = "sync-user-change-from-admin-site";

    public const string UpdateUser = "update-user";

    #region employeeWorkingShift
    public const string WorkingShiftGetUsers = "working-shift-get-user";

    public const string WorkingShiftUpdateSingleShift = "working-shift-update-single-shift";

    public const string WorkingShiftUpdateRemarkWeekly = "working-shift-update-remark-weekly";

    public const string WorkingShiftUpdateRemarkDaily = "working-shift-update-remark-daily";

    public const string EmployeeWorkingShiftInsertNote = "employee-working-shift-insert-note";

    public const string EmployeeWorkingShiftQueryNoteById = "employee-working-shift-query-note-by-id";

    public const string EmployeeWorkingShiftUpdateNote = "employee-working-shift-update-note";

    public const string EmployeeWorkingShiftDeleteNote = "employee-working-shift-delete-note";
    #endregion employeeWorkingShift

}