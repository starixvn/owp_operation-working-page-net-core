using System.Text;
using System.Text.Json;
using Olympus.Xml;
using MediatR;
using Microsoft.Extensions.Configuration;
using OWP.Enums;
using OWP.PrematchAssignment.Criteria;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;
using OWP.Utils;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class CreateRCDiscussionRequest : IRequest<Unit>
{
    public int AdminSiteId { get; set; } = 116493050;

    public List<DiscussionInfo> DiscussionInfos { get; set; } = new List<DiscussionInfo>()
    {
        new DiscussionInfo
        {
            AdminSiteMatchId = 116493050,
            BetType = 1,
            HomeName = "Home",
            AwayName = "Away",
            GhostName = "Ghost 1",
            GhostDetail = "Ghost Detail 1",
            HdpOu = 2.0m,
            Odds = "1.00",
            Note = "Note",
            BetOn = "Home",
            Remark = "Remark1",
        },
        new DiscussionInfo
        {
            AdminSiteMatchId = 116493050,
            BetType = 7,
            HomeName = "Home",
            AwayName = "Away",
            GhostName = "Ghost 2",
            GhostDetail = "Ghost Detail 2",
            HdpOu = 3.0m,
            Odds = "1.00",
            Note = "Note",
            BetOn = "Away",
            Remark = "Remark2",
        },
        new DiscussionInfo
        {
            AdminSiteMatchId = 116493050,
            BetType = 7,
            HomeName = "Home",
            AwayName = "Away",
            GhostName = "Ghost 3",
            GhostDetail = "Ghost Detail 3",
            HdpOu = 4.0m,
            Odds = "1.00",
            Note = "Note",
            Remark = "Remark",
            BetOn = "HDP/OU",
        },
        new DiscussionInfo
        {
            AdminSiteMatchId = 116493050,
            BetType = 3,
            HomeName = "Home",
            AwayName = "Away",
            GhostName = "Ghost 4",
            GhostDetail = "Ghost Detail 4",
            HdpOu = 5.0m,
            Odds = "1.00",
            Note = "Note",
            Remark = "Remark",
            BetOn = "HDP/OU",
        },
    };
}

