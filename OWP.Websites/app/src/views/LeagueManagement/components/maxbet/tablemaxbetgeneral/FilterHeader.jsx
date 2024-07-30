import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { Button } from 'reactstrap';
import CloseIcon from 'mdi-react/CloseIcon';
import FilterIcon from 'mdi-react/FilterIcon';
import {
  updateStatusDisplayPopupFilter,
  executeFilter,
  clearFilter
} from '../../../actions/action';
import { useSelector, useDispatch } from 'react-redux';
import { selectDisplayPopupFilter } from '../../../selector/SelectorMaxbetGeneral';
import isEqual from 'lodash.isequal';

const propTypes = {
  title: PropTypes.string.isRequired,
  filterKey: PropTypes.string.isRequired,
  keyword: PropTypes.string
};

const FilterHeader = ({ title, filterKey, keyword }) => {
  const [currentKeyword, setCurrentKeyword] = useState(keyword);
  const dispatch = useDispatch();
  const model = useSelector(selectDisplayPopupFilter);
  const onTogglePopup = () => {
    dispatch(
      updateStatusDisplayPopupFilter({
        filterKey,
        isDisplay: isEqual(filterKey, model.filterKey) ? !model.isDisplay : true
      })
    );
  };

  const onHandleClearTextFilter = () => {
    onTogglePopup();
    dispatch(clearFilter());
  };

  const onHandleClickSubmit = () => {
    onTogglePopup();
    dispatch(
      executeFilter({
        filterKey,
        keyword: currentKeyword
      })
    );
  };

  const onKeyPress = event => {
    if (event.key === 'Enter') {
      onHandleClickSubmit();
    }
  };

  useEffect(() => {
    return () => {
      dispatch(
        updateStatusDisplayPopupFilter({
          filterKey: '',
          isDisplay: false
        })
      );
    };
  }, [dispatch]);

  return (
    <div className='title-header'>
      <span>{title}</span>
      <div className='filter-table'>
        <span
          className={`icon-filter ${keyword ? 'is-filter' : ''}`}
          onClick={onTogglePopup}>
          <FilterIcon size={14} />
        </span>
        {isEqual(model.filterKey, filterKey) && model.isDisplay && (
          <div className='filter-popup filter-popup is-show'>
            <span className='close-popup' onClick={onTogglePopup}>
              <CloseIcon size={14} />
            </span>
            <input
              className='input-field'
              placeholder='Search'
              value={currentKeyword}
              onChange={e => setCurrentKeyword(e.target.value)}
              onKeyPress={onKeyPress}
            />
            <span className='clear-all' onClick={onHandleClearTextFilter}>
              Clear All Filters
            </span>
            <Button
              className='btn-search'
              disabled={!currentKeyword}
              onClick={onHandleClickSubmit}>
              Search
            </Button>
          </div>
        )}
      </div>
    </div>
  );
};

FilterHeader.propTypes = propTypes;
export default FilterHeader;
