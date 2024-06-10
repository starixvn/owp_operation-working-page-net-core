import PropTypes from 'prop-types';
import TicketCellCommon from '../../Shared/ticketEvent/component/TicketCellCommon';

const propTypes = {
  dataSource: PropTypes.array,
  rowIndex: PropTypes.number,
  onShowTicketList: PropTypes.func.isRequired,
  updateDataSources: PropTypes.func
};

const TicketCell = props => {
  const { dataSource, rowIndex, onShowTicketList, updateDataSources } = props;
  return (
    <TicketCellCommon
      dataSource={dataSource}
      rowIndex={rowIndex}
      onShowTicketList={onShowTicketList}
      updateDataSources={updateDataSources}
    />
  );
};

TicketCell.propTypes = propTypes;

export default TicketCell;
