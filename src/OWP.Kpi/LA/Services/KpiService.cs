using AutoMapper;
using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.ViewModels;

namespace OWP.Kpi.LA.Services;

public interface KpiService
{
    Task<IList<KpiInfoViewModel>> BuildKpiInfo(QueryModel request, bool isGap);
}

public class KpiServiceImp : KpiService
{
    private readonly Repository repository;
    private readonly IMapper mapper;

    public KpiServiceImp(Repository repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<IList<KpiInfoViewModel>> BuildKpiInfo(QueryModel request, bool isGap)
    {
        var kpiData = await repository.GetKpiInfo(request);
        var kpiInfos = mapper.Map<IList<KpiInfoViewModel>>(kpiData);
        BuildKpiInfo(kpiInfos, isGap);
        return kpiInfos;
    }

    public void BuildKpiInfo(IList<KpiInfoViewModel> kpiInfos, bool isGap)
    {
        var initialNo = 1;
        foreach (var kpi in kpiInfos)
        {
            kpi.No = initialNo;
            initialNo++;
            if (!isGap)
            {
                continue;
            }

            var progress = ProgressHelper.Evaluate(ConditionConstant.ConditionInfos.ToList(), kpi.AchievedGames);
            kpi.GapWithTarget = CalculateGapHelper.CalculateGap(kpi.TargetId, kpi.AchievedGames);
            kpi.Progress = progress;
            kpi.Reward = Array.Find(ConditionConstant.ConditionInfos, x => x.Progress == progress)?.Reward;
        }
    }
}
