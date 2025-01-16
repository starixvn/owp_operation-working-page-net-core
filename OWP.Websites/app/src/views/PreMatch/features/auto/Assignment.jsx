import React from 'react';
import PropTypes from 'prop-types';
import { UserPlusOutlineIcon, UserRemoveOutlineIcon } from 'mdi-react';
import { useDispatch } from 'react-redux';

import InputAssignment from './InputAssignment';

import { ASSIGNMENT_TYPE, BETTYPE } from '../../const/const';
import { addAssignmentType, removeAssignmentType, addPairs, updatePairs, removePairs } from './autoSlice';

const Assignment = ({ data, fieldName }) => {
    const dispatch = useDispatch();

    const findAssignmentByBettypeAndType = (pairs, bettype, assignmentType) =>
        pairs.find(a => a.bettype === bettype && a.assignmentType === assignmentType);

    const handleChangeAssignment = (pairIndex, bettype, assignmentType, userId, username) => {
        dispatch(updatePairs({ fieldName, pairIndex, bettype, assignmentType, userId, username }));
    };

    const handleAddAssignmentType = (pairIndex, bettype) => {
        dispatch(addAssignmentType({ fieldName, pairIndex, bettype, assignmentType: ASSIGNMENT_TYPE.OTHER }));
    };
    const handleRemoveAssignmentType = (pairIndex, bettype) => {
        dispatch(removeAssignmentType({ fieldName, pairIndex, bettype, assignmentType: ASSIGNMENT_TYPE.OTHER }));
    };

    const handleAddPairs = () => dispatch(addPairs({ fieldName }));
    const handleRemovePairs = pairIndex => dispatch(removePairs({ fieldName, pairIndex }));

    return (
        <div className='assignment-container'>
            <div className='assignment-header'>
                <div className='content'>
                    <label className='hdp'>HDP</label>
                    <label className='ou'>OU</label>
                </div>
            </div>
            <div className='assignment-content'>
                {
                    data.map((pairs, index) => {
                        const hdpMain = findAssignmentByBettypeAndType(pairs, BETTYPE.HDP, ASSIGNMENT_TYPE.MAIN);
                        const hdpOther = findAssignmentByBettypeAndType(pairs, BETTYPE.HDP, ASSIGNMENT_TYPE.OTHER);
                        const ouMain = findAssignmentByBettypeAndType(pairs, BETTYPE.OU, ASSIGNMENT_TYPE.MAIN);
                        const ouOther = findAssignmentByBettypeAndType(pairs, BETTYPE.OU, ASSIGNMENT_TYPE.OTHER);
                        return (
                            <div className='user-pairs' key={`${fieldName}-${index}`}>
                                <div className='content'>
                                    <div className='user-hdp'>
                                        <InputAssignment key={`${index}-${BETTYPE.HDP}-${ASSIGNMENT_TYPE.MAIN}-${hdpMain.userId}`}
                                            pairIndex={index}
                                            userId={hdpMain.userId}
                                            username={hdpMain.username}
                                            assignmentType={ASSIGNMENT_TYPE.MAIN}
                                            bettype={BETTYPE.HDP}
                                            showAddOther={hdpMain.userId > 0 && hdpOther === undefined}
                                            onChange={handleChangeAssignment}
                                            onAdd={handleAddAssignmentType}
                                        />
                                        {
                                            hdpOther &&
                                            <InputAssignment key={`${index}-${BETTYPE.HDP}-${ASSIGNMENT_TYPE.OTHER}-${hdpOther.userId}`}
                                                pairIndex={index}
                                                userId={hdpOther.userId}
                                                username={hdpOther.username}
                                                assignmentType={ASSIGNMENT_TYPE.OTHER}
                                                bettype={BETTYPE.HDP}
                                                showRemoveOther={true}
                                                onChange={handleChangeAssignment}
                                                onRemove={handleRemoveAssignmentType}
                                            />
                                        }
                                    </div>
                                    <div className='user-ou'>
                                        <InputAssignment key={`${index}-${BETTYPE.OU}-${ASSIGNMENT_TYPE.MAIN}-${ouMain.userId}`}
                                            pairIndex={index}
                                            userId={ouMain.userId}
                                            username={ouMain.username}
                                            assignmentType={ASSIGNMENT_TYPE.MAIN}
                                            bettype={BETTYPE.OU}
                                            showAddOther={ouMain.userId > 0 && ouOther === undefined}
                                            onAdd={handleAddAssignmentType}
                                            onChange={handleChangeAssignment}
                                        />
                                        {
                                            ouOther &&
                                            <InputAssignment key={`${index}-${BETTYPE.OU}-${ASSIGNMENT_TYPE.OTHER}-${ouOther.userId}`}
                                                pairIndex={index}
                                                userId={ouOther.userId}
                                                username={ouOther.username}
                                                assignmentType={ASSIGNMENT_TYPE.OTHER}
                                                bettype={BETTYPE.OU}
                                                showRemoveOther={true}
                                                onChange={handleChangeAssignment}
                                                onRemove={handleRemoveAssignmentType}
                                            />
                                        }
                                    </div>
                                </div>
                                {
                                    index < data.length - 1 &&
                                    <UserRemoveOutlineIcon className='user-remove-icon' size={15}
                                        onClick={() => handleRemovePairs(index)}
                                    />
                                }
                                {
                                    index === data.length - 1 && (hdpMain.userId > 0 || ouMain.userId > 0) &&
                                    <UserPlusOutlineIcon className='user-plus-icon' size={15}
                                        onClick={handleAddPairs}
                                    />
                                }
                            </div>
                        );
                    })
                }
            </div>
        </div>
    );
};

Assignment.propTypes = {
    data: PropTypes.arrayOf(PropTypes.array),
    fieldName: PropTypes.string
};

export default Assignment;