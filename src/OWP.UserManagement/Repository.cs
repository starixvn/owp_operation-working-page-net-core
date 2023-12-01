namespace OWP.UserManagement
{
    using System.Threading.Tasks;
    using Criteria;
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Configuration;
    using Models;
    using OWP.Models;
    using OWP.Services;

    public interface Repository
    {
        Task<UserManagementViewModel> GetUserPagingAsync(QueryModel model);

        Task<int> GetSectionIdByUserId(int userId);

        Task<IList<ReferenceResource>> GetLevelBySectionId(int? sectionId);
    }

    public class RepositoryImp : Repository
    {
        private readonly IDynamicRepository dynamicRepository;

        private readonly INapSettings napSettings;

        private readonly RefResourceService refResource;


        public RepositoryImp(IDynamicRepository dynamicRepository, INapSettings napSettings, RefResourceService refResource)
        {
            this.dynamicRepository = dynamicRepository;
            this.napSettings = napSettings;
            this.refResource = refResource;
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

        public async Task<UserManagementViewModel> GetUserPagingAsync(QueryModel model)
        {
            var criteria = new QueryUsersCriteria
            {
                CategoryId = string.Join(",", model.CategoryId),
                DepartmentId = string.Join(",", model.DepartmentId),
                LocationCode = string.Join(",", model.LocationCode),
                RoleId = string.Join(",", model.RoleId),
                SectionId = string.Join(",", model.SectionId),
                User = model.User,
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

            return new UserManagementViewModel
            {
                Users = users
            };
        }
    }
}