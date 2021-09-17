using Olympus.SimpleDataApi;
using Fanex.NAP.Core.Business;

namespace OWP.SimpleDataApi
{
    public class SimpleFunctionPermissionFactoryImp : SimpleFunctionPermissionFactoryBase
    {
        static SimpleFunctionPermissionFactoryImp()
        {
            AddFunctionPermission(ConstantApiKey.TicketCenterGetRecipient, FunctionKeys.TicketCenter, Constant.Policy.Read);
        }
    }
}
