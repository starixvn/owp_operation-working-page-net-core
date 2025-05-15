import { useRef, useState } from 'react';
import DatePicker, { ReactDatePicker } from 'react-datepicker';
import CalendarIcon from 'mdi-react/CalendarIcon';
import { useDispatch } from 'react-redux';
import { getReportData } from '../table/reportThunkNative';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { displayTable } from '../table/reportSlice';
import {
  buildRequestURL,
  validDateRange,
  toDateDefault,
  fromDateDefault
} from '../../helper/helper';
import { exportReport } from '../../Const.json';
import dayjs from 'dayjs';

const Filter = () => {
  const dispatch = useDispatch();
  const toDatePicker = useRef<ReactDatePicker>(null);
  const fromDatePicker = useRef<ReactDatePicker>(null);

  const [fromDate, setFromDate] = useState<Date>(fromDateDefault);
  const [toDate, setToDate] = useState<Date>(toDateDefault);

  const handleSearch = () => {
    if (validDateRange(fromDate, toDate)) {
      ToastrWrapper.showError(
        <h4>Please select date range within 365 days</h4>
      );
      return;
    }
    dispatch(
      getReportData({
        fromDate,
        toDate
      })
    );
    dispatch(displayTable(true));
  };

  const handleExport = () => {
    const url = buildRequestURL(exportReport);
    const fromDateFormatted = dayjs(fromDate).format('YYYY-MM-DD');
    const toDateFormatted = dayjs(toDate).format('YYYY-MM-DD');
    window.location.href = `${url}?FromDate=${fromDateFormatted}&ToDate=${toDateFormatted}`;
  };

  return (
    <div className='top-control'>
      <div className='left-control'>
        <div className='filter-control calendar'>
          <div className='label'>From Date</div>
          <DatePicker
            onChange={date => date && setFromDate(date)}
            selected={fromDate}
            maxDate={toDate}
            dateFormat='MMM dd, yyyy'
            ref={fromDatePicker}
            className='from-date-picker'
          />
          <span
            className='icon-calendar'
            id='icon-calendar-from-date'
            onClick={() => fromDatePicker?.current?.setOpen(true)}>
            <CalendarIcon size={14} />
          </span>
        </div>
        <div className='filter-control calendar'>
          <div className='label'>To Date</div>
          <DatePicker
            onChange={date => date && setToDate(date)}
            minDate={fromDate}
            selected={toDate}
            dateFormat='MMM dd, yyyy'
            ref={toDatePicker}
            className='to-date-picker'
          />
          <span
            className='icon-calendar'
            id='icon-calendar-to-date'
            onClick={() => toDatePicker?.current?.setOpen(true)}>
            <CalendarIcon size={14} />
          </span>
        </div>
        <div className='filter-control btn-block'>
          <button className='button-submit' onClick={handleSearch}>
            Search
          </button>
          <button className='button-export' onClick={handleExport}>
            Export
          </button>
        </div>
      </div>
    </div>
  );
};

export default Filter;
