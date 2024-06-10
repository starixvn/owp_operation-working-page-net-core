import { useState } from 'react';
import { useSelector } from 'react-redux';
import { Input } from 'reactstrap';
import { TimePicker } from 'antd';
import PropTypes from 'prop-types';

import {
  unShowLeaguesWithTimeCondition,
  showLeaguesWithTimeCondition
} from '../../features/league/shiftSlice/shiftSlice';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import {
  getDefaultTimeCondition,
  saveTimeConditionToLocalStorage
} from '../../helper/preMatchHelper';

const propTypes = {
  isNightShift: PropTypes.bool
};
const format = 'h:mm A';
const ShowLeague = props => {
  const { isNightShift } = props;
  const stableDispatch = useStableDispatch();

  const { filterDate } = useSelector(state => state.filter);

  const isShow = useSelector(
    state => state.dayShift?.leagueWithTimeCondition.isShow
  );

  const defaultTime = getDefaultTimeCondition(isNightShift);

  const [startTimeSelected, setStartTimeSelected] = useState(defaultTime);

  const handleClick = () => {
    const datetime = getDateTimeCondition();
    isShow
      ? stableDispatch(unShowLeaguesWithTimeCondition())
      : stableDispatch(
          showLeaguesWithTimeCondition({ datetime, isNightShift })
        );
  };

  const handleOnBlur = () => {
    saveTimeConditionToLocalStorage(isNightShift, startTimeSelected);
    if (isShow === true) {
      const datetime = getDateTimeCondition();
      stableDispatch(showLeaguesWithTimeCondition({ datetime, isNightShift }));
    }
  };

  const getDateTimeCondition = () => {
    const timeArray = startTimeSelected.format('HH:mm').split(':');
    const date = new Date(Date.parse(filterDate));
    date.setHours(timeArray[0], timeArray[1]);
    if (date.getHours() >= 0 && date.getHours() < 12) {
      date.setDate(date.getDate() + 1);
    }

    return date;
  };

  return (
    <div className='checkbox show-league'>
      <Input
        id='prematch-filter-time-condition-message'
        type='checkbox'
        defaultChecked={isShow}
        className='form-check-input'
        onClick={handleClick}
      />
      <label
        className='form-check-label'
        htmlFor='prematch-filter-time-condition-message'>
        Show league
      </label>
      <label className='label-after-before'>
        {isNightShift ? 'after' : 'before'}
      </label>
      <TimePicker
        inputReadOnly
        className='input-field'
        id='startTime'
        use12Hours
        onSelect={value => setStartTimeSelected(value)}
        value={startTimeSelected}
        format={format}
        allowClear={false}
        showNow={false}
        onBlur={handleOnBlur}
      />
    </div>
  );
};

ShowLeague.propTypes = propTypes;
export default ShowLeague;
