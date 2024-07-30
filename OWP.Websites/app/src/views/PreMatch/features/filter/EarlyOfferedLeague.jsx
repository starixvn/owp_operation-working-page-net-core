import { useState, useCallback } from 'react';
import { MultiSelect } from 'react-multi-select-component';
import { useDispatch } from 'react-redux';

import { EARLY_OFFERED } from '../../const/const';
import { updateFilterEarlyOffered } from '../filter/filterSlice';

const EarlyOfferedLeague = () => {
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);
  const [selected, setSelected] = useState(EARLY_OFFERED);

  const handleChange = newMatchOffer => {
    setSelected(newMatchOffer);
    stableDispatch(updateFilterEarlyOffered(newMatchOffer.map(x => x.value)));
  };

  return (
    <div className='multi-select-dropdown'>
      <div className='label'>Early Offered League</div>
      <MultiSelect
        options={EARLY_OFFERED}
        value={selected}
        disableSearch
        ClearSelectedIcon={null}
        onChange={handleChange}
      />
    </div>
  );
};

export default EarlyOfferedLeague;
