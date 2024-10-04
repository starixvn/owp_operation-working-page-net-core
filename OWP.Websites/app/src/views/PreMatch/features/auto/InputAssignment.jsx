import React, { useEffect, useRef, useState } from 'react';
import PropTypes from 'prop-types';
import { PlusCircleIcon } from 'mdi-react';
import UserService from '../../../Shared/User/UserService';

const InputAssignment = props => {
   const {
    onChange,
    userId,
    assignmentType,
    bettype
  }=props;
  const [userName, setUserName]= useState(props.userName);
  const [suggestion, setSuggestion] = useState([]);
  const inputRef= useRef(null);
  const inputAssignmentRef= useRef(null);
 

  const handleChange=e=>{
  const {value}= e.target;
  if (value && value.trim() && value.length >= 2) {
       const response = UserService.getSuggestion(value);
        if (response) {
         setSuggestion(response.map(({UserId, UserName, NickName})=>({
            userId: UserId,
            userName:UserName,
              nickName:NickName
         })));
        }
  }else{
    setSuggestion([]);
  }
  setUserName(value);
  };
  
  useEffect(() => {
    const handleClickOutside = event => {
      if (inputAssignmentRef.current && !inputAssignmentRef.current.contains(event.target)) {
        setSuggestion([]);
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [inputAssignmentRef]);

  const renderSuggestion=()=>
        suggestion.length ? <div className='input__suggestion'>
      <ul className='suggestion__item__container'>
      {suggestion.map(({userName, userId}, index)=>
        <li className='suggestion__item'
      onClick={()=>onChange({userName, userId,
         assignmentType,
    bettype })}
         key={index}>
        {userName}
      </li>
      )}
      </ul>
    </div>
    : null;

  return (
    <div className='input__assignment' ref={inputAssignmentRef}>
    <input
      ref={inputRef}
      type='text'
      name='league'
      className='input-field'
      autoFocus={false}
      onBlur={()=>{}}
      value={userName}
      maxLength='100'
      autoComplete='off'
      placeholder='Type a username'
      onChange={handleChange}
  />
  {userName && <PlusCircleIcon
      size={15}
      className='btn-add'
      onClick={() => handleShowHideIconPlus(true)}
    />}
    {renderSuggestion()}
</div>);
};

InputAssignment.propTypes = {
  onChange: PropTypes.func.isRequired,
  userName: PropTypes.string,
  userId: PropTypes.number,
   assignmentType: PropTypes.number,
    bettype: PropTypes.number,
};

export default InputAssignment;