using OWP.EventPicking.Middleware.Models;
using OWP.Middleware;

namespace OWP.EventPicking.Middleware;
public class GetEventValidator : AsyncFilter<EventPickingRequest>
{
    public override (bool, int, string) Execute(EventPickingRequest model)
    {
        var statusCode = 400;
        if (!model.SelectGame.Any())
        {
            return (false, statusCode, "Please select at least one option for Select Game");
        }
        if (!model.FilterMatch.Any())
        {
            return (false, statusCode, "Please select at least one option for Filter Match");
        }

        if (!model.HasProvider.Any())
        {
            return (false, statusCode, "Please select at least one option for Provider");
        }
        if (!model.HasOtherProvider.Any())
        {
            return (false, statusCode, "Please select at least one option for Other Provider");
        }
        statusCode = 200;
        return (true, statusCode, string.Empty);
    }
}