import { useEffect, useState } from 'react';
import dayjs from 'dayjs';
import { useAppDispatch, useAppSelector } from '../../app/hookNative';
import { getEvents } from '../table/tableThunk';
import FilterDate from './FilterDate';
import SummarizeProviderIcon from '../summarizeProvider/sum/SummarizeProviderIcon';
import CompareAgent from '../summarizeProvider/compare/CompareAgentIcon';
import MismatchEvent from './MismatchEvent';
import ConfirmProvider from './ConfirmProvider';
import LSTBookProvider from './LSTBookProvider';
import EyeIcon from 'mdi-react/EyeIcon';
import ModalExportExcel from './ModalExportExcel';

const Filter = () => {
  const dispatch = useAppDispatch();
  const [isModalOpen, setIsModalOpen] = useState(false);

  const {
    dataSource,
    filterState: {
      eventDate,
      onlyShowIsMismatchEvent,
      confirmProvider,
      lstBookProvider
    },
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
    return selectedProviders.length === allProviders?.length
      ? null
      : providerValue;
  };

  const handleSearch = () => {
    dispatch(
      getEvents({
        eventDate,
        onlyShowIsMismatchEvent,
        confirmProvider: getProviderValue(
          confirmProvider,
          confirmProviderOption
        ),
        lstBookProvider: getProviderValue(
          lstBookProvider,
          LSTBookProviderOption
        )
      })
    );
  };

  useEffect(() => {
    handleSearch();
  }, []);

  const handleExport = () => {
    showModal();
  };

  const handleClick = () => {
    window.open(
      `/ProviderVerification/check-difference-provider/${dayjs(
        eventDate
      ).format('YYYY-MM-DD')}`,
      '_blank'
    );
  };

  const showModal = () => {
    setIsModalOpen(true);
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
          <button className={`button-export`} onClick={handleExport}>
            Export
          </button>
        </div>
      </div>
      <div className='right-control'>
        <div
          className={`pearl-icon ${
            dataSource.length === 0 ? 'btn-disable' : ''
          }`}
          onClick={handleClick}>
          <EyeIcon size={30} />
        </div>
        <CompareAgent />
        <SummarizeProviderIcon />
      </div>
      <ModalExportExcel
        isModalOpen={isModalOpen}
        setIsModalOpen={setIsModalOpen}
        onlyShowIsMismatchEvent={onlyShowIsMismatchEvent}
        confirmProvider={confirmProvider}
        lstBookProvider={lstBookProvider}
        confirmProviderOption={confirmProviderOption}
        LSTBookProviderOption={LSTBookProviderOption}
      />
    </div>
  );
};

export default Filter;
