using Fanex.NAP.Core.Business;
using OWP.SimpleDataApi.Core;

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