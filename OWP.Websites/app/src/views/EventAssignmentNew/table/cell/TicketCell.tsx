import { TTicketCell } from '../../type/type';
import TicketCellCommon from '../../../Shared/ticketEvent/component/TicketCellCommon';
import { useAppDispatch } from '../../app/hooks';
import { setDataTable } from '../eventAssignmentTableSlice';
const TicketCell = (props: TTicketCell) => {
  const stableDispatch = useAppDispatch();
  const { dataSource, rowIndex, onShowTicketList } = props;

  return (
    <TicketCellCommon
      dataSource={dataSource}
      rowIndex={rowIndex}
      onShowTicketList={onShowTicketList}
      updateDataSources={data => stableDispatch(setDataTable(data))}
    />
  );
};

export default TicketCell;
