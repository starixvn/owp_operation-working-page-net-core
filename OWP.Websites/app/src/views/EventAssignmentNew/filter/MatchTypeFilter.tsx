import { MultiSelect } from 'react-multi-select-component';
import { MATCH_TYPE_OPTION } from './FilterConst.json';
import { useAppSelector, useAppDispatch } from '../app/hooks';
import { updateFilterState } from './filterSlice';

const MatchTypeFilter = () => {
  const dispatch = useAppDispatch();
  const selectedMatchType = useAppSelector(
    state => state.filter.filterState.selectedMatchType
  );
  const handleOnChange = (value: { label: string, value: string }) => {
    const model = {
      key: 'selectedMatchType',
      value
    };
    dispatch(updateFilterState(model));
  };

  return (
    <div className='filter-control multiselect'>
      <div className='label'>Match Type</div>
      <MultiSelect
        options={MATCH_TYPE_OPTION}
        disableSearch
        ClearSelectedIcon={null}
        value={selectedMatchType}
        onChange={handleOnChange}
        labelledBy={''}
      />
    </div>
  );
};
export default MatchTypeFilter;
