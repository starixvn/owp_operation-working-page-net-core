
namespace OWP.ProviderVerification.ProviderVerification.UploadAgent;

public record SubmitAgentRequest(string SessionId) : Query<ResultT<IList<ProviderVerificationModel>>>;

public class SubmitAgentHandler(ICache cache): QueryHandler<SubmitAgentRequest, ResultT<IList<ProviderVerificationModel>>>
{
    public async Task<ResultT<IList<ProviderVerificationModel>>> Handle(SubmitAgentRequest request, CancellationToken cancellationToken)
    {
        var (_, excelProviderModel)=await cache.TryGetAsync<IList<ExcelProviderModel>>(request.SessionId);
        var compareResult = await Compare(excelProviderModel);
        return ResultT<IList<ProviderVerificationModel>>.Success(compareResult);
    }

    public async Task<IList<ProviderVerificationModel>> Compare(IList<ExcelProviderModel> excelProviderModels)
    {
        // TODO: Implement the logic to compare the excelProviderModels with the database
        return new List<ProviderVerificationModel>();
    }
}
