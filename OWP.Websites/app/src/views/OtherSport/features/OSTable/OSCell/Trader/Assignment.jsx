import PropTypes from 'prop-types';
import { formatOtherAssignment } from './AssignmentHelper';
import { TooltipText } from '../../../../../Shared/common/Tooltip';

const Assignment = props => {
  const { assignmentNote, mainTrader, rowIndex } = props;
  const { noteFirst, noteSecond, userName } = formatOtherAssignment(
    assignmentNote
  );
  const mainAssignment = formatOtherAssignment(mainTrader);
  return (
    <>
      {mainTrader && (
        <TooltipText
          text={
            <span className='trader main-trader'>
              {mainAssignment.noteFirst}
              <b>{mainAssignment.userName}</b>
              {mainAssignment.noteSecond}
            </span>
          }
          toolTipContent={
            mainAssignment.noteFirst +
            mainAssignment.userName +
            mainAssignment.noteSecond
          }
          innerClassName='main-trader'
          rowIndex={rowIndex}
        />
      )}
      {assignmentNote && (
        <TooltipText
          text={
            <span className='trader other-trader'>
              {noteFirst}
              <b>{userName}</b>
              {noteSecond}
            </span>
          }
          toolTipContent={noteFirst + userName + noteSecond}
          innerClassName='other-trader'
          rowIndex={rowIndex}
        />
      )}
    </>
  );
};

Assignment.propTypes = {
  assignmentNote: PropTypes.string,
  mainTrader: PropTypes.string,
  rowIndex: PropTypes.number
};

export default Assignment;
