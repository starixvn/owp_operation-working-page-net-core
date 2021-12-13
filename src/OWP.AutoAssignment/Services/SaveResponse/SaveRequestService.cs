using Newtonsoft.Json;
using OWP.AutoAssignment.Models.InternalApi;
using OWP.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.SaveResponse
{
    public interface SaveRequestService
    {
        Task<ResultDataObject> Save(HttpRequestMessage request);
    }

    public class SaveRequestServiceImp : SaveRequestService
    {
        private readonly Repository repository;

        public SaveRequestServiceImp(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<ResultDataObject> Save(HttpRequestMessage request)
        {
            var jsonString = await request.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<SaveRequestModel>(jsonString);
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
}