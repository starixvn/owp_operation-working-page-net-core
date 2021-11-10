namespace OWP.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;

    public interface RefResourceService
    {
        Task<IEnumerable<RefResource>> GetRefResourcesByName(string name);
    }

    public class RefResourceServiceImp : RefResourceService
    {
        private readonly IDynamicRepository dynamicRepository;

        public RefResourceServiceImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<RefResource>> GetRefResourcesByName(string refName)
            => await dynamicRepository.FetchAsync<RefResource>(new GetRefResourceByNameCriteria { RefName = refName });
    }

    public class RefResource
    {
        public int ResourceId { get; set; }

        public string Value { get; set; }
    }

    public class GetRefResourceByNameCriteria : CriteriaBase
    {
        public string RefName { get; set; }

        public override bool IsValid() => !string.IsNullOrEmpty(RefName);

        public override string GetSettingKey() => "OWP_RefResourceGetByName";
    }
}