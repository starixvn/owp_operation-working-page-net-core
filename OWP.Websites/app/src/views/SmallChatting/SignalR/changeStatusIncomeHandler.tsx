import { setTicketStatus as setPopupTicketStatus } from '../../Shared/chatBoardNew/chatBoardSlice';
import store from '../app/store';

const changeStatusIncomeHandler = (ticketInfo: any) => { //NOSONAR
  const model = store.getState();
  const { ticketDetail } = model.chatBoard;
  const { ticketId, ticketStatus } = ticketInfo;
  const { ticketId: openTicketId } = ticketDetail;

  openTicketId === ticketId &&
    store.dispatch(setPopupTicketStatus(ticketStatus));
};

export default changeStatusIncomeHandler;
