import store from '../app/storeNative';
import { reloadPopupDetail } from './signalRHelper';

const changeSubTypeIncomeHandler = () => {
  const { issueModel } = store.getState().signalR;
  reloadPopupDetail(issueModel);
};

export default changeSubTypeIncomeHandler;
