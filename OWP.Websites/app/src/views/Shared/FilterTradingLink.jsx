import { useState } from 'react';
import PropTypes from 'prop-types';
import FilterIcon from 'mdi-react/FilterIcon';
import CloseIcon from 'mdi-react/CloseIcon';
import { Button } from 'reactstrap';
import TradingMenuPopup from './TradingMenuPopup';

const FilterTradingLink = ({
  name,
  title,
  keyword,
  onClear,
  onSubmit,
  clearFilterTitle = 'Clear All Filters',
  firstMatchCheckedIsSaba,
  matchesSelected
}) => {
  const [isOpen, setIsOpen] = useState(false);
  const [value, setValue] = useState(keyword || '');

  const onHandleClickOpen = () => setIsOpen(!isOpen);
  const onHandleTextChange = event => setValue(event.target.value);
  const onHandleClearTextFilter = () => {
    setIsOpen(false);
    setValue('');
    onClear(name);
  };
  const onHandleClickSubmit = () => {
    setIsOpen(false);
    onSubmit(name, value);
  };
  const onKeyPress = event => {
    if (event.key === 'Enter') {
      onHandleClickSubmit();
    }
  };

  return (
    <div className='title-header'>
      <span>{title}</span>
      <TradingMenuPopup
        firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}
        matchesSelected={matchesSelected}
      />
      <div className='filter-table'>
        <span
          id={name}
          className={`icon-filter ${keyword ? 'is-filter' : ''}`}
          onClick={onHandleClickOpen}>
          <FilterIcon size={14} />
        </span>
        {isOpen && (
          <div className='filter-popup filter-popup is-show'>
            <span className='close-popup' onClick={onHandleClickOpen}>
              <CloseIcon size={14} />
            </span>
            <input
              className='input-field'
              placeholder='Search'
              value={value}
              autoFocus
              onChange={onHandleTextChange}
              onKeyPress={onKeyPress}
            />
            <span className='clear-all' onClick={onHandleClearTextFilter}>
              {clearFilterTitle}
            </span>
            <Button
              className='btn-search'
              disabled={!value}
              onClick={onHandleClickSubmit}>
              Search
            </Button>
          </div>
        )}
      </div>
    </div>
  );
};

FilterTradingLink.propTypes = {
  name: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired,
  keyword: PropTypes.string.isRequired,
  onClear: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
  clearFilterTitle: PropTypes.string,
  firstMatchCheckedIsSaba: PropTypes.bool.isRequired,
  matchesSelected: PropTypes.arrayOf(
    PropTypes.shape({
      AdminSiteId: PropTypes.oneOfType([PropTypes.string, PropTypes.number])
    })
  ).isRequired
};

export default FilterTradingLink;
