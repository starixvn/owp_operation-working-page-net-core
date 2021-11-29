using OWP.AutoAssignment.Models.PublicApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWP.AutoAssignment.Services.PublicApi
{
    public interface SabaMatchService
    {
        Task<IEnumerable<SabaMaxbetInfo>> GetSabaMaxbet(IList<int> matchIds);
    }

    public class SabaMatchServiceIpm : SabaMatchService
    {
        private readonly Repository repository;

        public SabaMatchServiceIpm(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<SabaMaxbetInfo>> GetSabaMaxbet(IList<int> matchIds)
            => await repository.GetSabaMaxbet(matchIds);
    }
}