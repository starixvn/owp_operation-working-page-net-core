import { useCallback } from 'react';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';
import { FILTER_KEY } from '../../Const.json';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { setFilter } from './filterSlice';
import store from '../../app/storeNative';

const { LOCATION_ID } = FILTER_KEY;
const FilterLocation = () => {
  const dispatch = useStableDispatch();
  const { filter } = store.getState();
  const { locationOption } = filter.dropdownOptions;

  const handleChange = useCallback(
    event => {
      dispatch(
        setFilter({
          key: LOCATION_ID,
          value: event.map(e => e.value)
        })
      );
    },
    [dispatch]
  );

  return (
    <MultipleSelectionTemplate
      label='Location'
      options={locationOption}
      value={locationOption}
      disabled={false}
      handleChange={handleChange}
    />
  );
};

export default FilterLocation;
