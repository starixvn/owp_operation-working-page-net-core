import { memo } from 'react';
import PropTypes from 'prop-types';
import TicketCellCommon from '../../../../Shared/ticketEvent/component/TicketCellCommon';
import { setDataTable } from '../../tableSlice/tableSlice';
import { useDispatch } from 'react-redux';

const propTypes = {
  dataSource: PropTypes.array,
  rowIndex: PropTypes.number,
  handleShowTicket: PropTypes.func
};

const TicketCell = memo(props => {
  const { dataSource, rowIndex, handleShowTicket } = props;

  const stableDispatch = useDispatch();

  return (
    <TicketCellCommon
      dataSource={dataSource}
      rowIndex={rowIndex}
      onShowTicketList={handleShowTicket}
      updateDataSources={data => stableDispatch(setDataTable(data))}
    />
  );
});

TicketCell.propTypes = propTypes;

export default TicketCell;
