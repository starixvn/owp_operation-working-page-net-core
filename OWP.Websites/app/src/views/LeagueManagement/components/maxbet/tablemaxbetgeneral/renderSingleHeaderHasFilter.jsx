import { Column, ColumnGroup, Cell } from 'fixed-data-table-2';
import renderCellFactory from './renderCellFactory.native';
import FilterHeader from './FilterHeader';
import { getKeyword } from '../../../helper/filterHelper.native';
import { v4 as uuidv4 } from 'uuid';

const renderSingleHeaderHasFilter = (
  dataSource,
  filterInfo,
  headerInfo,
  fixed
) => {
  const { columnKey, headerName, width, color } = headerInfo;
  const keyWord = getKeyword(filterInfo, columnKey);
  return (
    <ColumnGroup
      fixed={fixed}
      key={columnKey}
      header={<Cell className={`header ${color}`} />}>
      <Column
        columnKey={columnKey}
        fixed={fixed}
        header={
          <Cell className={`header ${color}`}>
            <FilterHeader
              key={uuidv4()}
              keyword={keyWord}
              filterKey={columnKey}
              title={headerName}
            />
          </Cell>
        }
        cell={
          ({ rowIndex }) =>
            renderCellFactory(dataSource, columnKey, rowIndex)
        }
        width={width}
      />
    </ColumnGroup>
  );
};

export default renderSingleHeaderHasFilter;
