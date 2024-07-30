import { useCallback, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { updateFilterDate } from './filterSlice';
import {
  getShiftByDateAndEmployee,
  clearEntities
} from '../league/shiftSlice/shiftSlice';
import { updateStartTimeFilter } from '../header/headerSlice';
import { TimePicker } from 'antd';
import { URL_NIGHT_SHIFT } from '../../const/const';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { DateTimeFilterCommon } from '../../ImportPrematchHelperNative';
import CollapseBtn from '../expandCollapse/CollapseBtn';
import ExpandBtn from '../expandCollapse/ExpandBtn';
import ShowUnreadMessageCheckBox from '../expandCollapse/ShowUnreadMessageCheckBox';
import {
  getMMDDYYYYStringFromDateTime,
  getHourAndMinutes,
  getDayjsFromTime
} from '../../../Shared/DateTime/dateTimeHelper';

const format = 'h:mm A';

const EditFilterNightShift = () => {
  const filterDate = useSelector(state => state.filter.filterDate);
  const startTimeOriginal = useSelector(
    state => state.header.startTimeOriginal
  );
  const startTime = startTimeOriginal
    ? getDayjsFromTime(startTimeOriginal)
    : null;

  const permission = useSelector(state => state.policy.permission);
  const dispatch = useDispatch();
  const [startTimeSelected, setStartTimeSelected] = useState(startTime);
  const stableDispatch = useStableDispatch();

  const handleSubmit = useCallback(
    selectedDate => {
      const startTimePicker = startTimeSelected
        ? startTimeSelected.format('HH:mm')
        : '';
      const model = {
        EventDate: getMMDDYYYYStringFromDateTime(selectedDate),
        Employee: null,
        url: URL_NIGHT_SHIFT,
        ActorId: permission.ActorId,
        StartTime: getHourAndMinutes(startTimePicker) || ''
      };

      startTimePicker
        ? stableDispatch(getShiftByDateAndEmployee(model))
        : dispatch(clearEntities());
      dispatch(updateFilterDate(selectedDate));
      dispatch(updateStartTimeFilter(startTimePicker));
    },
    [dispatch, stableDispatch, startTimeSelected, permission.ActorId]
  );

  return (
    <div className='left-filter-dayshift edit-mode page-nightshift'>
      <div className='inner'>
        <DateTimeFilterCommon
          defaultDate={new Date(filterDate)}
          onSubmit={handleSubmit}
          isEdit>
          <div className='filter-control'>
            <div className='label'>Start time(GMT+8)</div>
            <TimePicker
              inputReadOnly
              className='input-field'
              id='startTime'
              use12Hours
              defaultValue={startTime}
              onSelect={value => setStartTimeSelected(value)}
              value={startTimeSelected}
              format={format}
              allowClear={false}
              showNow={false}
            />
          </div>
        </DateTimeFilterCommon>
        <CollapseBtn />
        <ExpandBtn />
      </div>
      <ShowUnreadMessageCheckBox />
    </div>
  );
};

export default EditFilterNightShift;
