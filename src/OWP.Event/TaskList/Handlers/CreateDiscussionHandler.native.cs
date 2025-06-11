using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Configuration;
using OWP.Event.TaskList.Model;

namespace OWP.Event.TaskList.Handlers;

public class CreateDiscussionRequest : IRequest<string>
{
    public string T_Name { get; set; }
    public List<string> Users { get; set; }
    public string Reply { get; set; }
    public int? OwpMatchId { get; set; }
}

public class CreateDiscussionHandler : IRequestHandler<CreateDiscussionRequest, string>
{
    private readonly TaskListRepository _taskListRepository;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public CreateDiscussionHandler(TaskListRepository taskListRepository, HttpClient httpClient, IConfiguration configuration)
    {
        _taskListRepository = taskListRepository;
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> Handle(CreateDiscussionRequest request, CancellationToken cancellationToken)
    {
        var serverUrl = _configuration["ChatServer:Url"];
        var token = _configuration["ChatServer:BotToken"];
        var userId = _configuration["ChatServer:BotUserId"];
        var parentChannelId = _configuration["ChatServer:LATodayMatchChannelId"];

        var payload = new
        {
            prid = parentChannelId,
            t_name = request.T_Name,
            users = request.Users,
            topic = "Live Soccer",
            reply = request.Reply
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

        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return string.Empty;
        }

        var resultObj = JsonSerializer.Deserialize<CreateDiscussionResponse>(responseBody);
        var discussionName = resultObj?.Discussion?.Name;

        await _taskListRepository.CreateTicketForRCDiscussion(request.OwpMatchId, discussionName);

        return discussionName ?? string.Empty;
    }
}

