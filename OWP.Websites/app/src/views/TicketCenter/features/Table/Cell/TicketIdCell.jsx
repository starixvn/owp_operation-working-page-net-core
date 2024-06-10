import { Cell } from 'fixed-data-table-2';
import PropTypes from 'prop-types';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const TicketIdCell = props => {
  const { dataSource, rowIndex } = props;
  const { ticketId, isRead, isPinned } = dataSource[rowIndex];

  const isReadClassName = !isRead
    ? 'has-unread ticket-id-cell'
    : 'cell ticket-id-cell';
  const isPinnedClassName = isPinned ? ' pinned-ticket' : '';

  return (
    <Cell className={isReadClassName + isPinnedClassName}>
      <label id={`TicketId${rowIndex}`} className='header-TicketID'>
        {isPinned && <span className='icon-pin'></span>}
        {ticketId}
      </label>
    </Cell>
  );
};

TicketIdCell.propTypes = propTypes;
export default TicketIdCell;
