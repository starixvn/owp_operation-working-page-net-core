using AutoMapper;
using Fanex.Data.Repository;
using OWP.Kpi.LA.Constant;
using OWP.Kpi.LA.Criteria;
using OWP.Kpi.LA.Helper;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.ViewModels;

namespace OWP.Kpi.LA.Services;

public interface KpiService
{
    Task<IList<KpiInfoViewModel>> BuildKpiInfo(DateTime fromDate, DateTime toDate, bool isGap);
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

    public async Task<IList<KpiInfoViewModel>> BuildKpiInfo(DateTime fromDate, DateTime toDate, bool isGap)
    {
        var kpiData = await repository.GetKpiInfo(fromDate, toDate);
        var kpiInfos = mapper.Map<IList<KpiInfoViewModel>>(kpiData);
        BuildKpiInfo(kpiInfos, isGap);
        return kpiInfos;
    }

    public void BuildKpiInfo(IList<KpiInfoViewModel> kpiInfos, bool isGap)
    {
        int initialNo = 1;
        foreach (var kpi in kpiInfos)
        {
            kpi.No = initialNo;
            initialNo++;
            if (isGap)
            {
                var progress = ProgressHelper.Evaluate(ConditionConstant.ConditionInfos.ToList(), kpi.AchievedGames);
                kpi.GapWithTarget = CalculateGapHelper.CalculateGap(kpi.TargetId, kpi.AchievedGames);
                kpi.Progress = progress;
                kpi.Reward = ConditionConstant.ConditionInfos.FirstOrDefault(x => x.Progress == progress)?.Reward;
            }
        }
    }
}
