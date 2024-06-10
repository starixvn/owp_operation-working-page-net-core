import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../../app/hookNative';
import { getEvents } from '../table/tableThunk';
import FilterDate from './FilterDate';
import SummarizeProviderIcon from '../summarizeProvider/sum/SummarizeProviderIcon';
import CompareAgent from '../summarizeProvider/compare/CompareAgentIcon';

const Filter = () => {
  const dispatch = useAppDispatch();
  const filterState = useAppSelector(state => state.filterState.filter);
  const handleOnSearch = () => {
    dispatch(getEvents(filterState.eventDate));
  };

  useEffect(() => {
    dispatch(getEvents(filterState.eventDate));
  }, []);

  return (
    <div className='top-control'>
      <div className='left-control'>
        <FilterDate />

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
