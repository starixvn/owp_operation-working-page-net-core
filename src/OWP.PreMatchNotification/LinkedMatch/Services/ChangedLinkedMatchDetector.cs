namespace OWP.PreMatchNotification.LinkedMatch.Services
{
    using Models;
    using OWP.PreMatchNotification.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ChangedLinkedMatchDetector
    {
        Task<List<ChangedValue>> Create(List<GeneralMatch> oldMatches, List<GeneralMatch> newMatches, List<PrematchLeague> prematchLeagues);
    }

    public class ChangedLinkedMatchDetectorImp : ChangedLinkedMatchDetector
    {
        private readonly ChangedLiveMatchGenerator changedLiveMatchGenerator;
        private readonly ChangedMatchStatusGenerator changedMatchStatusGenerator;

        public ChangedLinkedMatchDetectorImp(
            ChangedLiveMatchGenerator changedLiveMatchGenerator,
            ChangedMatchStatusGenerator changedMatchStatusGenerator)
        {
            this.changedLiveMatchGenerator = changedLiveMatchGenerator;
            this.changedMatchStatusGenerator = changedMatchStatusGenerator;
        }

        public async Task<List<ChangedValue>> Create(List<GeneralMatch> oldMatches, List<GeneralMatch> newMatches, List<PrematchLeague> prematchLeagues)
        {
            var filteredOldMatches = FilterExistPrematches(oldMatches, prematchLeagues);
            var filteredNewMatches = FilterExistPrematches(newMatches, prematchLeagues);

            var deletedMatches = GetDeletedLinkedMatches(filteredOldMatches, filteredNewMatches);
            var insertedMatches = GetInsertedLinkedMatches(filteredOldMatches, filteredNewMatches);
            var changedMatches = GetChangedLinkedMatches(filteredOldMatches, filteredNewMatches);

            //var liveMatchMessage = await changedLiveMatchGenerator.Create();
            //var liveMatchMessage = await changedLiveMatchGenerator.Create();

            return new List<ChangedValue>();
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
