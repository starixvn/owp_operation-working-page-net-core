using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.PublicApi
{
    public interface SabaMatchService
    {
        Task<int> GetSabaMaxbet(IList<int> matchIds);
    }

    internal class SabaMatchServiceIpm : SabaMatchService
    {
        private readonly Repository repository;

        public SabaMatchServiceIpm(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<int> GetSabaMaxbet(IList<int> matchIds)
        {
            return await repository.GetSabaMaxbet(string.Join(',', matchIds));
        }
    }
}