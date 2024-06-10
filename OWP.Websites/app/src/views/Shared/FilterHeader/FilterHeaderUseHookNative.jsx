import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';
import FilterHeaderNative from './FilterHeaderNative';
import { uniqueId } from 'lodash';

const propTypes = {
  filterFunc: PropTypes.func.isRequired,
  clearFilterFunc: PropTypes.func
};

const FilterHeaderUseHookNative = props => {
  const { filterFunc, clearFilterFunc, ...rest } = props;
  const dispatch = useDispatch();
  return (
    <FilterHeaderNative
      key={uniqueId()}
      {...rest}
      clearFilterTitle='Clear All Filters'
      onSubmit={(columnKey, keyword) =>
        dispatch(filterFunc({ columnKey, keyword }))
      }
      onClear={columnKey => dispatch(clearFilterFunc(columnKey))}
    />
  );
};

FilterHeaderUseHookNative.propTypes = propTypes;

export default FilterHeaderUseHookNative;
