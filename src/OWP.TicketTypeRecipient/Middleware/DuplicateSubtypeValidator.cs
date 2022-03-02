using Microsoft.AspNetCore.Mvc.Filters;
using OWP.TicketTypeRecipient.Handlers;
using Microsoft.AspNetCore.Mvc;
using Fanex.Data.Repository;
using OWP.Extensions;
using OWP.TicketTypeRecipient.Middleware.Models;
using OWP.TicketTypeRecipient.Models;

namespace OWP.TicketTypeRecipient.Middleware
{
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

        if (!unique || lstSubtypeExistsAnotherTicketType.Count() > 0)
        {
          var subtypeExistToString = string.Join(",", lstSubtypeExistsAnotherTicketType.Concat(subTypeExistsCurrentTicketType).Distinct());
          context.HttpContext.Response.StatusCode = 400;
          context.Result = new ContentResult
          {
            Content = $"A subtype with this name '{subtypeExistToString}' already exists. Please try again"
          };
          return;
        }

      }
      await next();
    }

    private bool DuplicateSubtypeInInternalTicketType(SaveTicketTypeRecipientsCommand model)
    => model.Subtypes
        .GroupBy(x => x.SubtypeName)
        .All(g => g.Count() == 1);

    private IList<SubtypeChecker> BuildSubtypesCheck(SaveTicketTypeRecipientsCommand model)
    => model
          .Subtypes
          .Select(x => new SubtypeChecker
          {
            SubtypeName = x.SubtypeName
          })
          .ToList();


    private async Task<IList<string>> CheckExistsSubtype(IList<SubtypeChecker> subtypeNames, int ticketTypeId)
    {
      var criteria = new CheckExistsSubtypeCriteria
      {
        Xml = subtypeNames.ToXml().ToString(),
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
}