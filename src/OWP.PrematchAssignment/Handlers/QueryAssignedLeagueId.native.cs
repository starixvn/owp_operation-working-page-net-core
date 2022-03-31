using Fanex.Data.Repository;
using MediatR;
using OWP.Utils;

namespace OWP.PreMatchAssignment.Handlers
{
  public class QueryAssignedLeagueIdRequest : IRequest<AssignedLeagueId>
  {
    public int ActorId { get; set; }
  }

  public class QueryAssignedLeagueIdHandler : IRequestHandler<QueryAssignedLeagueIdRequest, AssignedLeagueId>
  {
    private readonly IDynamicRepository dynamicRepository;

    public QueryAssignedLeagueIdHandler(IDynamicRepository dynamicRepository)
    {
      this.dynamicRepository = dynamicRepository;
    }

    public async Task<AssignedLeagueId> Handle(QueryAssignedLeagueIdRequest request, CancellationToken cancellationToken)
    {
      var owpLeageNames = await QueryAssignedLeagueNames(request.ActorId);
      var mainDbLeagueIds = await QueryAssignedLeagueIdsFromMainDB(owpLeageNames);

      if(owpLeageNames.Count() > mainDbLeagueIds.Count())
      {
        var unSupportLeagueNames = owpLeageNames.Except(mainDbLeagueIds.Select(x => x.LeagueName));

        return new AssignedLeagueId
        {
          UnSupportedLeagueNames = unSupportLeagueNames.ToList()
        };
      }

      return new AssignedLeagueId
      {
        LeagueIds = string.Join(",", mainDbLeagueIds)
      };
    }

    public async Task<IEnumerable<string>> QueryAssignedLeagueNames(int actorId)
    {
      var criteria = new QueryAssignedLeagueNamesCriteria { 
        ActorId = actorId,
        Date = DateTime.Now.ToGmt8()
      };

      return await dynamicRepository.FetchAsync<string>(criteria);
    }

    public async Task<IEnumerable<LeagueInfo>> QueryAssignedLeagueIdsFromMainDB(IEnumerable<string> leagueNames)
    {
      var criteria = new QueryAssignedLeagueIdsFromMainDBCriteria
      {
        LeagueNames = string.Join(",", leagueNames)
      };

      return await dynamicRepository.FetchAsync<LeagueInfo>(criteria);
    }
  }

  public class AssignedLeagueId
  {
    public string LeagueIds { get; set; }

    public List<string> UnSupportedLeagueNames { get; set; }
  }

  public class LeagueInfo
  {
    public int LeagueId { get; set; }

    public string LeagueName { get; set; }
  }

  public class QueryAssignedLeagueIdsFromMainDBCriteria : CriteriaBase
  {
    public string LeagueNames { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_QueryAssignedLeagueIdFromMainDB";

    public override bool IsValid() => true;
  }

  public class QueryAssignedLeagueNamesCriteria : CriteriaBase
  {
    public int ActorId { get; set; }

    public DateTime Date { get; set; }

    public override string GetSettingKey() => "OWP_PreMatchAssignment_QueryAssignedLeagueNames";

    public override bool IsValid() => ActorId > 0;
  }
}