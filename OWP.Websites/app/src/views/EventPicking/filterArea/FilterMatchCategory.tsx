import {
  FILTER_MATCH_CATEGORY_OPTIONS,
  FILTER_MATCH_CATEGORY
} from './filterConst.json';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';

const FilterMatchCategory = () => {
  return (
    <MultipleSelectionTemplate
      label='Match Category'
      options={FILTER_MATCH_CATEGORY_OPTIONS}
      value={FILTER_MATCH_CATEGORY_OPTIONS}
      filterKey={FILTER_MATCH_CATEGORY}
      disabled={false}
    />
  );
};

export default FilterMatchCategory;
