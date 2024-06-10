import { createRef, Component } from 'react';
import PropTypes from 'prop-types';
import DatePicker from 'react-datepicker';
import ReportConst from './constant/ReportConst';
import CalendarIcon from 'mdi-react/CalendarIcon';
import ToggleColumn from '../Shared/ShowHideColumn/ToggleColumn';
import { isEqual } from 'lodash';
import { FunctionKeys } from '../Shared/Constants';
import {
  buildTimeRangeForFromDate,
  buildTimeRangeForToDate,
  buildFromDateToDateDefaultWithServerTime
} from '../Shared/DateTime/dateTimeHelper';

const propTypes = {
  onSubmit: PropTypes.func.isRequired,
  onExport: PropTypes.func.isRequired,
  onShowColumnChange: PropTypes.func.isRequired,
  canShowHideColumn: PropTypes.bool,
  policy: PropTypes.object,
  serverTime: PropTypes.string
};

class Filter extends Component {
  constructor(props) {
    super(props);
    const { serverTime } = this.props;
    const dateDefault = buildFromDateToDateDefaultWithServerTime(serverTime);
    this.state = {
      fromDate: dateDefault.fromDate,
      toDate: dateDefault.toDate
    };

    this.fromDatePicker = createRef();
    this.toDatePicker = createRef();
    this.handleSearch = this.handleSearch.bind(this);
    this.handleExport = this.handleExport.bind(this);
    this.onShowColumnChange = this.onShowColumnChange.bind(this);
    this.preventDefaultAndTriggerCallBack = this.preventDefaultAndTriggerCallBack.bind(
      this
    );
    this.generateStorageKey = this.generateStorageKey.bind(this);
    this.selectOption = this.selectOption.bind(this);
  }

  selectOption = policy => {
    const { traderAssignment, traderPerformace } = policy;
    let option = [];
    if (traderAssignment.HasRead) {
      option = [...option, ...ReportConst.columnTraderAssignment];
    }
    if (traderPerformace.HasRead) {
      option = [...option, ...ReportConst.columnTraderPerformace];
    }
    return option.map(col => {
      return {
        label: col.name,
        value: col.name
      };
    });
  };

  generateStorageKey = policy => {
    const { userName, traderAssignment, traderPerformace } = policy;
    let localStorageKey = '';
    const localStoragePrefix = `localStorageKey_ShowHideColumn_LstReport_${userName}`;
    if (traderAssignment.HasRead && traderPerformace.HasRead) {
      localStorageKey = `${localStoragePrefix}_showall`;
    } else {
      if (traderAssignment.HasRead) {
        localStorageKey = `${localStoragePrefix}_${FunctionKeys.TraderReport_TraderAssignment}`;
      }
      if (traderPerformace.HasRead) {
        localStorageKey = `${localStoragePrefix}_${FunctionKeys.TraderReport_TraderPerformance}`;
      }
    }
    return localStorageKey;
  };

  onShowColumnChange = columns => {
    const column = [
      ...ReportConst.columnTraderAssignment,
      ...ReportConst.columnTraderPerformace
    ];
    const columnSelected = column.filter(col =>
      columns.some(c => isEqual(c.value, col.name))
    );
    this.props.onShowColumnChange(columnSelected);
  };

  dateChange = (key, value) => {
    this.setState({
      [key]: value
    });
  };

  openCalendar = action => {
    const { openFromDate, openToDate } = ReportConst.calendar;
    switch (action) {
      case openFromDate:
        this.fromDatePicker.current.setOpen(true);
        break;
      case openToDate:
        this.toDatePicker.current.setOpen(true);
        break;
      default:
    }
  };

  handleExport = e => {
    this.preventDefaultAndTriggerCallBack(e, this.props.onExport);
  };

  handleSearch = e => {
    this.preventDefaultAndTriggerCallBack(e, this.props.onSubmit);
  };

  preventDefaultAndTriggerCallBack = (e, callBack) => {
    e.preventDefault();
    const { fromDate, toDate } = this.state;
    callBack({
      fromDate,
      toDate
    });
  };

  render() {
    const { fromDate, toDate } = this.state;
    const { policy, canShowHideColumn, serverTime } = this.props;
    const { openFromDate, openToDate } = ReportConst.calendar;
    const [timeRangeFromDate, timeRangeToDate] = [
      buildTimeRangeForFromDate(toDate),
      buildTimeRangeForToDate(fromDate, serverTime)
    ];
    return (
      <>
        <div className='filter-control calendar'>
          <div className='label'>From Date</div>
          <DatePicker
            minDate={timeRangeFromDate.minFromDate}
            maxDate={timeRangeFromDate.maxFromDate}
            selected={fromDate}
            onChange={e => this.dateChange('fromDate', e)}
            dateFormat='MMM dd, yyyy'
            ref={this.fromDatePicker}
            className='from-date'
          />
          <span
            className='icon-calendar'
            onClick={() => this.openCalendar(openFromDate)}>
            <CalendarIcon size={14} />
          </span>
        </div>
        <div className='filter-control calendar'>
          <div className='label'>To Date</div>
          <DatePicker
            minDate={timeRangeToDate.minToDate}
            maxDate={timeRangeToDate.maxToDate}
            selected={toDate}
            onChange={e => this.dateChange('toDate', e)}
            dateFormat='MMM dd, yyyy'
            ref={this.toDatePicker}
            className='to-date'
          />
          <span
            className='icon-calendar'
            onClick={() => this.openCalendar(openToDate)}>
            <CalendarIcon size={14} />
          </span>
        </div>
        <div className='filter-control btn-block'>
          <button className='button-submit' onClick={this.handleSearch}>
            Search
          </button>
          <button className='button-export' onClick={this.handleExport}>
            Export
          </button>
        </div>
        {canShowHideColumn && (
          <div className='filter-control btn-block btn-block--right'>
            {policy.userName && (
              <ToggleColumn
                onSelected={e => this.onShowColumnChange(e)}
                options={this.selectOption(policy)}
                localStorageKey={this.generateStorageKey(policy)}
              />
            )}
          </div>
        )}
      </>
    );
  }
}

Filter.propTypes = propTypes;
export default Filter;
