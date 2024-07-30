import { useState } from 'react';
import { IInputMaxbetSpread, TInputCustom } from '../../typing/type';
import NumericTextbox from 'react-numeric-textbox';

const InputCustom = (props: IInputMaxbetSpread) => {
  const { onChange, label, isRequired, disabled, max, min } = props;
  const [value, setValue] = useState<TInputCustom>(props.value);

  const handleChange = (newValue: string | null) => {
    setValue(newValue);
    onChange && onChange(newValue);
  };

  return (
    <div>
      <span className='label'>{label}</span>
      {isRequired && <span className='required-field'> *</span>}
      <NumericTextbox
        className='input-field'
        value={value}
        min={min}
        max={max}
        decimals={3}
        autoCorrect
        format=''
        disabled={disabled}
        onChange={handleChange}
      />
    </div>
  );
};

export default InputCustom;
