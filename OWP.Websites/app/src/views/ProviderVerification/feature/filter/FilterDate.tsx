import { useRef } from 'react';
import { ReactDatePicker } from 'react-datepicker';
import DatePickerComponent from '../../../Shared/DatePickerComponent/DatePicker';
import { useAppDispatch, useAppSelector } from '../../app/hookNative';
import { updateFilter } from './FilterSlice';
import { buildDefaultFilterDateProviderVerification } from '../../../Shared/DateTime/dateTimeHelper';
import { getLSTBookProvider } from '../../feature/table/tableThunk';

const FilterDate = () => {
  const dispatch = useAppDispatch();
  const datePickerRef = useRef<ReactDatePicker>(null);
  const filterDate = useAppSelector(
    state => state.filterState.filter.eventDate
  );
  const maxDate = buildDefaultFilterDateProviderVerification();

  const onChangeFilterDate = (date: Date) => {
    const model = {
      key: 'eventDate',
      value: date
    };

    dispatch(getLSTBookProvider({ eventDate: date }));
    dispatch(updateFilter(model));
  };

  return (
    <DatePickerComponent
      maxDate={maxDate}
      selectedDate={filterDate}
      datePickerRef={datePickerRef}
      onChangeDate={onChangeFilterDate}
      name='Date'
    />
  );
};
export default FilterDate;
