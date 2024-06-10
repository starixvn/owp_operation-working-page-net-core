import { useRef, useState } from 'react';
import DatePicker, { ReactDatePicker } from 'react-datepicker';
import CalendarIcon from 'mdi-react/CalendarIcon';
import { Input } from 'reactstrap';
import { useDispatch } from 'react-redux';
import { getReportData } from '../table/reportThunkNative';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { displayTable } from '../table/reportSlice';
import {
  buildRequestURL,
  isValidTimeRange,
  toDateDefault,
  fromDateDefault
} from '../helpers/helpers';
import { exportRematchReport } from '../../Const.json';
import dayjs from 'dayjs';

const Filter = () => {
  const toDatePicker = useRef<ReactDatePicker>(null);
  const fromDatePicker = useRef<ReactDatePicker>(null);

  const [fromDate, setFromDate] = useState<Date>(fromDateDefault);
  const [toDate, setToDate] = useState<Date>(toDateDefault);
  const [trader, setTrader] = useState('');

  const dispatch = useDispatch();

  const handleSearch = () => {
    if (!isValidTimeRange(fromDate, toDate)) {
      ToastrWrapper.showError(
        <h4>Please select date range within 366 days</h4>
      );
      return;
    }
    dispatch(displayTable(true));
    dispatch(
      getReportData({
        fromDate,
        toDate,
        keyWord: trader
      })
    );
  };

  const handleExport = () => {
    const url = buildRequestURL(exportRematchReport);
    const formattedFromDate = dayjs(fromDate).format('YYYY-MM-DD');
    const formattedToDateFormatted = dayjs(toDate).format('YYYY-MM-DD');
    window.location.href = `${url}?FromDate=${formattedFromDate}&ToDate=${formattedToDateFormatted}&KeyWord=${trader}`;
  };

  return (
    <>
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
              className='to-date'
            />
            <span
              className='icon-calendar'
              id='icon-calendar-to-date'
              onClick={() => toDatePicker?.current?.setOpen(true)}>
              <CalendarIcon size={14} />
            </span>
          </div>
          <div className='filter-control'>
            <div className='label'>Trader</div>
            <Input
              type='text'
              className='input-field'
              value={trader}
              onChange={e => setTrader(e.target.value)}
              placeholder='Username or Nickname'
            />
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
    </>
  );
};

export default Filter;
