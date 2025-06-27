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
            <th>Product</th>
            <th>Condition 1</th>
            <th>Condition 2</th>
            <th>Condition 3</th>
            <th>Time range</th>
            <th>Reward</th>
          </tr>
        </thead>
        <tbody>
          {conditionData.map((x, index) => (
            <tr key={uniqueId()}>
              <td>{x.level}</td>
              {index === 0 ? (
                <>
                  <td className='condition-column-title'>
                    <div className='condition-element'>
                      <span>Virtual Matches + Multiple Matches</span>
                    </div>
                  </td>
                  <td>
                    <span>{`>= `}</span>
                    <span>{x.conditionFirst.totalVirtualMultipleMatches}</span>
                  </td>
                  <td>
                    <span>{`>= `}</span>
                    <span>{x.conditionSecond.totalVirtualMultipleMatches}</span>
                  </td>
                  <td>
                    <span>Multiple Matches = </span>
                    <span>{x.conditionThird.multipleMatches}</span>
                  </td>
                </>
              ) : (
                <ConditionColumn
                  conditionFirst={x.conditionFirst}
                  conditionSecond={x.conditionSecond}
                  conditionThird={x.conditionThird}
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
