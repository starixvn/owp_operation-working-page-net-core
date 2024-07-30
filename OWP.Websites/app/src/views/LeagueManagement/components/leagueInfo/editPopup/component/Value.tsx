import { useState } from 'react';
import { Input } from 'reactstrap';
import { useDispatch } from 'react-redux';
import { updateExpression } from '../slice/editSettingSlice';

const Value = (props: { value: string | number | undefined, index: number, hasWrite: boolean, property: "round" | "division" }) => {
  const { value, index, hasWrite, property } = props;
  const dispatch = useDispatch();
  const [inputValue, setInputValue] = useState(value);

  return (
    <Input
      disabled={!hasWrite}
      type='text'
      className='input-field'
      defaultValue={inputValue}
      maxLength={100}
      onBlur={() => dispatch(
        updateExpression({
          property,
          key: 'value',
          value: inputValue,
          index
        }))
      }
      onChange={event => setInputValue(event.target.value)}
    />
  );
};

export default Value;
