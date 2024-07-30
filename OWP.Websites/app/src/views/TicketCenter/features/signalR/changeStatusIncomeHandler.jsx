import { setTicketStatus as setPopupTicketStatus } from '../../../Shared/chatBoardNew/chatBoardSlice';
import store from '../../app/storeNative';
import { setTicketStatus as setTableTicketStatus } from '../Table/eventSlice/eventCheckingSlice';

const changeStatusIncomeHandler = async ticketInfo => {
  const model = store.getState();
  const { filterModel } = model.signalR;
  const { isOpenPopup, ticketDetail } = model.chatBoard;
  const { ticketId, ticketStatus } = ticketInfo;
  const { ticketId: openTicketId } = ticketDetail;

  store.dispatch(setTableTicketStatus({ ticketInfo, filterModel }));
  isOpenPopup &&
    openTicketId === ticketId &&
    store.dispatch(setPopupTicketStatus(ticketStatus));
};

export default changeStatusIncomeHandler;
