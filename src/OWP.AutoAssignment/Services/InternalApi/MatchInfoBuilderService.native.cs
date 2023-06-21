using AutoMapper;
using OWP.AutoAssignment.Constant;
using OWP.AutoAssignment.Models.InternalApi;
using System.Globalization;

namespace OWP.AutoAssignment.Services.InternalApi;

public interface MatchInfoBuilderService
{
    Task Build(
        IList<ExportMatchModel> models,
        IList<AutoAssignmentResultModel> autoAssignModels
    );
}

public class MatchInfoBuilderServiceImp : MatchInfoBuilderService
{
    private readonly Repository repository;

    private readonly IMapper mapper;

    public MatchInfoBuilderServiceImp(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task Build(IList<ExportMatchModel> models, IList<AutoAssignmentResultModel> autoAssignModels)
    {
        var matchQueryInfo = BuildMatchQueryInfo(autoAssignModels);
        var matchInfos = (await QueryMatchInfo(matchQueryInfo))
            .OrderBy(x => x.EventDateTime).ToList();

        BuildExportMatchModel(models, matchInfos);
    }

    private IList<MatchQueryInfo> BuildMatchQueryInfo(IEnumerable<AutoAssignmentResultModel> autoAssignModels)
        => autoAssignModels
            .Select(model => new MatchQueryInfo
            {
                InfoSiteMatchId = model.InfoSiteMatchId,
                BetType = model.Bettype
            }).ToList();

    private async Task<IEnumerable<InfoSiteMatchModel>> QueryMatchInfo(IList<MatchQueryInfo> queryInfo)
        => await repository.GetInfoSiteMatchInfo(queryInfo);

    private void BuildExportMatchModel(IList<ExportMatchModel> models,
        List<InfoSiteMatchModel> matchInfos)
    {
        foreach (var match in matchInfos)
        {
            var matchModel = BuildExportMatchInfo(match);
            models.Add(matchModel);
        }
    }

    private ExportMatchModel BuildExportMatchInfo(InfoSiteMatchModel matchInfo)
        => matchInfo.Bettype == BetType.MainMatch ? mapper.Map<ExportMatchModel>(matchInfo)
            : new ExportMatchModel
            {
                EventDateTime = matchInfo.EventDateTime
                    .ToString("hh:mm tt", CultureInfo.InvariantCulture),
                LeagueName = matchInfo.LeagueName,
                AwayName = matchInfo.AwayName,
                HomeName = matchInfo.HomeName,
                InfoSiteMatchId = GetInfoSiteCornerMatchId(matchInfo),
                Bettype = matchInfo.Bettype
            };

    private int GetInfoSiteCornerMatchId(InfoSiteMatchModel matchInfo)
        => matchInfo.InfoSiteMatchId.HasValue
            ? matchInfo.InfoSiteMatchId.Value : 0;
}