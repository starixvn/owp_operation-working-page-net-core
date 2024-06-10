import { ColumnGroup, Cell } from 'fixed-data-table-2';

const renderGroupHeader = (headerInfo, renderSubheaderCallback) => {
  const { columnKey, color, headerName, width, fixed } = headerInfo;
  return (
    <ColumnGroup
      key={columnKey}
      fixed={fixed}
      header={
        <Cell width={width} className={`header ${color}`}>
          {headerName}
        </Cell>
      }>
      {renderSubheaderCallback()}
    </ColumnGroup>
  );
};

export default renderGroupHeader;
