using OWP.Event.LiveGameAssignment.Download.Models;
using OWP.Extensions;

namespace OWP.Event.LiveGameAssignment.Download.Service;

using OWP.Event.EventShared.Enum;
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
        var dateTime = DateTime.Now.ToGmt8(); // NOSONAR
        var timeRange = dateTime.TimeRangeOfDate();

        return matches.Where(m => timeRange.CheckMatch(m.EventDateTime).IsMatchInTimeRange).ToList();
    }

    public static List<EventModel> FilterEventModelByUser(List<EventModel> events, string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return events;
        }

        return events
            .Where(e => AssignmentContainsTrader(e.Assignments, username))
            .ToList();
    }

    private static bool AssignmentContainsTrader(AssignmentModel assignments, string username)
    {
        foreach (var property in typeof(AssignmentModel).GetProperties())
        {
            var assignmentItem = property.GetValue(assignments);
            if (assignmentItem is int)
            {
                continue;
            }
            var mainTraderValue = GetPropertyValue(assignmentItem, "MainTrader")?.ToString();
            var otherTradersValue = GetPropertyValue(assignmentItem, "OtherTraders")?.ToString();

            if (mainTraderValue == username || otherTradersValue?.Contains('#' + username) == true)
            {
                return true;
            }
        }

        return false;
    }

    private static object GetPropertyValue(object obj, string propertyName)
    {
        return obj.GetType().GetProperty(propertyName)?.GetValue(obj);
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
