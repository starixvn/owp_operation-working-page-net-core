import { getTicketDetail } from '../../Shared/chatBoardNew/chatBoardSlice';
import store from '../app/storeNative';

const reloadPopupDetail = model => {
  const { isOpenPopup } = store.getState().chatBoard;
  isOpenPopup && store.dispatch(getTicketDetail(model));
};

export { reloadPopupDetail };
