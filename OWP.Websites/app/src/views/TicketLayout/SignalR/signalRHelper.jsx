import { getTicketDetail } from '../../Shared/chatBoardNew/chatBoardSlice';
import store from '../store';

const reloadPopupDetail = model => {
  const { isOpenPopup } = store.getState().chatBoard;
  isOpenPopup && store.dispatch(getTicketDetail(model));
};

export { reloadPopupDetail };
