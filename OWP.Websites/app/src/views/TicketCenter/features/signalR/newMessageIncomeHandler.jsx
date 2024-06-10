import { setNewMessage as setNewMessageOnChatBoard } from '../../../Shared/chatBoardNew/chatBoardSlice';
import store from '../../app/storeNative';
import { updateTicketItemIntoDataSource } from '../Table/eventSlice/eventCheckingSlice';

const handleNewMessageIncome = (newMsgInfo, ticketInfo) => {
  const { chatBoard, signalR } = store.getState();
  const { filterModel, issueModel } = signalR;

  store.dispatch(updateTicketItemIntoDataSource({ ticketInfo, filterModel }));
  issueModel.ticketId === newMsgInfo.ticketId &&
    chatBoard.isOpenPopup &&
    store.dispatch(setNewMessageOnChatBoard(newMsgInfo));
};

export { handleNewMessageIncome };
