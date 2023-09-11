using Olympus.SimpleDataApi;

namespace OWP.SimpleDataApi;

using Fanex.NAP.Client.Helpers.Business;

public class SimpleFunctionPermissionFactoryImp : SimpleFunctionPermissionFactoryBase
{
    static SimpleFunctionPermissionFactoryImp()
    {
        AddFunctionPermission(ConstantApiKey.TicketCenterGetRecipient, FunctionKeys.TicketCenter, Constant.Policy.Read);
    }
}