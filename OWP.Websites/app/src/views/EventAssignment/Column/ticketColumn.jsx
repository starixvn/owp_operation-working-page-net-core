import { Column } from 'fixed-data-table-2';
import TicketCell from '../Cell/TicketCell';
import { Ticket } from '../constants/Column';

const renderTicketColumn = (
  dataSource,
  onShowTicketList,
  updateDataSources
) => {
  return (
    <Column
      key={Ticket.columnKey}
      columnKey={Ticket.columnKey}
      header={Ticket.title}
      width={Ticket.width}
      fixed={Ticket.fixed}
      cell={({ rowIndex }) => (
        <TicketCell
          rowIndex={rowIndex}
          dataSource={dataSource}
          onShowTicketList={onShowTicketList}
          updateDataSources={updateDataSources}
        />
      )}
    />
  );
};

export { renderTicketColumn };
