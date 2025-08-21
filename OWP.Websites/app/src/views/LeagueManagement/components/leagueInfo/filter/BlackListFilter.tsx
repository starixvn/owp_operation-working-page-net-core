import { IS_BLACKLIST_OPTIONS } from './filterConst.json';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';


const BlackListFilter = () => {
  return (
      <MultipleSelectionTemplate
        label='Blacklist'
        options={IS_BLACKLIST_OPTIONS}
        value={IS_BLACKLIST_OPTIONS}
        filterKey='IsBackList'
        disabled={false} />
  );
};

export default BlackListFilter;
