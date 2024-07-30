import ConditionColumn from './ConditionColumn';
import { uniqueId } from 'lodash';
import useStore from '../../app/store';

const TableConditionConfig = () => {
  const conditionData = useStore(state => state.conditionData);
  return (
    <>
      <table className='condition-table'>
        <thead>
          <tr>
            <th>Level</th>
            <th>Condition</th>
            <th>Time range</th>
            <th>Reward</th>
          </tr>
        </thead>
        <tbody>
          {conditionData.map((x, index) => (
            <tr key={uniqueId()}>
              <td>{x.level}</td>
              {index === 0 ? (
                <td className='condition-column'>
                  <div className='condition-element-new-level'>
                    <span>Virtual Matches + Multiple Matches</span>
                    <span>{`>=`}</span>
                    <span>{x.conditionFirst.totalVirtualMultipleMatches}</span>
                  </div>
                </td>
              ) : (
                <ConditionColumn
                  conditionFirst={x.conditionFirst}
                  conditionSecond={x.conditionSecond}
                />
              )}
              <td>{x.timeRange}</td>
              <td>{x.reward}</td>
            </tr>
          ))}
        </tbody>
      </table>
      <div className='note-condition'>
        <p className='note'>Note:</p>
        <p className='note-content'>
          <span className='box-note'></span>
          <span className='text-note'>
            Win the average of two conditions, almost win the target level
          </span>
        </p>
      </div>
    </>
  );
};

export default TableConditionConfig;
