import store from '../app/storeNative';
import { reloadPopupDetail } from './signalRHelper';

const handleNewAddRecipient = () => {
  const { issueModel } = store.getState().signalR;
  reloadPopupDetail(issueModel);
};

export { handleNewAddRecipient };
