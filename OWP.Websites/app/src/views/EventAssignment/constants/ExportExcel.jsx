import dayjs from 'dayjs';

export const exportExcelTypes = {
  EventPickingList: 'EventPickingList',
  EventPickingListNew: 'EventPickingList_New',
  LiveGameAssignment: 'LiveGameAssignment'
};
export const eventPickingListFileName = date =>
  `Event Picking-${dayjs(date).format('YYYY-MM-DD')}.xlsx`;

export const liveGameAssignmentFileName = date =>
  `Assignment - ${dayjs(date).format('YYYYMMDD')}.xlsx`;

export const liveGameAssignmentUrl = eventModel => {
  const {
    EventDate,
    Username,
    MatchStatusIds,
    OffVRIds,
    LastUpdated,
    SyncLiveStatus,
    MatchType
  } = eventModel;
  const params = `date=${EventDate}&userName=${Username}&matchStatusIds=${MatchStatusIds.toString()}&lastUpdated=${LastUpdated}&syncLiveStatus=${SyncLiveStatus}&matchType=${MatchType}&offVRs=${OffVRIds.toString()}`;
  const url = `TaskScheduling/EventAssignment/DownloadLiveGameAssignment/?${params}`;
  return url;
};

export const eventPickingListUrl = eventModel => {
  const { EventDate } = eventModel;
  const url = `TaskScheduling/EventPicking/SyncedEventPickingExcel/?date=${EventDate}`;
  return url;
};

export const eventPickingListUrlNew = eventModel => {
  const { EventDate } = eventModel;
  const url =
    process.env.NODE_ENV === 'development'
      ? `api-netcore/EventPicking/export-misc-match`
      : `${window.netCoreRootUrl}/api/EventPicking/export-misc-match`;
  return `${url}?date=${EventDate}`;
};
