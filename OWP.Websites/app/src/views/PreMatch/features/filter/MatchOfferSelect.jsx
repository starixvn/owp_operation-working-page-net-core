import { useState, useCallback } from 'react';
import { MultiSelect } from 'react-multi-select-component';
import { useDispatch } from 'react-redux';

import { MATCH_OFFER } from '../../const/const';
import { updateFilterMatchOffers } from '../filter/filterSlice';

const MatchOfferSelect = () => {
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);

  const [selected, setSelected] = useState(MATCH_OFFER);

  const handleChange = newMatchOffer => {
    setSelected(newMatchOffer);
    stableDispatch(updateFilterMatchOffers(newMatchOffer.map(x => x.value)));
  };

  return (
    <div className='multi-select-dropdown'>
      <div className='label'>Match Offer</div>
      <MultiSelect
        options={MATCH_OFFER}
        value={selected}
        disableSearch
        ClearSelectedIcon={null}
        onChange={handleChange}
      />
    </div>
  );
};

export default MatchOfferSelect;
