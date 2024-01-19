namespace Fanex.NAP.OWP.TaskScheduling.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface ITaskListTodayRepository
    {
        IEnumerable<MatchInfo> GetMatchListToday(ICriteria criteria);

        IEnumerable<DateTime> UpdateTradeStatus(ICriteria criteria);
    }
}