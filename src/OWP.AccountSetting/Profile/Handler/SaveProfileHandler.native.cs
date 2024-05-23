using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.AccountSetting.Profile.Criteria;

namespace OWP.AccountSetting.Profile;

public class SaveProfileRequest : IRequest
{
    public int FileId { get; set; }
}

public class SaveProfileHandler : IRequestHandler<SaveProfileRequest>
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession session;

    public SaveProfileHandler(IDynamicRepository dynamicRepository, INapSession session)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
    }

    public async Task Handle(SaveProfileRequest request, CancellationToken cancellationToken)
    {
        var criteria = new SaveUserProfileCriteria { UserId = session.UserId, FileId = request.FileId };
        await dynamicRepository.FetchAsync<string>(criteria, cancellationToken);
    }
}
