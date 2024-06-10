import PropTypes from 'prop-types';
import ReadOnlyAssignmentCell from './ReadOnlyAssignmentCell';
import EditableAssignmentCellWrapper from './EditableAssignmentCellWrapper';
const propTypes = {
  columnId: PropTypes.string.isRequired,
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired,
  isEditing: PropTypes.bool.isRequired
};

const AssignmentCellFactory = props => {
  const { isEditing } = props;
  return (
    <>
      {isEditing ? (
        <EditableAssignmentCellWrapper {...props} />
      ) : (
        <ReadOnlyAssignmentCell {...props} />
      )}
    </>
  );
};

AssignmentCellFactory.propTypes = propTypes;
export default AssignmentCellFactory;
