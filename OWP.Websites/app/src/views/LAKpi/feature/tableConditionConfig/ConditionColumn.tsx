import { TTotalGameConditionInfo } from '../type/TableType.native';
import { keyOfTotalGameInfo } from '../../TableConst.json';

type ConditionColumnProps = {
  conditionFirst: TTotalGameConditionInfo;
  conditionSecond: TTotalGameConditionInfo;
  conditionThird: TTotalGameConditionInfo;
};

const ConditionColumn = (props: ConditionColumnProps) => {
  const { conditionFirst, conditionSecond, conditionThird } = props;

  const keyGenerate = keyOfTotalGameInfo.filter(x => {
    const value = conditionFirst[x.key as keyof TTotalGameConditionInfo];
    return value > 0;
  });

  const renderConditionValues = (condition: TTotalGameConditionInfo) =>
    keyGenerate.map(x => {
      const value = condition[x.key as keyof TTotalGameConditionInfo];
      return (
        <div key={x.key} className='condition-element'>
          <span>{value || '-'}</span>
        </div>
      );
    });

  return (
    <>
      <td className='condition-column-title'>
        {keyGenerate.map(x => (
          <div key={x.key} className='condition-element'>
            <span>{x.label}</span>
          </div>
        ))}
      </td>
      <td className='condition-column'>{renderConditionValues(conditionFirst)}</td>
      <td className='condition-column'>{renderConditionValues(conditionSecond)}</td>
      <td className='condition-column'>{renderConditionValues(conditionThird)}</td>
    </>
  );
};

export default ConditionColumn;