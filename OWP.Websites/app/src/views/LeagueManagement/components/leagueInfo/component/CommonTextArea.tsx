import React from 'react';

interface CommonTextAreaProps {
  value: string;
  rows?: number;
  handleOnChange: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
}

const CommonTextArea: React.FC<CommonTextAreaProps> = ({
  value,
  rows,
  handleOnChange
}) => {
  return (
    <textarea
      className='common-textarea'
      value={value}
      onChange={handleOnChange}
      rows={rows || 3}
    />
  );
};

export default CommonTextArea;
