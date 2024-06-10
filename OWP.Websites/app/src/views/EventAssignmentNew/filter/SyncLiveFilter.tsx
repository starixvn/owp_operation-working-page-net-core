import Select from 'react-select';
import { SYNC_LIVE_OPTION } from './FilterConst.json';
import { useAppSelector, useAppDispatch } from '../app/hooks';
import { TOptions } from '../type/type';
import { updateFilterState } from './filterSlice';

const SyncLiveFilter = () => {
  const dispatch = useAppDispatch();
  const selectedSyncLive = useAppSelector(
    state => state.filter.filterState.selectedSyncLive
  );

  const handleOnChange = (value: TOptions) => {
    const model = {
      key: 'selectedSyncLive',
      value
    };
    dispatch(updateFilterState(model));
  };
  return (
    <div className='filter-control multiselect sync-live'>
      <div className='label'>Sync Live</div>
      <Select
        defaultValue={selectedSyncLive}
        className='basic-multi-select'
        classNamePrefix='select'
        options={SYNC_LIVE_OPTION}
        onChange={handleOnChange}
      />
    </div>
  );
};
export default SyncLiveFilter;
