using Fanex.NAP.Client.Helpers.Web;

namespace OWP.UserManagement.Services
{
    using System.Threading.Tasks;
    using Constants;
    using Models;
    using OWP.Services;

    public interface UsersService
    {
        Task<UserManagementViewModel> GetUserPagingAsync(QueryModel model);
    }

    public class UsersServiceImp : UsersService
    {
        private readonly ModelService modelService;
        private readonly Repository repository;
        private readonly INapSession currentSession;
        private readonly RefResourceService refResourceService;

        public UsersServiceImp(ModelService modelService, Repository repository, INapSession currentSession, RefResourceService refResourceService)
        {
            this.modelService = modelService;
            this.repository = repository;
            this.currentSession = currentSession;
            this.refResourceService = refResourceService;
        }

        public async Task<UserManagementViewModel> GetUserPagingAsync(QueryModel model)
        {
            model.Permission = modelService.BuildPermissionViewModel();
            var result = await repository.GetUserPagingAsync(model);
            var userId = currentSession.UserId;
            var userSectionId = await repository.GetSectionIdByUserId(userId);
            var allResourceTask = await refResourceService.GetAllReferenceResource();

            var sections = allResourceTask.FirstOrDefault(x =>
                x.Name == ReferenceResourceNames.Section && x.ResourceId == userSectionId);
            var userColumnModels = modelService.BuildUserColumnModel(model.Permission, sections?.Value);
            result.UserColumnModels = userColumnModels;


            return result;
        }
    }
}