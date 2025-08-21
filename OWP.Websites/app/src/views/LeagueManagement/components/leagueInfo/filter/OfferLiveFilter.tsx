import { IS_OFFERLIVE_OPTIONS } from './filterConst.json';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';


const OfferLiveFilter = () => {
  return (
      <MultipleSelectionTemplate
        label='Offer Live'
        options={IS_OFFERLIVE_OPTIONS}
        value={IS_OFFERLIVE_OPTIONS}
        filterKey='IsOfferLive'
        disabled={false} />
  );
};

export default OfferLiveFilter;
