import dayjs from 'dayjs';
import { getCurrentDateGmtPlus8 } from '../../Shared/DateTime/dateTimeHelper';
import { DataSource } from '../type/TTaskListTable';
import { timeLine } from '../features/const/TableSchema.json';

const getHeaderDateTime = () => {
  const today = getToday();
  const yesterday = dayjs(today)
    .add(-1, 'days')
    .toDate();
  return { today, yesterday };
};

const getToday = () => {
  const currentDay = getCurrentDateGmtPlus8();
  return currentDay.getHours() < timeLine
    ? dayjs(currentDay)
        .add(-1, 'days')
        .toDate()
    : currentDay;
};

const filterMatchesByShowUnreadMsg = (
  matches: Array<DataSource>,
  isUnRead: boolean
) => matches.filter(x => !x.IsRead || !isUnRead);

const filterMatchesByHideCompletedMatches = (
  matches: Array<DataSource>,
  isHideCompleted: boolean
) => matches.filter(x => !x.IsCompletedMatch || !isHideCompleted);

const filterMatchByCriteria = (matches: Array<DataSource>) => (
  isUnRead: boolean
) => {
  const matchFilter = filterMatchesByShowUnreadMsg(matches, isUnRead);
  return (isHide: boolean) =>
    filterMatchesByHideCompletedMatches(matchFilter, isHide);
};

export {
  getHeaderDateTime,
  filterMatchesByShowUnreadMsg,
  filterMatchesByHideCompletedMatches,
  filterMatchByCriteria,
  getToday
};
