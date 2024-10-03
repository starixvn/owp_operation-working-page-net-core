import React from 'react';
import PropTypes from 'prop-types';
import { useDispatch, useSelector } from 'react-redux';

const MainPair = () => {
  const dispatch = useDispatch();
  const assignee = useSelector(state => state.auto.pairs.major);
  return (
    <div className='main__pair wrapper'>
        <label className='title'>Main Pair</label>
        {renderAssignment()}
        </div>
    
  );
};

MainPair.propTypes = {
  
};

export default MainPair;