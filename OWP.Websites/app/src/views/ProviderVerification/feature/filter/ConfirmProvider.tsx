import { Option } from '../../../Shared/MultipleSelection/type';
import { useAppDispatch, useAppSelector } from '../../app/hookNative';
import store from '../../app/storeNative';
import { MultiSelect } from 'react-multi-select-component';
import { updateFilter } from './FilterSlice';

const ConfirmProvider = () => {
  const dispatch = useAppDispatch();
  const { confirmProviderOption } = store.getState().common.confirmProvider;
  const confirmProvider = useAppSelector(
    s => s.filterState.filter.confirmProvider
  );

  const handleChange = (value: Option[]) => {
    const model = {
      key: 'confirmProvider',
      value
    };
    dispatch(updateFilter(model));
  };

  return (
    <div className='filter-control multiselect'>
      <div className='label'>Confirm Provider</div>
      <MultiSelect
        options={confirmProviderOption}
        disableSearch
        ClearSelectedIcon={null}
        value={confirmProvider}
        onChange={handleChange}
        labelledBy={''}
      />
    </div>
  );
};

export default ConfirmProvider;
