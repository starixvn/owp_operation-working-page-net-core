import { ColumnModels } from '../ConfigNative';
import LastCommentCell from './LastCommentCell';
import TicketIdCell from './TicketIdCell';
import TicketTypeCell from './TicketTypeCell';
import ConversationCell from './ConversationCell';
import ActionCell from './ActionCell';
import StatusCell from './StatusCell';

const DynamicCell = (rowIndex, cellModel) => {
  const { dataSource, columnKey } = cellModel;
  const {
    LeagueColumn,
    TicketIdColumn,
    LastCommentColumn,
    TicketTypeColumn,
    StatusColumn,
    ActionColumn
  } = ColumnModels;

  switch (columnKey) {
    case LeagueColumn.columnKey:
      return <ConversationCell dataSource={dataSource} rowIndex={rowIndex} />;
    case LastCommentColumn.columnKey:
      return <LastCommentCell dataSource={dataSource} rowIndex={rowIndex} />;
    case TicketIdColumn.columnKey:
      return <TicketIdCell dataSource={dataSource} rowIndex={rowIndex} />;
    case TicketTypeColumn.columnKey:
      return <TicketTypeCell dataSource={dataSource} rowIndex={rowIndex} />;
    case StatusColumn.columnKey:
      return <StatusCell dataSource={dataSource} rowIndex={rowIndex} />;
    case ActionColumn.columnKey:
      return <ActionCell dataSource={dataSource} rowIndex={rowIndex} />;
    default:
      return null;
  }
};

export default DynamicCell;
