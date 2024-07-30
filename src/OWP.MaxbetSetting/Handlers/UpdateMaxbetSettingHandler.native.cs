using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.MaxbetSetting.Criteria;

namespace OWP.MaxbetSetting.Handlers;

public class UpdateMaxbetSettingRequest : IRequest<Unit>
{
    public decimal FTHDP { get; set; }
    public string Column { get; set; }
    public decimal? Value { get; set; }
}

public class UpdateMaxbetSettingHandler : IRequestHandler<UpdateMaxbetSettingRequest, Unit>
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly INapSession session;

    public UpdateMaxbetSettingHandler(IDynamicRepository dynamicRepository, INapSession session)
    {
        this.dynamicRepository = dynamicRepository;
        this.session = session;
    }

    public async Task<Unit> Handle(UpdateMaxbetSettingRequest request, CancellationToken cancellationToken)
    {
        var criteria = new UpdateMaxbetSettingCriteria
        {
            FT_HDP = request.FTHDP,
            Column = request.Column,
            Value = request.Value,
            ActorId = session.UserId
        };

        await dynamicRepository.ExecuteAsync(criteria);

        return Unit.Value;
    }
}
