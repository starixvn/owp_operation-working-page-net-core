using Fanex.Data.Repository;
using OWP.MatchCountLiveReport.Models;

namespace OWP.MatchCountLiveReport;

public interface Repository
{
  Task<IList<ReportInfo>> QueryReportInfo(CriteriaBase criteria);
}

public class RepositoryImp : Repository
{
  private readonly IDynamicRepository dynamicRepository;

  public RepositoryImp(IDynamicRepository dynamicRepository)
  {
    this.dynamicRepository = dynamicRepository;
  }

  public async Task<IList<ReportInfo>> QueryReportInfo(CriteriaBase criteria)
  => (await dynamicRepository.FetchAsync<ReportInfo>(criteria)).ToList();
}