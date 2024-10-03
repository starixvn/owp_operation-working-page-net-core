import React from 'react';
import PropTypes from 'prop-types';

const Assignment = props => {
  return (
    <div className='assignment'>
        <div className='hdp__wrapper'>
        <label className='hdp'>HDP</label>
        {props.assignmentHdp}
        </div>
        <div className='ou__wrapper'>
        <label className='ou'>OU</label>
        {props.assignmentOu}
        </div>
      </div>
  );
};

Assignment.propTypes = {
  assignmentHdp: PropTypes.element.isRequired,
  assignmentOu: PropTypes.element.isRequired,

};

export default Assignment;