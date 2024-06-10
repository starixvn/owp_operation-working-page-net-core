import { Cell } from 'fixed-data-table-2';
import PropTypes from 'prop-types';
import { v4 as uuidv4 } from 'uuid';
const propTypes = {
  rowIndex: PropTypes.number.isRequired
};
const NoCell = ({ rowIndex }) => {
  return (
    <Cell key={uuidv4()} className='no'>
      {rowIndex + 1}
    </Cell>
  );
};

NoCell.propTypes = propTypes;
export default NoCell;
