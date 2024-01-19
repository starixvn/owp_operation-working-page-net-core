namespace Fanex.NAP.OWP.TaskScheduling.Service.Interfaces
{
    using Criteria.TaskListToday;
    using global::OWP.Models;
    using System;
    using System.Collections.Generic;

    public interface ITaskListTodayService
    {
        Task<IEnumerable<MatchInfo>> GetMatchListToday(GetTaskListTodayCriteria criteria);

        OWPCommandResult UpdateMatchNoteStatus(UpdateMatchNoteViewCriteria criteria);

        IEnumerable<DateTime> UpdateReadyTradeStatus(UpdateReadyTradeStatusCriteria criteria);
    }
}