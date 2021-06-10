namespace OWP.PreMatchNotification.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public interface ChangedValueBySnapshot
    {
        List<ChangedValue> Create(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot);
    }

    public class ChangedValueBySnapshotImp : ChangedValueBySnapshot
    {
        private readonly NewLeagueValueGenerator newLeagueGenerator;
        private readonly ChangedMatchChangedValueGenerator changedMatchChangedValueGenerator;

        public ChangedValueBySnapshotImp(
            NewLeagueValueGenerator newLeagueGenerator,
            ChangedMatchChangedValueGenerator changedMatchChangedValueGenerator)
        {
            this.newLeagueGenerator = newLeagueGenerator;
            this.changedMatchChangedValueGenerator = changedMatchChangedValueGenerator;
        }

        public List<ChangedValue> Create(IList<InfoSiteMatch> oldSnapshot, IList<InfoSiteMatch> newSnapshot)
        {
            var newLeaguedValues = newLeagueGenerator.Create(changedSnapshot.ChangedMatches, oldSnapshot, newSnapshot);
            var changedMatchChagedValues = changedMatchChangedValueGenerator.Create(changedSnapshot.ChangedMatches, oldSnapshot, newSnapshot);

            return new[]
            {
                newLeaguedValues,
                changedMatchChagedValues
            }
            .SelectMany(v => v)
            .ToList();
        }
    }
}