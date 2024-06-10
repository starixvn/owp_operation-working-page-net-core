import { useCallback } from 'react';
import PropTypes from 'prop-types';
import SearchableCell from '../common/SearchableCell';
import { useDispatch, useSelector } from 'react-redux';
import { search, clearAllFilters } from '../../actions/action';
import { reportSelector } from '../../selector/reportSelector';

const propTypes = {
  columnId: PropTypes.string.isRequired,
  columnName: PropTypes.string.isRequired
};

const FilterHeader = ({ columnId, columnName }) => {
  const dispatch = useDispatch();
  const stableDispatch = useCallback(dispatch, [dispatch]);
  const model = useSelector(reportSelector);

  const handleSearch = data => {
    const postModel = { columnId: data.columnId, keyWord: data.searchKey };
    stableDispatch(search(postModel));
  };

  const handleClearAllFilters = isClearAll => {
    stableDispatch(clearAllFilters(isClearAll));
  };

  return (
    <SearchableCell
      columnId={columnId}
      columnName={columnName}
      onSearch={handleSearch}
      onClearAllFilters={handleClearAllFilters}
      isClearALL={model.isClearAll}
    />
  );
};

FilterHeader.propTypes = propTypes;
export default FilterHeader;
