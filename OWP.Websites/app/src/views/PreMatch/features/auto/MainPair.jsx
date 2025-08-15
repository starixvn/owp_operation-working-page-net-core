import React from 'react';
import { useSelector } from 'react-redux';

import Assignment from './Assignment';

const MainPair = () => {
    const mainPairs = useSelector(state => state.auto.mainPairs);

    return (
        <div className='main__pair wrapper'>
            <label className='title'>Main Pair</label>
            <Assignment data={mainPairs} fieldName={'mainPairs'} />
        </div>

    );
};

MainPair.propTypes = {

};

export default MainPair;