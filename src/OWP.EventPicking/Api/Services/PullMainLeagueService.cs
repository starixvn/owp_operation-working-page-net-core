using Newtonsoft.Json;
using System.Text;

namespace OWP.EventPicking.Api.Services;

public interface PullMainLeagueService
{
    Task<IList<main_league_info>> GetMainLeague(List<EventPickingModel> matches, CancellationToken cancellationToken);
}

public  class PullMainLeagueServiceImp(IHttpClientFactory httpClientFactory,
    IDynamicRepository dynamicRepository): PullMainLeagueService
{
    public async Task<IList<main_league_info>> GetMainLeague(List<EventPickingModel> matches, CancellationToken cancellationToken)
    {

        using var client = httpClientFactory.CreateClient("Infosite");

        var data = new
        {
            sportTypeId= 1,
            teams = matches.Select(m => m.HomeName).Concat(matches.Select(m => m.AwayName)).Distinct()
        };

        var bodyContent = new StringContent(JsonConvert.SerializeObject(data),
            Encoding.UTF8,
            "application/json");


        // TODO
        var response = await client.PostAsync("", bodyContent, cancellationToken);

        var mainLeagueInfo=await  HandleResponse(response, cancellationToken);
        return mainLeagueInfo;
    }

    private async Task <IList<main_league_info>> HandleResponse(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to pull main league data: {response.ReasonPhrase}");
        }

        var json = await response.Content.ReadAsStringAsync(cancellationToken);
        await LogDetailedPayload(DateTime.Now.ToGmt8(), "QueryMainLeague", json);
        return JsonConvert.DeserializeObject<IList<main_league_info>>(json);
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
