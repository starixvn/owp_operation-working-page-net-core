namespace OWP.AutoAssignment.Services.PublicApi;

using Models.PublicApi;

public interface GetRequestService
{
    Task<IEnumerable<RequestSender>> GetRequest();
}

public class GetRequestServiceImp : GetRequestService
{
    private readonly Repository repository;
    private readonly RequestBuilder requestBuilder;

    public GetRequestServiceImp(
        Repository repository,
        RequestBuilder requestBuilder)
    {
        this.repository = repository;
        this.requestBuilder = requestBuilder;
    }

    public async Task<IEnumerable<RequestSender>> GetRequest()
    {
        var request = await repository.GetPublicRequests();
        return await requestBuilder.BuildRequestSenderModel(request);
    }
}