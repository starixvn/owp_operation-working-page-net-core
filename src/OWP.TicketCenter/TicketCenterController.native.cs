using MediatR;
using Microsoft.AspNetCore.Mvc;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Middleware;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter;

[Route("api/[controller]")]
[ApiController]
public class TicketCenterController : ControllerBase
{
    private readonly IMediator mediator;

    private readonly GetInfoFeedback getInfoFeedback;

    public TicketCenterController(IMediator mediator, GetInfoFeedback getInfoFeedback)
    {
        this.mediator = mediator;
        this.getInfoFeedback = getInfoFeedback;
    }

    [HttpPost("new-message")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task<IActionResult> NewMessage(NewMessageRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [Route("on-click-issue-notification")]
    [HttpPost]
    [ServiceFilter(typeof(PermissionOnOpenTicketDetail))]
    public async Task OpenChatBoard(OnClickIssueNotificationRequest request)
        => await mediator.Send(request);

    #region new ticket

    [HttpPost("new-ticket-issue-event")]
    public async Task<int> NewTicketIssueEvent(NewTicketIssueEventRequest request)
        => await mediator.Send(request);

    [HttpPost("new-ticket-issue-other")]
    public async Task<int> NewTicketIssueOther(NewTicketIssueOtherRequest request)
        => await mediator.Send(request);

    [HttpPost("new-ticket-issue-support")]
    public async Task<int> NewTicketIssueSupport(NewTicketIssueSupportRequest request)
        => await mediator.Send(request);

    [HttpPost("new-ticket-issue-league")]
    public async Task<int> NewTicketIssueLeague(NewTicketIssueLeagueRequest request)
        => await mediator.Send(request);

    [HttpPost("new-ticket-issue-live-incident")]
    public async Task<int> NewTicketIssueLga(NewTicketIssueLiveIncidentRequest request)
        => await mediator.Send(request);

    [HttpPost("new-ticket-issue-log-work")]
    public async Task<int> NewTicketIssueLogWork(NewTicketIssueLogWorkRequest request)
        => await mediator.Send(request);
    #endregion new ticket

    [HttpGet("new-ticket/get-event")]
    [Produces("application/json")]
    public async Task<IActionResult> GetEventInfo(string keyword)
    {
        var eventSender = await mediator.Send(new GetTicketEventOptionRequest
        {
            Keyword = keyword
        });
        return Ok(eventSender);
    }
    [HttpGet("new-ticket/get-event-lga")]
    [Produces("application/json")]
    public async Task<IActionResult> GetEventInfoLga(string keyword)
    {
        var eventSender = await mediator.Send(new GetTicketEventOptionLgaRequest
        {
            Keyword = keyword
        });
        return Ok(eventSender);
    }

    [HttpPost("get-event-ticket")]
    public async Task<IList<TicketInfo>> GetEventTicket(QueryEventTicketsRequest request)
        => await mediator.Send(request);

    [HttpGet("get-ticket-type")]
    public async Task<TicketTypeModel> GetTicketType()
        => await mediator.Send(new TicketTypeRequest());

    #region get detail

    [HttpPost("ticket-detail/get-league")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task<IActionResult> GetTicketLeagueDetail(TicketLeagueDetailRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("ticket-detail/get-match")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task<IActionResult> GetTicketMatchDetail(TicketMatchDetailRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("ticket-detail/get-live-incident")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task<IActionResult> GetTicketLiveIncidentDetail(TicketLiveIncidentDetailRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("ticket-detail/get-log-work")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task<IActionResult> GetTicketLogWorkDetail(TicketLogWorkDetailRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("ticket-detail/get-other")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task<IActionResult> GetTicketOtherDetail(TicketOtherDetailRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("ticket-detail/get-support")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task<IActionResult> GetTicketSupportDetail(TicketSupportDetailRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    #endregion get detail

    [HttpPost("ticket-detail/insert-single-recipient")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task<IActionResult> AddSingleRecipient(InsertSingleRecipientRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("ticket-detail/remove-single-recipient")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    [ServiceFilter(typeof(PermissionRemoveLastRecipient), Order = 2)]
    public async Task<IActionResult> RemoveSingleRecipient(RemoveSingleRecipientRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("ticket-detail/update-ticket-status")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task<IActionResult> UpdateTicketStatus(UpdateTicketStatusRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpPost("ticket-detail/update-ticket-subtype")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task<IActionResult> UpdateTicketSubType(UpdateTicketSubTypeRequest request)
    {
        var result = await mediator.Send(request);
        return Ok(result);
    }

    [HttpGet("ticket-detail/get-status-history")]
    public async Task<IEnumerable<StatusHistory>> GetStatusHistoryByTicketId(int ticketId)
        => await mediator.Send(new GetStatusHistoryRequest { TicketId = ticketId });

    [HttpPost("ticket-detail/update-save-message")]
    [ServiceFilter(typeof(PermissionOnExistedTicket), Order = 0)]
    [ServiceFilter(typeof(PermissionFilter), Order = 1)]
    public async Task SaveMessage(SaveMessageRequest request)
        => await mediator.Send(request);

    [ServiceFilter(typeof(PermissionOnAction))]
    [HttpPost("pin-ticket")]
    public async Task PinTicket(PinTicketRequest request)
        => await mediator.Send(request);

    [ServiceFilter(typeof(PermissionOnAction))]
    [HttpPost("delete-ticket")]
    public async Task DeleteTicket(DeleteTicketRequest request)
        => await mediator.Send(request);

    [HttpPost("get-subtype")]
    public async Task<IEnumerable<SubTypeModel>> GetSubTypeByTicketType(GetSubTypeRequest request)
        => await mediator.Send(request);


    [HttpGet("mark-all-tickets")]
    public async Task MarkAllTicketAsRead(int actorId)
        => await mediator.Send(new MarkAllTicketsRequest { ActorId = actorId });

    [HttpPost("get-next-list-messages")]
    public async Task<ShowMoreMessageModel> GetNextListMessages(GetNextListMessagesRequest request)
        => await mediator.Send(request);

    [HttpGet("get-match-info")]
    public async Task<IActionResult> GetMatchInfo(int adminsiteId)
    {
        var result = await mediator.Send(new GetMatchInfoByAdminSiteIdRequest { AdminSiteId = adminsiteId });
        return Ok(result);
    }

    [HttpGet("get-function-feedback")]
    public async Task<IActionResult> GetFunctionFeedback()
    {
        var result = await mediator.Send(new GetFunctionFeedbackRequest());
        return Ok(result);
    }

    [HttpGet("get-user-feedback-info")]
    public async Task<IActionResult> GetUserFeedbackInfo()
    {
        var result = await getInfoFeedback.Get();
        return Ok(result);
    }

    [HttpPost("update-message-icon")]
    public async Task UpdateMessageIcon(UpdateMessageIconRequest request)
        => await mediator.Send(request);
}