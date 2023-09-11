using Fanex.NAP.Client.Models.ValueObject;
using OWP.Models;

namespace OWP.Extensions;

using Fanex.NAP.Client.Helpers.Business;

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