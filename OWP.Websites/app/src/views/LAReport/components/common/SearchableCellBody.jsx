import { useRef, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Button } from 'reactstrap';
import CloseIcon from 'mdi-react/CloseIcon';
import { isEqual } from 'lodash';

const propTypesBody = {
  setIsOpening: PropTypes.func.isRequired,
  setSearchKey: PropTypes.func.isRequired,
  onKeyPress: PropTypes.func.isRequired,
  onClearAllFilters: PropTypes.func.isRequired,
  onClickSearch: PropTypes.func.isRequired,
  searchKey: PropTypes.string.isRequired,
  refIcon: PropTypes.object.isRequired
};

const SearchableCellBody = props => {
  const {
    setIsOpening,
    setSearchKey,
    onKeyPress,
    searchKey,
    onClearAllFilters,
    onClickSearch,
    refIcon
  } = props;
  const areaComponent = useRef(null);
  useEffect(() => {
    const handelClickOutside = event => {
      const { target } = event;
      if (
        !areaComponent.current.contains(target) &&
        !refIcon.current.contains(target)
      ) {
        setIsOpening(false);
      }
    };
    document.addEventListener('click', handelClickOutside);
    return () => {
      document.removeEventListener('click', handelClickOutside);
    };
  }, [refIcon, setIsOpening]);

  return (
    <div ref={areaComponent} className='filter-popup filter-popup is-show'>
      <span className='close-popup' onClick={() => setIsOpening(false)}>
        <CloseIcon size={14} />
      </span>
      <input
        className='input-field'
        placeholder='Search'
        value={searchKey}
        onChange={e => setSearchKey(e.target.value)}
        onKeyPress={e => onKeyPress(e)}
      />
      <span className='clear-all' onClick={onClearAllFilters}>
        Clear All Filters
      </span>
      <Button
        className='btn-search'
        disabled={isEqual(searchKey, '')}
        onClick={onClickSearch}>
        Search
      </Button>
    </div>
  );
};

SearchableCellBody.propTypes = propTypesBody;
export default SearchableCellBody;
