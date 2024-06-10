import { uniqueId } from 'lodash';
import { KeyboardEvent, useRef } from 'react';

type Props = {
  value: string | null;
  handleOnBlur: (newValue: string) => void;
  disabled?: boolean;
  isError?: boolean;
};

const CustomInput = ({
  value,
  handleOnBlur,
  disabled = false,
  isError = false
}: Props) => {
  const inputRef = useRef<HTMLInputElement>(null);

  const handleKeyPress = (event: KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') {
      event.preventDefault();
      inputRef.current?.blur();
    }
  };

  return (
    <div>
      <input
        key={uniqueId()}
        type='text'
        defaultValue={value ?? ''}
        onBlur={e => handleOnBlur(e.target.value)}
        onKeyDown={handleKeyPress}
        style={{ display: 'block' }}
        disabled={disabled}
        ref={inputRef}
        className={isError ? 'input-error' : ''}
      />
    </div>
  );
};

export default CustomInput;
