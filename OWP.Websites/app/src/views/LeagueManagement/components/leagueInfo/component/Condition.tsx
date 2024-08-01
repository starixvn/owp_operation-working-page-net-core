import { useDispatch } from 'react-redux';
import Select from 'react-select-ts';
import MaxbetConst from '../../../constants/MaxbetConst';
import { TOption } from '../editPopup/EditType';

interface ConditionProps {
  condition: {
    value: number;
    label: string;
  };
  hasWrite: boolean;
  index: number;
  property: 'round' | 'division';
  handleChangeExpression: (
    selectedOption: TOption | null,
    property: 'round' | 'division',
    index: number
  ) => void;
}

const Condition = ({
  condition,
  hasWrite = true,
  property,
  index,
  handleChangeExpression
}: ConditionProps) => {
  const handleChangeExpressionCallBack = (selectedOption: TOption | null) => {
    handleChangeExpression(selectedOption, property, index);
  };
  return (
    <Select
      isDisabled={!hasWrite}
      options={MaxbetConst.expressionOption}
      defaultValue={condition}
      className='basic-multi-select'
      classNamePrefix='select'
      name='list-condition'
      onChange={handleChangeExpressionCallBack}
    />
  );
};

export default Condition;
