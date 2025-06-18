import { useEffect } from 'react';
import { MultiSelect } from 'react-multi-select-component';

const Filter = () => {
  const handleSearch = () => {
  };

  const handleChange = () => {
  };

  return (
    <div className='top-control'>
      <div className='left-control'>
        <div className='filter-control multiselect filter-lst-book-provider'>
          <div className='label'>League Group</div>
          <MultiSelect
            options={[]}
            disableSearch
            ClearSelectedIcon={null}
            value={[]}
            onChange={handleChange}
            labelledBy={''}
          />
        </div>
        <div className='filter-control btn-block'>
          <button className='button-submit' onClick={handleSearch}>
            Search
          </button>
        </div>
      </div>
    </div>
  );
};

export default Filter;
