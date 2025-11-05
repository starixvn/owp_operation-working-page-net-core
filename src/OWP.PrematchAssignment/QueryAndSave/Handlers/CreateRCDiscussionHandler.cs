using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Configuration;
using OWP.Extensions;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class CreateRCDiscussionRequest : IRequest<Unit>
{
    public int AdminSiteId { get; set; } = 115773747;

    public List<DiscussionInfo> DiscussionInfos { get; set; }
}

public class CreateRCDiscussionHandler(
    PreMatchAssignmentService preMatchAssignmentService,
    HttpClient httpClient,
    IConfiguration configuration) : IRequestHandler<CreateRCDiscussionRequest, Unit>
{

    public async Task<Unit> Handle(CreateRCDiscussionRequest request, CancellationToken cancellationToken)
    {
        var assignments = await preMatchAssignmentService.GetMatchAssignments(request.AdminSiteId);
        var (currentDiscussionInfos, matchInfo) = await preMatchAssignmentService.GetMatchDiscussionInfo(request.AdminSiteId);

        if (matchInfo != null)
        {
            UpdateDiscussionInfosWithMatchInfo(request.DiscussionInfos, matchInfo);
        }

        var (newDiscussionGroups, existingDiscussionGroups) = GroupAndSeparateDiscussions(request.DiscussionInfos, currentDiscussionInfos);

        await ProcessNewDiscussions(newDiscussionGroups, assignments, cancellationToken);
        await ProcessExistingDiscussions(existingDiscussionGroups, assignments, cancellationToken);

        if (request.DiscussionInfos != null && request.DiscussionInfos.Any())
        {
            await preMatchAssignmentService.InsertDiscussion(new InsertDiscussionCriteria
            {
                DiscussionInfoXML = request.DiscussionInfos.ToXmlString(),
                AdminSiteMatchId = request.AdminSiteId
            });
        }

        return Unit.Value;
    }

    private void UpdateDiscussionInfosWithMatchInfo(List<DiscussionInfo> discussionInfos, DiscussionMatchInfo matchInfo)
    {
        foreach (var discussionInfo in discussionInfos)
        {
            discussionInfo.LeagueName = matchInfo.LeagueName;
            discussionInfo.HomeId = matchInfo.HomeId;
            discussionInfo.HomeName = matchInfo.HomeName;
            discussionInfo.AwayId = matchInfo.AwayId;
            discussionInfo.AwayName = matchInfo.AwayName;
        }
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
        var serverUrl = configuration["ChatServer:Url"];
        var token = configuration["ChatServer:BotToken"];
        var userId = configuration["ChatServer:BotUserId"];
        var parentChannelId = configuration["ChatServer:LATodayMatchChannelId"];

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
        var serverUrl = configuration["ChatServer:Url"];
        var token = configuration["ChatServer:BotToken"];
        var userId = configuration["ChatServer:BotUserId"];

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

        var response = await httpClient.SendAsync(httpRequest, cancellationToken);

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

        await httpClient.SendAsync(httpRequest, cancellationToken);
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