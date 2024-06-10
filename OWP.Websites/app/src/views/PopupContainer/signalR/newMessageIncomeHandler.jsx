import store from '../app/storeNative';
import { reloadPopupDetail } from './signalRHelper';

const handleNewMessageIncome = () => {
  const { issueModel } = store.getState().signalR;
  reloadPopupDetail(issueModel);
};

export { handleNewMessageIncome };
