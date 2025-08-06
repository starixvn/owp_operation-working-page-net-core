import Select from 'react-select-ts';
import { TOption } from './Type';

interface ConditionProps {
  condition: {
    value: number;
    label: string;
  };
  option: Array<{
    value: number;
    label: string;
  }>;
  hasWrite: boolean;
  index: number;
  property: 'round' | 'division';
  handleUpdateValueExpression: (
    property: 'round' | 'division',
    key: 'value' | 'type',
    value: string,
    index: number
  ) => void;
}

const Condition = ({
  condition,
  option,
  hasWrite = true,
  property,
  index,
  handleUpdateValueExpression
}: ConditionProps) => {
  const handleUpdateExpressionCallBack = (selectedOption: TOption | null) => {
    handleUpdateValueExpression(
      property,
      'type',
      selectedOption?.label || '',
      index
    );
  };
  return (
    <Select
      isDisabled={!hasWrite}
      options={option}
      defaultValue={condition}
      className='basic-multi-select'
      classNamePrefix='select'
      name='list-condition'
      onChange={handleUpdateExpressionCallBack}
    />
  );
};

export default Condition;
