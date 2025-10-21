import { maxbetSettingTable } from './Const.json';
import { CellInfo, Column } from 'react-table';
import { ColumnModel, SubColumnModel } from './typeNative';
import Cell from './Cell';

const {
  fullTimeMaxbetRefer,
  mainMatch,
  noOfCorners,
  nextCorners,
  totalBookings,
  nextBooking,
  secondAcceptValue,
  lastUpdate
} = maxbetSettingTable;

const groupColumns = [
  mainMatch,
  noOfCorners,
  nextCorners,
  totalBookings,
  nextBooking,
  secondAcceptValue
];

const renderColumn = () => {
  const fullTimeMaxbetReferCol = renderFixedColumns(fullTimeMaxbetRefer);
  const groupCols = renderGroupColumn();
  const lastUpdatedFixedCol = renderFixedColumns(lastUpdate);
  return [
    ...fullTimeMaxbetReferCol,
    ...groupCols,
    ...lastUpdatedFixedCol
  ] as Array<Column>;
};

const renderGroupColumn = () => {
  const renderSubColumn = (
    subColumns: SubColumnModel[],
    width: number,
    headerClassName: string) => {
    return subColumns.map(subCol => {
      return {
        Header: subCol.name,
        headerClassName: `${headerClassName}-header`,
        accessor: `${subCol.id}`,
        width,
        id: `${subCol.id}`,
        Cell: (cellInfo: CellInfo) => <Cell
          value={cellInfo.original[`${subCol.id}`]}
          id={subCol.id}
          maxBetValue={cellInfo.row.fT_HDP} />
      };
    });
  };

  return groupColumns.map(column => {
    return {
      Header: column.name,
      headerClassName: `${column.id}-group-header`,
      accessor: column.name,
      id: column.id,
      columns: renderSubColumn(column.subColumn, column.subColWidth, column.headerClassName)
    };
  });
};

const renderFixedColumns = (column: ColumnModel) => {
  const renderFixedColumn = (subCol: SubColumnModel) => {
    return {
      Header: <div>{subCol.name}</div>,
      headerClassName: `${subCol.id}-header`,
      id: `${subCol.id}`,
      accessor: `${subCol.id}`,
      className: `${subCol.id}-value`,
      Cell: (cellInfo: CellInfo) => <Cell
        value={cellInfo.original[subCol.id]}
        id={subCol.id}
        maxBetValue={cellInfo.row.fT_HDP}
      />
    };
  };

  return [
    {
      Header: column.name,
      accessor: '',
      headerClassName: `${column.id}-group-header`,
      fixed: column.fixed,
      id: column.id,
      columns: [renderFixedColumn(column.subColumn[0])]
    }
  ];
};

export {
  renderColumn,
  renderGroupColumn,
  renderFixedColumns
};
