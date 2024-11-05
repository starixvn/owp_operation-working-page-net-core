import React from 'react';
import { useSelector } from 'react-redux';

import Assignment from './Assignment';

const MajorPair = () => {
    const { majorPairs, majorTakeOverPairs } = useSelector(state => state.auto);

    return (
        <div className='major__pair--container wrapper'>
            <div className='major__pair'>
                <label className='title'>Major Pair</label>
                <Assignment data={majorPairs} fieldName={'majorPairs'} />
            </div>
            <div className='major__pair--take-over'>
                <label className='title'>Major Pair (Take Over)</label>
                <Assignment data={majorTakeOverPairs} fieldName={'majorTakeOverPairs'} />
            </div>
        </div>
    );
};

MajorPair.propTypes = {

};

export default MajorPair;