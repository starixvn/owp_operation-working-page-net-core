namespace Fanex.NAP.OWP.TaskScheduling.Controllers
{
    using Criteria.TaskListToday;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Helpers.Web;
    using global::OWP.Constants;
    using global::OWP.Services;
    using Microsoft.AspNetCore.Mvc;
    using Models.TaskListToday;
    using Service.Interfaces;
    using System;
    using System.Web.Mvc;

    public class TaskListController : ControllerBase
    {
        readonly ITaskListTodayService taskListService;
        readonly PermissionService permissionService;
        readonly INapSession currentSession;

        public TaskListController(
            ITaskListTodayService taskListService,
            INapSession currentSession,
            PermissionService permissionService)
        {
            this.currentSession = currentSession;
            this.taskListService = taskListService;
            this.permissionService = permissionService;
        }

        public ActionResult Index()
        {
            var tabsDate = GetTabsDate();

            var model = new TaskListModel
            {
                HasReadNotePermission = permissionService.HasRight(FunctionKeys.Note, Constant.Policy.Read),
                Today = FormatDate(tabsDate.Item1),
                Yesterday = FormatDate(tabsDate.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult UpdateMatchNoteStatus(int matchId)
        {
            var criteria = new UpdateMatchNoteViewCriteria
            {
                UserId = currentSession.UserId,
                MatchId = matchId
            };
            var result = taskListService.UpdateMatchNoteStatus(criteria);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateReadyTradeStatus(int matchId)
        {
            var criteria = new UpdateReadyTradeStatusCriteria()
            {
                UserId = currentSession.UserId,
                OWPMatchId = matchId
            };
            var result = taskListService.UpdateReadyTradeStatus(criteria);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> TaskListTable(bool isHideCompletedMatches = false)
        {
            var tabsDate = GetTabsDate();
            return await GetEventByDay(tabsDate.Item1, isHideCompletedMatches);
        }

        [HttpPost]
        public async Task<ActionResult> YesterdayEventTable()
        {
            var tabsDate = GetTabsDate();
            return await GetEventByDay(tabsDate.Item2);
        }

        public async Task<ActionResult> GetEventByDay(DateTime eventDate, bool isHideCompletedMatches = false)
        {
            var criteria = new GetTaskListTodayCriteria
            {
                UserId = currentSession.UserId,
                EventDate = eventDate
            };

            var model = new TaskListModel
            {
                HasReadNotePermission = permissionService.HasRight(FunctionKeys.Note, Constant.Policy.Read),
                Matches = await taskListService.GetMatchListToday(criteria),
                HasDetailInformationPermission = permissionService.HasRight(FunctionKeys.DetailsInformation, Constant.Policy.Read),
                IsHideCompletedMatches = isHideCompletedMatches
            };
            return PartialView("_TaskListTable", model);
        }

        private ValueTuple<DateTime, DateTime> GetTabsDate()
        {
            var checkpoint = DateTime.Now.AddHours(12);

            if (checkpoint.Hour < 11)
            {
                return (DateTime.Now.Date, DateTime.Now.AddDays(-1).Date);
            }

            return (checkpoint.Date, checkpoint.Date.AddDays(-1));
        }

        private string FormatDate(DateTime date)
            => date.ToString("MMM dd");
    }
}