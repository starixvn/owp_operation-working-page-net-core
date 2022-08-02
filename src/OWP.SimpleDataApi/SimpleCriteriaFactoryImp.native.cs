using Olympus.SimpleDataApi;
using OWP.SimpleDataApi.Criteria.OsLeagueSetting;
using OWP.SimpleDataApi.Criteria.TicketCenter;

namespace OWP.SimpleDataApi
{
    public class SimpleCriteriaFactoryImp : SimpleCriteriaFactoryBase
    {
        static SimpleCriteriaFactoryImp()
        {
            RegisterTicketCenter();
            RegisterOsLeagueSetting();
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