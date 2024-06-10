import { updateMessageIconRealTime } from '../../Shared/chatBoardNew/chatBoardSlice';
import store from '../store';

const changeMessageIconHandler = (messageReactIcon, action, ticketId) => {
  const { chatBoard, signalR } = store.getState();
  const { issueModel } = signalR;
  issueModel.ticketId === ticketId &&
    chatBoard.isOpenPopup &&
    store.dispatch(updateMessageIconRealTime({ messageReactIcon, action }));
};
export default changeMessageIconHandler;
