import { useState, createRef, useEffect } from 'react';
import MultipleSelection from '../../Shared/MultipleSelection/MultipleSelection';
import {
  DEFAULT_STATUS_OPTION,
  MATCH_TYPE_OPTION
} from '../LiveGameListConst.json';
import useStableDispatch from './../../Shared/customHook/useStableDispatch';
import { getEvents } from '../table/tableSlice/tableThunk';
import CreateTicketButton from '../../Shared/createTicketNew/CreateTicketButton';
import { BUTTON_TYPE } from '../../Shared/createTicketNew/const/const';
import GoToMyTicket from '../../Shared/TicketCenterPrematch/GoToMyTicket';
import DatePicker from 'react-datepicker';
import CalendarIcon from 'mdi-react/CalendarIcon';
import { buildDefaultLGLFilterDate } from '../../Shared/DateTime/dateTimeHelper';
import { getByRefName, authorize } from '../service/Service';
import axios from 'axios';
import { FunctionKeys, MatchStatuses } from '../../Shared/Constants';
import { useSelector } from 'react-redux';
import {
  filterByUnReadTicket,
  setRefreshModel
} from '../table/tableSlice/tableSlice';
import ToastrWrapper from '../../components/ToastrWrapper';
import Utils from '../../Shared/Utils';
import SearchEmployee from '../../Shared/SearchEmployee/SearchEmployee';

const { LIVE_GAME } = BUTTON_TYPE;
const Filter = () => {
  const [selectedDate, setSelectedDate] = useState(buildDefaultLGLFilterDate());
  const [searchDisabled, setSearchDisabled] = useState(false);
  const datePicker = createRef();
  const [selectedMatchStatus, setSelectedMatchStatus] = useState(
    DEFAULT_STATUS_OPTION
  );
  const [selectedMatchType, setSelectedMatchType] = useState(MATCH_TYPE_OPTION);
  const [statusOptions, setStatusOptions] = useState([]);
  const [userIds, setUserIds] = useState([]);
  const { isShowUnReadTicketOnly } = useSelector(state => state.events);
  const dispatch = useStableDispatch();

  useEffect(() => {
    (async () => {
      const [statusRaw, fullTimeView, timeRangeView] = await axios.all([
        getByRefName('MatchStatus'),
        authorize(FunctionKeys.FullTimeView),
        authorize(FunctionKeys.TimeRangeView)
      ]);
      const status = statusRaw.map(x => ({
        label: x.Value,
        value: x.Id
      }));
      setStatusOptions(status);
      const disable = !fullTimeView.HasRead && !timeRangeView.HasRead;
      setSearchDisabled(disable);
      if (!disable) {
        const model = {
          filterDate: selectedDate,
          statusId: selectedMatchStatus.map(ms => ms.value).join(),
          matchType: selectedMatchType.map(ms => ms.value).join()
        };
        dispatch(getEvents(model));
        dispatch(setRefreshModel(model));
      }
    })();
  }, []);

  const handleOnSubmit = () => {
    if (!selectedMatchStatus.length) {
      ToastrWrapper.showError(
        'Please select at least one option for Match Status'
      );
      return;
    }

    if (!selectedMatchType.length) {
      ToastrWrapper.showError(
        'Please select at least one option for Match Type'
      );
      return;
    }

    if (!searchDisabled) {
      const model = {
        userIds: userIds.map(x => x.value).join(),
        filterDate: selectedDate,
        statusId: selectedMatchStatus.map(ms => ms.value).join(),
        matchType: selectedMatchType.map(ms => ms.value).join()
      };
      dispatch(getEvents(model));
      dispatch(setRefreshModel(model));
    }
  };

  useEffect(() => {
    if (statusOptions && statusOptions.length === 0) {
      return;
    }
    let selectedMatchStatusByDate = statusOptions;
    const today = new Date();
    if (Utils.isSameOrAfter(selectedDate, today)) {
      selectedMatchStatusByDate = statusOptions
        .filter(
          x =>
            x.label === MatchStatuses.Live || x.label === MatchStatuses.InPlay
        )
        .sort((a, b) => a.value - b.value);
    }
    setSelectedMatchStatus(selectedMatchStatusByDate);
  }, [selectedDate]);

  const handleSelectedUserIds = selected => setUserIds(selected);

  return (
    <div className='filter-control'>
      <div className='date-dropdown calendar'>
        <div className='label'>Date</div>
        <DatePicker
          selected={selectedDate}
          onChange={value => setSelectedDate(value)}
          dateFormat='MMM dd, yyyy'
          ref={datePicker}
        />
        <span
          className='icon-calendar'
          onClick={() => datePicker.current.setOpen(true)}>
          <CalendarIcon size={14} />
        </span>
      </div>
      <div className='employee-filter'>
        <SearchEmployee onChangeOptions={handleSelectedUserIds} />
      </div>
      <MultipleSelection
        label='Match Type'
        options={MATCH_TYPE_OPTION}
        value={selectedMatchType}
        onChange={value => setSelectedMatchType(value)}
      />
      <MultipleSelection
        label='Match Status'
        options={statusOptions}
        value={selectedMatchStatus}
        onChange={value => setSelectedMatchStatus(value)}
      />

      <button
        className='button-submit'
        onClick={handleOnSubmit}
        disabled={searchDisabled}>
        Search
      </button>

      <div className='header-right'>
        <div className='filter-control-checkbox'>
          <label>
            <input
              className='unread-checkbox'
              type='checkbox'
              checked={isShowUnReadTicketOnly}
              onChange={() =>
                dispatch(filterByUnReadTicket(!isShowUnReadTicketOnly))
              }
            />{' '}
            Show Unread Message Only
          </label>
        </div>
        <div>
          <div className='go-my-ticket'>
            <GoToMyTicket />
          </div>
          <CreateTicketButton buttonType={LIVE_GAME} />
        </div>
      </div>
    </div>
  );
};
export default Filter;
