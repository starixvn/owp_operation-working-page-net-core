using OWP.Middleware;
using OWP.UserManagement.Models;

namespace OWP.UserManagement.Middleware;

public class GetUserValidator : AsyncFilter<QueryModel>
{
    public override (bool, int, string) Execute(QueryModel model)
    {
        var statusCode = 400;
        if (string.IsNullOrEmpty(model.RoleId))
        {
            return (false, statusCode, "Please select at least one option for Role");
        }
        if (string.IsNullOrEmpty(model.SectionId))
        {
            return (false, statusCode, "Please select at least one option for Section");
        }

        if (string.IsNullOrEmpty(model.DepartmentId))
        {
            return (false, statusCode, "Please select at least one option for Department");
        }
        if (string.IsNullOrEmpty(model.CategoryId))
        {
            return (false, statusCode, "Please select at least one option for Category");
        }
        if (string.IsNullOrEmpty(model.LocationCode))
        {
            return (false, statusCode, "Please select at least one option for Location");
        }

        statusCode = 200;
        return (true, statusCode, string.Empty);
    }
}