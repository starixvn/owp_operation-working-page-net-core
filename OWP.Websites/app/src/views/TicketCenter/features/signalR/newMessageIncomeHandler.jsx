import { setNewMessage as setNewMessageOnChatBoard } from '../../../Shared/chatBoardNew/chatBoardSlice';
import store from '../../app/storeNative';
import { updateTicketItemIntoDataSource } from '../Table/eventSlice/eventCheckingSlice';
import { TAB_CONSOLIDATE, TAB_MY_TICKET } from '../../const/const.json';
const handleNewMessageIncome = (newMsgInfo, ticketInfo) => {
  const { chatBoard, signalR } = store.getState();
  const { filterModel, issueModel, activeTab } = signalR;

  activeTab === TAB_MY_TICKET &&
    store.dispatch(updateTicketItemIntoDataSource({ ticketInfo, filterModel }));
  activeTab === TAB_MY_TICKET &&
    issueModel.ticketId === newMsgInfo.ticketId &&
    chatBoard.isOpenPopup &&
    store.dispatch(setNewMessageOnChatBoard(newMsgInfo));
  console.log(newMsgInfo);
  activeTab === TAB_CONSOLIDATE &&
    store.dispatch(setNewMessageOnChatBoard(newMsgInfo));
};

export { handleNewMessageIncome };
