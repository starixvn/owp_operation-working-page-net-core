// eslint-disable-next-line no-warning-comments
/* eslint-disable react-hooks/exhaustive-deps */
import { useState, useRef, useEffect, useCallback } from 'react';
import DatePicker from 'react-datepicker';
import CalendarIcon from 'mdi-react/CalendarIcon';
import ToggleColumnHook from './common/ToggleColumnHook';
import { TableConst, FilterConst, USER_STATUS } from '../constants/reportConst';
import { isEqual } from 'lodash';
import { useDispatch, useSelector } from 'react-redux';
import { serverTimeSelector } from '../selector/serverTimeSelector';
import { reportSelector } from '../selector/reportSelector';
import {
  queryDataWithParams,
  queryServerTime,
  queryPolicy
} from '../actions/action';
import { QUERY_LA } from '../constants/action-type';
import { policySelector } from '../selector/policySelector';
import PropTypes from 'prop-types';
import Loading from '../../components/Loading';
import { FunctionKeys } from '../../Shared/Constants';
import {
  buildFromDateToDateDefaultWithServerTime,
  getMMDDYYYYStringFromDateTime
} from '../../Shared/DateTime/dateTimeHelper';
import { isDateRangeValid } from '../helper/filterHelper';
import ToastrWrapper from '../../components/ToastrWrapper';
import { MultiSelect } from 'react-multi-select-component';

const urlGetLaData = `/api/report/la`;

const urlExportLa = `/api/report/la/export-excel`;

const exportUrl = (fromDate, toDate, activeUser, resignUser) => {
  const url = `${urlExportLa}?fromDate=${fromDate}&toDate=${toDate}&activeUser=${activeUser}&resignUser=${resignUser}`;
  return url;
};

const propTypes = {
  onShowColumnChange: PropTypes.func.isRequired
};

const Filter = ({ onShowColumnChange }) => {
  const [dateModel, setDateModel] = useState(FilterConst.defaultDateModel);
  const [userStatusSelected, setUserStatusSelected] = useState([
    USER_STATUS[0]
  ]);
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, []);
  const toDatePicker = useRef(null);
  const fromDatePicker = useRef(null);
  const timeServerModel = useSelector(serverTimeSelector);
  const { isDisplayTable } = useSelector(reportSelector);
  const policyModel = useSelector(policySelector);

  const { serverTime } = timeServerModel;
  const { laPolicy } = policyModel;
  const { fromDate, toDate } = dateModel;

  const handleShowColumnChange = columns => {
    const columnSelected = TableConst.unfixedColumnLA.filter(col =>
      columns.some(c => isEqual(c.value, col.name))
    );
    onShowColumnChange(columnSelected);
  };

  const selectOption = () =>
    TableConst.unfixedColumnLA.map(item => {
      return {
        label: item.name,
        value: item.name
      };
    });

  const handleSearch = event => {
    event.preventDefault();
    if (isDateRangeValid(fromDate, toDate)) {
      ToastrWrapper.showError(
        <h4>Please select date range within 365 days</h4>
      );
      return;
    }
    if (!userStatusSelected.length) {
      ToastrWrapper.showError(<h4>Please select at least one Account Type</h4>);
      return;
    }

    const postModel = {
      FromDate: getMMDDYYYYStringFromDateTime(fromDate),
      ToDate: getMMDDYYYYStringFromDateTime(toDate),
      activeUser: userStatusSelected.some(s => s.value === 'active'),
      resignUser: userStatusSelected.some(s => s.value === 'resigned')
    };
    stableDispatch(queryDataWithParams(urlGetLaData, postModel, QUERY_LA));
  };

  const handleExport = event => {
    event.preventDefault();
    const fromDateString = getMMDDYYYYStringFromDateTime(fromDate);
    const toDateString = getMMDDYYYYStringFromDateTime(toDate);
    window.location.href = exportUrl(
      fromDateString,
      toDateString,
      userStatusSelected.some(s => s.value === 'active'),
      userStatusSelected.some(s => s.value === 'resigned')
    );
  };

  useEffect(() => {
    if (serverTime) {
      const defaultTime = buildFromDateToDateDefaultWithServerTime(serverTime);
      setDateModel(defaultTime);
    }
  }, [serverTime]);

  useEffect(() => {
    stableDispatch(queryPolicy());
    stableDispatch(queryServerTime());
  }, [stableDispatch]);

  if (timeServerModel.pending && policyModel.pending) {
    return <Loading />;
  }

  return (
    <div className='top-control'>
      <div className='left-control'>
        <div className='filter-control calendar'>
          <div className='label'>From Date</div>
          <DatePicker
            selected={fromDate}
            dateFormat='MMM dd, yyyy'
            ref={fromDatePicker}
            onChange={date =>
              setDateModel(preDate => ({
                ...preDate,
                fromDate: date
              }))
            }
          />
          <span
            className='icon-calendar'
            onClick={() => fromDatePicker.current.setOpen(true)}>
            <CalendarIcon size={14} />
          </span>
        </div>
        <div className='filter-control calendar'>
          <div className='label'>To Date</div>
          <DatePicker
            selected={toDate}
            dateFormat='MMM dd, yyyy'
            ref={toDatePicker}
            onChange={date =>
              setDateModel(preDate => ({
                ...preDate,
                toDate: date
              }))
            }
          />
          <span
            className='icon-calendar'
            onClick={() => toDatePicker.current.setOpen(true)}>
            <CalendarIcon size={14} />
          </span>
        </div>
        <div className='filter-control multi-select-dropdown'>
          <div className='label'>Account Type</div>
          <MultiSelect
            labelledBy='Select Account Type'
            className='status'
            options={USER_STATUS}
            value={userStatusSelected}
            disableSearch
            ClearSelectedIcon={null}
            onChange={setUserStatusSelected}
          />
        </div>
        <div className='filter-control btn-block'>
          <button className='button-submit' onClick={handleSearch}>
            Search
          </button>
          <button className='button-export' onClick={handleExport}>
            Export
          </button>
        </div>
      </div>

      {isDisplayTable && (
        <div className='right-control'>
          <ToggleColumnHook
            options={selectOption()}
            localStorageKey={`${laPolicy.Username}${FunctionKeys.La_Report}`}
            onSelected={handleShowColumnChange}
          />
        </div>
      )}
    </div>
  );
};

Filter.propTypes = propTypes;
export default Filter;
