import { useRef } from 'react';
import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';
import { nanoid } from '@reduxjs/toolkit';
import { updatePrematch } from '../../../../ImportPrematchHelperNative';
import IconAddNewInput from './IconAddNewInput';
import { validateAssignment } from './AssignmentHelper';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import Spinner from '../../../../../components/Spinner';

const propTypes = {
    columnId: PropTypes.string.isRequired,
    rowIndex: PropTypes.number.isRequired,
    dataSource: PropTypes.array.isRequired,
    handleShowHideIconPlus: PropTypes.func,
    isShowIconPlus: PropTypes.bool,
    hightLightClassName: PropTypes.string
};

const MainAssignment = props => {
    const {
        columnId,
        rowIndex,
        dataSource,
        handleShowHideIconPlus,
        isShowIconPlus,
        hightLightClassName
    } = props;
    const input = useRef(null);
    const assignment = dataSource[rowIndex][columnId];
    const columnIdHint = `${columnId}Hint`;
    const assignmentHint = dataSource[rowIndex][columnIdHint];
    const otherColumnId = `Other${columnId}`;
    const otherColumnIdHint = `Other${columnId}Hint`;
    const otherAssignmentHint = dataSource[rowIndex][otherColumnIdHint];

    const dispatch = useDispatch();

    const handleUpdateAssignment = async () => {
        let canUpdatePrematch = true;
        let { value } = input.current;
        if (value) {
            Spinner.show();
            const {
                validateResult,
                valueAfterValidate,
                valueBeforeValidate
            } = await validateAssignment(value, assignment);
            Spinner.hide();

            !validateResult && (await ToastrWrapper.showError('User not found'));
            canUpdatePrematch = validateResult;
            input.current.value = valueAfterValidate || valueBeforeValidate;
            value = valueAfterValidate;
        } else {
            dispatch(
                updatePrematch({
                    rowIndex,
                    newValue: null,
                    key: otherColumnId
                })
            );
            !otherAssignmentHint && handleShowHideIconPlus(false);
        }
        if (canUpdatePrematch) {
            dispatch(
                updatePrematch({
                    rowIndex,
                    newValue: value || null,
                    key: columnId
                })
            );
        }
    };

    return (
        <div className={`league-group league-name ${hightLightClassName}`}>
            <IconAddNewInput isHidden={!isShowIconPlus} {...props} />
            <input
                ref={input}
                key={nanoid()}
                id='league'
                type='text'
                name='league'
                className='input-field'
                defaultValue={assignment}
                autoFocus={false}
                onBlur={handleUpdateAssignment}
                maxLength='100'
                autoComplete='off'
                placeholder={assignmentHint}
            />
        </div>
    );
};

MainAssignment.propTypes = propTypes;
export default MainAssignment;
