import { useState, useRef } from 'react';
import DatePicker from 'react-datepicker';
import CalendarIcon from 'mdi-react/CalendarIcon';
import { getDefaultDateRange } from '../../../Helper/TicketCenterHelper';
import { useAppDispatch } from '../../../app/hooksNative';
import store from '../../../app/storeNative';
import { setFilterTimeRange } from './filterSlice';

const { minDate, toDateDefault } = getDefaultDateRange();

const DateRangeFilter = () => {
  const dispatch= useAppDispatch();
  const {filterInfo: {fromDateFilter, toDateFilter}}= store.getState().filter.entities;
  const [fromDate, setFromDate] = useState(fromDateFilter);
  const [toDate, setToDate] = useState(toDateFilter);

  const toDateRef = useRef<DatePicker>(null);
  const fromDateRef = useRef<DatePicker>(null);

  const handleChangeFromDate = (date: Date) => {
    setFromDate(date);
    dispatch(setFilterTimeRange( {key:'fromDateFilter', value: date}));
  };

  const handleChangeToDate = (date: Date) => {
    setToDate(date);
    dispatch(setFilterTimeRange( {key:'toDateFilter', value: date}));
  };

  return (
    <>
      <div className='calendar'>
        <div className='label'>From Date</div>
        <DatePicker
          maxDate={toDate}
          minDate={minDate}
          selected={fromDate}
          dateFormat='MMM dd, yyyy'
          ref={fromDateRef}
          onChange={handleChangeFromDate}
        />
        <span
          className='icon-calendar'
          onClick={() => fromDateRef?.current?.setOpen(true)}>
          <CalendarIcon size={14} />
        </span>
      </div>
      <div className='calendar'>
        <div className='label'>To Date</div>
        <DatePicker
          maxDate={toDateDefault}
          minDate={fromDate}
          selected={toDate}
          dateFormat='MMM dd, yyyy'
          ref={toDateRef}
          onChange={handleChangeToDate}
        />
        <span
          className='icon-calendar'
          onClick={() => toDateRef?.current?.setOpen(true)}>
          <CalendarIcon size={14} />
        </span>
      </div>
    </>
  );
};

export default DateRangeFilter;
