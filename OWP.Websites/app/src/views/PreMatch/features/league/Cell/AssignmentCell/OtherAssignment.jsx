import { useRef } from 'react';
import PropTypes from 'prop-types';
import { useDispatch } from 'react-redux';
import { updatePrematch } from '../../../../ImportPrematchHelperNative';
import ToastrWrapper from '../../../../../components/ToastrWrapper';
import Spinner from '../../../../../components/Spinner';
import { validateAssignment } from './AssignmentHelper';
import { nanoid } from '@reduxjs/toolkit';

const propTypes = {
  columnId: PropTypes.string.isRequired,
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired,
  handleShowHideIconPlus: PropTypes.func,
  hightLightClassName: PropTypes.string
};

const OtherAssignment = props => {
  const {
    columnId,
    rowIndex,
    dataSource,
    handleShowHideIconPlus,
    hightLightClassName
  } = props;
  const mainAssignment = dataSource[rowIndex][columnId];
  const otherColumnId = `Other${columnId}`;
  const otherColumnIdHint = `Other${columnId}Hint`;
  const originalOtherAssignment = dataSource[rowIndex][otherColumnId];
  const otherAssignmentHint = dataSource[rowIndex][otherColumnIdHint];
  const input = useRef(null);
  const dispatch = useDispatch();

  const handleUpdateOtherAssignmentOnBlur = async () => {
    let { value } = input.current;


    let canUpdatePrematch = true;

    if (value) {
      Spinner.show();
      const {
        validateResult,
        valueAfterValidate,
        valueBeforeValidate
      } = await validateAssignment(value, originalOtherAssignment);
      Spinner.hide();
      !validateResult && (await ToastrWrapper.showError('User not found'));
      canUpdatePrematch = validateResult;
      input.current.value = valueAfterValidate || valueBeforeValidate;
      value = valueAfterValidate;
      handleShowHideIconPlus(true);
    } else {
      if (!otherAssignmentHint) {
        handleShowHideIconPlus(false);
      }
    }
    if (canUpdatePrematch) {
      dispatch(
        updatePrematch({
          rowIndex,
          newValue: value || null,
          key: otherColumnId
        })
      );

    }
  };

  const handleKeyDown = () => !mainAssignment && ToastrWrapper.showError('You must assign the main trader first');

  return (
    <div className={`other-assignment ${hightLightClassName}`}>
      <input
        id='input'
        ref={input}
        key={nanoid()}
        type='text'
        name='league'
        defaultValue={originalOtherAssignment}
        onBlur={handleUpdateOtherAssignmentOnBlur}
        className='input-field'
        onKeyDown={handleKeyDown}
        onPaste={handleKeyDown}
        autoFocus={false}
        autoComplete="off"
        maxLength='100'
        placeholder={otherAssignmentHint}
      />
    </div>
  );
};

OtherAssignment.propTypes = propTypes;
export default OtherAssignment;
