import React from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';

const EarlyPair = () => {
  const dispatch = useDispatch();
  const assignee = useSelector(state => state.auto.pairs.major);
  return (
    <div className='early__pair wrapper'>
        <label className='title'>Early Pair</label>
        {renderAssignment()}
        </div>
  );
};

EarlyPair.propTypes = {
  
};

export default EarlyPair;