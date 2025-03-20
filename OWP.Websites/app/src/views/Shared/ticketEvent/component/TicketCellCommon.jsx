import PropTypes from 'prop-types';
import CreateTicketHandler from '../handler/CreateTicketHandler';
import QuickAddTicketHandler from '../handler/QuickAddTicketHandler';
import OpenTicketListHandler from '../handler/OpenTicketListHandler';
import { useDispatch, useSelector } from 'react-redux';
import {
  closePopup,
  getModelByRowIndex
} from '../../chatBoardNew/chatBoardSlice';
import {
  closeCreatePopup,
  openPopupCreateTicketInLGA
} from '../../createTicketNew/createTicketBoardSliceNew';

const propTypes = {
  allowQuickAdd: PropTypes.bool,
  dataSource: PropTypes.any,
  rowIndex: PropTypes.number,
  onShowTicketList: PropTypes.func,
  updateDataSources: PropTypes.func
};

const TicketCellCommon = props => {
  const {
    allowQuickAdd,
    dataSource,
    rowIndex,
    onShowTicketList,
    updateDataSources
  } = props;

  const hasWrite = useSelector(state => state.pagePolicy).permission.HasWrite;
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

  const renderOpenTicketListHandler = () => (
    <OpenTicketListHandler
      dataSources={dataSource}
      index={rowIndex}
      onShowTicketList={onShowTicketList}
      updateDataSources={updateDataSources}
      handleOpenCreateTicket={handleOpenCreateTicket}
    />
  );

  const renderQuickAddTicketHandler = () => (
    <div className='btn-block-add-new'>
      <div className='dropdown-list'>
        <button className='button-submit'>Create</button>
        <div className='dropdown-content-ticket'>
          <li className='button-event'>Match Group Ticket</li>
          <li className='button-event' onClick={handleOpenCreateTicket}>
            New Ticket{' '}
          </li>
        </div>
      </div>
    </div>
  );

  const renderCreateTicketHandler = () => (
    <CreateTicketHandler
      hasWrite={hasWrite}
      handleOpenCreateTicket={handleOpenCreateTicket}
    />
  );

  return (
    <div className='ticket-cell'>
      {dataSource[rowIndex].HasTicket
        ? renderOpenTicketListHandler()
        : allowQuickAdd && hasWrite
        ? renderQuickAddTicketHandler()
        : renderCreateTicketHandler()}
    </div>
  );
};

TicketCellCommon.propTypes = propTypes;

export default TicketCellCommon;
