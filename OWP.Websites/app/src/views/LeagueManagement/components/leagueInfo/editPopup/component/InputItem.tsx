import React, { useEffect, useState } from 'react';
import { TEditSetting } from '../EditType';
import { useAppDispatch } from '../../app/hooks';
import { updateObject } from '../slice/editSettingSlice';

interface InputItemProps {
  label: string;
  inputKey: keyof TEditSetting;
  value: string;
}

export const InputItem: React.FC<InputItemProps> = ({ label, inputKey, value }) => {
  const dispatch = useAppDispatch();
  const [inputValue, setInputValue] = useState('');
  useEffect(() => {
    setInputValue(value);
  }, [value]);

  const handleOnChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = event.target.value;
    setInputValue(newValue);
  };

  const handleOnBlur = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = event.target.value;
    newValue !== value && dispatch(updateObject({ property: inputKey, value: newValue }));
  };

  return (
    <div className='input-item'>
      <div className='label'>{label}</div>
      <input
        value={inputValue}
        className='input'
        onChange={handleOnChange}
        onBlur={handleOnBlur}
      />
    </div>
  );
};
