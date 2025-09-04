using Fanex.NAP.Client;
using AutoMapper;

namespace OWP.NewLeagueSetting.Command;

public class UpdateLeagueSettingRequest : LeagueSettingRequest, Command<Unit> { }

public class UpdateLeagueSettingHandler(Repository repository, INapSession session, IMapper mapper) : CommandHandler<UpdateLeagueSettingRequest, Unit>
{
    public async Task<Unit> Handle(UpdateLeagueSettingRequest request, CancellationToken cancellationToken)
    {
        StandardizedLeagueInfo.BuildMaxbet(request, request.MaxbetWD_HDP, request.MaxbetWK_HDP);
        StandardizedLeagueInfo.BuildNoHdp(request, request.WD_NoOfHDP, request.WK_NoOfHDP);
        StandardizedLeagueInfo.BuildSpread(request, request.Spread);
        
        if (request.LeagueSettingId != 0)
        {
            await repository.UpdateLeagueSetting(request, session.UserId);
        }
        else
        {
            var insertCriteria = new InsertLeagueSettingCriteria
            {
                League = request.League,
                LeagueGroupId = request.LeagueGroupId,
                Spread_HDP_Value = request.Spread_HDP_Value,
                Spread_OU_Value = request.Spread_OU_Value,
                Spread_Note = request.Spread_Note,
                MaxbetWD_HDP_Value = request.MaxbetWD_HDP_Value,
                MaxbetWD_HDP_Note = request.MaxbetWD_HDP_Note,
                WD_NoOfHDP_Value = request.WD_NoOfHDP_Value,
                WD_NoOfHDP_Note = request.WD_NoOfHDP_Note,
                MaxbetWK_HDP_Value = request.MaxbetWK_HDP_Value,
                MaxbetWK_HDP_Note = request.MaxbetWK_HDP_Note,
                WK_NoOfHDP_Value = request.WK_NoOfHDP_Value,
                WK_NoOfHDP_Note = request.WK_NoOfHDP_Note,
                LstNote = request.LstNote,
                Remark2 = request.Remark2,
                Comment = request.Comment,
                FullConditionDescription = request.FullConditionDescription,
                NoOfferLive = request.NoOfferLive,
                ActorId = session.UserId
            };
            await repository.ExecuteNonQueryCommand(insertCriteria);
        }
        
        return Unit.Value;
    }
}
