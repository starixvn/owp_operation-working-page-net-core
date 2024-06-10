import { useCallback } from 'react';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';
import { FILTER_KEY } from '../../Const.json';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { setFilter } from './filterSlice';
import store from '../../app/storeNative';

const { ROLE_ID } = FILTER_KEY;
const FilterRole = () => {
  const dispatch = useStableDispatch();
  const { filter } = store.getState();
  const { roleOption } = filter.dropdownOptions;

  const roleOptionCustom = roleOption.map(item =>
    item.value === 0 ? { ...item, label: 'None' } : item
  );

  const handleChange = useCallback(
    event => {
      dispatch(
        setFilter({
          key: ROLE_ID,
          value: event.map(e => e.value)
        })
      );
    },
    [dispatch]
  );

  return (
    <MultipleSelectionTemplate
      label='Role'
      options={roleOptionCustom}
      value={roleOption}
      disabled={false}
      handleChange={handleChange}
    />
  );
};

export default FilterRole;
