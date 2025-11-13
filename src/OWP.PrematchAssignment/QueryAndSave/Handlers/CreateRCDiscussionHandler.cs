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
using OWP.Models;
using Fanex.Logging;
using Fanex.NAP.Client;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class CreateRCDiscussionRequest : IRequest<ResultDataObject>
{
    public int AdminSiteId { get; set; }
    public bool IsIncludeTrader { get; set; }
    public List<DiscussionInfo> DiscussionInfos { get; set; }
}

public class CreateRCDiscussionHandler(
    PreMatchAssignmentService preMatchAssignmentService,
    HttpClient httpClient,
    IConfiguration configuration,
    INapSession currentSession,
    ILogger logger) : IRequestHandler<CreateRCDiscussionRequest, ResultDataObject>
{
    // Global configuration values for reuse
    private readonly string _serverUrl = configuration["ChatServer:Url"];
    private readonly string _botToken = configuration["ChatServer:BotToken"];
    private readonly string _botUserId = configuration["ChatServer:BotUserId"];
    private readonly string _parentChannelId = configuration["ChatServer:PrematchDirectionChannelId"];

    public async Task<ResultDataObject> Handle(CreateRCDiscussionRequest request, CancellationToken cancellationToken)
    {
        var (currentDiscussionInfos, matchInfo) = await preMatchAssignmentService.GetMatchDiscussionInfo(request.AdminSiteId);

        if (matchInfo == null)
        {
            return new ResultDataObject
            {
                Code = (int)ErrorCode.Error,
                Message = "Match info is not found"
            };
        }

        UpdateDiscussionInfosWithMatchInfo(request.DiscussionInfos, matchInfo, request);

        var errorGroups = new Dictionary<(int AdminSiteMatchId, int BetType), string>();

        if (request.IsIncludeTrader)
        {
             var assignments = await preMatchAssignmentService.GetMatchAssignments(request.AdminSiteId);
             var eventRecipients = await preMatchAssignmentService.GetEventRecipients();

            var (newDiscussionGroups, existingDiscussionGroups) = GroupAndSeparateDiscussions(request.DiscussionInfos, currentDiscussionInfos);
            
            var newErrors = await ProcessNewDiscussions(newDiscussionGroups, assignments, eventRecipients, cancellationToken);
            
            var existingErrors = await ProcessExistingDiscussions(existingDiscussionGroups, currentDiscussionInfos.ToList(), cancellationToken);

            foreach (var error in newErrors)
            {
                errorGroups[error.Key] = error.Value;
            }
            foreach (var error in existingErrors)
            {
                errorGroups[error.Key] = error.Value;
            }
        }

        var discussionInfosToSave = request.DiscussionInfos?
                .Where(d => !errorGroups.ContainsKey((d.AdminSiteMatchId, d.BetType)))
                .ToList() ?? new List<DiscussionInfo>();


        await preMatchAssignmentService.InsertDiscussion(new InsertDiscussionCriteria
        {
            DiscussionInfoXML = XmlConverter.Serialize(discussionInfosToSave),
            AdminSiteMatchId = request.AdminSiteId,
            IsIncludeTrader = request.IsIncludeTrader
        });

        if (errorGroups.Any())
        {
            await logger.ErrorAsync($"Error in CreateRCDiscussion: {string.Join(", ", errorGroups.Keys.ToList())} ---- {string.Join(", ", errorGroups.Values.ToList())}", "CreateRCDiscussion_Error");
            return new ResultDataObject
            {
                Code = (int)ErrorCode.Error,
                Message = "Some RCDiscussion creations failed",
                Data = errorGroups
            };
        }

        return new ResultDataObject
        {
            Code = (int)ErrorCode.Ok,
            Message = "RCDiscussion created successfully"
        };

    }

    private void UpdateDiscussionInfosWithMatchInfo(List<DiscussionInfo> discussionInfos, DiscussionMatchInfo matchInfo, CreateRCDiscussionRequest request)
    {
        foreach (var discussionInfo in discussionInfos)
        {
            discussionInfo.AdminSiteMatchId = request.AdminSiteId;
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
        IEnumerable<EventRecipient> eventRecipients,
        CancellationToken cancellationToken)
    {
        var errorGroups = new Dictionary<(int AdminSiteMatchId, int BetType), string>();

        foreach (var group in newDiscussionGroups)
        {
            var firstDiscussion = group.First();
            var groupIdentifier = (firstDiscussion.AdminSiteMatchId, firstDiscussion.BetType);

            var tName = BuildDiscussionName(firstDiscussion);
            var users = GetUsersFromAssignments(assignments, eventRecipients, firstDiscussion.BetType);
            var reply = BuildGroupedReplyMessage(group.ToList());

            var (success, errorMessage, rcDiscussionGroupId) = await CreateNewRCDiscussion(tName, users, reply, cancellationToken);

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
        List<DiscussionInfo> currentDiscussionInfos,
        CancellationToken cancellationToken)
    {
        var errorGroups = new Dictionary<(int AdminSiteMatchId, int BetType), string>();
        
        var currentUsername = currentSession.UserName;
        var isUserCreator = currentDiscussionInfos.Any(d => d.CreatedBy.ToString() == currentUsername);

        foreach (var group in existingDiscussionGroups)
        {
            var firstDiscussion = group.First();
            var groupIdentifier = (firstDiscussion.AdminSiteMatchId, firstDiscussion.BetType);

            var discussionList = group.ToList();
            var message = BuildGroupedReplyMessage(discussionList);
            var (success, errorMessage) = await SendMessageToDiscussion(firstDiscussion.RCDiscussionGroupId, message, cancellationToken);


            if (!success)
            {
                errorGroups[groupIdentifier] = errorMessage ?? "Failed to send message to RocketChat discussion";
            }

            if(!isUserCreator)
            {
                var (inviteSuccess, inviteError) = await InviteUserToRoomAsync(firstDiscussion.RCDiscussionGroupId, currentUsername, cancellationToken);
                if (!inviteSuccess)
                {
                    errorGroups[groupIdentifier] = inviteError;
                }
                var (setOwnerSuccess, setOwnerError) = await SetOwnerForDiscussionAsync(firstDiscussion.RCDiscussionGroupId, currentUsername, cancellationToken);
                if (!setOwnerSuccess)
                {
                    errorGroups[groupIdentifier] = setOwnerError;
                }
            }
        }

        return errorGroups;
    }

    private List<string> GetUsersFromAssignments(IEnumerable<Assignment> assignments, IEnumerable<EventRecipient> eventRecipients, int betType)
    {
        var currentUsername = currentSession.UserName;
        var users = assignments
            .Where(a => a.Bettype == betType)
            .Select(a => a.Username)
            .Distinct()
            .ToList();
        users.Add(currentUsername);
        users.AddRange(eventRecipients.Select(e => e.UserName));
        return users;
    }
    private async Task<(bool Success, string ErrorMessage, string DiscussionGroupId)> CreateNewRCDiscussion(
        string discussionName,
        List<string> users,
        string reply,
        CancellationToken cancellationToken)
    {
        try
        {
            var payload = new
            {
                prid = _parentChannelId,
                t_name = discussionName,
                users,
                topic = "Live Soccer",
                reply
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_serverUrl}rooms.createDiscussion");
            httpRequest.Headers.Add("X-Auth-Token", _botToken);
            httpRequest.Headers.Add("X-User-Id", _botUserId);
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

            var discussionGroupId = resultObj?.Discussion?.Id
                ?? resultObj?.Discussion?.Rid
                ?? string.Empty;

            // Get discussion name for return value (used elsewhere in the code)

            if (string.IsNullOrEmpty(discussionGroupId))
            {
                return (false, "Failed to create RocketChat discussion: Empty discussion group ID or name returned", string.Empty);
            }

            var currentUsername = currentSession.UserName;
            var (setOwnerSuccess, setOwnerError) = await SetOwnerForDiscussionAsync(discussionGroupId, currentUsername, cancellationToken);

            if (!setOwnerSuccess)
            {
                return (false, $"Failed to set owner for discussion {discussionGroupId}: {setOwnerError}", string.Empty);
            }

            return (true, null, discussionGroupId);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Exception while creating RocketChat discussion: {ex.Message}";
            return (false, errorMessage, string.Empty);
        }
    }

    private async Task<(bool Success, string ErrorMessage)> SetOwnerForDiscussionAsync(
        string discussionRoomId,
        string username,
        CancellationToken cancellationToken)
    {
        try
        {
            var payload = new
            {
                roomId = discussionRoomId,
                username,
                roleName = "owner"
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_serverUrl}roles.addUserToRole");
            request.Headers.Add("X-Auth-Token", _botToken);
            request.Headers.Add("X-User-Id", _botUserId);
            request.Content = content;

            var response = await httpClient.SendAsync(request, cancellationToken);

            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return (false, $"Failed to set owner. Status: {response.StatusCode}, Body: {body}");
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    private async Task<(bool Success, string ErrorMessage)> InviteUserToRoomAsync(
        string roomId,
        string username,
        CancellationToken cancellationToken)
    {
        try
        {
            var payload = new
            {
                roomId,
                username
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_serverUrl}groups.invite");
            request.Headers.Add("X-Auth-Token", _botToken);
            request.Headers.Add("X-User-Id", _botUserId);
            request.Content = content;

            var response = await httpClient.SendAsync(request, cancellationToken);

            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                return (false, $"Failed to invite user. Status: {response.StatusCode}, Body: {body}");

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    private async Task<(bool Success, string ErrorMessage)> SendMessageToDiscussion(
        string roomId,
        string message,
        CancellationToken cancellationToken)
    {
        try
        {
            var payload = new
            {
                roomId,
                text = message
            };

            var content = new StringContent(
                JsonSerializer.Serialize(payload),
                Encoding.UTF8,
                "application/json"
            );

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{_serverUrl}chat.postMessage");
            httpRequest.Headers.Add("X-Auth-Token", _botToken);
            httpRequest.Headers.Add("X-User-Id", _botUserId);
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
        {
            return string.Empty;
        }
            

        var firstDiscussion = discussions[0];
        var message = new StringBuilder();

        // [League name]
        message.AppendLine($"{firstDiscussion.LeagueName}");

        // [Home name] - [Away name]
        message.AppendLine($"{firstDiscussion.HomeName} - {firstDiscussion.AwayName}");

        var infoCounter = 1;

        // Process each discussion in the group
        foreach (var discussionInfo in discussions)
        {
            message.AppendLine($"**# info{infoCounter}** {discussionInfo.GhostName} - {discussionInfo.GhostDetail}");

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
        var betOnMessage = string.Empty;
        if (discussionInfo.BetOn == "Home")
        {
            betOnMessage = discussionInfo.HomeName;
        }
        else if (discussionInfo.BetOn == "Away")
        {
            betOnMessage = discussionInfo.AwayName;
        }
        else
        {
            betOnMessage = discussionInfo.BetOn;
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
    [System.Text.Json.Serialization.JsonPropertyName("_id")]
    public string Id { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("rid")]
    public string Rid { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("name")]
    public string Name { get; set; }
}