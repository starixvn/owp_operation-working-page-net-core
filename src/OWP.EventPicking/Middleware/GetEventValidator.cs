using OWP.EventPicking.Middleware.Models;
using OWP.Middleware;

namespace OWP.EventPicking.Middleware;
public class GetEventValidator : AsyncFilter<EventPickingRequest>
{
    public override (bool, int, string) Execute(EventPickingRequest model)
    {
        var canNext = true;
        var statusCode = 400;
        if (model.SelectGame is null)
        {
            canNext = false;
            return (canNext, statusCode, "Please select at least one option for Select Game");
        }
        if (model.FilterMatch is null)
        {
            canNext = false;
            return (canNext, statusCode, "Please select at least one option for Filter Match");
        }

        if (model.HasProvider is null)
        {
            canNext = false;
            return (canNext, statusCode, "Please select at least one option for Other Provider");
        }
        if (model.HasOtherProvider is null)
        {
            canNext = false;
            return (canNext, statusCode, "Please select at least one option for Has Other Provider");
        }
        statusCode = 200;
        canNext = true;
        return (canNext, statusCode, string.Empty);
    }
}