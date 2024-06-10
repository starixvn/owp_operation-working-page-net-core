import { setNewMessage as setNewMessageOnChatBoard } from '../../Shared/chatBoardNew/chatBoardSlice';
import store from '../app/store';

const handleNewMessageIncome = (newMsgInfo: any, ticketInfo: any) => {
  const { chatBoard } = store.getState();
  const { ticketDetail } = chatBoard;
  const { ticketId } = ticketDetail;

  ticketId === newMsgInfo.ticketId &&
    store.dispatch(setNewMessageOnChatBoard(newMsgInfo));
};

export default handleNewMessageIncome;
