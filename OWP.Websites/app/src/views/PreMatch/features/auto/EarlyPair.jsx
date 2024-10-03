import React from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';
import Assignment from './Assignment';
import InputAssignment from './InputAssignment';

const EarlyPair = () => {
  const dispatch = useDispatch();
  const assignee = useSelector(state => state.auto.pairs.major);
  const handleChangeAssignment=()=>{

  }
  
  return (
    <div className='early__pair wrapper'>
        <label className='title'>Early Pair</label>
        <Assignment
    assignmentHdp={<InputAssignment onChange={handleChangeAssignment} userId={8} userName={'sa'}/>}
    assignmentOu={<InputAssignment  onChange={handleChangeAssignment} userId={8} userName={'sa'}/>}/>

        </div>
  );
};

EarlyPair.propTypes = {
  
};

export default EarlyPair;