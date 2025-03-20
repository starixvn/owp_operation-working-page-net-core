import PropTypes from 'prop-types';
import CreateTicketHandler from '../handler/CreateTicketHandler';
import QuickAddTicketHandler from '../handler/QuickAddTicketHandler';
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
      ) : allowQuickAdd ? (
        // <QuickAddTicketHandler
        //   closeAnotherPopup={closeAnotherPopup}
        //   data={dataSource[rowIndex]}
        // />
        <div className='btn-block-add-new'>
          <div className='dropdown-list'>
            <button className='button-submit'>Create</button>
            <div className='dropdown-content-ticket'>
              <li className='button-event'>Match Group Ticket</li>
              <li className='button-event'>New Ticket </li>
            </div>
          </div>
        </div>
      ) : (
        <CreateTicketHandler handleOpenCreateTicket={handleOpenCreateTicket} />
      )}
    </div>
  );
};

TicketCellCommon.propTypes = propTypes;

export default TicketCellCommon;
