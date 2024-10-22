using Fanex.Data.Repository;
using Fanex.NAP.Client.Configuration;
using Fanex.NAP.Client.Services;
using Newtonsoft.Json;
using OWP.CQRS;
using OWP.TicketSetting.TicketTypeRecipient.Criteria;

namespace OWP.TicketSetting.TicketTypeRecipient.Handlers;
public class GetRecipientRequest: Query<IList<object>>
{
    public bool IncludeGroup { get; set; }

    public bool IncludeSection { get; set; }
}

public class GetRecipientHandler: QueryHandler<GetRecipientRequest, IList<object>>
{
    private readonly IDynamicRepository repository;

    private readonly INapSettings napSettings;

    private readonly IUserService userService;

    public GetRecipientHandler(IDynamicRepository repository, INapSettings napSettings, IUserService userService)
    {
        this.repository = repository;
        this.napSettings = napSettings;
        this.userService = userService;
    }

    public async Task<IList<object>> Handle(GetRecipientRequest request, CancellationToken cancellationToken)
    {
        var allUserActive = (await userService.GetBySiteAsync(napSettings.SiteId, true)).ToList();

        return (await repository.FetchAsync<object>(new GetAllRecipientCriteria()
        {
            IncludeGroup = request.IncludeGroup,
            IncludeSection = request.IncludeSection,
            AllUserJson = JsonConvert.SerializeObject(allUserActive.Select(u => new
            {
                UserId = u.Id,
                IsActive = true
            }))
        })).ToList();
    }
}
