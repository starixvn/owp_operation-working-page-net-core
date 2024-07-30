import { useState, KeyboardEvent } from 'react';
import useStore from '../../app/useStoreNative';
import { shallow } from 'zustand/shallow';

const SearchAssignee = () => {
  const filterByAssignee = useStore(state => state.filterByAssignee, shallow);

  const [keyword, setKeyword] = useState('');

  const handleKeyDown = (event: KeyboardEvent<HTMLInputElement>) =>
    event.key === 'Enter' && filterByAssignee(keyword);

  return (
    <div className='search-assignee'>
      <input
        type='text'
        className='input-field'
        value={keyword}
        onChange={event => setKeyword(event.target.value)}
        onKeyDown={handleKeyDown}
        placeholder='Search Assignees'
      />
    </div>
  );
};

export default SearchAssignee;
