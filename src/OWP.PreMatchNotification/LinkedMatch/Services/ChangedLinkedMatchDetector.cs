namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using Models;
    using OWP.PreMatchNotification.Constants;
    using OWP.PreMatchNotification.Models;
    using OWP.PreMatchNotification.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ChangedLinkedMatchDetector
    {
        Task<List<ChangedValue>> Create(List<GeneralMatch> oldMatches, List<GeneralMatch> newMatches, Dictionary<int, PrematchLeague> prematchLeagues);
    }

    public class ChangedLinkedMatchDetectorImp : ChangedLinkedMatchDetector
    {
        private readonly ChangedLiveMatchGenerator changedLiveMatchGenerator;
        private readonly ChangedMatchStatusGenerator changedMatchStatusGenerator;
        private readonly CommandRepository commandRepository;

        public ChangedLinkedMatchDetectorImp(
            ChangedLiveMatchGenerator changedLiveMatchGenerator,
            ChangedMatchStatusGenerator changedMatchStatusGenerator,
            CommandRepository commandRepository)
        {
            this.changedLiveMatchGenerator = changedLiveMatchGenerator;
            this.changedMatchStatusGenerator = changedMatchStatusGenerator;
            this.commandRepository = commandRepository;
        }

        public async Task<List<ChangedValue>> Create(List<GeneralMatch> oldMatches, List<GeneralMatch> newMatches, Dictionary<int, PrematchLeague> prematchLeagues)
        {
            var changedSnapshot = GetChangedSnapshot(oldMatches, newMatches, prematchLeagues);
            var prematchAssignees = await GetPrematchAssigneeIds(prematchLeagues);
            var allPrematchEmployees = await commandRepository.GetEmployeesBySection(Section.PreMatch);

            var liveMatchMessage = changedLiveMatchGenerator.Create(changedSnapshot, allPrematchEmployees, prematchAssignees);
            var matchStatusMessage = changedMatchStatusGenerator.Create(changedSnapshot, prematchAssignees);

            return liveMatchMessage.Union(matchStatusMessage).ToList();
        }

        public ChangedSnapshot GetChangedSnapshot(List<GeneralMatch> oldMatches, List<GeneralMatch> newMatches, Dictionary<int, PrematchLeague> prematchLeagues)
        {
            var prematchLeagueList = prematchLeagues.Values.ToList();
            var filteredOldMatches = FilterExistPrematches(oldMatches, prematchLeagueList);
            var filteredNewMatches = FilterExistPrematches(newMatches, prematchLeagueList);

            var deletedMatches = GetDeletedLinkedMatches(filteredOldMatches, filteredNewMatches);
            var insertedMatches = GetInsertedLinkedMatches(filteredOldMatches, filteredNewMatches);
            var changedMatches = GetChangedLinkedMatches(filteredOldMatches, filteredNewMatches);

            return new ChangedSnapshot
            {
                DeletedMatches = deletedMatches,
                InsertedMatches = insertedMatches,
                ChangedMatches = changedMatches,
                OldSnapshot = oldMatches,
                NewSnapshot = newMatches,
                PrematchLeagues = prematchLeagues
            };
        }

        public async Task<List<Assignee>> GetPrematchAssigneeIds(Dictionary<int, PrematchLeague> prematchLeagues)
        {
            var prematchLeagueIds = prematchLeagues.Values.Select(x => x.PreMatchLeagueId).Distinct().ToList();
            var prematchAssignees = await commandRepository.GetPreMatchLeagueAssignees(prematchLeagueIds);
            return prematchAssignees.ToList();
        }

        public IEnumerable<GeneralMatch> FilterExistPrematches(List<GeneralMatch> matches, List<PrematchLeague> prematchLeagues)
        {
            var existPrematchInfosite = prematchLeagues.Select(x => x.InfoSiteMatchId).Distinct();
            return matches.Where(x => existPrematchInfosite.Contains(x.InfoSiteMatchId));
        }

        public List<GeneralMatch> GetDeletedLinkedMatches(IEnumerable<GeneralMatch> oldMatches, IEnumerable<GeneralMatch> newMatches)
        {
            return oldMatches.Except(newMatches, new LinkedMatchComparer()).ToList();
        }

        public List<GeneralMatch> GetInsertedLinkedMatches(IEnumerable<GeneralMatch> oldMatches, IEnumerable<GeneralMatch> newMatches)
        {
            return newMatches.Except(oldMatches, new LinkedMatchComparer()).ToList();
        }

        public List<GeneralMatch> GetChangedLinkedMatches(IEnumerable<GeneralMatch> oldMatches, IEnumerable<GeneralMatch> newMatches)
        {
            return oldMatches.Intersect(newMatches, new LinkedMatchComparer()).ToList();
        }

        internal class LinkedMatchComparer : IEqualityComparer<GeneralMatch>
        {
            public bool Equals(GeneralMatch x, GeneralMatch y)
            {
                return x.InfoSiteMatchId == y.InfoSiteMatchId &&
                       x.MatchId == y.MatchId;
            }

            public int GetHashCode(GeneralMatch obj)
            {
                return obj.MatchId.GetHashCode();
            }
        }
    }
}
