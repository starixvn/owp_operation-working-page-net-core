
import { useEffect } from 'react';
import useStableDispatch from '../../../../Shared/customHook/useStableDispatch';
import { sumProvider } from '../summarizeProviderSlice';
import { useAppSelector } from '../../../app/hookNative';

const SummarizeProviderTable = () => {
  const dispatch = useStableDispatch();
  const { dataSource, agentConfirmOption } = useAppSelector(state => state.events);
  const sumTable = useAppSelector(state => state.providerSum.sumTable);

  useEffect(() => {
    dispatch(sumProvider({
      data: dataSource,
      agentOptions: agentConfirmOption
    }));
  }, [dataSource, agentConfirmOption]);

  return (
    <table className='sum-provider-table'>
      <thead>
        <tr className='header-table'>
          <th className='left'>Provider</th>
          <th className='right'>Sum</th>
        </tr>
      </thead>
      <tbody>
        <tr ><td>&nbsp; </td></tr>
        {sumTable.length && sumTable.map((x, index) => {
          return (
            <tr key={index}>
              <td className='left'>{x.agentName}</td>
              <td className='right'>{x.count}</td>
            </tr>
          );
        })}
        <tr><td>&nbsp; </td></tr>
      </tbody>
      <tfoot>
        <tr className='total'>
          <td className='left'>Total</td>
          <td className='right'>{sumTable.reduce((sum, agent) => sum + agent.count, 0)}</td>
        </tr>
      </tfoot>
    </table >
  );
};

export default SummarizeProviderTable;