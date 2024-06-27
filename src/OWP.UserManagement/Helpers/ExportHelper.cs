using OWP.Models;

namespace OWP.UserManagement.Helpers;
public static class ExportHelper
{
    public static void BuildCorrectDataToExport(IList<User> user)
    {
        foreach (var item in user)
        {
            item.Pickup = ConvertValue(item.Pickup);
            item.IsSecondAccount = ConvertValue(item.IsSecondAccount);
        }
    }

    private static string ConvertValue(string value) => value.ToLowerInvariant() switch
    {
        "true" => "yes",
        "false" => "no",
        _ => "none"
    };
}
