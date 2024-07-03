import { createRef, PureComponent } from 'react';
import PropTypes from 'prop-types';
import Select from 'react-select';
import DatePicker from 'react-datepicker';
import { MultiSelect } from 'react-multi-select-component';
import CalendarIcon from 'mdi-react/CalendarIcon';
import Utils from '../Shared/Utils';
import { UserSuggestion } from '../components/Assignment/UserSuggestion';
import dayjs from 'dayjs';
import ToastrWrapper from '../components/ToastrWrapper';
import ExportSelector from './ExportSelector';
import { connect } from 'react-redux';
import {
  buildTimeRangeLast30Day,
  buildDefaultFilterDate
} from '../Shared/DateTime/dateTimeHelper';

import { SYNC_LIVE_OPTION } from './constants/SyncLive';
import { MATCH_TYPE_OPTION } from './constants/MatchType';
import { setFilterDate } from './reducer/tableReducer';


const propTypes = {
  hasWrite: PropTypes.bool.isRequired,
  submit: PropTypes.func.isRequired,
  matchStatuses: PropTypes.array.isRequired,
  offVRs: PropTypes.array.isRequired,
  onToggleModal: PropTypes.func.isRequired,
  isAllowAddEvent: PropTypes.bool.isRequired,
  isHostEventPicking: PropTypes.bool.isRequired,
  lastUpdated: PropTypes.string.isRequired
};

let selectedDate = buildDefaultFilterDate();
const Latest30Days = buildTimeRangeLast30Day(new Date());

