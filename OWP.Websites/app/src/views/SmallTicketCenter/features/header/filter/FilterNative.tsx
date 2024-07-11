import { useEffect, useState } from 'react';
import Command from './Command';
import DateRangeFilter from './DateRangeFilter';
import Event from './Event';
import Status from './Status';
import TicketType from './TicketType';
import { KeyboardArrowDownIcon, KeyboardArrowUpIcon } from 'mdi-react';
import { useAppDispatch, useAppSelector } from '../../../app/hooksNative';
import { getEventTickets } from '../../table/slice/ticketSliceThunkNative';
import Loading from '../../../../components/Loading';
import { getTicketTypeOption } from './filterThunkNative';

const FilterWrapper = () => {
  const dispatch = useAppDispatch();
  const optionsIsReady = useAppSelector(s => s.filter.ticketTypeOptionReady);

  useEffect(() => {
    (() => {
      dispatch(getTicketTypeOption());
    })();
  }, [dispatch]);

  if (!optionsIsReady) {
    return <Loading />;
  }

  return <Filter />;
};

const Filter = () => {
  const [showFilter,setShowFilter]= useState(false);
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(getEventTickets());
  }, [dispatch]);

  return (
    <div className='block-filter has-dropdown'>
      {showFilter && (
        <div className='show-info' onClick={() => setShowFilter(false)}>
          <KeyboardArrowUpIcon />
          <p>Hide Filter</p>
        </div>
      )}
      <div
        className='filter-control'
        style={{
          display: !showFilter ? 'none' : 'flex'
        }}>
        <DateRangeFilter />
        <Event />
        <TicketType />
        <Status />
        <Command />
      </div>
      {!showFilter && (
        <div className='show-info' onClick={() => setShowFilter(true)}>
          <KeyboardArrowDownIcon />
          <p>Show Filter</p>
        </div>
      )}
    </div>
  );
};

export default FilterWrapper;
