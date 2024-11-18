import CalendarIcon from 'mdi-react/CalendarIcon';
import { useState } from 'react';
import DatePicker from 'react-datepicker';
import useStableDispatch from '../../Shared/customHook/useStableDispatch';
import { maxDate, minDate } from '../helper/filterHelper';
import { DATE } from './filterConst.json';
import { setFilter } from './slice/filterSlice';
import Utils from '../../Shared/Utils';

function DateTimePicker() {
    const [openCalendar, setOpenCalendar] = useState(false);
    const [date, setDate] = useState(new Date());
    const dispatch = useStableDispatch();

    const handleDateChange = (newDate: Date | null) => {
        if (newDate) {
            setOpenCalendar(false);
            setDate(newDate);
            dispatch(
                setFilter({
                    key: DATE,
                    value: Utils.formatDate(newDate)
                })
            );
        }
    };

    return (
        <div className='calendar'>
            <div className='label'>Date</div>
            <DatePicker
                selected={date}
                maxDate={maxDate}
                minDate={minDate}
                dateFormat='MMM dd, yyyy'
                className='from-date-picker'
                open={openCalendar}
                openToDate={date}
                onClickOutside={_ => setOpenCalendar(false)}
                onChange={handleDateChange}
            />
            <span className='icon-calendar' onClick={() => setOpenCalendar(true)}>
                <CalendarIcon size={14} />
            </span>
        </div>
    );
}

export default DateTimePicker;
