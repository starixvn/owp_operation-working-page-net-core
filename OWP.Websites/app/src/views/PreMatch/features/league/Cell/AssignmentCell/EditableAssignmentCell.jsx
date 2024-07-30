import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import MainAssignment from './MainAssignment';
import { useStableDispatch } from '../../../../ImportPrematchHelperNative';
import { changeRowHightLevelWhenPrepareTyping } from '../../shiftSlice/shiftSlice';
import { Cell } from 'fixed-data-table-2';
import OtherAssignment from './OtherAssignment';
const propTypes = {
  columnId: PropTypes.string.isRequired,
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const EditableAssignmentCell = props => {
  const { dataSource, rowIndex, columnId } = props;
  const { PreMatchLeagueId } = dataSource[rowIndex];
  const otherColumnId = `Other${columnId}`;
  const otherColumnIdHint = `Other${columnId}Hint`;
  const otherAssignment = dataSource[rowIndex][otherColumnId];
  const otherAssignmentHint = dataSource[rowIndex][otherColumnIdHint];

  const isShowInputOtherAssignmentState = Boolean(otherAssignment) || Boolean(otherAssignmentHint);
  const [isShowInputOtherAssignment, setIsShowInputOtherAssignment] = useState(isShowInputOtherAssignmentState);

  const dispatch = useStableDispatch();

  const handleOnClick = stateButtonPlus => {
    !otherAssignment && setIsShowInputOtherAssignment(stateButtonPlus);
    dispatch(
      changeRowHightLevelWhenPrepareTyping({
        PreMatchLeagueId,
        stateButtonPlus
      })
    );
  };

  useEffect(() => {
    setIsShowInputOtherAssignment(Boolean(otherAssignment) || Boolean(otherAssignmentHint));
  }, [otherAssignment, otherAssignmentHint]);

  const renderOtherAssignment = () => (
    <OtherAssignment handleShowHideIconPlus={handleOnClick} {...props} />
  );

  const renderMainAssignment = () => {
    return (
      <MainAssignment
        isShowIconPlus={!isShowInputOtherAssignment}
        handleShowHideIconPlus={handleOnClick}
        {...props}
      />
    );
  };

  return (
    <Cell className='edit-assignment'>
      {renderMainAssignment()}
      {isShowInputOtherAssignment && renderOtherAssignment()}
    </Cell>
  );
};

EditableAssignmentCell.propTypes = propTypes;
export default EditableAssignmentCell;