public class CreateRCDiscussionHandler(
    PreMatchAssignmentService preMatchAssignmentService,
    HttpClient httpClient,
    IConfiguration configuration) : IRequestHandler<CreateRCDiscussionRequest, Unit>
{

    public async Task<Unit> Handle(CreateRCDiscussionRequest request, CancellationToken cancellationToken)
    {
        var (currentDiscussionInfos, matchInfo) = await preMatchAssignmentService.GetMatchDiscussionInfo(request.AdminSiteId);

        if (matchInfo == null)
        {
            throw new Exception("Match info is not found");
        }

        UpdateDiscussionInfosWithMatchInfo(request.DiscussionInfos, matchInfo);
        var assignments = await preMatchAssignmentService.GetMatchAssignments(request.AdminSiteId);

        var (newDiscussionGroups, existingDiscussionGroups) = GroupAndSeparateDiscussions(request.DiscussionInfos, currentDiscussionInfos);

        var errorGroups = new Dictionary<(int AdminSiteMatchId, int BetType), string>();
        
        var newErrors = await ProcessNewDiscussions(newDiscussionGroups, assignments, cancellationToken);
        foreach (var error in newErrors)
        {
            errorGroups[error.Key] = error.Value;
        }
        
        var existingErrors = await ProcessExistingDiscussions(existingDiscussionGroups, assignments, cancellationToken);
        foreach (var error in existingErrors)
        {
            errorGroups[error.Key] = error.Value;
        }

        // Filter out DiscussionInfo items that belong to error groups
        var discussionInfosToSave = request.DiscussionInfos?
            .Where(d => !errorGroups.ContainsKey((d.AdminSiteMatchId, d.BetType)))
            .ToList() ?? new List<DiscussionInfo>();

        if (discussionInfosToSave.Any())
        {
            await preMatchAssignmentService.InsertDiscussion(new InsertDiscussionCriteria
            {
                DiscussionInfoXML = XmlConverter.Serialize(discussionInfosToSave),
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

    private async Task<Dictionary<(int AdminSiteMatchId, int BetType), string>> ProcessNewDiscussions(
        List<IGrouping<object, DiscussionInfo>> newDiscussionGroups,
        IEnumerable<Assignment> assignments,
        CancellationToken cancellationToken)
    {
        var errorGroups = new Dictionary<(int AdminSiteMatchId, int BetType), string>();
        var serverUrl = configuration["ChatServer:Url"];
        var token = configuration["ChatServer:BotToken"];
        var userId = configuration["ChatServer:BotUserId"];
        var parentChannelId = configuration["ChatServer:LATodayMatchChannelId"];

        foreach (var group in newDiscussionGroups)
        {
            var firstDiscussion = group.First();
            var groupIdentifier = (firstDiscussion.AdminSiteMatchId, firstDiscussion.BetType);

           
            var tName = BuildDiscussionName(firstDiscussion);
            var users = GetUsersFromAssignments(assignments, firstDiscussion.BetType);
            var reply = BuildGroupedReplyMessage(group.ToList());

            var (success, errorMessage, rcDiscussionGroupId) = await CreateNewRCDiscussion(
                serverUrl, token, userId, parentChannelId, tName, users, reply, cancellationToken);

            if (success && !string.IsNullOrEmpty(rcDiscussionGroupId))
            {
                foreach (var item in group)
                {
                    item.RCDiscussionGroupId = rcDiscussionGroupId;
                }
            }
            else
            {
                errorGroups[groupIdentifier] = errorMessage;
            }
        }
        return errorGroups;
    }

    private async Task<Dictionary<(int AdminSiteMatchId, int BetType), string>> ProcessExistingDiscussions(
        List<IGrouping<object, DiscussionInfo>> existingDiscussionGroups,
        IEnumerable<Assignment> assignments,
        CancellationToken cancellationToken)
    {
        var errorGroups = new Dictionary<(int AdminSiteMatchId, int BetType), string>();
        var serverUrl = configuration["ChatServer:Url"];
        var token = configuration["ChatServer:BotToken"];
        var userId = configuration["ChatServer:BotUserId"];

        foreach (var group in existingDiscussionGroups)
        {
            var firstDiscussion = group.First();
            var groupIdentifier = (firstDiscussion.AdminSiteMatchId, firstDiscussion.BetType);
            
            var discussionList = group.ToList();
            var message = BuildGroupedReplyMessage(discussionList);
            var (success, errorMessage) = await SendMessageToDiscussion(
                serverUrl, token, userId, firstDiscussion.RCDiscussionGroupId, message, cancellationToken);
            
            if (!success)
            {
                errorGroups[groupIdentifier] = errorMessage ?? "Failed to send message to RocketChat discussion";
            }
            
        }

        return errorGroups;
    }

    private List<string> GetUsersFromAssignments(IEnumerable<Assignment> assignments, int betType)
    {
        return assignments
            .Where(a => a.Bettype == betType)
            .Select(a => a.Username)
            .Distinct()
            .ToList();
    }

    private async Task<(bool Success string ErrorMessage, string DiscussionGroupId)> CreateNewRCDiscussion(
        string serverUrl,
        string token,
        string userId,
        string parentChannelId,
        string discussionName,
        List<string> users,
        string reply,
        CancellationToken cancellationToken)
    {
        try
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
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                var errorMessage = $"Failed to create RocketChat discussion. Status: {response.StatusCode}, Error: {errorBody}";
                return (false, errorMessage, string.Empty);
            }

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            var resultObj = JsonSerializer.Deserialize<CreateDiscussionResponse>(responseBody);
            var discussionGroupId = resultObj?.Discussion?.Name ?? string.Empty;
            
            if (string.IsNullOrEmpty(discussionGroupId))
            {
                return (false, "Failed to create RocketChat discussion: Empty discussion group ID returned", string.Empty);
            }

            return (true, null, discussionGroupId);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Exception while creating RocketChat discussion: {ex.Message}";
            return (false, errorMessage, string.Empty);
        }
    }


    private async Task<(bool Success, string ErrorMessage)> SendMessageToDiscussion(
        string serverUrl,
        string token,
        string userId,
        string roomId,
        string message,
        CancellationToken cancellationToken)
    {
        try
        {
            var payload = new
            {
                roomId = roomId,
                text = message
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{serverUrl}chat.postMessage");
            httpRequest.Headers.Add("X-Auth-Token", token);
            httpRequest.Headers.Add("X-User-Id", userId);
            httpRequest.Content = content;

            var response = await httpClient.SendAsync(httpRequest, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
                var errorMessage = $"Failed to send message to RocketChat discussion. Status: {response.StatusCode}, Error: {errorBody}";
                return (false, errorMessage);
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    private string BuildDiscussionName(DiscussionInfo discussionInfo)
    {
        var betType = (MajorBetType)discussionInfo.BetType;
        var betTypeName = EnumUtils<MajorBetType>.GetDescription(betType, string.Empty);
        return $"Direction Ticket - {discussionInfo.HomeName} -vs- {discussionInfo.AwayName} - {betTypeName}";
    }


    private string BuildGroupedReplyMessage(List<DiscussionInfo> discussions)
    {
        if (discussions == null || !discussions.Any())
            return string.Empty;

        var firstDiscussion = discussions.First();
        var message = new StringBuilder();

        // [League name]
        message.AppendLine($"{firstDiscussion.LeagueName}");

        // [Home name] - [Away name]
        message.AppendLine($"{firstDiscussion.HomeName} - {firstDiscussion.AwayName}");

        var infoCounter = 1;

        // Process each discussion in the group
        foreach (var discussionInfo in discussions)
        {
            message.AppendLine($"**#info{infoCounter}** {discussionInfo.GhostName} - {discussionInfo.GhostDetail}");

            // Bet on: [Bet on] [HDP/OU]@[Odds]
            var betLine = $"Bet on: {buildBetOnMessage(discussionInfo)} {discussionInfo.HdpOu}@{discussionInfo.Odds}";
            message.AppendLine(betLine);

            message.AppendLine($"#note: {discussionInfo.Note}");

            infoCounter++;
        }

        return message.ToString().Trim();
    }
    
    public string buildBetOnMessage(DiscussionInfo discussionInfo)
    {
        var betOnMessage = discussionInfo.BetOn;
        if (discussionInfo.BetOn == "Home")
        {
            betOnMessage = discussionInfo.HomeName;
        }
        else if (discussionInfo.BetOn == "Away")
        {
            betOnMessage = discussionInfo.AwayName;
        }
        return betOnMessage;
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