import TableHeader from './TableHeader';
import { isEqual } from 'lodash';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import TableBody from './TableBody';
import { useSelector } from 'react-redux';
import { selectDataTaskList } from '../selector/selector';

const TaskListTable = () => {
  const { matches, loading, hasDetailInformationPermission } = useSelector(
    selectDataTaskList
  );

  if (isEqual(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return (
    <>
      <table>
        <tbody>
          <TableHeader />
          <TableBody
            hasDetailInformationPermission={hasDetailInformationPermission}
            dataSources={matches}
          />
        </tbody>
      </table>
      <span className='total-record'>Total records: {matches.length}</span>
    </>
  );
};

export default TaskListTable;
