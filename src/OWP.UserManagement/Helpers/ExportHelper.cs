using OWP.Models;
using OWP.UserManagement.Models;

namespace OWP.UserManagement.Helpers;
public static class ExportHelper
{
    public static void BuildCorrectDataToExport(UserManagementViewModel dataResult)
    {
        foreach (var item in dataResult.Users)
        {
            item.Pickup = ConvertValue(item.Pickup);
            item.IsSecondAccount = ConvertValue(item.IsSecondAccount);
        }
    }

    private static string ConvertValue(string value)
    {
        switch (value.ToLower()) 
        {
            case "true":
                return "yes";
            case "false":
                return "no";;
            default:
                return "none"; 
        }
    }
}
