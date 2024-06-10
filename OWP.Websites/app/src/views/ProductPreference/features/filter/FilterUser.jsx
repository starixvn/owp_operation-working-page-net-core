import { useState } from 'react';
import { FILTER_KEY } from '../../Const.json';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { setFilter } from './filterSlice';

const FilterUser = () => {
  const [value, setValue] = useState('');
  const dispatch = useStableDispatch();
  const handleChangeUserFilter = event => {
    setValue(event.target.value);
    dispatch(
      setFilter({
        key: FILTER_KEY.USER,
        value: event.target.value
      })
    );
  };
  return (
    <div className='filter-control user'>
      <span>User </span>
      <input
        className='input-field'
        placeholder='Account Name or Nick Name'
        value={value}
        onChange={handleChangeUserFilter}
      />
    </div>
  );
};

export default FilterUser;