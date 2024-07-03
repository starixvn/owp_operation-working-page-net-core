import PropTypes from 'prop-types';
import CreateTicketHandler from '../handler/CreateTicketHandler';
import OpenTicketListHandler from '../handler/OpenTicketListHandler';
import { useDispatch } from 'react-redux';
import {
  closePopup,
  getModelByRowIndex
} from '../../chatBoardNew/chatBoardSlice';
import {
  closeCreatePopup,
  openPopupCreateTicketInLGA
} from '../../createTicketNew/createTicketBoardSliceNew';

const propTypes = {
  dataSource: PropTypes.any,
  rowIndex: PropTypes.number,
  onShowTicketList: PropTypes.func,
  updateDataSources: PropTypes.func
};

const TicketCellCommon = props => {
  const { dataSource, rowIndex, onShowTicketList, updateDataSources } = props;
  const stableDispatch = useDispatch();
  const closeAnotherPopup = () => {
    stableDispatch(closePopup());
    stableDispatch(closeCreatePopup());
  };

  const handleOpenCreateTicket = () => {
    closeAnotherPopup();
    stableDispatch(getModelByRowIndex(dataSource[rowIndex]));
    stableDispatch(openPopupCreateTicketInLGA());
  };

  return (
    <div className='ticket-cell'>
      {dataSource[rowIndex].HasTicket ? (
        <OpenTicketListHandler
          dataSources={dataSource}
          index={rowIndex}
          onShowTicketList={onShowTicketList}
          updateDataSources={updateDataSources}
          handleOpenCreateTicket={handleOpenCreateTicket}
        />
      ) : (
        <CreateTicketHandler handleOpenCreateTicket={handleOpenCreateTicket} />
      )}
    </div>
  );
};

TicketCellCommon.propTypes = propTypes;

export default TicketCellCommon;
