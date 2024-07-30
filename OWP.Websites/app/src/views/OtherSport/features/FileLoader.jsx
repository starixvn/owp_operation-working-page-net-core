// eslint-disable-next-line no-warning-comments
/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import ToastrWrapper from '../../components/ToastrWrapper';
import useStableDispatch from '../../Shared/customHook/useStableDispatch';
import { setErrorMessage, setFromDate, setToDate } from './fileLoaderSlice';
import DatePicker from 'react-datepicker';
import {
  getCurrentDateGmtPlus8,
  addDays,
  buildDateString
} from '../../Shared/DateTime/dateTimeHelper';
import { dateRange } from '../otherSport.json';

const propTypes = {
  onFileChange: PropTypes.func.isRequired
};

const FileLoader = props => {
  const dispatch = useStableDispatch();
  const fileLoader = useSelector(state => state.fileLoader);
  const [TextInput, setTextInput] = useState('Drag your file here');

  const defaultFromDate = getCurrentDateGmtPlus8();
  const minDate = addDays(defaultFromDate, -1);
  const maxDate = addDays(minDate, dateRange);
  const [selectedFromDate, setSelectedFromDate] = useState(defaultFromDate);
  const [selectedToDate, setSelectedToDate] = useState(defaultFromDate);
  const { onFileChange } = props;

  const handleOnChange = async event => {
    event.preventDefault();
    const file = event.target.files[0];
    file && setTextInput(file.name);
    onFileChange(file);
  };

  const handleChangeFromDate = date => {
    setSelectedFromDate(date);
    dispatch(setFromDate(buildDateString(date)));
  };
  const handleChangeToDate = date => {
    setSelectedToDate(date);
    dispatch(setToDate(buildDateString(date)));
  };

  useEffect(() => {
    onFileChange(undefined);
  }, []);

  useEffect(() => {
    (async () => {
      const { hasError, message } = fileLoader.validationResult;

      dispatch(setFromDate(buildDateString(selectedFromDate)));
      dispatch(setToDate(buildDateString(selectedToDate)));
      if (hasError) {
        await ToastrWrapper.showError(message);
        await dispatch(
          setErrorMessage({
            hasError: false,
            message: ''
          })
        );
      }
    })();
  });

  return (
    <>
      <div className='calendar-filter'>
        <div className='text-filter'>
          Please select a date range for an assignment file that you would like
          to upload:
        </div>
        <div className='from-control'>
          <div className='label'>From</div>
          <DatePicker
            className='from-date'
            selected={selectedFromDate}
            onChange={handleChangeFromDate}
            minDate={minDate}
            maxDate={selectedToDate}
            dateFormat='MMM dd, yyyy'
          />
        </div>
        <div className='to-control'>
          <div className='label'>To</div>
          <DatePicker
            className='to-date'
            selected={selectedToDate}
            onChange={handleChangeToDate}
            minDate={selectedFromDate}
            maxDate={maxDate}
            dateFormat='MMM dd, yyyy'
          />
        </div>
      </div>

      <div className='control-field'>
        <span className='text-input-file'>{TextInput}</span>
        <label htmlFor='myFile' className='button-input-file'>
          Browse
        </label>
        <input
          type='file'
          id='myFile'
          name='filename'
          accept='.xlsx'
          onChange={handleOnChange}
        />
      </div>
    </>
  );
};

FileLoader.propTypes = propTypes;
export default FileLoader;
