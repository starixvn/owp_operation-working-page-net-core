import { CellProps } from '../../../type/TableType';
import AssignmentComponent from '../../../component/Assignment';
import OtherAssignment from '../../../component/OtherAssignment';
import { useSelector } from 'react-redux';
import { RootState } from '../../../app/storeNative';

const LaCell = (props: CellProps): JSX.Element => {
  const { rowIndex, dataSource } = props;
  const { Assignment } = dataSource[rowIndex];
  const { LA, LAOthers } = Assignment;
  const { userNameFilter } = useSelector((state: RootState) => state.events);
  return (
    <div className='la-cell'>
      {LA.length > 0 && (
        <AssignmentComponent assignment={LA} userNameFilter={userNameFilter} />
      )}
      {LAOthers.length > 0 && (
        <OtherAssignment
          assignment={LAOthers}
          userNameFilter={userNameFilter}
        />
      )}
    </div>
  );
};

export default LaCell;
