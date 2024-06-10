import PropTypes from 'prop-types';
import { isEqual } from 'lodash';
import Select from 'react-select';
import Animated from 'react-select/animated';

const propTypes = {
  onPageChanged: PropTypes.func.isRequired,
  currentPage: PropTypes.number,
  onPageSizeChange: PropTypes.func,
  pageSizeOptions: PropTypes.array,
  isLastPage: PropTypes.bool
};

const Pagination = props => {
  const gotoPage = page => {
    const { onPageChanged } = props;
    const currentPage = Math.max(0, page);

    const paginationData = {
      currentPage
    };

    onPageChanged(paginationData);
  };

  const { pageSizeOptions } = props;
  return (
    <>
      <div className='paging-move-btn'>
        <button
          onClick={() => gotoPage(1)}
          disabled={isEqual(props.currentPage, 1)}>
          &lt;&lt; First page
        </button>
        <button
          onClick={() => gotoPage(props.currentPage - 1)}
          disabled={isEqual(props.currentPage, 1)}>
          Back
        </button>
        <button
          onClick={() => gotoPage(props.currentPage + 1)}
          disabled={props.isLastPage}>
          Next
        </button>
      </div>
      {pageSizeOptions && (
        <div className='page-size'>
          Show
          <Select
            components={Animated}
            name='page-size'
            options={pageSizeOptions}
            defaultValue={pageSizeOptions[0]}
            className='basic-multi-select'
            classNamePrefix='select'
            onChange={props.onPageSizeChange}
            menuPlacement='auto'
          />
          entries
        </div>
      )}
    </>
  );
};

Pagination.propTypes = propTypes;
export default Pagination;
