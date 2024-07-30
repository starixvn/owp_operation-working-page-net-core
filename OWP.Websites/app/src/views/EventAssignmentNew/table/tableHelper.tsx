import { TABLE, COLUMN_FULL_BETTYPE } from '../const.json';
import { TBettype, TColumn, TPickCellModel, TNoteCell } from '../type/type';
import NoCell from './cell/NoCell';
import KOTimeCell from './cell/KOTimeCell';
import EventCell from './cell/EventCell';
import TicketCell from './cell/TicketCell';
import NoteCell from './cell/NoteCell';
import BettypeCell from '../components/BettypeCell';
import { onClearFilter, onColumnFilter } from './eventAssignmentTableSlice';
import IPTVCell from './cell/IPTVCell';
import ProviderCell from './cell/ProviderCell';
import MBCell from './cell/MBCell';
import RemarkCell from './cell/RemarkCell';

const {
  HEADER: {
    MATCH_NO,
    KO_TIME,
    EVENT,
    TICKET,
    NOTE,
    IP_TV,
    PROVIDER,
    MB,
    REMARK
  },
  WIDTH,
  ROW_HEIGHT,
  DEFAULT_HEIGHT,
  HEADER_HEIGHT,
  FILTER_OFFSET
} = TABLE;

const buildColumn = (column: TColumn) => {
  const { HEADER_NAME, COLUMN_KEY, IS_CAN_FILTER, IS_FREEZE } = column;
  return {
    headerName: HEADER_NAME,
    columnKey: COLUMN_KEY,
    isCanFilter: IS_CAN_FILTER,
    width: column.WIDTH,
    isFreeze: IS_FREEZE
  };
};

const buildTableInfo = () => ({
  columnSchema: [
    buildColumn(MATCH_NO),
    buildColumn(KO_TIME),
    buildColumn(EVENT),
    buildColumn(TICKET),
    buildColumn(IP_TV),
    buildColumn(PROVIDER),
    buildColumn(MB),
    buildColumn(REMARK),
    buildColumn(NOTE),
    ...COLUMN_FULL_BETTYPE.map(x => buildColumn(x))
  ],
  tableStyle: {
    rowHeight: ROW_HEIGHT,
    headerHeight: HEADER_HEIGHT,
    defaultHeight: DEFAULT_HEIGHT,
    filterOffset: FILTER_OFFSET
  },
  dynamicCell,
  filterFunc: onColumnFilter,
  clearFilterFunc: onClearFilter,
  tableWidth: () => WIDTH
});

const dynamicCell = (rowIndex: Required<number>, cellModel: TPickCellModel) => {
  const { dataSource, columnKey, handleShowTicket } = cellModel;
  const cellInfo = {
    rowIndex,
    dataSource
  };

  const noteCellInfo: TNoteCell = {
    rowIndex,
    dataSource,
    onShowNote: handleShowTicket
  };

  switch (columnKey) {
    case MATCH_NO.COLUMN_KEY:
      return <NoCell {...cellInfo} />;
    case KO_TIME.COLUMN_KEY:
      return <KOTimeCell {...cellInfo} />;
    case EVENT.COLUMN_KEY:
      return <EventCell {...cellInfo} />;
    case TICKET.COLUMN_KEY:
      return <TicketCell {...cellInfo} onShowTicketList={handleShowTicket} />;
    case NOTE.COLUMN_KEY:
      return <NoteCell {...noteCellInfo} />;
    case IP_TV.COLUMN_KEY:
      return <IPTVCell {...noteCellInfo} />;
    case PROVIDER.COLUMN_KEY:
      return <ProviderCell {...noteCellInfo} />;
    case MB.COLUMN_KEY:
      return <MBCell {...noteCellInfo} />;
    case REMARK.COLUMN_KEY:
      return <RemarkCell {...noteCellInfo} />;
  }
  return (
    <BettypeCell {...cellInfo} bettypeName={cellModel.columnKey as TBettype} />
  );
};

export { buildTableInfo };
