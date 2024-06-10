import { useState, useRef } from 'react';
import PropTypes from 'prop-types';
import DatePicker from 'react-datepicker';
import CalendarIcon from 'mdi-react/CalendarIcon';
import { getDefaultDateRange } from '../../../Helper/TicketCenterHelper';
import Utils from '../../../../Shared/Utils';
const propTypes = {
  updateFilter: PropTypes.func.isRequired
};

const DateRangeFilter = ({ updateFilter }) => {
  const { fromDateDefault, toDateDefault, minDate } = getDefaultDateRange();
  const [toDateFilter, setToDateFilter] = useState(toDateDefault);
  const [fromDateFilter, setFromDateFilter] = useState(fromDateDefault);

  const toDatePicker = useRef(null);
  const fromDatePicker = useRef(null);

  const handleChangeFromDate = date => {
    setFromDateFilter(date);
    updateFilter('FromDateFilter', Utils.formatDate(date));
  };

  const handleChangeToDate = date => {
    setToDateFilter(date);
    updateFilter('ToDateFilter', Utils.formatDate(date));
  };

  return (
    <>
      <div className='calendar'>
        <div className='label'>From Date</div>
        <DatePicker
          maxDate={toDateFilter}
          minDate={minDate}
          selected={fromDateFilter}
          dateFormat='MMM dd, yyyy'
          ref={fromDatePicker}
          onChange={handleChangeFromDate}
        />
        <span
          className='icon-calendar'
          onClick={() => fromDatePicker.current.setOpen(true)}>
          <CalendarIcon size={14} />
        </span>
      </div>
      <div className='calendar'>
        <div className='label'>To Date</div>
        <DatePicker
          maxDate={toDateDefault}
          minDate={fromDateFilter}
          selected={toDateFilter}
          dateFormat='MMM dd, yyyy'
          ref={toDatePicker}
          onChange={handleChangeToDate}
        />
        <span
          className='icon-calendar'
          onClick={() => toDatePicker.current.setOpen(true)}>
          <CalendarIcon size={14} />
        </span>
      </div>
    </>
  );
};

DateRangeFilter.propTypes = propTypes;
export default DateRangeFilter;
