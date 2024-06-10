import { useEffect, useState, useCallback } from 'react';
import MarketFilter from './MarketFilter';
import { onSubmitFilterModel } from '../../signalR/signalRSlice';
import update from 'immutability-helper';
import {
  getEventTickets,
  useStableDispatch
} from '../../../ImportTicketCenterHelperNative';
import PropTypes from 'prop-types';
import ToastrWrapper from '../../../../components/ToastrWrapper';
import { STATUS } from '../../../const/const.json';
import TicketTypeFilter from './TicketTypeFilter';
import { getTicketTypeOption } from '../../../services/service';
import {
  buildTicketTypeFilter,
  getDefaultDateRange
} from '../../../Helper/TicketCenterHelper';
import Loading from '../../../../components/Loading';
import { MultiSelect } from 'react-multi-select-component';
import DateRangeFilter from './DateRangeFilter';
import Utils from '../../../../Shared/Utils';

const propTypes = {
  defaultValue: PropTypes.object.isRequired
};

const FilterTicketCenter = props => {
  const { defaultValue } = props;
  const { fromDateDefault, toDateDefault } = getDefaultDateRange();
  const defaultStatus = STATUS.filter(
    x => x.value === defaultValue.TicketStatuses
  );
  const [filter, setFilter] = useState({});
  const [ticketTypeOption, setTicketTypeOption] = useState([]);
  const [defaultOption, setDefaultOption] = useState([]);
  const [selectedStatuses, setSelectedStatuses] = useState(defaultStatus);
  const [loading, setLoading] = useState(true);
  const stableDispatch = useStableDispatch();

  const handleSubmit = useCallback(() => {
    const { TicketStatuses, TicketTypeId } = filter;

    if (!TicketTypeId) {
      ToastrWrapper.showError(
        'Please select at least one option for Ticket Type or Subtype'
      );
      return;
    }

    if (!TicketStatuses) {
      ToastrWrapper.showError('Please select at least one option for Status');
      return;
    }

    stableDispatch(getEventTickets(filter));
    stableDispatch(onSubmitFilterModel(filter));
  }, [filter, stableDispatch]);

  const updateFilter = (key, value) => {
    const newFilter = update(filter, {
      [key]: {
        $set: value
      }
    });
    setFilter(newFilter);
  };

  const handleOnChange = (select, key) => {
    const { value } = select;
    updateFilter(key, value);
  };

  const handleSelectTicketType = value => {
    const ticketTypeInString = value.join(',');
    updateFilter('TicketTypeId', ticketTypeInString);
  };

  const handleSelectStatuses = selectStatuses => {
    setSelectedStatuses(selectStatuses);
    const statusesInString = selectStatuses.map(s => s.value).join(',');
    updateFilter('TicketStatuses', statusesInString);
  };

  useEffect(() => {
    (async () => {
      const { parents, children } = await getTicketTypeOption();
      const { parentOption, childrenOption, emptyOption } =
        buildTicketTypeFilter(parents, children);
      const initTicketTypeValue = [...childrenOption, ...emptyOption].map(
        i => i.value
      );
      setDefaultOption(initTicketTypeValue);
      defaultValue.TicketTypeId = initTicketTypeValue.join(',');
      defaultValue.FromDateFilter = Utils.formatDate(fromDateDefault);
      defaultValue.ToDateFilter = Utils.formatDate(toDateDefault);
      setFilter({ ...defaultValue });

      setTicketTypeOption([...parentOption, ...childrenOption, ...emptyOption]);
      stableDispatch(getEventTickets(defaultValue));
      stableDispatch(onSubmitFilterModel(defaultValue));
      setLoading(false);
    })();
  }, [stableDispatch, defaultValue]);

  if (loading) {
    return <Loading />;
  }

  return (
    <>
      <DateRangeFilter updateFilter={updateFilter} />
      <div className='search-text'>
        <div className='label'>Event</div>
        <input
          className='input-text-event'
          placeholder='League/Match/Subject'
          defaultValue={filter.Keyword}
          onChange={e => handleOnChange(e.target, 'Keyword')}
        />
      </div>
      <div className='search-text'>
        <div className='label'>Ticket ID</div>
        <input
          className='input-text-ticketId'
          onChange={e => handleOnChange(e.target, 'TicketId')}
        />
      </div>
      <div className='ticket-type-dropdown'>
        <div className='show-by-filter-control'>
          <div className='label'>Ticket Type - Subtype</div>
          <TicketTypeFilter
            defaultValue={defaultOption}
            options={ticketTypeOption}
            onSelectedChange={select => handleSelectTicketType(select)}
          />
        </div>
      </div>
      <div className='multi-select-dropdown'>
        <div className='label'>Status</div>
        <MultiSelect
          className='status'
          options={STATUS}
          value={selectedStatuses}
          disableSearch
          ClearSelectedIcon={null}
          onChange={handleSelectStatuses}
        />
      </div>
      <div className='market-dropdown'>
        <MarketFilter
          onSelectedChange={select => handleOnChange(select, 'MarketKey')}
        />
      </div>
      <button className='button-submit' onClick={handleSubmit}>
        Search
      </button>
    </>
  );
};

FilterTicketCenter.propTypes = propTypes;
export default FilterTicketCenter;
