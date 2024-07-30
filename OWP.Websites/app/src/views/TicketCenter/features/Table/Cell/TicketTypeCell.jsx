import { Cell } from 'fixed-data-table-2';
import PropTypes from 'prop-types';
import { UncontrolledTooltip } from 'reactstrap';

const propTypes = {
  dataSource: PropTypes.array.isRequired,
  rowIndex: PropTypes.number.isRequired
};

const TicketTypeCell = props => {
  const { dataSource, rowIndex } = props;
  const { typeName, ticketSubTypeName, isRead } = dataSource[rowIndex];

  return (
    <Cell className={!isRead ? 'has-unread' : 'cell'}>
      <label id={`TicketType${rowIndex}`} className='header-TicketType'>
        <span> {typeName}</span>
        <span id={`link-tooltip-${rowIndex}`}> {ticketSubTypeName}</span>
      </label>
      <UncontrolledTooltip
        placement='bottom'
        target={`link-tooltip-${rowIndex}`}>
        {ticketSubTypeName}
      </UncontrolledTooltip>
    </Cell>
  );
};

TicketTypeCell.propTypes = propTypes;
export default TicketTypeCell;
