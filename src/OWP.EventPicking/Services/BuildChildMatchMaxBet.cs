using MediatR;
using OWP.Constants;
using OWP.EventPicking.Models;
using OWP.Extensions;
using OWP.MaxbetSetting.Handlers;
using OWP.MaxbetSetting.Models;
using OWP.Utils;

namespace OWP.EventPicking.Services;

public interface BuildChildMatchMaxBet
{
    Task BuildChildMatchMaxbet(IEnumerable<EventPickingModel> mainMatches, IEnumerable<EventPickingModel> childMatches);
}

public class BuildChildMatchMaxBetImp : BuildChildMatchMaxBet
{
    private readonly IMediator mediator;

    public BuildChildMatchMaxBetImp(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task BuildChildMatchMaxbet(IEnumerable<EventPickingModel> mainMatches, IEnumerable<EventPickingModel> childMatches)
    {
        var maxBet = await mediator.Send(new QueryMaxbetSettingRequest());
        var maxBetInDict = maxBet.ToDictionary(x => x.FT_HDP);
        var mainMatchesHasMaxBet = mainMatches.Where(x => x.MaxBetHDPNumber.HasValue).ToList();

        BuildMaxBetChildMatch(mainMatchesHasMaxBet, childMatches, maxBetInDict);
    }

    private static void BuildMaxBetChildMatch(IEnumerable<EventPickingModel> mainMatchesHasMaxBet, IEnumerable<EventPickingModel> childMatches, Dictionary<decimal, MaxBetSettingModel> maxBetDefault)
    {
        foreach (var child in childMatches)
        {
            var parentMaxBet = mainMatchesHasMaxBet.FirstOrDefault(x => x.InfoSiteMatchId == child.InfoSiteMatchId);
            if (parentMaxBet != null)
            {
                var maxBet = maxBetDefault.ContainsKey((decimal)parentMaxBet.MaxBetHDPNumber)
                    ? maxBetDefault.GetValue((decimal)parentMaxBet.MaxBetHDPNumber)
                    : maxBetDefault.GetValue((-1000m));
                switch (child.MinorBettypeId)
                {
                    case Bettype.Corner:
                        child.MaxBetHDP = maxBet.NoOfCorner_FT_HDP.ToOWPMaxBetFormat();
                        child.MaxBetOU = maxBet.NoOfCorner_FT_OU.ToOWPMaxBetFormat();
                        child.MaxBet1HHDP = maxBet.NoOfCorner_1H_HDP.ToOWPMaxBetFormat();
                        child.MaxBet1HOU = maxBet.NoOfCorner_1H_OU.ToOWPMaxBetFormat();
                        break;
                    case Bettype.TotalBooking:
                        child.MaxBetHDP = maxBet.TotalBooking_FT_HDP.ToOWPMaxBetFormat();
                        child.MaxBetOU = maxBet.TotalBooking_FT_OU.ToOWPMaxBetFormat();
                        child.MaxBet1HHDP = maxBet.TotalBooking_1H_HDP.ToOWPMaxBetFormat();
                        child.MaxBet1HOU = maxBet.TotalBooking_1H_OU.ToOWPMaxBetFormat();
                        break;
                    case Bettype.NextCorner:
                        child.MaxBetHDP = maxBet.NextCorner_FT_HDP.ToOWPMaxBetFormat();
                        break;
                    case Bettype.NextBooking:
                        child.MaxBetHDP = maxBet.NextBooking_FT_HDP.ToOWPMaxBetFormat();
                        break;
                    default:
                        child.MaxBetHDP = string.Empty;
                        break;
                }
            }
        }
    }
}