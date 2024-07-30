import { forwardRef, createRef } from 'react';
import PropTypes from 'prop-types';
import { Table } from 'fixed-data-table-2';
import { isEqual } from 'lodash';
import Loading from '../../../components/Loading';
import ColumnsFactory from './TableFactory/ColumnsFactory';
import { REQUEST_STATUS } from '../../Constants';
import { calculateTableDimension } from './Helper';
import ReactDOM from 'react-dom';
import NotePopupTableCss from '../../../Shared/NotePopupTableCss';
import { withResizeDetector } from 'react-resize-detector';

const propTypes = {
  model: PropTypes.object.isRequired,
  config: PropTypes.object.isRequired
};

const CustomTable = forwardRef(({ model, config }, ref) => {
  const {
    columnSchema,
    dynamicCell,
    sortFunc,
    filterFunc,
    clearFilterFunc,
    tableStyle,
    tableWidth,
    onCalculateRowHeight,
    onRowClassNameGetter,
    onRowClick,
    collapseAllRowFunc,
    expandAllRowFunc
  } = config;
  const tableRef = createRef();
  const { rowHeight, headerHeight, defaultHeight, filterOffset } = tableStyle;
  const {
    sortState,
    filterState,
    canCheck,
    collapseExpandAllRowState,
    collapseExpandRowState
  } = model;

  columnSchema.map(item =>
    Object.assign(item, {
      sortState,
      filterState,
      collapseExpandAllRowState,
      collapseExpandRowState
    })
  );
  const handleShowTicket = show => {
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react/no-find-dom-node
    const tableElement = ReactDOM.findDOMNode(tableRef.current);
    NotePopupTableCss(tableElement, show);
  };

  const handleShowDropdownListOption = show => {
    // eslint-disable-next-line no-warning-comments
    // eslint-disable-next-line react/no-find-dom-node
    const tableElement = ReactDOM.findDOMNode(tableRef.current);
    NotePopupTableCss(tableElement, show);
  };

  const newProps = {
    dataSource: model.entities,
    columnSchema,
    canCheck,
    dynamicCell,
    sortFunc,
    sortState,
    filterFunc,
    clearFilterFunc,
    collapseAllRowFunc,
    expandAllRowFunc,
    handleShowTicket,
    onShowDropdownListOption: handleShowDropdownListOption
  };

  const { loading, entities } = model;
  const dataLength = entities.length || 0;

  const width = window.innerWidth - 110;
  const widthInfo = {
    containerWidth: width,
    width: tableWidth(columnSchema)
  };

  const height = window.innerHeight - 200;
  const heightInfo = {
    containerHeight: height,
    defaultHeight,
    filterOffset
  };

  const { actualWidth, actualHeight } = calculateTableDimension(
    widthInfo,
    heightInfo
  );

  const handleCalculateActualHeight = index => {
    return isEqual(typeof onCalculateRowHeight, 'undefined')
      ? rowHeight
      : onCalculateRowHeight(index);
  };
  const handleRowClassNameGetter = index =>
    isEqual(typeof onRowClassNameGetter, 'undefined')
      ? ''
      : onRowClassNameGetter(index, entities);

  const handleRowClick = (event, index) => {
    !isEqual(typeof onRowClick, 'undefined') &&
      onRowClick(event, index, entities);
  };

  return (
    <div className='table' ref={ref}>
      {isEqual(loading, REQUEST_STATUS.pending) && <Loading />}
      <Table
        onRowClick={handleRowClick}
        ref={tableRef}
        rowHeight={rowHeight}
        rowHeightGetter={handleCalculateActualHeight}
        rowClassNameGetter={handleRowClassNameGetter}
        bufferRowCount={20}
        rowsCount={dataLength}
        width={actualWidth}
        height={actualHeight}
        headerHeight={headerHeight}>
        {ColumnsFactory(newProps)}
      </Table>
      <div className='total'>
        <b>Total records: {dataLength}</b>
      </div>
    </div>
  );
});

CustomTable.propTypes = propTypes;
CustomTable.displayName = 'CustomTable';
export default withResizeDetector(CustomTable);
