using Fanex.Logging;
using Microsoft.Extensions.Configuration;
using OWP.EventPicking.Api.Criteria;
using OWP.EventPicking.Api.Models;

namespace OWP.EventPicking.Api.Services;

public interface TGRSService
{
    Task<(IList<TurnOverWinLossModel>, bool)> GetTurnOverWinLoss(GetTurnOverWinLossCriteria criteria);
}

public class TGRSServiceImp : TGRSService
{
    private readonly IConfiguration configuration;

    private readonly RichHttpClient richHttpClient;

    public TGRSServiceImp(RichHttpClient richHttpClient, IConfiguration configuration)
    {
        this.configuration = configuration;
        this.richHttpClient = richHttpClient;
    }

    public async Task<(IList<TurnOverWinLossModel>, bool)> GetTurnOverWinLoss(GetTurnOverWinLossCriteria criteria)
    {
        try
        {
            var requestUrl = $"{configuration.GetSection("TRGS_TurnOver_WinLoss_ApiEndpoint").Value}/LeaguePerformance/GetLeaguePerformance";
            return (await richHttpClient.Post<List<TurnOverWinLossModel>>(requestUrl, criteria), false);
        }
        catch (Exception exception)
        {
            await Logger.Log.ErrorAsync(exception.Message);
            return (new List<TurnOverWinLossModel>(), true);
        }
    }
}