namespace OWP.SimpleDataApi;

using Fanex.NAP.Core.Business;
using Olympus.SimpleDataApi;

public class SimpleFunctionPermissionFactoryImp : SimpleFunctionPermissionFactoryBase
{
    static SimpleFunctionPermissionFactoryImp()
    {
        AddFunctionPermission(ConstantApiKey.TicketCenterGetRecipient, FunctionKeys.TicketCenter, Constant.Policy.Read);
    }
}