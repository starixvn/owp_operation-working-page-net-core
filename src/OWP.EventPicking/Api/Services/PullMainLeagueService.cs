using Fanex.Logging;
using Newtonsoft.Json;
using System.Text;

namespace OWP.EventPicking.Api.Services;

public interface PullMainLeagueService
{
    Task<List<main_league_info>> GetMainLeague(List<EventPickingModel> matches, CancellationToken cancellationToken);
}

public class PullMainLeagueServiceImp(IHttpClientFactory httpClientFactory,
    IDynamicRepository dynamicRepository, ILogger logger): PullMainLeagueService
{
    public async Task<List<main_league_info>> GetMainLeague(List<EventPickingModel> matches, CancellationToken cancellationToken)
    {

        using var client = httpClientFactory.CreateClient("Infosite");

        var data = new
        {
            sportTypeId = 1,
            teams = matches.Select(m => m.HomeName).Concat(matches.Select(m => m.AwayName)).Distinct()
        };

        var bodyContent = new StringContent(JsonConvert.SerializeObject(data),
            Encoding.UTF8,
            "application/json");

        var mainLeagueInfo = new List<main_league_info>();

        try
        {
            // TODO
            var response = await client.PostAsync("", bodyContent, cancellationToken);

            mainLeagueInfo = await HandleResponse(response, cancellationToken);
        }
        catch (Exception ex)
        {
            await logger.ErrorAsync($"Unexpected error: {ex.Message}");
        }

        return mainLeagueInfo;
    }

    private async Task<List<main_league_info>> HandleResponse(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            await logger.ErrorAsync($"Failed to pull main league data: {response.StatusCode} - {response.ReasonPhrase}");
            return new List<main_league_info>();
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        await LogDetailedPayload(DateTime.Now.ToGmt8(), "QueryMainLeague", json);
        
        var result = JsonConvert.DeserializeObject<List<main_league_info>>(json);
        return result ?? new List<main_league_info>();
    }

    private async Task LogDetailedPayload(DateTime CreatedDate, string Action, string Payload)
    {
        await dynamicRepository.ExecuteAsync(new OwpLogCriteria()
        {
            CreatedDate = CreatedDate,
            Action = Action,
            Payload = Payload
        });
    }
}
