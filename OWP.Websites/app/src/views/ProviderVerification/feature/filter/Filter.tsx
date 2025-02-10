import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hookNative';
import { getEvents } from '../table/tableThunk';
import FilterDate from './FilterDate';
import SummarizeProviderIcon from '../summarizeProvider/sum/SummarizeProviderIcon';
import CompareAgent from '../summarizeProvider/compare/CompareAgentIcon';
import MismatchEvent from './MismatchEvent';
import store from '../../app/storeNative';

const Filter = () => {
  const dispatch = useAppDispatch();
  const {
    filterState: {
      filter: { eventDate, onlyShowIsMismatchEvent }
    }
  } = store.getState();
  const handleOnSearch = () => {
    dispatch(getEvents({ eventDate, onlyShowIsMismatchEvent }));
  };

  useEffect(handleOnSearch, []);

  return (
    <div className='top-control'>
      <div className='left-control'>
        <FilterDate />
        <MismatchEvent />
        <div className='filter-control btn-block'>
          <button className='button-submit' onClick={handleOnSearch}>
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
