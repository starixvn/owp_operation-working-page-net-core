import { useEffect } from 'react';
import { useAppDispatch } from '../../app/hookNative';
import { getEvents } from '../table/tableThunk';
import FilterDate from './FilterDate';
import SummarizeProviderIcon from '../summarizeProvider/sum/SummarizeProviderIcon';
import CompareAgent from '../summarizeProvider/compare/CompareAgentIcon';
import MismatchEvent from './MismatchEvent';
import store from '../../app/storeNative';
import ConfirmProvider from './ConfirmProvider';

const Filter = () => {
  const dispatch = useAppDispatch();

  const handleSearch = () => {
    const {
      filterState: {
        filter: { eventDate, onlyShowIsMismatchEvent }
      }
    } = store.getState();
    dispatch(getEvents({ eventDate, onlyShowIsMismatchEvent }));
  };

  useEffect(handleSearch, []);

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
        </div>
      </div>
      <div className='right-control'>
        <CompareAgent />
        <SummarizeProviderIcon />
      </div>
    </div>
  );
};

export default Filter;
