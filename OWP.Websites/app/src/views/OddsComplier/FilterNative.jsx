import { createRef, PureComponent } from 'react';
import PropTypes from 'prop-types';
import DatePicker from 'react-datepicker';
import { MultiSelect } from 'react-multi-select-component';
import Utils from '../Shared/Utils';
import { Input } from 'reactstrap';
import Toastr from '../components/ToastrWrapper';
import { MessageText } from './constants/Resoures';
import CalendarIcon from 'mdi-react/CalendarIcon';
import {
  buildTimeRangeLast30Day,
  getMMDDYYYYStringFromDateTime
} from '../Shared/DateTime/dateTimeHelper';

const propTypes = {
  submit: PropTypes.func.isRequired,
  export: PropTypes.func.isRequired,
  matchStatuses: PropTypes.array.isRequired
};

const Latest30Days = buildTimeRangeLast30Day(new Date());

class FilterNative extends PureComponent {
  constructor(props) {
    super(props);
    this.state = {
      selectedMatchStatus: [],
      League: '',
      selectedDate: new Date(),
      isExcludeSpecialProducts: true
    };

    this.matchStatusOptions = [];
    this.datePicker = createRef();
    this.handleSubmit = this.handleSubmit.bind(this);
    this.handleExport = this.handleExport.bind(this);
  }

  componentDidMount() {
    const { matchStatuses } = this.props;
    this.matchStatusOptions = Utils.buildMatchStatusOption(matchStatuses);
    const selectedMatchStatus = Utils.buildSelectedMatchStatus(
      this.matchStatusOptions
    );
    this.setState(
      {
        selectedMatchStatus
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
    this.setState({
      selectedMatchStatus,
      selectedDate: date
    });
  }

  openCalendar() {
    this.datePicker.current.setOpen(true);
  }

  onChangeLeague(event) {
    this.setState({ League: event.target.value });
  }

  handleSubmit() {
    const { submit } = this.props;
    const {
      selectedMatchStatus,
      League,
      isExcludeSpecialProducts,
      selectedDate
    } = this.state;
    if (selectedMatchStatus.length < 1) {
      Toastr.showError(MessageText.warningSelectMatchStatus);
      return;
    }

    const postModel = {
      selectedDate,
      League,
      selectedMatchStatus: selectedMatchStatus.map(x => x.value),
      isExcludeSpecialProducts
    };
    submit(postModel, null);
  }

  handleExport = e => {
    const {
      selectedMatchStatus,
      League,
      isExcludeSpecialProducts,
      selectedDate
    } = this.state;
    const date = getMMDDYYYYStringFromDateTime(selectedDate);
    const selectedMatchStatusIds = selectedMatchStatus
      .map(x => x.value)
      .join(',');

    e.preventDefault();
    const postModel = {
      date,
      league: League || '',
      matchStatuses: selectedMatchStatusIds,
      isExcludeSpecialProducts
    };

    this.props.export(postModel);
  };

  render() {
    const { selectedMatchStatus, isExcludeSpecialProducts, selectedDate } =
      this.state;
    return (
      <div className='filter'>
        <div className='filter-control calendar'>
          <div className='label'>Date</div>
          <DatePicker
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
          <div className='label'>League</div>
          <Input
            onChange={event => this.onChangeLeague(event)}
            className='input-field'
          />
        </div>
        <div className='filter-control multiselect'>
          <div className='label'>Match Status</div>
          <MultiSelect
            options={this.matchStatusOptions}
            value={selectedMatchStatus}
            disableSearch
            ClearSelectedIcon={null}
            onChange={selected =>
              this.setState({ selectedMatchStatus: selected })
            }
          />
        </div>
        <div className='filter-control checkbox form-check'>
          <Input
            defaultChecked={isExcludeSpecialProducts}
            id='chb-old-league'
            type='checkbox'
            name='oldLeague'
            className='form-check-input'
            onChange={() =>
              this.setState({
                isExcludeSpecialProducts: !isExcludeSpecialProducts
              })
            }
          />
          <label className='form-check-label' htmlFor='chb-old-league'>
            Exclude Special Products
          </label>
        </div>
        <div className='filter-control btn-block'>
          <button className='button-submit' onClick={this.handleSubmit}>
            Submit
          </button>
          <button className='button-export' onClick={this.handleExport}>
            Export
          </button>
        </div>
      </div>
    );
  }
}

FilterNative.propTypes = propTypes;
export default FilterNative;
