import { NoOfferLive } from '../../../constants/MaxbetConst';
import MultipleSelectionTemplate from './MultipleSelectionTemplate';

const OfferLiveFilter = () => {
  return (
    <MultipleSelectionTemplate
      label='No Offer Live'
      options={NoOfferLive}
      value={NoOfferLive}
      filterKey='OfferLive'
      disabled={false}
    />
  );
};

export default OfferLiveFilter;
