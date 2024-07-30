// eslint-disable-next-line no-warning-comments
/* eslint-disable react/no-unescaped-entities */
import { Table } from 'reactstrap';
import { useSelector } from 'react-redux';
import { selectLogHistoryState } from '../../../../selector/SelectorSpecialProduct';
import RenderBody from './RenderBody';
import Loading from '../../../../../components/Loading';

const HistoryTable = () => {
  const state = useSelector(selectLogHistoryState);
  if (state.Pending) {
    return <Loading />;
  }
  return (
    <Table>
      <thead>
        <tr>
          <th className='product'>Product's Setting</th>
          <th className='action'>Action</th>
          <th className='old-value'>Old Value</th>
          <th className='new-value'>New Value</th>
          <th className='modifyDate'>Last Modify</th>
          <th className='modifyBy'>By</th>
        </tr>
      </thead>
      <RenderBody dataRows={state.Data} />
    </Table>
  );
};

export default HistoryTable;
