import { createRef } from 'react';
import DatePicker, { ReactDatePicker } from 'react-datepicker';
import CalendarIcon from 'mdi-react/CalendarIcon';
import { buildTimeRangeLast30Day } from '../../Shared/DateTime/dateTimeHelper';
import { useAppDispatch } from '../app/hooks';
import { updateFilterState } from './filterSlice';
import { useSelector } from 'react-redux';
import { RootState } from '../app/store';
import Utils from '../../Shared/Utils';
import { TOptions } from '../type/type';

const DatePickerFilter = () => {
  const dispatch = useAppDispatch();
  const { filterState, filterOptions } = useSelector(
    (state: RootState) => state.filter
  );
  const { MatchStatus } = filterOptions;
  const { selectedDate } = filterState;
  const datePicker = createRef<ReactDatePicker>();
  const Latest30Days = buildTimeRangeLast30Day(new Date());
  const openCalendar = () => {
    datePicker.current && datePicker.current.setOpen(true);
  };
  const handleChangeDate = (date: Date) => {
    const model = {
      key: 'selectedDate',
      value: date
    };
    const matchStatusOptions: TOptions[] =
      Utils.buildMatchStatusOption(MatchStatus);
    const selectedMatchStatus = Utils.handleOnDateChange(
      matchStatusOptions,
      date
    );
    const selectedMatchStatusModel = {
      key: 'selectedMatchStatus',
      value: selectedMatchStatus
    };
    dispatch(updateFilterState(model));
    dispatch(updateFilterState(selectedMatchStatusModel));
  };
  return (
    <div className='filter-control calendar'>
      <div className='label'>Date</div>
      <DatePicker
        data-testid='contextMenu-testId'
        minDate={Latest30Days}
        selected={selectedDate}
        onChange={handleChangeDate}
        dateFormat='MMM dd, yyyy'
        ref={datePicker}
      />
      <span className='icon-calendar' onClick={openCalendar}>
        <CalendarIcon size={14} />
      </span>
    </div>
  );
};
export default DatePickerFilter;
