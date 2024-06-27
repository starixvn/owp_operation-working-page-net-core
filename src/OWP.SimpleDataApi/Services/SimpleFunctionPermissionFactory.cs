namespace OWP.SimpleDataApi.Services;

public interface SimpleFunctionPermissionFactory
{
    FunctionPermission GetFunctionPermission(string apiKey);
}

public class SimpleFunctionPermissionFactoryBase : SimpleFunctionPermissionFactory
{
    protected static Dictionary<string, FunctionPermission> FunctionPermissions = new();

    protected static void AddFunctionPermission(string apiKey, string functionKey, Constant.Policy policy)
    {
        var functionPermission = new FunctionPermission
        {
            Key = functionKey,
            Policy = policy
        };

        FunctionPermissions.Add(apiKey, functionPermission);
    }

    public virtual FunctionPermission GetFunctionPermission(string apiKey)
    {
        return FunctionPermissions.ContainsKey(apiKey) ? FunctionPermissions[apiKey] : null;
    }
}