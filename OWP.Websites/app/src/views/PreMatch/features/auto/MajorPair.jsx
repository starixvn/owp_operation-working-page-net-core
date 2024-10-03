import React from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';
import Assignment from './Assignment';
import InputAssignment from './InputAssignment';

const MajorPair = () => {
  const dispatch = useDispatch();
  const assignee = useSelector(state => state.auto.pairs.major);
  const handleChangeAssignment=()=>{

  }
  return (
    <Assignment
    assignmentHdp={<InputAssignment onChange={handleChangeAssignment} userId={8} userName={'sa'}/>}
    assignmentOu={<InputAssignment  onChange={handleChangeAssignment} userId={8} userName={'sa'}/>}/>
  );
};

MajorPair.propTypes = {
  
};

export default MajorPair;