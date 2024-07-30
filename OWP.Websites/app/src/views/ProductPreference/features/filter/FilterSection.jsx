import { useCallback } from 'react';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';
import { FILTER_KEY } from '../../Const.json';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { setFilter } from './filterSlice';
import store from '../../app/storeNative';

const { SECTION_ID } = FILTER_KEY;
const FilterSection = () => {
  const dispatch = useStableDispatch();
  const { filter } = store.getState();
  const { sectionOption } = filter.dropdownOptions;
  const handleChange = useCallback(
    event => {
      dispatch(
        setFilter({
          key: SECTION_ID,
          value: event.map(e => e.value)
        })
      );
    },
    [dispatch]
  );

  return (
    <MultipleSelectionTemplate
      label='Section'
      options={sectionOption}
      value={sectionOption}
      disabled={false}
      handleChange={handleChange}
    />
  );
};

export default FilterSection;
