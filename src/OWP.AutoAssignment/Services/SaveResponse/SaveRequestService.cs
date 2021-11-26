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
        public Task<ResultDataObject> Save(HttpRequestMessage request)
        {
            throw new System.NotImplementedException();
        }
    }
}