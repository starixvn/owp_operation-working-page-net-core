import { FILTER_MATCH_OPTIONS, FILTER_MATCH } from './filterConst.json';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';

const FilterMatch = () => {
  return (
    <MultipleSelectionTemplate
      label='Filter Match'
      options={FILTER_MATCH_OPTIONS}
      value={FILTER_MATCH_OPTIONS}
      filterKey={FILTER_MATCH}
      disabled={false} />
  );
};

export default FilterMatch;