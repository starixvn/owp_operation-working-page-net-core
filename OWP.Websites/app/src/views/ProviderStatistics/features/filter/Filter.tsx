import { useRef, useState } from 'react';
import dayjs from 'dayjs';
import Select from 'react-select';
import DatePicker, { ReactDatePicker } from 'react-datepicker';
import CalendarIcon from 'mdi-react/CalendarIcon';
import ToastrWrapper from '../../../components/ToastrWrapper';
import {
  buildRequestURL,
  validDateRange,
  toDateDefault,
  fromDateDefault
} from '../../helper/helper';
import { VIEW_BY_OPTION } from '../../Const.json';
import { Option } from 'react-multi-select-component';
import Utils from '../../../Shared/Utils';
import { exportExcel } from '../../../Shared/common/ExportExcel/Helper';
import { buildDefaultFilterDateProviderVerification } from '../../../Shared/DateTime/dateTimeHelper';

const Filter = () => {
  const toDatePicker = useRef<ReactDatePicker>(null);
  const fromDatePicker = useRef<ReactDatePicker>(null);

  const [fromDate, setFromDate] = useState<Date>(fromDateDefault);
  const [toDate, setToDate] = useState<Date>(toDateDefault);
  const [viewBy, setViewBy] = useState<Option>(VIEW_BY_OPTION[0]);

  const isByDayView = (viewByValue: string): boolean =>
    viewByValue === VIEW_BY_OPTION[0].value;

  const getExportURL = (viewByValue: string): string =>
    buildRequestURL(isByDayView(viewByValue) ? 'export/by-day' : 'export/by-league');

  const getExportParams = (fromDate: Date, toDate: Date): string =>
    new URLSearchParams({
      fromDate: Utils.formatDate(fromDate),
      toDate: Utils.formatDate(toDate)
    }).toString();

  const getExportFileName = (viewByValue: string, fromDate: Date, toDate: Date): string => {
    const formattedFrom = dayjs(fromDate).format('YYYY-MM-DD');
    const formattedTo = dayjs(toDate).format('YYYY-MM-DD');

    return isByDayView(viewByValue)
      ? `Monthly_Total_Summaries_${formattedFrom}_to_${formattedTo}.xlsx`
      : `Soccer_Provider_Usage_Summaries_By_Leagues_${formattedFrom}_to_${formattedTo}.xlsx`;
  };

  const handleExport = () => {
    if (validDateRange(fromDate, toDate)) {
      ToastrWrapper.showError(
        <h4>Please select date range within 365 days</h4>
      );
      return;
    }

    const url = getExportURL(viewBy.value);
    const params = getExportParams(fromDate, toDate);
    const fileName = getExportFileName(viewBy.value, fromDate, toDate);

    exportExcel(`${url}?${params}`, fileName);
  };

  const maxDate = buildDefaultFilterDateProviderVerification();

  return (
    <div className='top-control'>
      <div className='left-control'>
        <div className='filter-control multiselect sync-live'>
          <div className='label'>View By</div>
          <Select
            defaultValue={viewBy}
            className='basic-multi-select'
            classNamePrefix='select'
            options={VIEW_BY_OPTION}
            onChange={(value: Option) => setViewBy(value)}
          />
        </div>
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
            maxDate={toDate}
            selected={maxDate}
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
          <button className='button-export' onClick={handleExport}>
            Export
          </button>
        </div>
      </div>
    </div>
  );
};

export default Filter;
