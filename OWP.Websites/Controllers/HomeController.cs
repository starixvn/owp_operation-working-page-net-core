using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Authorization;
using OWP.Constants;
using OWP.Services;

namespace OWP.Websites.Controllers;

public class HomeController(AccessCounter accessCounter) : Controller
{
    [Authorize]
    public IActionResult Index() => View("~/wwwroot/welcome/server.cshtml");

    [AllowAnonymous]
    public IActionResult ThrowException(string message) => throw new Exception("Throw exception");

    [AllowAnonymous]
    public IActionResult ExpiredSession()
    {
        return View();
    }

    [Route("EventManagement/LiveDetail/Index/{id:int}")]
    [NapAuth(FunctionKey = FunctionKeys.DetailsInformation, Policy = Constant.Policy.Read)]
    public ActionResult LiveDetail(int? id) => View("~/wwwroot/live-detail/server.cshtml");

    [Route("UserProfile/NotificationSetting")]
    public ActionResult NotificationSetting() => View("~/wwwroot/notification-setting/server.cshtml");

    [Route("UserProfile/AccountSetting")]
    public ActionResult AccountSetting() => View("~/wwwroot/account-setting/server.cshtml");

    [Route("EventAssignmentNew")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> EventAssignmentNew()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.LiveGameAssignment);
        return View("~/wwwroot/event-assignment-new/server.cshtml");
    }

    [Route("EventUpload")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Host)]
    public async Task<ActionResult> EventUpload()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.EventUploadNew);
        return View("~/wwwroot/event-upload/server.cshtml");
    }

    [Route("ResourceManagement")]
    [NapAuth(FunctionKey = FunctionKeys.UserManagement, Policy = Constant.Policy.Read)]
    public ActionResult ResourceManagement() => View("~/wwwroot/resource-management/server.cshtml");

    [Route("MessageBoxManagement")]
    public ActionResult MessageBoxManagement() => View("~/wwwroot/messagebox-management/server.cshtml");

    [Route("OddsCompiler")]
    [NapAuth(FunctionKey = FunctionKeys.OddsCompiler, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> OddsCompiler()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.OddsCompiler);
        return View("~/wwwroot/odds-compiler/server.cshtml");
    }

    // Add new page
    [Route("TraderReport")]
    [NapAuth(FunctionKey = FunctionKeys.TraderReport, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> LSTReport()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.TraderReport);
        return View("~/wwwroot/lst-report/server.cshtml");
    }

    // Add new page
    [Route("LeagueManagement/{id=999}/{leagueName=''}/{isActive='false'}")]
    [NapAuth(FunctionKey = FunctionKeys.LeagueManagement, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> LeagueManagement()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.LeagueManagement);
        return View("~/wwwroot/league-management/server.cshtml");
    }

    // Add new page
    [Route("LAReport")]
    [NapAuth(FunctionKey = FunctionKeys.LAReport, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> LAReport()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.LAReport);
        return View("~/wwwroot/la-report/server.cshtml");
    }

    [Route("OvertimeReport")]
    [NapAuth(FunctionKey = FunctionKeys.OvertimeReport, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> OvertimeReport()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.OvertimeReport);
        return View("~/wwwroot/overtime-report/server.cshtml");
    }

    [Route("DayShiftAssignment")]
    [NapAuth(FunctionKey = FunctionKeys.DayShiftAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> DayShiftAssignment()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.DayShiftAssignment);
        return View("~/wwwroot/day-shift-assignment/server.cshtml");
    }

    // Add new page
    [Route("NightShiftAssignment")]
    [NapAuth(FunctionKey = FunctionKeys.NightShiftAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> NightShiftAssignment()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.NightShiftAssignment);
        return View("~/wwwroot/night-shift-assignment/server.cshtml");
    }

    [Route("MyTickets")]
    [NapAuth(FunctionKey = FunctionKeys.MyTickets, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> TicketCenter()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.MyTickets);
        return View("~/wwwroot/ticket-center/server.cshtml");
    }

    [Route("TicketTypeRecipient")]
    [NapAuth(FunctionKey = FunctionKeys.TicketTypeSettings, Policy = Constant.Policy.Host)]
    public async Task<ActionResult> TicketTypeRecipient()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.TicketTypeSettings);
        return View("~/wwwroot/ticket-type-recipient/server.cshtml");
    }

    [Route("LiveGameList")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameList, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> LiveGameList()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.LiveGameListNew);
        return View("~/wwwroot/live-game-list/server.cshtml");
    }

    [Route("RecipientGroup")]
    [NapAuth(FunctionKey = FunctionKeys.RecipientGroup, Policy = Constant.Policy.Host)]
    public async Task<ActionResult> RecipientGroup()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.RecipientGroup);
        return View("~/wwwroot/recipient-group/server.cshtml");
    }

    [Route("OSLiveGameAssignment")]
    [NapAuth(FunctionKey = FunctionKeys.OSLiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> OSLiveGameList()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.OSLiveGameAssignment);
        return View("~/wwwroot/other-sport/server.cshtml");
    }

    [Route("AssignmentConfiguration")]
    [NapAuth(FunctionKey = FunctionKeys.AssignmentConfiguration, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> TraderAssignmentConfiguration()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.AssignmentConfiguration);
        return View("~/wwwroot/product-preference/server.cshtml");
    }

    [Route("PrematchReport")]
    [NapAuth(FunctionKey = FunctionKeys.PrematchReport, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> PrematchReportConfiguration()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.PrematchReport);
        return View("~/wwwroot/prematch-report/server.cshtml");
    }

    [Route("MatchCountLiveReport")]
    [NapAuth(FunctionKey = FunctionKeys.MatchCountLiveReport, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> MatchCountLiveReport()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.MatchCountLiveReport);
        return View("~/wwwroot/match-count-live-report/server.cshtml");
    }

    [Route("OSBasketballMaxbet")]
    [NapAuth(FunctionKey = FunctionKeys.OSBasketballMaxbet, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> OSBasketballMaxbet()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.OSBasketballMaxBet);
        return View("~/wwwroot/os-league-setting/server.cshtml");
    }

    [Route("EventPickingNew")]
    [NapAuth(FunctionKey = FunctionKeys.EventPickingNew, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> EventPickingNew()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.EventPicking);
        return View("~/wwwroot/event-picking/server.cshtml");
    }


    [Route("TaskListNew")]
    [NapAuth(FunctionKey = FunctionKeys.OWPEventManagement, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> TaskList()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.TaskListNew);
        return View("~/wwwroot/task-list/server.cshtml");
    }

    [Route("ProviderArrangement")]
    [NapAuth(FunctionKey = FunctionKeys.ProviderArrangement, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> ProviderArrangement()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.ProviderArrangement);
        return View("~/wwwroot/provider-arrangement/server.cshtml");
    }

    [Route("AssigneeSuggestionList")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> AssigneeSuggestionList()
    {
        await accessCounter.UpdateCounter(AccessCounterConst.AssigneeSuggestionList);
        return View("~/wwwroot/assignee-suggestion-list/server.cshtml");
    }

    [Route("LAKpi")]
    [NapAuth(FunctionKey = FunctionKeys.LAKpi, Policy = Constant.Policy.None)]
    public async Task<ActionResult> LaKpi()
    {
        await accessCounter.UpdateCounter("LAKpi");
        return View("~/wwwroot/la-kpi/server.cshtml");
    }

    [Route("Feedback")]
    [NapAuth(FunctionKey = FunctionKeys.OWPEventManagement, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> FeedBack()
    {
        await accessCounter.UpdateCounter("Feedback");
        return View("~/wwwroot/feedback/server.cshtml");
    }

    [Route("UserManagement_New")]
    [NapAuth(FunctionKey = FunctionKeys.OWPEventManagement, Policy = Constant.Policy.Read)]
    public ActionResult UserManagement()
    {
        return View("~/wwwroot/user-management/server.cshtml");
    }

    [Route("SmallChatting")]
    [NapAuth(FunctionKey = FunctionKeys.MyTickets, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> SmallChatting()
    {
        await accessCounter.UpdateCounter("SmallChatting");
        return View("~/wwwroot/small-chatting/server.cshtml");
    }

    [Route("ProviderVerification")]
    [NapAuth(FunctionKey = FunctionKeys.ProviderVerification, Policy = Constant.Policy.Read)]
    public ActionResult ProviderVerification()
    {
        return View("~/wwwroot/provider-verification/server.cshtml");
    }


    [Route(template: "ProviderVerification/check-difference-provider/{date:DateTime}")]
    [NapAuth(FunctionKey = FunctionKeys.ProviderVerification, Policy = Constant.Policy.Read)]
    public ActionResult ProviderVerificationCheckDifferenceProvider()
    {
        return View("~/wwwroot/provider-verification-check-difference-provider/server.cshtml");
    }

    [Route("SmallTicket")]
    [NapAuth(FunctionKey = FunctionKeys.MyTickets, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> SmallTicketCenter()
    {
        await accessCounter.UpdateCounter("SmallTicket");
        return View("~/wwwroot/small-ticket-center/server.cshtml");
    }

    [Route("RocketChat")]
    [NapAuth(FunctionKey = FunctionKeys.RocketChat, Policy = Constant.Policy.Read)]
    public ActionResult RocketChat()
    {
        return View("~/wwwroot/rocket-chat/server.cshtml");
    }
}