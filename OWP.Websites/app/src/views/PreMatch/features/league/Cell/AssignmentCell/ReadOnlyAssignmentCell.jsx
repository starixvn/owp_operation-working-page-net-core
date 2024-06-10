import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';

const propTypes = {
  columnId: PropTypes.string.isRequired,
  rowIndex: PropTypes.number.isRequired,
  dataSource: PropTypes.array.isRequired
};

const ReadOnlyAssignmentCell = props => {
  const { columnId, rowIndex, dataSource } = props;
  const assignment = dataSource[rowIndex][columnId];
  const otherColumnId = `Other${columnId}`;
  const otherAssignment = dataSource[rowIndex][otherColumnId];

  return (
    <Cell>
      <label className='assignment-readonly'>{assignment}</label>
      <label className='show-other-assignment'>{otherAssignment}</label>
    </Cell>
  );
};

ReadOnlyAssignmentCell.propTypes = propTypes;
export default ReadOnlyAssignmentCell;
