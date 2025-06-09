import TicketCellCommon from '../../../../Shared/ticketEvent/component/TicketCellCommon';
import { setDataTable } from '../taskListSlice';
import { useAppDispatch, useAppSelector } from '../../../app/hooks';
import { selectDataTaskList } from '../../selector/selector';
import { CellProps } from '../../../type/TTaskListTable';

const TicketCell = ({ rowIndex, authToken }: CellProps) => {
  const { matches } = useAppSelector(selectDataTaskList);
  const stableDispatch = useAppDispatch();

  return (
    <div className='ticket-cell'>
      <TicketCellCommon
        allowQuickAdd={true}
        dataSource={matches}
        updateDataSources={data => stableDispatch(setDataTable(data))}
        rowIndex={rowIndex}
        authToken={authToken ?? ''}
      />
    </div>
  );
};

export default TicketCell;
