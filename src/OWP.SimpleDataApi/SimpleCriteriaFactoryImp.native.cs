using Olympus.SimpleDataApi;
using OWP.SimpleDataApi.Criteria.OsLeagueSetting;
using OWP.SimpleDataApi.Criteria.LeagueManagement;
using OWP.SimpleDataApi.Criteria.LiveDetail;
using OWP.SimpleDataApi.Criteria.TicketCenter;

namespace OWP.SimpleDataApi
{
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
            CriteriaTypes.Add(ConstantApiKey.GetAgentSuggestion, typeof(GetAgentSuggestionCriteria));

            CriteriaTypes.Add(ConstantApiKey.GetProviderSuggestion, typeof(GetProviderSuggestionCriteria));

            CriteriaTypes.Add(ConstantApiKey.GetOtherProviderSuggestion, typeof(GetOtherProviderSuggestionCriteria));

            CriteriaTypes.Add(ConstantApiKey.GetTelevisionSuggestion, typeof(GetTelevisionSuggestionCriteria));

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
            CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetStatusOption, typeof(GetStatusOptionCriteria));
            CriteriaTypes.Add(ConstantApiKey.OsLeagueSettingGetBetTypesOption, typeof(GetBetTypesOptionCriteria));
        }
        private static void RegisterTicketCenter()
        {
            CriteriaTypes.Add(ConstantApiKey.TicketCenterGetRecipient, typeof(GetRecipientCriteria));
            CriteriaTypes.Add(ConstantApiKey.TicketCenterGetDefaultRecipient, typeof(GetDefaultRecipientCriteria));
            CriteriaTypes.Add(ConstantApiKey.TicketSettingGetAllRecipients, typeof(GetAllRecipientCriteria));
        }
    }
}