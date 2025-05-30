using Fanex.NAP.Client;

namespace OWP.EventPicking.Api.Handlers;

public class UpdateConfirmedRequest : IRequest<Unit>
{
    public string InfoSiteMatchIds { get; set; }

    public bool Confirmed { get; set; }

    public long TimestampConfirm { get; set; }
}

public class UpdateConfirmedHandler : IRequestHandler<UpdateConfirmedRequest, Unit>
{
    private readonly Repository repository;

    private readonly INapSession session;

    public UpdateConfirmedHandler(Repository repository, INapSession session)
    {
        this.repository = repository;
        this.session = session;
    }

    public async Task<Unit> Handle(UpdateConfirmedRequest request, CancellationToken cancellationToken)
    {
        var utcTime = DateTimeOffset.FromUnixTimeMilliseconds(request.TimestampConfirm);
        var gmt8Zone = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
        var gmt8Time = TimeZoneInfo.ConvertTime(utcTime, gmt8Zone).DateTime;

        await repository.UpdateConfirmed(new UpdateConfirmedMatchCriteria
        {
            InfoSiteMatchIds = request.InfoSiteMatchIds,
            ModifiedBy = session.UserName,
            Confirmed = request.Confirmed,
            ConfirmedAt = gmt8Time
        });
        return Unit.Value;
    }
}