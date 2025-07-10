using Fanex.Data.Repository;
using Fanex.NAP.Client;
using MediatR;
using OWP.AccountSetting.Profile.Criteria;

namespace OWP.AccountSetting.Profile.Handler;

public class UpdateUserSettingRequest : IRequest
{
  public string Key { get; set; }
  public string Value { get; set; }
}

public class UpdateUserSettingHandler : IRequestHandler<UpdateUserSettingRequest>
{
  private readonly IDynamicRepository dynamicRepository;
  private readonly INapSession session;

  public UpdateUserSettingHandler(IDynamicRepository dynamicRepository, INapSession session)
  {
    this.dynamicRepository = dynamicRepository;
    this.session = session;
  }

  public async Task Handle(UpdateUserSettingRequest request, CancellationToken cancellationToken)
  {
    var criteria = new UpdateUserSettingCriteria { Key = request.Key, Value = request.Value, UserId = session.UserId };
    await dynamicRepository.ExecuteAsync(criteria);
  }
}
