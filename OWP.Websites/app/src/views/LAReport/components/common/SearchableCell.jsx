import { useState, useEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import FilterIcon from 'mdi-react/FilterIcon';
import SearchableCellBody from './SearchableCellBody';
import { isEqual } from 'lodash';

const propTypesFilterHeader = {
  columnId: PropTypes.string.isRequired,
  columnName: PropTypes.string.isRequired,
  onSearch: PropTypes.func.isRequired,
  onClearAllFilters: PropTypes.func.isRequired,
  isClearALL: PropTypes.bool
};

const SearchableCell = ({
  columnId,
  columnName,
  onSearch,
  onClearAllFilters,
  isClearALL
}) => {
  const [isOpening, setIsOpening] = useState(false);
  const [searchKey, setSearchKey] = useState('');
  const [isSearching, setIsSearching] = useState(false);
  const ref = useRef(null);
  const handleClickSearch = () => {
    setIsSearching(true);
    setIsOpening(false);
    onSearch({ columnId, searchKey });
  };

  const onKeyPress = event => {
    if (isEqual(event.key, 'Enter') && searchKey) {
      setIsSearching(true);
      setIsOpening(false);
      onSearch({ columnId, searchKey });
    }
  };

  const handelClearAllFilters = () => {
    setSearchKey('');
    setIsSearching(false);
    setIsOpening(false);
    onClearAllFilters(true);
  };

  useEffect(() => {
    if (isClearALL) {
      setSearchKey('');
      setIsSearching(false);
      onClearAllFilters(false);
    }
  }, [onClearAllFilters, isClearALL]);

  return (
    <div className='title-header'>
      <span>{columnName}</span>
      <div className='filter-table'>
        <span
          ref={ref}
          id={columnId}
          className={`icon-filter ${isSearching ? 'is-filter' : ''}`}
          onClick={() => setIsOpening(!isOpening)}>
          <FilterIcon size={14} />
        </span>
        {isOpening && (
          <SearchableCellBody
            setIsOpening={setIsOpening}
            setSearchKey={setSearchKey}
            onKeyPress={onKeyPress}
            searchKey={searchKey}
            onClearAllFilters={handelClearAllFilters}
            onClickSearch={handleClickSearch}
            refIcon={ref}
          />
        )}
      </div>
    </div>
  );
};

SearchableCell.propTypes = propTypesFilterHeader;
export default SearchableCell;
