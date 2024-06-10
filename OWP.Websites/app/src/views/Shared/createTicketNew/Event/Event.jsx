import { useEffect, useState, useCallback } from 'react';
import TextField from '@mui/material/TextField';
import Badge from '@mui/material/Badge';
import Autocomplete from '@mui/material/Autocomplete';
import { formatTime } from '../../DateTime/dateTimeHelper';
import { useSelector } from 'react-redux';
import useStableDispatch from '../../customHook/useStableDispatch';
import {
  selectedEvent,
  changeErrorInput,
  setEventChecking,
  setRecipientAssigned
} from '../createTicketBoardSliceNew';
import { filterResult } from '../Helper/HelperCreateTicket';
import Service from '../Service';
import { isEmpty, debounce } from 'lodash';
import PropTypes from 'prop-types';

const propTypes = {
  typeTicketText: PropTypes.string.isRequired,
  ticketType: PropTypes.number.isRequired
};
const Event = props => {
  const { typeTicketText, ticketType } = props;
  const dispatch = useStableDispatch();
  const { errorInput, eventChecking } = useSelector(
    state => state.createTicketBoard
  );
  const [options, setOptions] = useState([]);
  const [open, setOpen] = useState(false);
  const [eventOption, setEventOption] = useState([]);

  const handleChangedAutoComplete = (_, value) => {
    if (value) {
      const assignee = eventChecking.findIndex(
        x => x.eventInfoViewModel[0].owpLeagueId === value.owpLeagueId
      );
      eventChecking[assignee] &&
        dispatch(setRecipientAssigned(eventChecking[assignee].userViewModel));
    }
    setEventOption(value);
    dispatch(selectedEvent(value));
    dispatch(changeErrorInput(false));
  };

  const handleSearch = event => {
    if (!event.target.value) {
      setEventOption([]);
    } else {
      debounceSearchEventOptions(event.target.value);
    }
  };
  const debounceSearchEventOptions = useCallback(
    debounce(nextValue => getSearchEventOptions(nextValue), 600),
    []
  );

  const getSearchEventOptions = async value => {
    if (value.length >= 2) {
      const result = await Service.event(value, ticketType);
      dispatch(setEventChecking(result.data));
      const optionResult = filterResult(result.data);
      setOptions(optionResult);
      setOpen(true);
    } else {
      dispatch(setEventChecking([]));
      setOptions([]);
      setOpen(false);
    }
  };

  useEffect(() => {
    setOptions([eventOption]);
    if (isEmpty(eventOption)) {
      setOpen(false);
    }
  }, [eventOption]);

  return (
    <>
      <div className='header-popup'>
        <h1> Type: {typeTicketText} </h1>
      </div>
      <div className='form-createTicket'>
        <div className='left-form'>
          <label
            className='label'
            style={{ marginTop: '10px' }}
            htmlFor='subject'>
            Event:<span className='required'>*</span>
          </label>
        </div>
        <div className='right-form'>
          <Badge id='selected-event'>
            <Autocomplete
              id='tags-filled'
              freeSolo
              options={options}
              filterOptions={option => option}
              onChange={handleChangedAutoComplete}
              onInputChange={handleSearch}
              onClose={() => setOpen(false)}
              onOpen={() => {
                if (!isEmpty(options[0])) {
                  setOpen(true);
                }
              }}
              open={open}
              getOptionLabel={option => {
                if (!option) {
                  return '';
                }
                return option.homeName
                  ? `${formatTime(option.eventDateTimeGmt8, 'minute', 'lll')}`
                  : `${option.leagueName}`;
              }}
              renderInput={params => (
                <TextField
                  helperText={errorInput ? 'This field is required' : ''}
                  error={errorInput}
                  {...params}
                  label={
                    eventOption &&
                    eventOption.length !== 0 &&
                    !eventOption.isLeague ? (
                      <div className='selected-event'>
                        <span>{eventOption.leagueName}</span>
                        <span className='league-event-selected'>
                          {eventOption.homeName} vs {eventOption.awayName}
                        </span>
                      </div>
                    ) : (
                      ''
                    )
                  }
                  margin='normal'
                  variant='standard'
                />
              )}
              renderOption={(optionProps, option, state) => {
                if (option && option.homeName) {
                  return (
                    <div {...optionProps} key={state.index}>
                      <div className='event-option'>
                        <div className='league-name'>{option.leagueName}</div>
                        <div className='league-event'>
                          {option.homeName}&nbsp;vs&nbsp;{option.awayName}
                        </div>
                        <div className='league-time'>
                          {' '}
                          {`${formatTime(
                            option.eventDateTimeGmt8,
                            'minute',
                            'lll'
                          )}`}
                        </div>
                      </div>
                    </div>
                  );
                }
                if (option && !option.homeName && option.leagueName) {
                  return (
                    <div {...optionProps} key={state.index}>
                      <div className='league-name'>{option.leagueName}</div>
                    </div>
                  );
                }
                return <div key={state.index}></div>;
              }}
            />
          </Badge>
        </div>
      </div>
    </>
  );
};
Event.propTypes = propTypes;
export default Event;