class Filter extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      selectedMatchStatus: [],
      selectedOffVRs: [],
      selectedSyncLive: SYNC_LIVE_OPTION[0].value,
      selectedMatchType: MATCH_TYPE_OPTION,
      userName: '',
      userNameIsValid: true,
      isShowUnreadMessOnly: false
    };
    this.matchStatusOptions = [];
    this.offVRs = [];
    this.datePicker = createRef();
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleShowMessage = this.handleShowMessage.bind(this);
  }

  handleShowMessage(isShowUnreadMessOnly) {
    this.setState(
      {
        isShowUnreadMessOnly: !isShowUnreadMessOnly
      },
      this.handleSubmit
    );
  }

  componentDidMount() {
    const { matchStatuses, offVRs } = this.props;
    this.matchStatusOptions = Utils.buildMatchStatusOption(matchStatuses);
    this.offVRs = Utils.buildOffVROption(offVRs);
    const selectedMatchStatus = Utils.buildSelectedMatchStatus(
      this.matchStatusOptions
    );

    this.setState(
      {
        selectedMatchStatus,
        selectedOffVRs: this.offVRs
      },
      () => {
        this.handleSubmit();
      }
    );
  }

  dateChange(date) {
    const selectedMatchStatus = Utils.handleOnDateChange(
      this.matchStatusOptions,
      date
    );
    selectedDate = date;
    this.setState({
      selectedMatchStatus,
      selectedDate
    });
  }

  getUserInfo(userName, userNameIsValid) {
    this.setState({ userName, userNameIsValid });
  }

  openCalendar() {
    this.datePicker.current.setOpen(true);
  }

  handleSubmit() {
    const { submit } = this.props;
    const {
      selectedMatchStatus,
      selectedOffVRs,
      selectedSyncLive,
      selectedMatchType,
      userName,
      userNameIsValid,
      isShowUnreadMessOnly
    } = this.state;
    const selectedMatchStatusIsValid = selectedMatchStatus.length > 0;
    const selectedOffVRIsValid = selectedOffVRs.length > 0;
    const selectedMatchTypeIsValid = selectedMatchType.length > 0;
    if (
      selectedMatchStatusIsValid &&
      selectedOffVRIsValid &&
      userNameIsValid &&
      selectedMatchTypeIsValid
    ) {
      const postModel = {
        selectedDate,
        userName,
        selectedMatchStatus: selectedMatchStatus.map(x => x.value),
        selectedOffVRs: selectedOffVRs.map(x => x.value),
        selectedMatchType: selectedMatchType.map(ms => ms.value).join(),
        selectedSyncLive,
        showUnreadMessage: isShowUnreadMessOnly
      };

      const today = new Date();
      const yesterDay = dayjs(today).subtract(1, 'day');
      const isAllowAddEvent = Utils.isSameOrAfter(selectedDate, yesterDay);
      submit(postModel, isAllowAddEvent, null);
      // eslint-disable-next-line no-warning-comments
      // eslint-disable-next-line react/prop-types
      this.props.dispatch(setFilterDate(Utils.formatDate(selectedDate)));
    } else {
      !selectedMatchStatusIsValid &&
        ToastrWrapper.showError('Please select at least one match status');
      !userNameIsValid && ToastrWrapper.showError('User not found');
      !selectedMatchTypeIsValid &&
        ToastrWrapper.showError('Please select at least one match type');
      !selectedOffVRIsValid &&
        ToastrWrapper.showError('Please select at least one OFF VR');
    }
  }

  render() {
    const { selectedMatchStatus, selectedMatchType, selectedOffVRs } =
      this.state;
    const {
      hasWrite,
      onToggleModal,
      isAllowAddEvent,
      isHostEventPicking,
      lastUpdated
    } = this.props;
    return (
      <>
        <div className='filter-control calendar'>
          <div className='label'>Date</div>
          <DatePicker
            data-testid='contextMenu-testId'
            minDate={Latest30Days}
            selected={selectedDate}
            onChange={e => this.dateChange(e)}
            dateFormat='MMM dd, yyyy'
            ref={this.datePicker}
          />
          <span className='icon-calendar' onClick={() => this.openCalendar()}>
            <CalendarIcon size={14} />
          </span>
        </div>
        <div className='filter-control'>
          <div className='label'>Employee</div>
          <UserSuggestion
            onHandleChange={(userName, userNameIsValid) =>
              this.getUserInfo(userName, userNameIsValid)
            }
          />
        </div>
        <div className='filter-control multiselect'>
          <div className='label'>Match Type</div>
          <MultiSelect
            options={MATCH_TYPE_OPTION}
            disableSearch
            ClearSelectedIcon={null}
            value={selectedMatchType}
            onChange={selected =>
              this.setState({ selectedMatchType: selected })
            }
          />
        </div>
        <div className='filter-control multiselect'>
          <div className='label'>Match Status</div>
          <MultiSelect
            options={this.matchStatusOptions}
            disableSearch
            ClearSelectedIcon={null}
            value={selectedMatchStatus}
            onChange={selected =>
              this.setState({ selectedMatchStatus: selected })
            }
          />
        </div>
        <div className='filter-control multiselect sync-live'>
          <div className='label'>Sync Live</div>
          <Select
            defaultValue={SYNC_LIVE_OPTION[0]}
            className='basic-multi-select'
            classNamePrefix='select'
            options={SYNC_LIVE_OPTION}
            onChange={e => this.setState({ selectedSyncLive: e.value })}
          />
        </div>
        <div className='filter-control multiselect off-vr'>
          <div className='label'>OFF VR</div>
          <MultiSelect
            options={this.offVRs}
            disableSearch
            ClearSelectedIcon={null}
            value={selectedOffVRs}
            onChange={selected => this.setState({ selectedOffVRs: selected })}
          />
        </div>
        <div className='filter-control btn-block'>
          <button className='button-submit' onClick={this.handleSubmit}>
            Submit
          </button>
          {hasWrite && isAllowAddEvent && (
            <button className='button-submit' onClick={onToggleModal}>
              Add Event
            </button>
          )}
          <ExportSelector
            isHostEventPicking={isHostEventPicking}
            selectedDate={selectedDate}
            selectedMatchStatus={selectedMatchStatus.map(x => x.value)}
            selectedOffVRs={selectedOffVRs.map(x => x.value)}
            selectedMatchType={selectedMatchType.map(ms => ms.value).join()}
            userName={this.state.userName}
            userNameIsValid={this.state.userNameIsValid}
            selectedSyncLive={this.state.selectedSyncLive}
            lastUpdated={lastUpdated}
          />
        </div>
        <div className='filter-control-checkbox'>
          <label>
            <input
              className='unread-checkbox'
              type='checkbox'
              checked={this.state.isShowUnreadMessOnly}
              onChange={() =>
                this.handleShowMessage(this.state.isShowUnreadMessOnly)
              }
            />{' '}
            Show Unread Message Only
          </label>
        </div>
      </>
    );
  }
}

Filter.propTypes = propTypes;
export default connect()(Filter);
