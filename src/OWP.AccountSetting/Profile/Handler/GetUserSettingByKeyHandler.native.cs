using Fanex.Data.Repository;
using Fanex.NAP.Client;
using MediatR;
using OWP.AccountSetting.Profile.Criteria;

namespace OWP.AccountSetting.Profile.Handler;

public class GetUserSettingByKeyRequest : IRequest<string>
{
    public string Key { get; set; }
}

public class GetUserSettingByKeyHandler : IRequestHandler<GetUserSettingByKeyRequest, string>
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession session;

    public GetUserSettingByKeyHandler(IDynamicRepository dynamicRepository, INapSession session)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
    }

    public async Task<string> Handle(GetUserSettingByKeyRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetUserSettingByKeyCriteria { Key = request.Key, UserId = session.UserId };
        var result = await dynamicRepository.GetAsync<string>(criteria);
        return result;
    }
}
