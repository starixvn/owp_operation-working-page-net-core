namespace OWP.LiveDetail.OtherProvider
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using OWP.Extensions;

    public interface OtherProviderRepository
    {
        Task<IEnumerable<OtherProvider>> GetAll();
    }

    public class OtherProviderRepositoryImp : OtherProviderRepository
    {
        private readonly IDynamicRepository repository;

        public OtherProviderRepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<OtherProvider>> GetAll()
        {
            var criteria = new GetAllCriteria();
            criteria.ThrowExceptionIfInvalid();
            return await repository.FetchAsync<OtherProvider>(criteria);
        }
    }
}