
namespace OWP.SimpleDataApi;

public class SimpleFunctionPermissionFactoryImp : SimpleFunctionPermissionFactoryBase
{
    static SimpleFunctionPermissionFactoryImp()
    {
        AddFunctionPermission(ConstantApiKey.TicketCenterGetRecipient, FunctionKeys.TicketCenter, Constant.Policy.Read);
        AddFunctionPermission(ConstantApiKey.WorkingShiftGetUsers, FunctionKeys.WorkingShiftManagement, Constant.Policy.Read);
        AddFunctionPermission(ConstantApiKey.WorkingShiftUpdateSingleShift, FunctionKeys.WorkingShiftManagement, Constant.Policy.Host);
        AddFunctionPermission(ConstantApiKey.WorkingShiftUpdateRemarkWeekly, FunctionKeys.WorkingShiftManagement, Constant.Policy.Host);
        AddFunctionPermission(ConstantApiKey.WorkingShiftUpdateRemarkDaily, FunctionKeys.DailyWorkingShift, Constant.Policy.Host);
        AddFunctionPermission(ConstantApiKey.EmployeeWorkingShiftInsertNote, FunctionKeys.DailyWorkingShift, Constant.Policy.Host);
        AddFunctionPermission(ConstantApiKey.EmployeeWorkingShiftDeleteNote, FunctionKeys.DailyWorkingShift, Constant.Policy.Host);
    }
}