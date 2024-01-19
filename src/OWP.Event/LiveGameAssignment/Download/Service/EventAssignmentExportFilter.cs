using OWP.Event.LiveGameAssignment.Download.Models;
using OWP.Extensions;

namespace OWP.Event.LiveGameAssignment.Download.Service.Implement;

using OWP.Event.EventShared.Enum;
using OWP.Event.LiveGameAssignment;
using OWP.Event.LiveGameAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using EventModel = EventModel;

public interface EventAssignmentExportFilter
{
    List<EventModel> Filter(List<EventModel> events, EventAssignmentPermissionModel permission, string username, int liveStatus, string matchType, string offVRs);
}

public class EventAssignmentExportFilterImp : EventAssignmentExportFilter
{

    public List<EventModel> Filter(List<EventModel> events, EventAssignmentPermissionModel permission, string username, int liveStatus, string matchType, string offVRs)
    {
        events = FilterEventModelByUser(events, username);
        events = FilterEventModelByPermision(events, permission);
        events = FilterEventModelBySyncLiveStatus(events, liveStatus);
        events = FilterEventByMatchType(events, matchType);
        events = FilterEventByOffVRs(events, offVRs);
        return events;
    }
    private static List<EventModel> FilterEventByMatchType(IList<EventModel> matchInfos, string matchType)
    {
        if (string.IsNullOrEmpty(matchType))
        {
            return matchInfos.ToList();
        }
        var types = matchType.Split(',').ToList();
        if (IsSelectAllMatchType(types))
        {
            return matchInfos.ToList();
        }

        return types.Exists(x => x == EventAssignmentConst.MatchVirtual) ?
            matchInfos.Where(x => x.IsSaBa).ToList() :
            matchInfos.Where(x => !x.IsSaBa).ToList();
    }

    public static bool IsSelectAllMatchType(List<string> matchType)
        => matchType.Exists(x => x == EventAssignmentConst.MatchReal)
           && matchType.Exists(x => x == EventAssignmentConst.MatchVirtual);

    public static List<EventModel> FilterEventModelBySyncLiveStatus(List<EventModel> events, int liveStatus)
    {
        if (liveStatus == (int)SyncLiveFilter.All)
        {
            return events;
        }

        return liveStatus == (int)SyncLiveFilter.Live
            ? events.Where(i => i.IsSyncLive).ToList()
            : events.Where(i => !i.IsSyncLive).ToList();
    }

    public List<EventModel> FilterEventModelByPermision(List<EventModel> events, EventAssignmentPermissionModel permission)
    {
        if (!permission.FullTimeView.IsRead)
        {
            events = FilterMatchesByTimeRange(events);

        }

        if (!permission.LiveGameAssignment_FullBetTypes.IsRead)
        {
            events.ForEach(eventModel =>
                           {
                               eventModel.Assignments.Handicap = new HandicapAssignmentModel();
                               eventModel.Assignments.Handicap1st = new Handicap1stAssignmentModel();
                               eventModel.Assignments.OverUnder = new OverUnderAssignmentModel();
                               eventModel.Assignments.OverUnder1st = new OverUnder1stAssignmentModel();
                               eventModel.Assignments.Specific10Mins = new Specific10MinsAssignmentModel();
                               eventModel.Assignments.Specific15Mins = new Specific15MinsAssignmentModel();
                           });
        }
        return events;
    }

    public List<EventModel> FilterMatchesByTimeRange(List<EventModel> matches)
    {
        var dateTime = DateTime.Now.ToGmt8();
        var timeRange = dateTime.TimeRangeOfDate();

        return matches.Where(m => timeRange.CheckMatch(m.EventDateTime).IsMatchInTimeRange)
            .ToList();
    }

    public List<EventModel> FilterEventModelByUser(List<EventModel> events, string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return events;
        }

        return events.Where(
            e =>
                e.Assignments.OneXTwo.MainTrader == username
                || e.Assignments.OneXTwo.OtherTraders.Contains('#' + username)
                || e.Assignments.FM.MainTrader == username
                || e.Assignments.FM.OtherTraders.Contains('#' + username)
                || e.Assignments.AIO.MainTrader == username
                || e.Assignments.AIO.OtherTraders.Contains('#' + username)
                || e.Assignments.Handicap.MainTrader == username
                || e.Assignments.Handicap.OtherTraders.Contains('#' + username)
                || e.Assignments.Handicap1st.MainTrader == username
                || e.Assignments.Handicap1st.OtherTraders.Contains('#' + username)
                || e.Assignments.OverUnder.MainTrader == username
                || e.Assignments.OverUnder.OtherTraders.Contains('#' + username)
                || e.Assignments.OverUnder1st.MainTrader == username
                || e.Assignments.OverUnder1st.OtherTraders.Contains('#' + username)
                || e.Assignments.OneXTwo1st.MainTrader == username
                || e.Assignments.OneXTwo1st.OtherTraders.Contains('#' + username)
                || e.Assignments.Specific15Mins.MainTrader == username
                || e.Assignments.Specific15Mins.OtherTraders.Contains('#' + username)
                || e.Assignments.Specific10Mins.MainTrader == username
                || e.Assignments.Specific10Mins.OtherTraders.Contains('#' + username)
                || e.Assignments.LA2nd.MainTrader == username
                || e.Assignments.LA2nd.OtherTraders.Contains('#' + username)).ToList();
    }

    private List<EventModel> FilterEventByOffVRs(List<EventModel> events, string offVRs)
    {
        if (string.IsNullOrEmpty(offVRs))
        {
            return events.ToList();
        }
        var offVRList = offVRs.Split(',').ToList();
        return events
            .Where(ev => offVRList.Exists(offVR => string.IsNullOrEmpty(offVR)
                ? ev.MatchOfferList.All(x => !x.MatchOfferValue.IsContains("OFF VR"))
                : ev.MatchOfferList.Any(x => offVR == x.Id.ToString())))
            .ToList();
    }
}
