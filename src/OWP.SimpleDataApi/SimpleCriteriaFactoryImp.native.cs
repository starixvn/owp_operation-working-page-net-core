using Olympus.SimpleDataApi;
using OWP.SimpleDataApi.Criteria.TicketCenter;

namespace OWP.SimpleDataApi
{
    public class SimpleCriteriaFactoryImp : SimpleCriteriaFactoryBase
    {
        static SimpleCriteriaFactoryImp()
        {
            RegisterTicketCenter();
        }

        private static void RegisterTicketCenter()
        {
            CriteriaTypes.Add(ConstantApiKey.TicketCenterGetRecipient, typeof(GetRecipientCriteria));
            CriteriaTypes.Add(ConstantApiKey.TicketCenterGetDefaultRecipient, typeof(GetDefaultRecipientCriteria));
            CriteriaTypes.Add(ConstantApiKey.TicketSettingGetAllRecipients, typeof(GetAllRecipientCriteria));
        }
    }
}