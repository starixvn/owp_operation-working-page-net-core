


namespace OWP.ProviderVerification.Handlers;

public class GetProviderVerificationRequest : IRequest<IEnumerable<ProviderVerificationModel>>
{
    public DateTime EventDate;
}
public class GetProviderVerificationHandler : IRequestHandler<GetProviderVerificationRequest, IEnumerable<ProviderVerificationModel>>
{
    private readonly ProviderVerificationRepository repository;

    public GetProviderVerificationHandler(ProviderVerificationRepository repository)
    {
        this.repository = repository;
    }
    public async Task<IEnumerable<ProviderVerificationModel>> Handle(GetProviderVerificationRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetProviderVerificationCriteria
        {
            EventDate = request.EventDate
        };

        return await repository.GetProviderVerification(criteria);

        //var fakeData = new List<ProviderVerificationModel>
        //{
        //    new ()
        //    {
        //        OwpMatchId = 123456,
        //        MatchNo = "123ac",
        //        MatchStatus = "Cancel",
        //        Agent = "test",
        //        AgentConfirm = new List<AgentConfirmOption>
        //        {
        //            new ()
        //            {
        //                Value = 1,
        //                Label = "test"
        //            },
        //            new ()
        //            {
        //                Value = 2,
        //                Label = "test2"
        //            }
        //        },
        //        Away = "test",
        //        EstNote = "test",
        //        HasTicket = false,
        //        Home = "test",
        //        IsRead = false,
        //        KoTime = "3:00 AM",
        //        League = "test",
        //        OtherProvider = "test",
        //        Provider = "test",
        //        Remark = "test",
        //        RemarkBy = "test"
        //    }

        //};

        //return fakeData;
    }
}
