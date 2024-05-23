using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.AccountSetting.Profile.Criteria;

namespace OWP.AccountSetting.Profile.Handler;

public class GetProfileRequest : IRequest<string> { }

public class GetProfileHandler : IRequestHandler<GetProfileRequest, string>
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession session;

    public GetProfileHandler(IDynamicRepository dynamicRepository, INapSession session)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
    }

    public async Task<string> Handle(GetProfileRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetUserProfileCriteria { UserId = session.UserId };
        var result = await dynamicRepository.FetchAsync<string>(criteria, cancellationToken);

        return result.FirstOrDefault() ?? string.Empty;
    }
}
