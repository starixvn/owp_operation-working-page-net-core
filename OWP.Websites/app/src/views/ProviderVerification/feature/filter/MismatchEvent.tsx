import { ChangeEvent, useState } from 'react';
import { useAppDispatch } from '../../app/hookNative';
import { updateFilter } from './FilterSlice';

const MismatchEvent = () => {
  const dispatch = useAppDispatch();
  const [isMismatchEvent, setIsMismatchEvent] = useState(false);

  const handleChange = (event: ChangeEvent<HTMLInputElement>) => {
    const model = {
      key: 'onlyShowIsMismatchEvent',
      value: event.target.checked
    };
    setIsMismatchEvent(event.target.checked);
    dispatch(updateFilter(model));
  };

  return (
    <div className='filter-control-checkbox'>
      <label>
        <input
          className='unread-checkbox'
          type='checkbox'
          checked={isMismatchEvent}
          onChange={handleChange}
        />{' '}
        Show mismatched matches
      </label>
    </div>
  );
};
export default MismatchEvent;
