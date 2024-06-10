import { CellProps } from '../../../type/TTaskListTable';
import Assignment from '../../component/Assignment';
import OtherAssignment from '../../component/OtherAssignment';

const LACell = (props: CellProps) => {
  const { dataSource } = props;
  const { LA, LAOthers } = dataSource.Assignment;

  return (
    <>
      <div className='la-cell'>
        {LA.length > 0 && <Assignment assignment={LA} />}
        {LAOthers.length > 0 && <OtherAssignment assignment={LAOthers} />}
      </div>
    </>
  );
};

export default LACell;
