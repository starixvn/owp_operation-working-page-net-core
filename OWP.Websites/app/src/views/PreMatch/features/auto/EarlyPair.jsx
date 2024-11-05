import React from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';

import Assignment from './Assignment';

const EarlyPair = () => {
    const earlyPairs = useSelector(state => state.auto.earlyPairs);

    return (
        <div className='early__pair wrapper'>
            <label className='title'>Early Pair</label>
            <Assignment data={earlyPairs} fieldName={'earlyPairs'} />
        </div>
    );
};

EarlyPair.propTypes = {

};

export default EarlyPair;