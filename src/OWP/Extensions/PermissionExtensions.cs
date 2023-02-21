using Fanex.NAP.Client.Models.ValueObject;
using Fanex.NAP.Core.Business;
using OWP.Models;

namespace OWP.Extensions;

public static class PermissionExtensions
{
    public static PermissionModel GetPermissionModel(this IList<FunctionPolicy> policies, string functionKey)
    {
        var policy = policies.FirstOrDefault(x => x.Key == functionKey);
        return policy != null
            ? new PermissionModel(policy.Policy >= (int)Constant.Policy.Read, policy.Policy == (int)Constant.Policy.Host)
            : new PermissionModel();
    }
}