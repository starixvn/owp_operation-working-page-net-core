using OWP.Constants;
using OWP.TaskList.Criteria;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Core.Business;
using OWP.TaskList.Models;
using OWP.Services;

namespace OWP.TaskList.Services
{
    public interface GetEventByDayService
    {
        Task<TaskListModel> GetEventByDay(DateTime eventDate, bool isHideCompletedMatches);
    }
    public class GetEventByDayServiceImp : GetEventByDayService
    {
        private readonly Repository repository;

        private readonly INapSession session;

        private readonly PermissionService permissionService;


        public GetEventByDayServiceImp(Repository repository, INapSession session, PermissionService permissionService)
        {
            this.repository = repository;
            this.session = session;
            this.permissionService = permissionService;
        }

        public async Task<TaskListModel> GetEventByDay(DateTime eventDate, bool isHideCompletedMatches)
        {
            var criteria = new GetTaskListCriteria
            {
                UserId = session.UserId,
                EventDate = eventDate
            };
            var model = new TaskListModel
            {
                HasReadNotePermission = permissionService.HasRight(FunctionKeys.Note, Constant.Policy.Read),
                Matches = await repository.GetTaskList(criteria),
                HasDetailInformationPermission = permissionService.HasRight(FunctionKeys.DetailsInformation, Constant.Policy.Read),
                IsHideCompletedMatches = isHideCompletedMatches
            };
            return model;
        }
    }
}
