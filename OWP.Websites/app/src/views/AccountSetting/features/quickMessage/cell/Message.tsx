import React, { useState } from 'react';
import { useAppDispatch } from '../../../app/hooks';
import { setEditModel } from '../slice/quickMessageSlice';

type ShortcutProps = {
  message: string;
  isEdit: boolean;
};

const Message = ({ message, isEdit }: ShortcutProps) => {
  const [inputValue, setInputValue] = useState<string>(message);
  const [isEmpty, setIsEmpty] = useState<boolean>(!message.trim());
  const dispatch = useAppDispatch();

  const handleOnBlur = (event: React.FocusEvent<HTMLInputElement>) => {
    const value = event.target.value;
    setIsEmpty(!value.trim());
    dispatch(setEditModel({ property: 'message', value }));
  };

  return (
    <td className='message-cell'>
      {isEdit
        ? <input
          className={`message-input ${isEmpty ? 'input-empty' : ''}`}
          onChange={e => setInputValue(e.target.value)}
          value={inputValue}
          onBlur={handleOnBlur}
        />
        : <span className='message-value'>{inputValue}</span>
      }
    </td>
  );
};


export default React.memo(Message);