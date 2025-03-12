import { useEffect } from 'react';
import dayjs from 'dayjs';
import { useAppDispatch } from '../../app/hookNative';
import { getEvents } from '../table/tableThunk';
import FilterDate from './FilterDate';
import SummarizeProviderIcon from '../summarizeProvider/sum/SummarizeProviderIcon';
import CompareAgent from '../summarizeProvider/compare/CompareAgentIcon';
import MismatchEvent from './MismatchEvent';
import store from '../../app/storeNative';
import ConfirmProvider from './ConfirmProvider';
import EyeIcon from 'mdi-react/EyeIcon';
import { useAppSelector } from '../../app/hookNative';
import { exportExcel } from '../../../Shared/common/ExportExcel/Helper';
import Utils from '../../../Shared/Utils';

const Filter = () => {
  const dispatch = useAppDispatch();
  const { dataSource, validationResponse } = useAppSelector(
    state => state.events
  );
  const { sessionId } = validationResponse.value;
  const { eventDate } = store.getState().filterState.filter;

  const handleSearch = () => {
    const {
      filterState: {
        filter: { eventDate, onlyShowIsMismatchEvent, confirmProvider }
      }
    } = store.getState();
    const { confirmProviderOption } = store.getState().common.confirmProvider;
    let confirmProviderValue = confirmProvider?.length
      ? confirmProvider.map(x => x.value).join(',')
      : null;

    if (confirmProviderValue && confirmProvider.length === confirmProviderOption.length) {
      confirmProviderValue = null;
    }

    dispatch(getEvents({ eventDate, onlyShowIsMismatchEvent, confirmProvider: confirmProviderValue }));
  };

  useEffect(() => {
    handleSearch();
  }, []);

  const handleExport = async () => {
    const date = Utils.formatDate(eventDate);
    const params = `eventDate=${date}&sessionId=${sessionId}`;
    const url = `api/ProviderVerification/export?${params}`;
    const fileName = `ProviderVerification - ${dayjs(date).format('MMDDYYYY')}.xlsx`;

    exportExcel(url, fileName);
  };

  const handleClick = () => {
    window.open(`/ProviderVerification/check-difference-provider?date=${dayjs(eventDate).format('MM-DD-YYYY')}`);
    // window.open(`http://localhost:3001/ProviderVerification/check-difference-provider?date=${dayjs(eventDate).format('MM/DD/YYYY')}`);
  };

  return (
    <div className='top-control'>
      <div className='left-control'>
        <FilterDate />
        <ConfirmProvider />
        <MismatchEvent />
        <div className='filter-control btn-block'>
          <button className='button-submit' onClick={handleSearch}>
            Search
          </button>
          <button className={`button-export ${dataSource.length === 0 ? "btn-disable" : ""}`}
            onClick={handleExport}
          >
            Export
          </button>

        </div>
      </div>
      <div className='right-control'>
        <div className={`pearl-icon ${dataSource.length === 0 ? "btn-disable" : ""}`}
          onClick={handleClick}>
          <EyeIcon size={30} />
        </div>
        <CompareAgent />
        <SummarizeProviderIcon />
      </div>
    </div>
  );
};

export default Filter;
