import CustomTable from '../../Shared/tableCommon/CustomTable';
import { useAppSelector } from '../app/hooks';
import TableHeader from './tableHeader/TableHeader';
import TableBody from './tableBody/TableBody';
import Loading from '../../components/Loading';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { isEqual } from 'lodash';
import { itemRowHeight } from './TableSchema.json';

const EventPickingTableEditor = () => {
  const model = useAppSelector(state => state.eventPickingTable);
  const { entities, loading } = model;
  return (
    <>
      {isEqual(loading, REQUEST_STATUS.pending) && <Loading />}
      <table>
        <TableHeader />
        <TableBody data={entities}
          displayStart={0}
          displayEnd={entities.length - 1} />
      </table>

    </>
  );
};
export default EventPickingTableEditor;
