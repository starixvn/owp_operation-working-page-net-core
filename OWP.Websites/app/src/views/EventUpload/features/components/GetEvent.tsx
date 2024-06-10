import CalendarIcon from 'mdi-react/CalendarIcon';
import { createRef, useState } from 'react';
import DatePicker, { ReactDatePicker } from 'react-datepicker';
import { useDispatch, useSelector } from 'react-redux';
import { addDays, getCurrentDateGmtPlus8 } from '../../../Shared/DateTime/dateTimeHelper';
import { RootState } from '../../app/storeNative';
import { buildDateString } from '../helper/buildHelper';
import { setEventDate } from '../slice/eventUploadSlice';
import { NextButton } from './Button';

const GetEvent = () => {
  const { eventDate } = useSelector((state: RootState) => state.uploadReducer.request);
  const [selectedDate, setSelectedDate] = useState<Date>(new Date(eventDate));
  const defaultDate = getCurrentDateGmtPlus8();
  const minDate = addDays(defaultDate, -1);
  const datePicker = createRef<ReactDatePicker>();

  const dispatch = useDispatch();

  const handleChangeDate = (date: Date) => {
    setSelectedDate(date);
    dispatch(setEventDate(buildDateString(date)));
  };

  const openCalendar = () => {
    datePicker.current && datePicker.current.setOpen(true);
  };

  const handleDownload = () => {
    window.location.href = `TaskScheduling/EventAssignment/DownloadAssignment?date=${buildDateString(selectedDate)}`;
  };

  return (
    <>
      <div className='filter-row'>
        <div className='filter-control calendar'>
          <div className='label'>Date</div>
          <DatePicker
            className='date'
            selected={selectedDate}
            onChange={handleChangeDate}
            minDate={minDate}
            dateFormat='MMM dd, yyyy'
            ref={datePicker} />
          <span className='icon-calendar' onClick={openCalendar}>
            <CalendarIcon size={16} />
          </span>
        </div>
        <button className='button-export' onClick={handleDownload}>Download</button>
      </div>
      <div className='wizard-buttons'>
        <NextButton />
      </div>
    </>
  );
};

export default GetEvent;