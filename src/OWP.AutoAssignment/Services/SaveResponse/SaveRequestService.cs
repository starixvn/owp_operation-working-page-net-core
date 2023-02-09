using Newtonsoft.Json;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.Models;

namespace OWP.AutoAssignment.Services.SaveResponse;

public interface SaveRequestService
{
    Task<ResultDataObject> Save(SaveRequestModel model);
}

public class SaveRequestServiceImp : SaveRequestService
{
    private readonly Repository repository;

    public SaveRequestServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<ResultDataObject> Save(SaveRequestModel model)
    {
        if (model == null || model.Data == null)
        {
            return new ResultDataObject
            {
                Code = 1,
                Message = "Data not found"
            };
        }

        var result = await repository.SaveRequest(model);

        return result;
    }
}