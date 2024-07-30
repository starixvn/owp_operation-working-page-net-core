import { useState } from 'react';
import { TimePicker } from 'antd';
import { ITimeRange } from '../../typing/type';
import dayjs from 'dayjs';
import constant from '../../osLeague.json';
import ErrorMessage from './ErrorMessage';
import useStore from '../../app/store';

const { fromTime, toTime } = constant.timeRange;

const KORangeTime = (props: ITimeRange) => {
  const { koFromTime, koToTime, onChange, disabled } = props;
  const errorMessage = useStore(state => state.errorMessage.rangeTime);
  const [timeRange, setTimeRange] = useState({
    [fromTime]: koFromTime ? dayjs(koFromTime, 'hh:mm') : null,
    [toTime]: koToTime ? dayjs(koToTime, 'hh:mm') : null
  });

  const handleChange = (key: Required<string>, time: dayjs.Dayjs | null) => {
    setTimeRange({
      ...timeRange,
      [key]: time
    });
    const timeSender = time ? time.format('h:mm A') : null;
    onChange(key, timeSender);
  };

  return (
    <div>
      <label className='range-time'>KO range time (GMT+8)</label>
      <div className='time-range'>
        <span>From</span>
        <TimePicker
          use12Hours
          format='h:mm A'
          className='from-time'
          value={timeRange[fromTime]}
          disabled={disabled}
          onChange={time => handleChange(fromTime, time)}
          onSelect={time => handleChange(fromTime, time)}
        />
        <span>To</span>
        <TimePicker
          use12Hours
          format='h:mm A'
          className='to-time'
          value={timeRange[toTime]}
          disabled={disabled}
          onChange={time => handleChange(toTime, time)}
          onSelect={time => handleChange(toTime, time)}
        />
      </div>
      <ErrorMessage message={errorMessage}></ErrorMessage>
    </div>
  );
};

export default KORangeTime;
