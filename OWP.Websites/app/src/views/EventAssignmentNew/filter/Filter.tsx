import { useAppSelector } from '../app/hooks';
import DatePickerFilter from './DatePickerFilter';
import UserSuggestionFilter from './UserSuggestionFilter';
import ButtonGroupFilter from './ButtonGroupFilter';
import { REQUEST_STATUS } from '../../Shared/Constants';
import Loading from '../../components/Loading';
import { isEquals } from 'immutability-helper';
import MatchStatusFilter from './MatchStatusFilter';
import SyncLiveFilter from './SyncLiveFilter';
import OffVRFilter from './OffVRFilter';
import MatchTypeFilter from './MatchTypeFilter';
import ControlRightFilter from './ControlRightFilter';
import ShowUnreadMessage from './ShowUnreadMessage';

const Filter = () => {
  const loading = useAppSelector(state => state.filter.loading);

  if (isEquals(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }
  return (
    <>
      <DatePickerFilter />
      <UserSuggestionFilter />
      <MatchTypeFilter />
      <MatchStatusFilter />
      <SyncLiveFilter />
      <OffVRFilter />
      <ButtonGroupFilter />
      <ShowUnreadMessage />
      <ControlRightFilter />
    </>
  );
};

export default Filter;
