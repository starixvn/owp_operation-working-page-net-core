import { Column, ColumnGroup, Cell } from 'fixed-data-table-2';
import renderCellFactory from './renderCellFactory.native';
import FilterHeader from './FilterHeader';
import { getKeyword } from '../../../helper/filterHelper.native';
import { v4 as uuidv4 } from 'uuid';

const renderSubHeader = (dataSource, filterInfo, subHeaderInfo) => {
  const { columnKey, color, headerName } = subHeaderInfo;
  const keyWord = getKeyword(filterInfo, columnKey);
  return (
    <Column
      columnKey={columnKey}
      header={
        <Cell className={`sub-header ${color}`}>
          <FilterHeader
            key={uuidv4()}
            keyword={keyWord}
            filterKey={columnKey}
            title={headerName}
          />
        </Cell>
      }
      flexGrow={1}
      cell={
        ({ rowIndex }) =>
          renderCellFactory(dataSource, columnKey, rowIndex)
      }
      width={subHeaderInfo.width}
    />
  );
};

const renderGroupHeaderHasFilter = (dataSource, filterInfo, headerInfo) => {
  const { columnKey, color, headerName } = headerInfo;
  const { min, max } = headerInfo.subHeader;
  return (
    <ColumnGroup
      key={columnKey}
      header={<Cell className={`header ${color}`}>{headerName}</Cell>}>
      {renderSubHeader(dataSource, filterInfo, min)}
      {renderSubHeader(dataSource, filterInfo, max)}
    </ColumnGroup>
  );
};

export default renderGroupHeaderHasFilter;
