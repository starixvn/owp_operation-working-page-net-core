import React, { useRef } from 'react';
import { useAppDispatch } from '../../../app/hooksNative';
import { setFilterParam } from './filterSlice';

const Event = () => {
  const dispatch= useAppDispatch();

  const inputRef= useRef<HTMLInputElement>(null);
  return (
    <div className='search-text'>
    <div className='label'>Event</div>
    <input
      className='input-text-event'
      placeholder='League/Match/Subject'
      defaultValue=''
      onBlur={()=> inputRef.current?.value && dispatch(setFilterParam({key: 'keyword', value: inputRef.current?.value}))}
      ref={inputRef}
    />
  </div>
  );
};

export default Event;