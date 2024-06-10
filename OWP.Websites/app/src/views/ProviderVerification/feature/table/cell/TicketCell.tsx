import TicketCellCommon from '../../../../Shared/ticketEvent/component/TicketCellCommon';
import { useAppDispatch } from '../../../app/hookNative';
import { TTicketCell } from '../../../type/table';
import { setDataTable } from '../tableSlice';

const TicketCell = ({ rowIndex, dataSource }: TTicketCell) => {
  const stableDispatch = useAppDispatch();

  return (
    <TicketCellCommon
      dataSource={dataSource}
      rowIndex={rowIndex}
      updateDataSources={data => stableDispatch(setDataTable(data))}
    />
  );
};

export default TicketCell;
