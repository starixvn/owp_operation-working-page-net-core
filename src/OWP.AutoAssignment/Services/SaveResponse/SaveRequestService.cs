namespace OWP.AutoAssignment.Services.SaveResponse;

using Models.InternalApi;
using OWP.Models;

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