namespace OWP.OtherSport.Download.Services;

using EventAssignment.Criteria;
using Fanex.Data.Repository;
using Submit.Models;

public interface CommandRepository
{
    Task<AssignmentSnapshot> Query(DownloadAssignmentRequest request);
}

public class CommandRepositoryImp : CommandRepository
{
    private readonly IDynamicRepository dynamicRepository;

    public CommandRepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<AssignmentSnapshot> Query(DownloadAssignmentRequest request)
    {
        var criteria = new GetEventAssignmentCriteria
        {
            FromDate = request.From,
            ToDate = request.To,
            Sports = request.Sports,
            OfferLives = request.OfferLives,
            LinkedMatches = request.LinkedMatches
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