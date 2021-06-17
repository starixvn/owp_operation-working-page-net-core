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
        List<ChangedValue> Create(List<GeneralMatch> oldMatches, List<GeneralMatch> newMatches, Employee employees);
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

        public List<ChangedValue> Create(List<GeneralMatch> oldMatches, List<GeneralMatch> newMatches, Employee employees)
        {
            var changedSnapshot = GetChangedSnapshot(oldMatches, newMatches, employees);

            var liveMatchMessage = changedLiveMatchGenerator.Create(changedSnapshot, employees);
            var matchStatusMessage = changedMatchStatusGenerator.Create(changedSnapshot, employees);

            return liveMatchMessage.Union(matchStatusMessage).ToList();
        }

        public ChangedSnapshot GetChangedSnapshot(List<GeneralMatch> oldMatches, List<GeneralMatch> newMatches, Employee employees)
        {
            var filteredOldMatches = FilterExistPrematches(oldMatches, employees.PrematchInfosites);
            var filteredNewMatches = FilterExistPrematches(newMatches, employees.PrematchInfosites);

            var deletedMatches = GetDeletedLinkedMatches(filteredOldMatches, filteredNewMatches);
            var insertedMatches = GetInsertedLinkedMatches(filteredOldMatches, filteredNewMatches);
            var changedMatches = GetChangedLinkedMatches(filteredOldMatches, filteredNewMatches);

            return new ChangedSnapshot
            {
                DeletedMatches = deletedMatches,
                InsertedMatches = insertedMatches,
                ChangedMatches = changedMatches,
                OldSnapshot = oldMatches,
                NewSnapshot = newMatches
            };
        }
               
        public IEnumerable<GeneralMatch> FilterExistPrematches(List<GeneralMatch> matches, Dictionary<int, InfositePrematchLeague> prematchLeagues)
        {
            var existInfositePrematch = prematchLeagues.Keys.Distinct();
            return matches.Where(x => existInfositePrematch.Contains(x.InfoSiteMatchId));
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
