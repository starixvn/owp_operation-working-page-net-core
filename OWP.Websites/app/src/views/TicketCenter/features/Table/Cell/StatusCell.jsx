import { Cell } from 'fixed-data-table-2';
import PropTypes from 'prop-types';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const StatusCell = ({ dataSource = [], rowIndex = 0 }) => {
  const { isRead, ticketStatus } = dataSource[rowIndex];

  return (
    <Cell className={!isRead ? 'has-unread status-cell' : 'cell status-cell'}>
      <span className={`status ${ticketStatus.toLowerCase()}`}>
        {ticketStatus}
      </span>
    </Cell>
  );
};

StatusCell.propTypes = propTypes;
export default StatusCell;
