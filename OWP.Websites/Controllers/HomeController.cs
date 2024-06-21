using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Models.ParameterObject;
using Fanex.NAP.Client.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using OWP.Constants;
using OWP.Services;
using System.Security.Claims;

namespace OWP.Websites.Controllers;

public class HomeController : Controller
{
    private readonly INapSession session;

    private readonly INapSettings napSettings;

    private readonly AccessCounterModel counterModel;

    private readonly AccessCounter accessCounter;

    public HomeController(INapSession session, INapSettings napSettings, IOptions<AccessCounterModel> counterOptions, AccessCounter accessCounter)
    {
        this.session = session;
        this.napSettings = napSettings;
        this.accessCounter = accessCounter;
        this.counterModel = counterOptions.Value;
    }

    [Authorize]

    public IActionResult Index() => View("~/wwwroot/welcome/server.cshtml");


    [AllowAnonymous]
    public string GetAccessCounterConfig() => counterModel.ToString();

    [AllowAnonymous]
    public IActionResult Test()
    {
        return Content("Test");
    }

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

    [Route("EventAssignment")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> EventAssignment()
    {
        const string functionName = FunctionKeys.LiveGameAssignment;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/event-assignment/server.cshtml");
    }

    [Route("EventAssignmentNew")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> EventAssignmentNew()
    {
        const string functionName = FunctionKeys.EventPickingNew;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/event-assignment-new/server.cshtml");
    }

    [Route("EventUpload")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Host)]
    public async Task<ActionResult> EventUpload()
    {
        await accessCounter.UpdateCounter("EventUpload");
        return View("~/wwwroot/event-upload/server.cshtml");
    }

    [Route("AutoAssignment")]
    public async Task<ActionResult> AutoAssignment()
    {
        const string functionName = FunctionKeys.AutoAssignment;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/auto-assignment/server.cshtml");
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
        const string functionName = FunctionKeys.OddsCompiler;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/odds-compiler/server.cshtml");
    }

    // Add new page
    [Route("TraderReport")]
    [NapAuth(FunctionKey = FunctionKeys.TraderReport, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> LSTReport()
    {
        const string functionName = FunctionKeys.TraderReport;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/lst-report/server.cshtml");
    }

    // Add new page
    [Route("LeagueManagement/{id=999}/{leagueName=''}/{isActive='false'}")]
    [NapAuth(FunctionKey = FunctionKeys.LeagueManagement, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> LeagueManagement()
    {
        const string functionName = FunctionKeys.LeagueManagement;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/league-management/server.cshtml");
    }

    // Add new page
    [Route("LAReport")]
    [NapAuth(FunctionKey = FunctionKeys.LAReport, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> LAReport()
    {
        const string functionName = FunctionKeys.LAReport;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/la-report/server.cshtml");
    }

    [Route("OvertimeReport")]
    [NapAuth(FunctionKey = FunctionKeys.OvertimeReport, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> OvertimeReport()
    {
        const string functionName = FunctionKeys.OvertimeReport;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/overtime-report/server.cshtml");
    }

    [Route("DayShiftAssignment")]
    [NapAuth(FunctionKey = FunctionKeys.DayShiftAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> DayShiftAssignment()
    {
        const string functionName = FunctionKeys.DayShiftAssignment;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/day-shift-assignment/server.cshtml");
    }

    // Add new page
    [Route("NightShiftAssignment")]
    [NapAuth(FunctionKey = FunctionKeys.NightShiftAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> NightShiftAssignment()
    {
        const string functionName = FunctionKeys.NightShiftAssignment;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/night-shift-assignment/server.cshtml");
    }

    [Route("MyTickets")]
    [NapAuth(FunctionKey = FunctionKeys.MyTickets, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> TicketCenter()
    {
        const string functionName = FunctionKeys.MyTickets;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/ticket-center/server.cshtml");
    }

    [Route("TicketTypeRecipient")]
    [NapAuth(FunctionKey = FunctionKeys.TicketTypeSettings, Policy = Constant.Policy.Host)]
    public async Task<ActionResult> TicketTypeRecipient()
    {
        const string functionName = FunctionKeys.TicketTypeSettings;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/ticket-type-recipient/server.cshtml");
    }

    [Route("LiveGameList")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameList, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> LiveGameList()
    {
        const string functionName = FunctionKeys.LiveGameList;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/live-game-list/server.cshtml");
    }

    [Route("RecipientGroup")]
    [NapAuth(FunctionKey = FunctionKeys.RecipientGroup, Policy = Constant.Policy.Host)]
    public async Task<ActionResult> RecipientGroup()
    {
        const string functionName = FunctionKeys.RecipientGroup;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/recipient-group/server.cshtml");
    }

    [Route("OSLiveGameAssignment")]
    [NapAuth(FunctionKey = FunctionKeys.OSLiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> OSLiveGameList()
    {
        const string functionName = FunctionKeys.OSLiveGameAssignment;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/other-sport/server.cshtml");
    }

    [Route("AssignmentConfiguration")]
    [NapAuth(FunctionKey = FunctionKeys.AssignmentConfiguration, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> TraderAssignmentConfiguration()
    {
        const string functionName = FunctionKeys.AssignmentConfiguration;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/product-preference/server.cshtml");
    }

    [Route("PrematchReport")]
    [NapAuth(FunctionKey = FunctionKeys.PrematchReport, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> PrematchReportConfiguration()
    {
        const string functionName = FunctionKeys.PrematchReport;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/prematch-report/server.cshtml");
    }

    [Route("MatchCountLiveReport")]
    [NapAuth(FunctionKey = FunctionKeys.MatchCountLiveReport, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> MatchCountLiveReport()
    {
        const string functionName = FunctionKeys.MatchCountLiveReport;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/match-count-live-report/server.cshtml");
    }

    [Route("OSBasketballMaxbet")]
    [NapAuth(FunctionKey = FunctionKeys.OSBasketballMaxbet, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> OSBasketballMaxbet()
    {
        const string functionName = FunctionKeys.OSBasketballMaxbet;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/os-league-setting/server.cshtml");
    }

    [Route("EventPickingNew")]
    [NapAuth(FunctionKey = FunctionKeys.EventPickingNew, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> EventPickingNew()
    {
        await accessCounter.UpdateCounter("EventPickingNew");
        return View("~/wwwroot/event-picking/server.cshtml");
    }


    [Route("TaskListNew")]
    [NapAuth(FunctionKey = FunctionKeys.OWPEventManagement, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> TaskList()
    {
        await accessCounter.UpdateCounter("TaskListNew");
        return View("~/wwwroot/task-list/server.cshtml");
    }

    [Route("ProviderArrangement")]
    [NapAuth(FunctionKey = FunctionKeys.ProviderArrangement, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> ProviderArrangement()
    {
        const string functionName = FunctionKeys.ProviderArrangement;
        await accessCounter.UpdateCounter(functionName);
        return View("~/wwwroot/provider-arrangement/server.cshtml");
    }

    [Route("AssigneeSuggestionList")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> AssigneeSuggestionList()
    {
        await accessCounter.UpdateCounter("AssigneeSuggestionList");
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
    public async Task<ActionResult> UserManagement()
    {
        const string functionName = FunctionKeys.UserManagement;
        await accessCounter.UpdateCounter(functionName);
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

    [Route("SmallTicket")]
    [NapAuth(FunctionKey = FunctionKeys.MyTickets, Policy = Constant.Policy.Read)]
    public async Task<ActionResult> SmallTicketCenter()
    {
        await accessCounter.UpdateCounter("SmallTicket");
        return View("~/wwwroot/small-ticket-center/server.cshtml");
    }
}