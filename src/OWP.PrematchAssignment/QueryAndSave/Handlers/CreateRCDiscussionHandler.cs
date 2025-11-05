using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Configuration;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class CreateRCDiscussionRequest : IRequest<Unit>
{
    public int AdminSiteId { get; set; }

    public List<DiscussionInfo> DiscussionInfos { get; set; }
}

public class CreateRCDiscussionHandler : IRequestHandler<CreateRCDiscussionRequest, Unit>
{
    private readonly PreMatchAssignmentService _preMatchAssignmentService;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CreateRCDiscussionHandler(
        PreMatchAssignmentService preMatchAssignmentService,
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _preMatchAssignmentService = preMatchAssignmentService;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<Unit> Handle(CreateRCDiscussionRequest request, CancellationToken cancellationToken)
    {
        var assignments = await _preMatchAssignmentService.GetMatchAssignments(request.AdminSiteId);
        var currentDiscussionInfos = await _preMatchAssignmentService.GetMatchDiscussionInfo(request.AdminSiteId);

        var (newDiscussionGroups, existingDiscussionGroups) = GroupAndSeparateDiscussions(request.DiscussionInfos, currentDiscussionInfos);

        await ProcessNewDiscussions(newDiscussionGroups, assignments, cancellationToken);
        await ProcessExistingDiscussions(existingDiscussionGroups, assignments, cancellationToken);

        return Unit.Value;
    }

    private (List<IGrouping<object, DiscussionInfo>> NewGroups, List<IGrouping<object, DiscussionInfo>> ExistingGroups) 
        GroupAndSeparateDiscussions(List<DiscussionInfo> discussionInfos, IEnumerable<DiscussionInfo> currentDiscussionInfos)
    {
        var groupedDiscussions = discussionInfos
            .GroupBy(d => new { d.AdminSiteMatchId, d.BetType })
            .ToList();

        var newDiscussionGroups = new List<IGrouping<object, DiscussionInfo>>();
        var existingDiscussionGroups = new List<IGrouping<object, DiscussionInfo>>();

        foreach (var group in groupedDiscussions)
        {
            var adminSiteMatchId = group.Key.AdminSiteMatchId;
            var betType = group.Key.BetType;
            var existingDiscussion = currentDiscussionInfos.FirstOrDefault(
                x => x.AdminSiteMatchId == adminSiteMatchId
                     && x.BetType == betType
                     && !string.IsNullOrEmpty(x.RCDiscussionGroupId));

            if (existingDiscussion != null)
            {
                // Set RCDiscussionGroupId for all items in this group
                foreach (var item in group)
                {
                    item.RCDiscussionGroupId = existingDiscussion.RCDiscussionGroupId;
                }
                existingDiscussionGroups.Add(group);
            }
            else
            {
                newDiscussionGroups.Add(group);
            }
        }

        return (newDiscussionGroups, existingDiscussionGroups);
    }

    private async Task ProcessNewDiscussions(
        List<IGrouping<object, DiscussionInfo>> newDiscussionGroups,
        IEnumerable<Assignment> assignments,
        CancellationToken cancellationToken)
    {
        var serverUrl = _configuration["ChatServer:Url"];
        var token = _configuration["ChatServer:BotToken"];
        var userId = _configuration["ChatServer:BotUserId"];
        var parentChannelId = _configuration["ChatServer:LATodayMatchChannelId"];

        foreach (var group in newDiscussionGroups)
        {
            var firstDiscussion = group.First();
            var tName = BuildDiscussionName(firstDiscussion);
            var users = GetUsersFromAssignments(assignments, firstDiscussion.BetType);
            var reply = BuildGroupedReplyMessage(group.ToList());

            var rcDiscussionGroupId = await CreateNewRCDiscussion(
                serverUrl, token, userId, parentChannelId, tName, users, reply, cancellationToken);

            if (!string.IsNullOrEmpty(rcDiscussionGroupId))
            {
                // Set RCDiscussionGroupId for all items in this group
                foreach (var item in group)
                {
                    item.RCDiscussionGroupId = rcDiscussionGroupId;
                }
            }
        }
    }

    private async Task ProcessExistingDiscussions(
        List<IGrouping<object, DiscussionInfo>> existingDiscussionGroups,
        IEnumerable<Assignment> assignments,
        CancellationToken cancellationToken)
    {
        var serverUrl = _configuration["ChatServer:Url"];
        var token = _configuration["ChatServer:BotToken"];
        var userId = _configuration["ChatServer:BotUserId"];

        foreach (var group in existingDiscussionGroups)
        {
            var firstDiscussion = group.First();
            var users = GetUsersFromAssignments(assignments, firstDiscussion.BetType);
            var reply = BuildGroupedReplyMessage(group.ToList());

            await InviteUsersToExistingDiscussion(
                serverUrl, token, userId, firstDiscussion.RCDiscussionGroupId, users, reply, cancellationToken);
        }
    }

    private List<string> GetUsersFromAssignments(IEnumerable<Assignment> assignments, int betType)
    {
        return assignments
            .Where(a => a.Bettype == betType)
            .Select(a => a.Username)
            .Distinct()
            .ToList();
    }

    private async Task<string> CreateNewRCDiscussion(
        string serverUrl,
        string token,
        string userId,
        string parentChannelId,
        string discussionName,
        List<string> users,
        string reply,
        CancellationToken cancellationToken)
    {
        var payload = new
        {
            prid = parentChannelId,
            t_name = discussionName,
            users = users,
            topic = "Live Soccer",
            reply = reply
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{serverUrl}rooms.createDiscussion");
        httpRequest.Headers.Add("X-Auth-Token", token);
        httpRequest.Headers.Add("X-User-Id", userId);
        httpRequest.Content = content;

        var response = await _httpClient.SendAsync(httpRequest, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var resultObj = JsonSerializer.Deserialize<CreateDiscussionResponse>(responseBody);
        return resultObj?.Discussion?.Name ?? string.Empty;
    }

    private async Task InviteUsersToExistingDiscussion(
        string serverUrl,
        string token,
        string userId,
        string roomId,
        List<string> users,
        string message,
        CancellationToken cancellationToken)
    {
        var payload = new
        {
            roomId = roomId,
            users = users,
            msg = message
        };

        var content = new StringContent(
            JsonSerializer.Serialize(payload),
            Encoding.UTF8,
            "application/json"
        );

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{serverUrl}rooms.invite");
        httpRequest.Headers.Add("X-Auth-Token", token);
        httpRequest.Headers.Add("X-User-Id", userId);
        httpRequest.Content = content;

        await _httpClient.SendAsync(httpRequest, cancellationToken);
    }

    private string BuildDiscussionName(DiscussionInfo discussionInfo)
    {
        // Build discussion name similar to CreateDiscussionHandler
        return $"{discussionInfo.LeagueName} - {discussionInfo.HomeName} vs {discussionInfo.AwayName}";
    }

    private string BuildGroupedReplyMessage(List<DiscussionInfo> discussions)
    {
        if (discussions == null || !discussions.Any())
            return string.Empty;

        var firstDiscussion = discussions.First();
        var message = new StringBuilder();
        
        // [League name]
        message.AppendLine($"[{firstDiscussion.LeagueName}]");
        
        // [Home name] - [Away name]
        message.AppendLine($"[{firstDiscussion.HomeName}] - [{firstDiscussion.AwayName}]");
        
        var infoCounter = 1;
        
        // Process each discussion in the group
        foreach (var discussionInfo in discussions)
        {
            // #info1 [Ghost name] - [Ghost detail]
            message.AppendLine($"#info{infoCounter} [{discussionInfo.GhostName}] - [{discussionInfo.GhostDetail}]");
            infoCounter++;
            
            // Bet on: [Bet on] [HDP/OU]@[Odds]
            var betLine = $"Bet on: [{discussionInfo.BetOn}] [{discussionInfo.HdpOu}]@[{discussionInfo.Odds}]";
            message.AppendLine(betLine);
            
            message.AppendLine($"#note: [{discussionInfo.Note}]");
            
            message.AppendLine($"#info{infoCounter} [{discussionInfo.Remark}]");
            infoCounter++;
        }

        return message.ToString().Trim();
    }
}

public class CreateDiscussionResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("discussion")]
    public DiscussionInfoResponse Discussion { get; set; }
}

public class DiscussionInfoResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Name { get; set; }
}