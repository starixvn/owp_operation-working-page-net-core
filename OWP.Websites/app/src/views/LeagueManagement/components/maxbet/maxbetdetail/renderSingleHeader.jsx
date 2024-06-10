import { ColumnGroup, Cell } from 'fixed-data-table-2';
import renderColumn from './renderColumn';

const renderSingleHeader = (dataSource, headerInfo) => {
  const { fixed, columnKey, color } = headerInfo;
  return (
    <ColumnGroup
      fixed={fixed}
      key={columnKey}
      header={<Cell className={`sub-header ${color}`} />}>
      {renderColumn(dataSource, headerInfo)}
    </ColumnGroup>
  );
};

export default renderSingleHeader;
