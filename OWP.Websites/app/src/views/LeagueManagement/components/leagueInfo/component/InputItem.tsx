import React from 'react';

interface InputItemProps {
  label: string;
  value: string;
  handleOnChange: (event: React.ChangeEvent<HTMLInputElement>) => void;
}

export const InputItem: React.FC<InputItemProps> = ({
  label,
  value,
  handleOnChange
}) => {
  return (
    <div className='input-item'>
      <div className='label'>{label}</div>
      <input value={value} className='input' onChange={handleOnChange} />
    </div>
  );
};
