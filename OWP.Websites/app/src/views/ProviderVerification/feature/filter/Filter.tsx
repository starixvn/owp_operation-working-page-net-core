import { useEffect } from 'react';
import dayjs from 'dayjs';
import { useAppDispatch } from '../../app/hookNative';
import { getEvents } from '../table/tableThunk';
import FilterDate from './FilterDate';
import SummarizeProviderIcon from '../summarizeProvider/sum/SummarizeProviderIcon';
import CompareAgent from '../summarizeProvider/compare/CompareAgentIcon';
import MismatchEvent from './MismatchEvent';
import ConfirmProvider from './ConfirmProvider';
import LSTBookProvider from './LSTBookProvider';
import EyeIcon from 'mdi-react/EyeIcon';
import { useAppSelector } from '../../app/hookNative';
import { exportExcel } from '../../../Shared/common/ExportExcel/Helper';
import Utils from '../../../Shared/Utils';

const Filter = () => {
  const dispatch = useAppDispatch();
  const {
    dataSource,
    filterState: { eventDate, onlyShowIsMismatchEvent, confirmProvider, lstBookProvider },
    common: {
      confirmProvider: { confirmProviderOption },
      LSTBookProvider: { LSTBookProviderOption }
    }
  } = useAppSelector(state => ({
    dataSource: state.events.dataSource,
    filterState: state.filterState.filter,
    common: state.common
  }));

  const getProviderValue = (selectedProviders: any[], allProviders: any[]) => {
    if (!selectedProviders?.length) return null;
    const providerValue = selectedProviders.map(x => x.value).join(',');
    return selectedProviders.length === allProviders?.length ? null : providerValue;
  };


  const handleSearch = () => {
    dispatch(getEvents({
      eventDate,
      onlyShowIsMismatchEvent,
      confirmProvider: getProviderValue(confirmProvider, confirmProviderOption),
      lstBookProvider: getProviderValue(lstBookProvider, LSTBookProviderOption)
    }));
  };

  useEffect(() => {
    handleSearch();
  }, []);

  const handleExport = async () => {
    const date = Utils.formatDate(eventDate);
    const confirmProviderValue = getProviderValue(confirmProvider, confirmProviderOption);
    const lstBookProviderValue = getProviderValue(lstBookProvider, LSTBookProviderOption);

    const params = new URLSearchParams({
      eventDate: date,
      onlyShowIsMismatchEvent: onlyShowIsMismatchEvent.toString(),
      ...(lstBookProviderValue !== null && { lstBookProvider: lstBookProviderValue }),
      ...(confirmProviderValue !== null && { confirmProvider: confirmProviderValue })
    }).toString();
    const url = `api/ProviderVerification/export?${params}`;
    const fileName = `ProviderVerification - ${dayjs(date).format('MMDDYYYY')}.xlsx`;

    exportExcel(url, fileName);
  };

  const handleClick = () => {
    window.open(`/ProviderVerification/check-difference-provider/${dayjs(eventDate).format('YYYY-MM-DD')}`, '_blank');
  };

  return (
    <div className='top-control'>
      <div className='left-control'>
        <FilterDate />
        <LSTBookProvider />
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
