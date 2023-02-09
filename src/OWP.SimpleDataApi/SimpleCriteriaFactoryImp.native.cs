namespace OWP.SimpleDataApi;

using Criteria;
using Criteria.LeagueManagement;
using Criteria.LiveDetail;
using Criteria.OsLeagueSetting;
using Criteria.TicketCenter;
using Olympus.SimpleDataApi;

public class SimpleCriteriaFactoryImp : SimpleCriteriaFactoryBase
{
    static SimpleCriteriaFactoryImp()
    {
        RegisterTicketCenter();
        RegisterOsLeagueSetting();
        RegisterLeagueManagement();
        RegisterLiveDetail();
    }

    private static void RegisterLiveDetail()
    {
        CriteriaTypes.Add(ConstantApiKey.GetTelevisionSuggestion, typeof(GetTelevisionSuggestionCriteria));

        CriteriaTypes.Add(ConstantApiKey.GetProviderSuggestion, typeof(GetProviderSuggestionCriteria));

        CriteriaTypes.Add(ConstantApiKey.GetOtherProviderSuggestion, typeof(GetOtherProviderSuggestionCriteria));

        CriteriaTypes.Add(ConstantApiKey.GetMatchSettingByMatchIdCriteria, typeof(GetMatchSettingByMatchIdCriteria));

        CriteriaTypes.Add(ConstantApiKey.GetAgentSuggestion, typeof(GetAgentSuggestionCriteria));
    }

    private static void RegisterLeagueManagement()
    {
        CommandTypes.Add(ConstantApiKey.LeagueManagementUpdateLeagueGroup, typeof(LeagueManagementUpdateLeagueGroupCriteria));

        CommandTypes.Add(ConstantApiKey.LeagueManagementUpdateLeagueIsActive, typeof(LeagueManagementUpdateIsActiveLeagueCriteria));

        CommandTypes.Add(ConstantApiKey.LeagueManagementUpdateLeagueIsBlacklist, typeof(LeagueManagementUpdateIsBlackListLeagueCriteria));

        CommandTypes.Add(ConstantApiKey.LeagueManagementInsertLeagueSetting, typeof(LeagueManagementInsertLeagueSettingCriteria));

        CommandTypes.Add(ConstantApiKey.LeagueManagementUpdateLeagueSetting, typeof(LeagueManagementUpdateLeagueSettingCriteria));

        CommandTypes.Add(ConstantApiKey.LeagueManagementDeleteLeagueSetting, typeof(LeagueManagementDeleteLeagueSettingCriteria));

        CommandTypes.Add(ConstantApiKey.LeagueManagementUpdateSpecialProduct, typeof(LeagueManagementUpdateSpecialProductCriteria));

        CommandTypes.Add(ConstantApiKey.LeagueManagementDeleteSpecialProduct, typeof(LeagueManagementDeleteSpecialProductCriteria));

        CriteriaTypes.Add(ConstantApiKey.LeagueManagementGetAllLeagueInfo, typeof(LeagueManagementGetAllLeagueInfoCriteria));

        CriteriaTypes.Add(ConstantApiKey.LeagueManagementGetLeagueSettingById, typeof(LeagueManagementGetLeagueSettingByLeagueIdCriteria));

        CriteriaTypes.Add(ConstantApiKey.LeagueManagementGetTVList, typeof(LeagueManagementGetTVListCriteria));

        CriteriaTypes.Add(ConstantApiKey.LeagueManagementGetAllSpecialProduct, typeof(LeagueManagementGetAllSpecialProductCriteria));

        CriteriaTypes.Add(ConstantApiKey.LeagueManagementGetTeamInfo, typeof(LeagueManagementGetTeamInfoCriteria));
    }

    private static void RegisterOsLeagueSetting()
    {
        CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetCategoryOption, typeof(GetCategoryOptionCriteria));
        CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetRoundOption, typeof(GetRoundOptionCriteria));
        CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetBetTypesOption, typeof(GetBetTypesOptionCriteria));
        CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetDetail, typeof(QueryLeagueSettingDetailCriteria));

        CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetBetDelayOption, typeof(GetBetDelayOptionCriteria));
        CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetOfferLiveOption, typeof(GetOfferLineOptionCriteria));
        CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetProviderOption, typeof(GetProviderOptionCriteria));
        CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetRegionOption, typeof(GetRegionOptionCriteria));
        CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetWeekMatchOption, typeof(GetWeekMatchOptionCriteria));
    }
    private static void RegisterTicketCenter()
    {
        CriteriaTypes.Add(ConstantApiKey.TicketCenterGetRecipient, typeof(GetRecipientCriteria));
        CriteriaTypes.Add(ConstantApiKey.TicketCenterGetDefaultRecipient, typeof(GetDefaultRecipientCriteria));
        CriteriaTypes.Add(ConstantApiKey.TicketSettingGetAllRecipients, typeof(GetAllRecipientCriteria));
    }
}