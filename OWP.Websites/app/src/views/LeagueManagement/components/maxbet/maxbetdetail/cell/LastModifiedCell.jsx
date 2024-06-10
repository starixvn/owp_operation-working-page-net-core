import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';

const LastModifiedCell = ({ lastModifiedBy }) => {
  return (
    <Cell>
      <span className='LastModified'>{lastModifiedBy}</span>
    </Cell>
  );
};

LastModifiedCell.propTypes = {
  lastModifiedBy: PropTypes.string
};

export default LastModifiedCell;
