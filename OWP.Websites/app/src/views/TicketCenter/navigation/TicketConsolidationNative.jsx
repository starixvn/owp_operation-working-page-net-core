import React from 'react';
import PropTypes from 'prop-types';
import Subtype from '../features/consolidation/subtype/Subtype';

const TicketConsolidationNative = props => {
  return <div className='ticket__consolidation'>
    <div className='subtype'>
      <Subtype/>
    </div>
    <div className='merge__ticket'>
      
    </div>
    <div className='ticket__detail'>
      
    </div>
  </div>;
};

TicketConsolidationNative.propTypes = {};

export default TicketConsolidationNative;
