import dayjs from 'dayjs';
import { timeLineColumn, startHour, startMinute } from '../../const/Const.json';
import {
  OVER_TIME_OPTIONS_VALUE,
  TAssigneeSuggestion,
  TMatchInfo,
  UserInfo,
  UserWorkingSchedule
} from '../TypeNative';


const getCurrentTimeLine = (filterDate: Date, koTime: Date) => {
  const currentDate = dayjs(koTime);
  const timeStartDefault = dayjs(filterDate)
    .set('hour', startHour)
    .set('minute', startMinute);
  const hourDiff = currentDate.diff(timeStartDefault, 'hour');
  return timeLineColumn.find(x => x.timeRange === hourDiff);
};

const getTotalRecords = ({
  fullyAvailable,
  partiallyAvailable,
  busy
}: TAssigneeSuggestion) =>
  fullyAvailable.length + partiallyAvailable.length + busy.length;

const searchByAssignee = (userInfo: UserInfo, keyword: string) => {

  const normalizedKeyword = keyword.toLowerCase().trim();
  return [
    userInfo.username,
    userInfo.levelLabel,
    userInfo.mainBetType,
    userInfo.maxMultipleGames
  ].some(prop => prop?.toLowerCase().includes(normalizedKeyword));
};

const searchByOtValue = (userInfo: UserInfo, otValue: OVER_TIME_OPTIONS_VALUE) => ({
  [OVER_TIME_OPTIONS_VALUE.ALL]: true,
  [OVER_TIME_OPTIONS_VALUE.OVERTIME_PRE_SHIFT]: userInfo.otPreShift,
  [OVER_TIME_OPTIONS_VALUE.OVERTIME_POST_SHIFT]: userInfo.otPostShift,
  [OVER_TIME_OPTIONS_VALUE.NO_OVERTIME]: !userInfo.otPreShift && !userInfo.otPostShift
}[otValue]);

const searchByAssigneeAndOtValue = (assignees: UserWorkingSchedule[], keyword: string, otValue: OVER_TIME_OPTIONS_VALUE) =>
  assignees.filter(assignee => assignee.userInfo && searchByAssignee(assignee.userInfo, keyword) && searchByOtValue(assignee.userInfo, otValue)
  );

const isShowPartially = (matchInfo: TMatchInfo) =>
  !matchInfo.leagueName.includes('SABA ');

const executeFilter = (
  data: TAssigneeSuggestion,
  keyword: string,
  otValue: OVER_TIME_OPTIONS_VALUE
) => {
  return {
    ...data,
    fullyAvailable: searchByAssigneeAndOtValue(data.fullyAvailable, keyword, otValue),
    partiallyAvailable: searchByAssigneeAndOtValue(data.partiallyAvailable, keyword, otValue),
    busy: searchByAssigneeAndOtValue(data.busy, keyword, otValue)
  };
};

export {
  getCurrentTimeLine,
  getTotalRecords,
  executeFilter,
  isShowPartially
};
