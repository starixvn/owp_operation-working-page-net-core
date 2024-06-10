import AddEvent from './AddEvent';
import store from '../app/store';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import { setNewEventInfo } from './newEventSlice';
import { TDataSource } from '../type/type';
import queryDataTableThunk from '../table/queryDataTableThunk';

const NewSpecialEvent = () => {
  const dispatch = useAppDispatch();
  const { policy, filter } = store.getState();
  const { entities: { Odds } } = policy;
  const { filterState: { selectedDate }, filterOptions: { MatchStatus } } = filter;
  const { event } = useAppSelector(x => x.newEvent);

  const onSaved = () => {
    dispatch(setNewEventInfo({} as TDataSource));
    dispatch(queryDataTableThunk());
  };
  return event.OWPMatchId
    ? <AddEvent
      oddsPolicy={Odds}
      parentEvent={event}
      selectedDate={selectedDate}
      matchStatuses={MatchStatus}
      policy={policy}
      onToggleModal={() => dispatch(setNewEventInfo({} as TDataSource))}
      onSaved={onSaved}
    />
    : null;
};


export default NewSpecialEvent;