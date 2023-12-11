namespace OWP.UserManagement
{
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Helpers.Web;
    using Models;
    using OWP.Models;
    using OWP.Services;

    public interface Repository
    {
        Task<IList<User>> GetUserPagingAsync(QueryModel model);

        Task<int> GetSectionIdByUserId(int userId);

        Task<IList<ReferenceResource>> GetLevelBySectionId(int? sectionId);

        Task UpdateUserInfo(NonQueryCommand criteria);
    }

    public class RepositoryImp : Repository
    {
        private readonly IDynamicRepository dynamicRepository;

        private readonly INapSettings napSettings;

        private readonly RefResourceService refResource;

        private readonly INapSession currentSession;


        public RepositoryImp(IDynamicRepository dynamicRepository, INapSettings napSettings, RefResourceService refResource, INapSession currentSession)
        {
            this.dynamicRepository = dynamicRepository;
            this.napSettings = napSettings;
            this.refResource = refResource;
            this.currentSession = currentSession;
        }

        public async Task<int> GetSectionIdByUserId(int userId)
        {
            var criteria = new GetSectionIdByUserIdCriteria
            {
                UserId = userId
            };
            return (await dynamicRepository.FetchAsync<int>(criteria)).FirstOrDefault();
        }

        public async Task<IList<ReferenceResource>> GetLevelBySectionId(int? sectionId)
        {
            var criteria = new GetLevelBySectionIdCriteria
            {
                SectionId = sectionId
            };

            return (await dynamicRepository.FetchAsync<ReferenceResource>(criteria)).ToList();
        }

        public async Task UpdateUserInfo(NonQueryCommand criteria)
            => await dynamicRepository.ExecuteAsync(criteria);


        public async Task<IList<User>> GetUserPagingAsync(QueryModel model)
        {
            var criteria = new QueryUsersCriteria
            {
                CategoryId = model.CategoryId,
                DepartmentId = model.DepartmentId,
                LocationCode = model.LocationCode,
                RoleId = model.RoleId,
                SectionId = model.SectionId,
                User = model.User ?? "",
                SiteId = napSettings.SiteId
            };

            var users = await dynamicRepository.FetchAsync<User>(criteria);

            var languageResources = await refResource.GetRefResourcesByName("Language");
            foreach (var item in users)
            {
                if (string.IsNullOrEmpty(item.LanguageId))
                {
                    continue;
                }

                var langIds = item.LanguageId.Split(',').Select(int.Parse);
                var text = languageResources.Where(y => langIds.Contains(y.ResourceId)).Select(x => x.Value).ToArray();
                item.Languages = string.Join(", ", text);
            }

            return users.ToList();
        }
    }
}