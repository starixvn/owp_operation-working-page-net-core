import { TTotalGameConditionInfo } from '../type/TableType.native';
import { keyOfTotalGameInfo } from '../../TableConst.json';
import { uniqueId } from 'lodash';

type ConditionColumnProps = {
  conditionFirst: TTotalGameConditionInfo;
  conditionSecond: TTotalGameConditionInfo;
};
const ConditionColumn = (props: ConditionColumnProps) => {
  const { conditionFirst, conditionSecond } = props;
  const keyGenerate = keyOfTotalGameInfo.filter(x => {
    const value = conditionFirst[x.key as keyof TTotalGameConditionInfo];
    return value > 0;
  });

  return (
    <>
      <td className='condition-column'>
        {keyGenerate.map((x, index) => {
          const value1 = conditionFirst[x.key as keyof TTotalGameConditionInfo];
          const value2 =
            conditionSecond[x.key as keyof TTotalGameConditionInfo];
          return (
            <div key={uniqueId()} className='condition-element'>
              <span>{x.label}</span>
              <span>{value1}</span>
              {index === 0 ? <span>or</span> : <span></span>}
              <span>{value2}</span>
            </div>
          );
        })}
      </td>
    </>
  );
};
export default ConditionColumn;
