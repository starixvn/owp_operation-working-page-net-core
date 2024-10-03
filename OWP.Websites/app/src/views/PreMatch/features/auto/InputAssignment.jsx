import React, { useRef } from 'react';
import PropTypes from 'prop-types';

const InputAssignment = props => {
  const inputRef= useRef(null);
  const {
    onChange,
    userName,
    userId
  }=props;

  const handleChange=()=>{
    onChange();
  };
  return (
    <input
    ref={inputRef}
    type='text'
    name='league'
    className='input-field'
    autoFocus={false}
    onBlur={handleChange}
    defaultValue={userName}
    maxLength='100'
    autoComplete='off'
    placeholder='Type a username'
  />);
};

InputAssignment.propTypes = {
  onChange: PropTypes.func.isRequired,
  userName: PropTypes.string,
  userId: PropTypes.number
};

export default InputAssignment;