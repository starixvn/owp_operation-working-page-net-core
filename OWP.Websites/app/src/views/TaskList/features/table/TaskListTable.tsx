import TableHeader from './TableHeader';
import { isEqual } from 'lodash';
import Loading from '../../../components/Loading';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import TableBody from './TableBody';
import { useSelector } from 'react-redux';
import { selectDataTaskList } from '../selector/selector';
import { useAppSelector } from '../../app/hooks';

const TaskListTable = () => {
  const { matches, loading, hasDetailInformationPermission } =
    useSelector(selectDataTaskList);

  const firstMatchCheckedIsSaba = useAppSelector(
    state => state.taskList.matchesSelected[0]?.IsSaba
  );

  if (isEqual(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  return (
    <>
      <table>
        <tbody>
          <TableHeader
            hasDetailInformationPermission={hasDetailInformationPermission}
            firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}
          />
          <TableBody
            firstMatchCheckedIsSaba={firstMatchCheckedIsSaba}
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
