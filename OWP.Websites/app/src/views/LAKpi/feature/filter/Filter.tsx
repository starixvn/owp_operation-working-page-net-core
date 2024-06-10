import { useEffect, useRef, useState } from 'react';
import DatePicker, { ReactDatePicker } from 'react-datepicker';
import CalendarIcon from 'mdi-react/CalendarIcon';
import ToastrWrapper from '../../../components/ToastrWrapper';
import {
  initDefaultFilterDate,
  getDaysInMonth,
  isValidTimeRange
} from '../../Utils';
import update from 'immutability-helper';
import useStore from '../../app/store';
import InfoConditionExplainIcon from '../tableConditionConfig/InfoConditionExplainIcon';
import EditTarget from './EditTarget';
import SectionOption from './SectionOption';
import { buildSectionInString } from '../../helper/LaKpiHelper';
import UserStatus from './UserStatus';

const { defaultFromDate, defaultToDate } = initDefaultFilterDate();

const Filter = () => {
  const fromDatePicker = useRef<ReactDatePicker>(null);
  const toDatePicker = useRef<ReactDatePicker>(null);
  const queryConditionData = useStore(state => state.queryConditionData);
  const exportDataTable = useStore(state => state.exportDataTable);
  const queryLevelOption = useStore(state => state.queryLevelOption);
  const queryDataTable = useStore(state => state.queryDataTable);
  const sectionSelected = useStore(state => state.sectionSelected);
  const userStatusSelected = useStore(state => state.userStatusSelected);

  const [filterDate, setFilterDate] = useState({
    fromDate: defaultFromDate,
    toDate: defaultToDate
  });

  const handleExport = () => {
    if (isValidTimeRange(filterDate.fromDate, filterDate.toDate)) {
      ToastrWrapper.showError(<h4>Time Range is only within 2 years</h4>);
      return;
    }
    if (!sectionSelected.length) {
      ToastrWrapper.showError('Please select at least one section');
      return;
    }
    exportDataTable(
      filterDate.fromDate,
      filterDate.toDate,
      buildSectionInString(sectionSelected),
      userStatusSelected.some(s => s.value === 'active'),
      userStatusSelected.some(s => s.value === 'resigned')
    );
  };

  const onFromDateChanged = (date: Date) => {
    date.setDate(1);
    const newDate = update(filterDate, {
      fromDate: { $set: date }
    });
    setFilterDate(newDate);
  };

  const onToDateChanged = (date: Date) => {
    date.setDate(getDaysInMonth(date));
    const newDate = update(filterDate, {
      toDate: { $set: date }
    });
    setFilterDate(newDate);
  };

  const handleOnSearch = () => {
    if (isValidTimeRange(filterDate.fromDate, filterDate.toDate)) {
      ToastrWrapper.showError(<h4>Time Range is only within 2 years</h4>);
      return;
    }
    if (!sectionSelected.length) {
      ToastrWrapper.showError('Please select at least one section');
      return;
    }
    if (!userStatusSelected.length) {
      ToastrWrapper.showError('Please select at least one Account Type');
      return;
    }
    queryDataTable(
      filterDate.fromDate,
      filterDate.toDate,
      buildSectionInString(sectionSelected),
      userStatusSelected.some(s => s.value === 'active'),
      userStatusSelected.some(s => s.value === 'resigned')
    );
  };

  useEffect(() => {
    sectionSelected.length && queryDataTable(
      filterDate.fromDate,
      filterDate.toDate,
      buildSectionInString(sectionSelected),
      userStatusSelected.some(s => s.value === 'active'),
      userStatusSelected.some(s => s.value === 'resigned')
    );
    queryConditionData();
    queryLevelOption();
  }, []);

  return (
    <div className='top-control'>
      <div className='left-control'>
        <div className='filter-control calendar'>
          <div className='label'>From Date</div>
          <DatePicker
            selected={filterDate.fromDate}
            showMonthYearPicker
            dateFormat='MMM yyyy'
            ref={fromDatePicker}
            className='to-date-picker'
            onChange={onFromDateChanged}
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
            selected={filterDate.toDate}
            showMonthYearPicker
            dateFormat='MMM yyyy'
            ref={toDatePicker}
            className='to-date-picker'
            onChange={onToDateChanged}
          />
          <span
            className='icon-calendar'
            id='icon-calendar-to-date'
            onClick={() => toDatePicker?.current?.setOpen(true)}>
            <CalendarIcon size={14} />
          </span>
        </div>
        <div className='filter-control'>
          <SectionOption />
        </div>
        <div className='filter-control'>
          <UserStatus />
        </div>
        <div className='filter-control btn-block'>
          <button className='button-submit' onClick={handleOnSearch}>
            Search
          </button>
          <button className='button-export' onClick={handleExport}>
            Export
          </button>
        </div>
        <div className='right-control icon-condition'>
          <EditTarget />
          <InfoConditionExplainIcon />
        </div>
      </div>
    </div>
  );
};

export default Filter;
