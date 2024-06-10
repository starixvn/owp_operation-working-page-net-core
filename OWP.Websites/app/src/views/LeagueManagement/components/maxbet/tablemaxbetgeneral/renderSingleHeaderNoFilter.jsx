import { Column, ColumnGroup, Cell } from 'fixed-data-table-2';
import renderCellFactory from './renderCellFactory.native';

const renderSingleHeaderNoFilter = (dataSource, headerInfo, fixed) => {
  const { columnKey, headerName, width, color } = headerInfo;
  return (
    <ColumnGroup
      fixed={fixed}
      key={columnKey}
      header={<Cell className={`header ${color}`} />}>
      <Column
        columnKey={columnKey}
        fixed={fixed}
        header={<Cell className={`header ${color}`}>{headerName}</Cell>}
        cell={
          ({ rowIndex }) =>
            renderCellFactory(dataSource, columnKey, rowIndex)
        }
        width={width}
      />
    </ColumnGroup>
  );
};

export default renderSingleHeaderNoFilter;
