import { updateMessageIconRealTime } from '../../Shared/chatBoardNew/chatBoardSlice';
import store from '../app/storeNative';

const changeMessageIconHandler = (messageReactIcon, action, ticketId) => {
  const { chatBoard } = store.getState();
  const { ticketDetail } = chatBoard;
  ticketDetail.ticketId === ticketId &&
    chatBoard.isOpenPopup &&
    store.dispatch(updateMessageIconRealTime({ messageReactIcon, action }));
};
export { changeMessageIconHandler };
