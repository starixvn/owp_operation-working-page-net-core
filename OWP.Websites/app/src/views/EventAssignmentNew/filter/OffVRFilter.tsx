import { MultiSelect } from 'react-multi-select-component';
import { useAppDispatch, useAppSelector } from '../app/hooks';
import { TOptions } from '../type/type';
import Utils from '../../Shared/Utils';
import { updateFilterState } from './filterSlice';

const OffVRFilter = () => {
  const dispatch = useAppDispatch();
  const offVR = useAppSelector(state => state.filter.filterOptions.OffVR);
  const selectedOffVRs = useAppSelector(
    state => state.filter.filterState.selectedOffVRs
  );
  const offVROptions: TOptions[] = Utils.buildOffVROption(offVR);
  const handleOnChange = (value: TOptions[]) => {
    const model = {
      key: 'selectedOffVRs',
      value
    };
    dispatch(updateFilterState(model));
  };
  return (
    <div className='filter-control multiselect off-vr'>
      <div className='label'>OFF VR</div>
      <MultiSelect
        options={offVROptions}
        disableSearch
        ClearSelectedIcon={null}
        value={selectedOffVRs}
        onChange={handleOnChange}
        labelledBy={''}
      />
    </div>
  );
};
export default OffVRFilter;
