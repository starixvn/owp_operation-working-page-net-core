import { Option } from '../../../Shared/MultipleSelection/type';
import { useAppDispatch, useAppSelector } from '../../app/hookNative';
import store from '../../app/storeNative';
import { MultiSelect } from 'react-multi-select-component';
import { updateFilter } from './FilterSlice';

const LSTBookProvider = () => {
  const dispatch = useAppDispatch();
  const { LSTBookProviderOption } = store.getState().common.LSTBookProvider;
  const selectedLSTBookProvider = useAppSelector(
    s => s.filterState.filter.lstBookProvider
  );

  const handleChange = (value: Option[]) => {
    const model = {
      key: 'lstBookProvider',
      value
    };
    dispatch(updateFilter(model));
  };

  return (
    <div className='filter-control multiselect filter-lst-book-provider'>
      <div className='label'>LST Book Provider</div>
      <MultiSelect
        options={LSTBookProviderOption}
        disableSearch
        ClearSelectedIcon={null}
        value={selectedLSTBookProvider}
        onChange={handleChange}
        labelledBy={''}
      />
    </div>
  );
};

export default LSTBookProvider;
