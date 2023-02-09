using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Olympus.Xml;
using OWP.TicketTypeRecipient.Handlers;
using OWP.TicketTypeRecipient.Middleware.Models;

namespace OWP.TicketTypeRecipient.Middleware;

public class DuplicateSubtypeValidator : IAsyncActionFilter
{
    private readonly IDynamicRepository repository;

    public DuplicateSubtypeValidator(IDynamicRepository dynamicRepository)
    {
        repository = dynamicRepository;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var param = context
            .ActionArguments
            .SingleOrDefault(p => p.Value is SaveTicketTypeRecipientsCommand)
            .Value;

        if (param is SaveTicketTypeRecipientsCommand model)
        {
            var unique = DuplicateSubtypeInInternalTicketType(model);

            var newSubtypes = BuildSubtypesCheck(model);

            var subTypeExistsCurrentTicketType = newSubtypes.GroupBy(x => x.SubtypeName).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
            var lstSubtypeExistsAnotherTicketType = await CheckExistsSubtype(newSubtypes, model.TicketTypeId);

            if (!unique || lstSubtypeExistsAnotherTicketType.Any() || subTypeExistsCurrentTicketType.Any())
            {
                var subTypeInvalid = lstSubtypeExistsAnotherTicketType.Concat(subTypeExistsCurrentTicketType).Select(x => x.ToLowerInvariant()).Distinct();
                context.HttpContext.Response.StatusCode = 400;
                context.Result = new ContentResult
                {
                    Content = BuildErrorMessage(subTypeInvalid.ToList()),
                };
                return;
            }
        }
        await next();
    }

    private string BuildErrorMessage(IList<string> subtypeInvalid)
        => $"A subtype with this name <b>{string.Join(", ", subtypeInvalid)}</b> already exists. Please try again!";

    private bool DuplicateSubtypeInInternalTicketType(SaveTicketTypeRecipientsCommand model)
        => model.Subtypes
            .GroupBy(x => x.SubtypeName)
            .All(g => g.Count() == 1);

    private IList<SubtypeChecker> BuildSubtypesCheck(SaveTicketTypeRecipientsCommand model)
        => model
            .Subtypes
            .Select(x => new SubtypeChecker
            {
                SubtypeName = x.SubtypeName.ToLowerInvariant()
            })
            .ToList();

    private async Task<IList<string>> CheckExistsSubtype(IList<SubtypeChecker> subtypeNames, int ticketTypeId)
    {
        var criteria = new CheckExistsSubtypeCriteria
        {
            Xml = XmlConverter.Serialize(subtypeNames),
            TicketTypeId = ticketTypeId
        };
        var result = await repository.FetchAsync<string>(criteria);
        return result.ToList();
    }
}

public class CheckExistsSubtypeCriteria : CriteriaBase
{
    public string Xml { get; set; }

    public int TicketTypeId { get; set; }

    public override string GetSettingKey() => "OWP_TicketTypeRecipient_CheckExists_Subtype";

    public override bool IsValid() => true;
}