import { TFilter } from '../type/type';
import Utils from '../../Shared/Utils';
import dayjs from 'dayjs';
import { baseApiUrl } from '../../Shared/Constants';

export const createModel = (eventModel: TFilter) => {
  return {
    EventDate: Utils.formatDate(eventModel.selectedDate),
    Username: eventModel.userNames || '',
    MatchStatusIds: eventModel.selectedMatchStatus.map(x => x.value) || [],
    OffVRIds: eventModel.selectedOffVRs.map(x => x.value) || [],
    MatchType: eventModel.selectedMatchType.map(x => x.value).join(',') || [],
    SyncLiveStatus: eventModel.selectedSyncLive.value
  };
};

export const liveGameAssignmentUrl = (eventModel: TFilter) => {
  const {
    EventDate,
    Username,
    MatchStatusIds,
    OffVRIds,
    SyncLiveStatus,
    MatchType
  } = createModel(eventModel);
  const params = `date=${EventDate}&userName=${Username}&matchStatusIds=${MatchStatusIds.toString()}&lastUpdated=${''}&syncLiveStatus=${SyncLiveStatus}&matchType=${MatchType}&offVRs=${OffVRIds.toString()}`;
  const url = `/export/download-live-game?${params}`;
  return url;
};

export const eventPickingOldUrl = (eventModel: TFilter) => {
  const { selectedDate } = eventModel;
  return `/event-picking-old/sync-event-picking?date=${Utils.formatDate(
    selectedDate
  )}`;
};

export const micsMatchUrl = (eventModel: TFilter) => {
  const { selectedDate } = eventModel;
  return `/event-picking-old/sync-event-picking?date=${Utils.formatDate(
    selectedDate
  )}`;
};

export const eventPickingListUrlNew = (eventModel: TFilter) => {
  const { selectedDate } = eventModel;
  const url = `${baseApiUrl}/api/EventPicking/export-misc-match`;
  return `${url}?date=${Utils.formatDate(selectedDate)}`;
};

export const eventPickingListFileName = (date: Date) =>
  `Event Picking-${dayjs(Utils.formatDate(date)).format('YYYY-MM-DD')}.xlsx`;

export const liveGameAssignmentFileName = (date: Date) =>
  `Assignment - ${dayjs(Utils.formatDate(date)).format('YYYYMMDD')}.xlsx`;
