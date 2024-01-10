using Fanex.Logging;
using Microsoft.Extensions.Configuration;
using OWP.EventPicking.Criteria;
using OWP.EventPicking.Models;

using OWP.Services;

namespace OWP.EventPicking.Services;

public interface TGRSService
{
    Task<IList<TurnOverWinLossModel>> GetTurnOverWinLoss(GetTurnOverWinLossCriteria criteria);
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

    public async Task<IList<TurnOverWinLossModel>> GetTurnOverWinLoss(GetTurnOverWinLossCriteria criteria)
    {
        try
        {
            // TODO
            var requestUrl = $"{configuration.GetSection("TRGS_Report_ApiEndpoint").Value}/Trader/GetPerformanceDetails";
            return await richHttpClient.Post<List<TurnOverWinLossModel>>(requestUrl, criteria);
        }
        catch (Exception exception)
        {
            await Logger.Log.ErrorAsync(exception.Message);
            return new List<TurnOverWinLossModel>();
        }
    }
}