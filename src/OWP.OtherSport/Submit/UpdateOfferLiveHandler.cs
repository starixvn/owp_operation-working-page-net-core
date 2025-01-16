using MediatR;
using OWP.OtherSport.Submit.Models;
using OWP.OtherSport.Submit.Services;
using OWP.RabbitMQ;
using OWP.RabbitMQ.Constants;
using System.Globalization;
using Fanex.NAP.Client;
using Olympus.Xml;
using OWP.OtherSport.Submit.Criteria;

namespace OWP.OtherSport.Submit;

public class UpdateOfferLiveRequest : IRequest<Unit>
{
    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public IList<OfferLiveInfo> OfferLiveInfos { get; set; }
}

public class UpdateOfferLiveHandler(
    CommandRepository commandRepository,
    CacheManipulator cacheManipulator,
    MessageClient messageClient,
    INapSession session) : IRequestHandler<UpdateOfferLiveRequest, Unit>
{
    public async Task<Unit> Handle(UpdateOfferLiveRequest request, CancellationToken cancellationToken)
    {
        var sessionId = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
        var oldSnapshot = await commandRepository.Query(request.FromDate, request.ToDate);
        await cacheManipulator.SaveOldAssignmentSnapshot(oldSnapshot, sessionId);

        var criteria = new UpdateOfferLiveCriteria { Xml = XmlConverter.Serialize(request.OfferLiveInfos) };
        await commandRepository.UpdateOfferLive(criteria);

        await messageClient.SendMessage(new
        {
            SessionId = sessionId,
            request.FromDate,
            request.ToDate,
            ActorId = session.UserId,
            session.UserName
        }, RoutingKey.OtherSport);

        return await Task.FromResult(Unit.Value);
    }
}