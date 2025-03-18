import TicketCellCommon from '../../../../Shared/ticketEvent/component/TicketCellCommon';
import { useAppDispatch } from '../../../app/hooks';
import { TTicketCell } from '../../../type/table';
import { setDataTable } from '../tableSlice';

const TicketCell = ({ rowIndex, dataSource }: TTicketCell) => {
  const stableDispatch = useAppDispatch();

  const transformedDataSource = dataSource.map(item => ({
    ...item,
    HasTicket: item.hasTicket,
    IsRead: item.isRead,
    Home: item.home,
    Away: item.away,
    EventDateTime: item.eventDateTime,
    OWPMatchId: item.owpMatchId,
    League: item.league,
  }));

  return (
    <TicketCellCommon
      dataSource={transformedDataSource}
      rowIndex={rowIndex}
      updateDataSources={data => stableDispatch(setDataTable(data))}
    />
  );
};

export default TicketCell;
