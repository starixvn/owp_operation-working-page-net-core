import store from '../app/storeNative';
import { reloadPopupDetail } from './signalRHelper';

const changeStatusIncomeHandler = () => {
  const { issueModel } = store.getState().signalR;
  reloadPopupDetail(issueModel);
};

export default changeStatusIncomeHandler;
