using Fanex.Data.Repository;
using OWP.OtherSport.EventAssignment.Criteria;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.EventAssignment.Services;

public interface CommandRepository
{
    Task<AssignmentSnapshot> Query(QueryEventAssignmentRequest request);
}

public class CommandRepositoryImp : CommandRepository
{
    private readonly IDynamicRepository dynamicRepository;

    public CommandRepositoryImp(
        IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<AssignmentSnapshot> Query(QueryEventAssignmentRequest request)
    {
        var criteria = new GetEventAssignmentCriteria
        {
            FromDate = request.From,
            ToDate = request.To,
            Sports = string.Join(",", request.Sports),
            OfferLives = string.Join(",", request.OfferLives),
            LinkedMatches = string.Join(",", request.LinkedMatches)
        };

        var (matches, assignments, maxbets) = await dynamicRepository.FetchMultipleAsync<MatchInfo, MatchAssignment, MatchSetting>(criteria);

        return new AssignmentSnapshot
        {
            Matches = matches.ToDictionary(m => m.OSMatchId),
            MatchAssignments = assignments.GroupBy(a => a.OSMatchId).ToDictionary(a => a.Key, a => a.ToList()),
            MatchSettings = maxbets.GroupBy(a => a.OSMatchId).ToDictionary(a => a.Key, a => a.ToList())
        };
    }
}