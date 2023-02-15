namespace OWP.MatchCountLiveReport;

using Fanex.Data.Repository;
using Models;

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