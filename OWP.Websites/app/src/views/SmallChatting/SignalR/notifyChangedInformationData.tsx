import { isEqual } from 'lodash';
import store from '../app/store';
import { setIsBlinkTitle } from '../../Shared/chatBoardNew/chatBoardSlice';

const notifyChangedInformationData = data => {
  const { chatBoard } = store.getState();
  const ticketId = chatBoard?.ticketDetail?.ticketId;
  isEqual(process.env.NODE_ENV, 'production') &&
    ticketId === data?.message?.ticketId &&
    data?.message?.isNewMessage &&
    store.dispatch(setIsBlinkTitle(true));
};

export default notifyChangedInformationData;
