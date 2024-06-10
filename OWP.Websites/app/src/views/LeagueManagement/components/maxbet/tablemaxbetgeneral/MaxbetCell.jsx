import PropTypes from 'prop-types';
import { Cell } from 'fixed-data-table-2';
import { v4 as uuidv4 } from 'uuid';

const propTypes = {
  value: PropTypes.string
};

const MaxbetCell = ({ value }) => {
  return (
    <Cell key={uuidv4()} className='maxbet-cell'>
      {value}
    </Cell>
  );
};

MaxbetCell.propTypes = propTypes;
export default MaxbetCell;
