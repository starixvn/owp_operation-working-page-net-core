import { Column, Cell } from 'fixed-data-table-2';
import renderCellFactory from './cell/renderCellFactory.native';
import { v4 as uuidv4 } from 'uuid';

const renderColumn = (dataSource, headerInfo) => {
  const { headerName, fixed, columnKey, color, width } = headerInfo;
  return (
    <Column
      key={uuidv4()}
      columnKey={columnKey}
      fixed={fixed}
      header={<Cell className={`header ${color}`}>{headerName}</Cell>}
      cell={
        ({ rowIndex }) =>
          renderCellFactory({ dataSource, columnKey, rowIndex })
      }
      width={width}
    />
  );
};

export default renderColumn;
