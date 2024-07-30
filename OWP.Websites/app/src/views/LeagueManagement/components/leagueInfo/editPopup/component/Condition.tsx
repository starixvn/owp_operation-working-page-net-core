
import { useDispatch } from 'react-redux';
import Select from 'react-select-ts';
import MaxbetConst from '../../../../constants/MaxbetConst';
import { TOption } from '../EditType';
import { updateExpression } from '../slice/editSettingSlice';

interface ConditionProps {
  condition: {
    value: number;
    label: string;
  };
  index: number;
  hasWrite: boolean;
  property: "round" | "division";
}

const Condition = ({ condition, index, hasWrite = true, property }: ConditionProps) => {
  const dispatch = useDispatch();

  const handleChangeExpression = (selectedOption: TOption | null) => {
    selectedOption && dispatch(
      updateExpression({
        property,
        key: 'type',
        value: selectedOption.label?.toString(),
        index
      })
    );
  };

  return (
    <Select
      isDisabled={!hasWrite}
      options={MaxbetConst.expressionOption}
      defaultValue={condition}
      className='basic-multi-select'
      classNamePrefix='select'
      name='list-condition'
      onChange={handleChangeExpression}
    />
  );
};

export default Condition;
