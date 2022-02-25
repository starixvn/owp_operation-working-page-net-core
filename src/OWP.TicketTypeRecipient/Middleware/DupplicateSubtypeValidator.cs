using Microsoft.AspNetCore.Mvc.Filters;
using OWP.TicketTypeRecipient.Handlers;
using Microsoft.AspNetCore.Mvc;
using Fanex.Data.Repository;
using OWP.Extensions;
using OWP.TicketTypeRecipient.Middleware.Models;
using OWP.TicketTypeRecipient.Models;

namespace OWP.TicketTypeRecipient.Middleware
{
  public class DupplicateSubtypeValidator : IAsyncActionFilter
  {
    private readonly IDynamicRepository repository;

    public DupplicateSubtypeValidator(IDynamicRepository dynamicRepository)
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

        var newSubtypes = buildSubtypesCheck(model);
        var lstSubtypeExists = await CheckExistsSubtype(newSubtypes);

        if (!unique || lstSubtypeExists.Count() > 0)
        {
          context.HttpContext.Response.StatusCode = 400;
          context.Result = new ContentResult
          {
            Content = "A subtype with this name already exists. Please try again"
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

    private IList<SubtypeChecker> buildSubtypesCheck(SaveTicketTypeRecipientsCommand model)
    => model
          .Subtypes
          .Where(x => x.SubtypeId == 0)
          .Select(x => new SubtypeChecker
          {
            SubtypeName = x.SubtypeName
          })
          .ToList();


    private async Task<IList<string>> CheckExistsSubtype(IList<SubtypeChecker> subtypeNames)
    {
      var criteria = new CheckExistsSubtypeCriteria
      {
        Xml = subtypeNames.ToXml().ToString()
      };
      var result = await repository.FetchAsync<string>(criteria);
      return result.ToList();
    }
  }

  public class CheckExistsSubtypeCriteria : CriteriaBase
  {
    public string Xml { get; set; }
    public override string GetSettingKey() => "OWP_TicketTypeRecipient_CheckExists_Subtype";


    public override bool IsValid() => true;

  }
}