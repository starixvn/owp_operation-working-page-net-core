import { useEffect } from 'react';
import dayjs from 'dayjs';
import { useAppDispatch } from '../../app/hookNative';
import { getEvents } from '../table/tableThunk';
import FilterDate from './FilterDate';
import SummarizeProviderIcon from '../summarizeProvider/sum/SummarizeProviderIcon';
import CompareAgent from '../summarizeProvider/compare/CompareAgentIcon';
import MismatchEvent from './MismatchEvent';
import ConfirmProvider from './ConfirmProvider';
import EyeIcon from 'mdi-react/EyeIcon';
import { useAppSelector } from '../../app/hookNative';
import { exportExcel } from '../../../Shared/common/ExportExcel/Helper';
import Utils from '../../../Shared/Utils';

const Filter = () => {
  const dispatch = useAppDispatch();
  const { dataSource } = useAppSelector(state => state.events);
  const { eventDate, onlyShowIsMismatchEvent, confirmProvider } = useAppSelector(state => state.filterState.filter);
  const { confirmProviderOption } = useAppSelector(state => state.common.confirmProvider);

  const getConfirmProviderValue = () => {
    if (!confirmProvider?.length) return null;
    const confirmProviderValue = confirmProvider.map(x => x.value).join(',');
    return confirmProvider.length === confirmProviderOption.length ? null : confirmProviderValue;
  };

  const handleSearch = () => {
    dispatch(getEvents({ eventDate, onlyShowIsMismatchEvent, confirmProvider: getConfirmProviderValue() }));
  };

  useEffect(() => {
    handleSearch();
  }, []);

  const handleExport = async () => {
    const date = Utils.formatDate(eventDate);
    const confirmProviderValue = getConfirmProviderValue();
    const params = new URLSearchParams({
      eventDate: date,
      onlyShowIsMismatchEvent: onlyShowIsMismatchEvent.toString(),
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
