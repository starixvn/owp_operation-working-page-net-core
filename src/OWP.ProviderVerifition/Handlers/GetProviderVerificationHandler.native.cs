
using Fanex.Data.Repository;
using MediatR;
using OWP.ProviderVerification.Criteria;
using OWP.ProviderVerification.Models;

namespace OWP.ProviderVerification.Handlers;

public class GetProviderVerificationRequest : IRequest<IEnumerable<ProviderVerificationModel>>
{
    public DateTime EventDate;
}
public class GetProviderVerificationHandler: IRequestHandler<GetProviderVerificationRequest, IEnumerable<ProviderVerificationModel>>
{
    private readonly IDynamicRepository repository;

    public GetProviderVerificationHandler(IDynamicRepository repository)
    {
        this.repository = repository;
    }
    public async Task<IEnumerable<ProviderVerificationModel>> Handle(GetProviderVerificationRequest request, CancellationToken cancellationToken)
    {
        //var criteria = new GetProviderVerificationCriteria
        //{
        //    EventDate = request.EventDate
        //};
        //return await repository.FetchAsync<ProviderVerificationModel>(criteria, cancellationToken);

        var fakeData = new List<ProviderVerificationModel>
        {
            new ()
            {
                MatchNo = "123ac",
                MatchStatus = "Cancel",
                Agent = "test",
                AgentConfirm = "test",
                Away = "test",
                ESTNote = "test",
                HasTicket = false,
                Home = "test",
                IsRead = false,
                KoTime = "3:00 AM",
                League = "test",
                OtherProvider = "test",
                Provider = "test",
                Remark = "test",
                RemarkBy = "test"
            }
            
        };

        return fakeData;
    }
}
