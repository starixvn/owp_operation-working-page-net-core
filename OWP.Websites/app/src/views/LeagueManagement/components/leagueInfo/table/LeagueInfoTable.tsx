import TableHeader from './TableHeader';
import CustomTable from '../../../../Shared/tableCommon/CustomTable';
import { itemRowHeight } from './TableSchema.json';
import TableBody from './TableBody';
import { useAppSelector } from '../app/hooks';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import Loading from '../../../../components/Loading';
const LeagueInfoTable = () => {
  const model = useAppSelector(state => state.leagueInfoTableSlice);
  const { entities, loading } = model;
  return (
    <>
      {isEqual(loading, REQUEST_STATUS.pending) && <Loading />}
      <CustomTable
        data={entities}
        tableHeader={<TableHeader />}
        classNameTable={'table-league-management'}
        itemRowHeight={itemRowHeight}>
        <TableBody />
      </CustomTable>
    </>
  );
};

export default LeagueInfoTable;
