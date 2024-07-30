import PropTypes from 'prop-types';
import { PlusCircleIcon } from 'mdi-react';
const propTypes = {
  columnId: PropTypes.string.isRequired,
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired,
  handleShowHideIconPlus: PropTypes.func,
  isHidden: PropTypes.bool
};

const IconAddNewInput = props => {
  const {
    dataSource,
    rowIndex,
    columnId,
    handleShowHideIconPlus,
    isHidden
  } = props;

  const assignment = dataSource[rowIndex][columnId];
  const otherColumnId = `Other${columnId}Note`;
  const otherAssignment = dataSource[rowIndex][otherColumnId];

  if (!assignment || otherAssignment || isHidden) {
    return null;
  }

  return (
    <PlusCircleIcon
      size={15}
      className='btn-add'
      onClick={() => handleShowHideIconPlus(true)}
    />
  );
};

IconAddNewInput.propTypes = propTypes;
export default IconAddNewInput;
