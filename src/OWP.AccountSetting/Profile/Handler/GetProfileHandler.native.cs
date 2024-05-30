using Fanex.NAP.Client.Helpers.Web;
using MediatR;

namespace OWP.AccountSetting.Profile.Handler;

public class GetProfileRequest : IRequest<string> { }

public class GetProfileHandler : IRequestHandler<GetProfileRequest, string>
{
    private readonly ProfileRepository repository;
    private readonly INapSession session;

    public GetProfileHandler(ProfileRepository dynamicRepository, INapSession session)
    {
        this.repository = dynamicRepository;
        this.session = session;
    }

    public async Task<string> Handle(GetProfileRequest request, CancellationToken cancellationToken)
    {
        var (_, avatarFileName) = await repository.QueryUserProfileInfo();

        return avatarFileName;
    }
}
