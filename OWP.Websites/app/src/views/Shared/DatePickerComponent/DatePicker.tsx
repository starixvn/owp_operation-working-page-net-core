import DatePicker from 'react-datepicker';
import CalendarIcon from 'mdi-react/CalendarIcon';

type DatePickerProps = {
  selectedDate: Date;
  datePickerRef: React.RefObject<DatePicker<never, undefined>>;
  onChangeDate: (date: Date) => void;
  name: string;
  isShowMonthYearPicker?: boolean;
  dateFormat?: string;
  maxDate?: Date;
};

const DatePickerComponent = (props: DatePickerProps) => {
  const {
    selectedDate,
    datePickerRef,
    onChangeDate,
    name,
    isShowMonthYearPicker,
    dateFormat,
    maxDate
  } = props;
  return (
    <div className='filter-control calendar'>
      <div className='label'>{name}</div>
      <DatePicker
        maxDate={maxDate}
        selected={selectedDate}
        showMonthYearPicker={isShowMonthYearPicker ?? false}
        dateFormat={dateFormat ?? 'MMM dd, yyyy'}
        ref={datePickerRef}
        className='to-date-picker'
        onChange={onChangeDate}
      />
      <span
        className='icon-calendar'
        id='icon-calendar-from-date'
        onClick={() => datePickerRef?.current?.setOpen(true)}>
        <CalendarIcon size={14} />
      </span>
    </div>
  );
};

export default DatePickerComponent;
