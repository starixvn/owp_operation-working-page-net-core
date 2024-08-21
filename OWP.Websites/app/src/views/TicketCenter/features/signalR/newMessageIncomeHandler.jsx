import { setNewMessage as setNewMessageOnChatBoard } from '../../../Shared/chatBoardNew/chatBoardSlice';
import { setNewMessageConsolidation } from '../consolidation/consolidationView/consolidationSlice';
import store from '../../app/storeNative';
import { updateTicketItemIntoDataSource } from '../Table/eventSlice/eventCheckingSlice';
import { TAB_CONSOLIDATE, TAB_MY_TICKET } from '../../const/const.json';
const handleNewMessageIncome = (newMsgInfo, ticketInfo) => {
  const {
    chatBoard,
    signalR,
    subtype: {
      subTypeSelected: { subTypeId, typeId }
    }
  } = store.getState();
  const { filterModel, issueModel, activeTab } = signalR;

  activeTab === TAB_MY_TICKET &&
    store.dispatch(updateTicketItemIntoDataSource({ ticketInfo, filterModel }));

  activeTab === TAB_MY_TICKET &&
    issueModel.ticketId === newMsgInfo.ticketId &&
    chatBoard.isOpenPopup &&
    store.dispatch(setNewMessageOnChatBoard(newMsgInfo));

  activeTab === TAB_CONSOLIDATE &&
    typeId === ticketInfo.ticketType &&
    subTypeId === ticketInfo.ticketSubTypeId &&
    store.dispatch(setNewMessageConsolidation(newMsgInfo));
};

export { handleNewMessageIncome };
