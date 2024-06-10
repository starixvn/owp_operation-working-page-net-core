import dayjs from 'dayjs';
import { filterPrematchUpdated } from '../../helper/preMatchHelper';
import { getHourAndMinutes } from '../../../Shared/DateTime/dateTimeHelper';
import { isEqual } from 'lodash';
import { netCoreUrl, URL } from '../../const/const';
import localizedFormat from 'dayjs/plugin/localizedFormat';
dayjs.extend(localizedFormat);

const getPrematchsHaveAssignment = shifts =>
  shifts.filter(s => s.HDP || s.HDPTakeOver || s.OU || s.OUTakeOver);

const executeBuilder = props => {
  const { startTimeFilter, entities, startTimeOriginal, ...rest } = props;

  const postModel = {
    url: URL.SAVE_PREMATCH_ASSIGNMENT,
    NetCoreUrl: netCoreUrl,
    ...rest
  };

  let message = '';
  postModel.StartTime = getHourAndMinutes(startTimeFilter);
  const startTimeOriginalConverted = getHourAndMinutes(startTimeOriginal);

  const startTimeFilterConverted = getHourAndMinutes(startTimeFilter);
  const startTimeChange = !isEqual(
    startTimeOriginalConverted,
    startTimeFilterConverted
  );
  if (entities.length && startTimeChange) {
    message = 'Change start time may cause some loss in your data. Continue?';
  }
  return {
    postModel,
    message,
    startTimeChange
  };
};

const IsFilterStartTimeLowerThanOriginalStartTime = (
  filterStartTime,
  originalStartTime
) => {
  const twelvePm = dayjs('12:00 PM', 'LT');
  const startTimeOriginalToDateTime =
    originalStartTime >= twelvePm
      ? originalStartTime
      : originalStartTime.add(1, 'day');
  const filterStartTimeToDateTime =
    filterStartTime >= twelvePm
      ? filterStartTime
      : filterStartTime.add(1, 'day');

  return filterStartTimeToDateTime < startTimeOriginalToDateTime;
};

const buildModelNightShiftSaver = (dayShift, header, filter) => {
  const { startTimeOriginal, startTimeFilter } = header;
  const { entities, prematchIdDeleted, prematchIdUpdated } = dayShift;
  const { filterDate } = filter;

  const isFilterStartTimeLowerThanOriginalStartTime = IsFilterStartTimeLowerThanOriginalStartTime(
    dayjs(startTimeFilter, 'LT'),
    dayjs(startTimeOriginal, 'LT')
  );

  const PrematchUpdated =
    !startTimeOriginal || isFilterStartTimeLowerThanOriginalStartTime
      ? getPrematchsHaveAssignment(entities)
      : filterPrematchUpdated(entities, prematchIdUpdated);

  const props = {
    startTimeFilter,
    entities,
    startTimeOriginal,
    PrematchIdDeleted: prematchIdDeleted,
    PrematchUpdated,
    Date: filterDate
  };
  return executeBuilder(props);
};

export {
  getPrematchsHaveAssignment,
  executeBuilder,
  buildModelNightShiftSaver
};
