import React, { useState } from 'react';
import { TEditSetting } from '../EditType';
import { useAppDispatch } from '../../app/hooks';
import { updateObject } from '../slice/editSettingSlice';

interface CommonTextAreaProps {
  value: string;
  inputKey: keyof TEditSetting;
  rows?: number
}

const CommonTextArea: React.FC<CommonTextAreaProps> = ({ value, inputKey, rows }) => {
  const [inputValue, setInputValue] = useState(value || '');
  const dispatch = useAppDispatch();

  const handleOnChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    const newValue = event.target.value;
    setInputValue(newValue);
  };

  const handleOnBlur = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
    const newValue = event.target.value;
    value !== newValue && dispatch(updateObject({ property: inputKey, value: newValue }));
  };

  return (
    <textarea
      className='common-textarea'
      value={inputValue}
      onChange={handleOnChange}
      onBlur={handleOnBlur}
      rows={rows || 3}
    />
  );
};

export default CommonTextArea;
