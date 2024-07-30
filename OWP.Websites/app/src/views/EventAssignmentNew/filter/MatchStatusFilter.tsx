import { MultiSelect } from 'react-multi-select-component';
import { useAppSelector, useAppDispatch } from '../app/hooks';
import Utils from '../../Shared/Utils';
import { updateFilterState } from './filterSlice';
import { TOptions } from '../type/type';

const MatchStatusFilter = () => {
  const dispatch = useAppDispatch();
  const matchStatus = useAppSelector(
    state => state.filter.filterOptions.MatchStatus
  );
  const selectedMatchStatus = useAppSelector(
    state => state.filter.filterState.selectedMatchStatus
  );

  const matchStatusOptions = Utils.buildMatchStatusOption(matchStatus);
  const handleOnChange = (value: TOptions[]) => {
    dispatch(updateFilterState({
      key: 'selectedMatchStatus',
      value
    }));
  };

  return (
    <div className='filter-control multiselect'>
      <div className='label'>Match Status</div>
      <MultiSelect
        options={matchStatusOptions}
        disableSearch
        ClearSelectedIcon={null}
        value={selectedMatchStatus}
        onChange={handleOnChange}
        labelledBy={''}
      />
    </div>
  );
};
export default MatchStatusFilter;
