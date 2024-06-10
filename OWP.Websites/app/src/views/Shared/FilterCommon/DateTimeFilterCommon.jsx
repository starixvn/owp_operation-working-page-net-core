import { useState, createRef } from 'react';
import CalendarIcon from 'mdi-react/CalendarIcon';
import DatePicker from 'react-datepicker';
import PropTypes from 'prop-types';
import { buildTimeRangeLast30Day } from '../DateTime/dateTimeHelper';
const propTypes = {
  defaultDate: PropTypes.object.isRequired,
  isEdit: PropTypes.bool,
  isHideButtonSubmit: PropTypes.bool,
  onSubmit: PropTypes.func,
  children: PropTypes.node
};

const DateTimeFilterCommon = props => {
  const datePicker = createRef();
  const { defaultDate, isEdit, onSubmit, children, isHideButtonSubmit } = props;
  const [selectedDate, setSelectedDate] = useState(defaultDate);

  return (
    <div className='filter pre-match'>
      <div className='filter-control calendar'>
        <div className='label'>Date</div>
        <DatePicker
          disabled={isEdit}
          minDate={buildTimeRangeLast30Day(new Date())}
          selected={selectedDate}
          onChange={date => setSelectedDate(date)}
          dateFormat='MMM dd, yyyy'
          ref={datePicker}
        />
        {!isEdit && (
          <span
            className='icon-calendar'
            onClick={() => datePicker.current.setOpen(true)}>
            <CalendarIcon size={14} />
          </span>
        )}
      </div>
      {children}
      <div className='filter-control btn-block'>
        {!isHideButtonSubmit && (
          <button
            className='button-submit'
            onClick={() => onSubmit(selectedDate)}>
            Submit
          </button>
        )}
      </div>
    </div>
  );
};

DateTimeFilterCommon.propTypes = propTypes;
export default DateTimeFilterCommon;
