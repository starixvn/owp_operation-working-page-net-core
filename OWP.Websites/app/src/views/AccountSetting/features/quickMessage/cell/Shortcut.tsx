import React, { useState } from 'react';
import { useAppDispatch } from '../../../app/hooks';
import { setEditModel } from '../slice/quickMessageSlice';

type ShortcutProps = {
  shortcut: string;
  isEdit: boolean;
};

const Shortcut = ({ shortcut, isEdit }: ShortcutProps) => {
  const [inputValue, setInputValue] = useState<string>(shortcut?.replace('/', ''));
  const [isEmpty, setIsEmpty] = useState<boolean>(!shortcut.trim());
  const dispatch = useAppDispatch();

  const handleOnBlur = (event: React.FocusEvent<HTMLInputElement>) => {
    const value = event.target.value;
    setIsEmpty(!value.trim());
    dispatch(setEditModel({ property: 'shortcut', value: '/' + value.trim() }));
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === '/') {
      e.preventDefault();
    }
  };

  return (
    < td className='shortcut-cell'>
      <span className='slash'>/</span>
      {isEdit
        ? <input
          className={`shortcut-input ${isEmpty ? 'input-empty' : ''}`}
          value={inputValue}
          onChange={e => setInputValue(e.target.value)}
          onBlur={handleOnBlur}
          onKeyDown={handleKeyDown}
          autoFocus
        />
        : <span className='shortcut-value'>{inputValue}</span>
      }
    </td>
  );
};

export default React.memo(Shortcut);
