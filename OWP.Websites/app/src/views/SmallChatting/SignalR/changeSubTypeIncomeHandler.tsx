import { updateSubTypeTicketDetailByTicketId } from '../../Shared/chatBoardNew/chatBoardSlice';
import store from '../app/store';

const changeSubTypeIncomeHandler = (ticketInfo: any) => { //NOSONAR
  store.dispatch(updateSubTypeTicketDetailByTicketId(ticketInfo));
};

export default changeSubTypeIncomeHandler;
