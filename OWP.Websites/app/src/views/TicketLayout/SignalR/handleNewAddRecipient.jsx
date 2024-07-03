import store from '../store';
import { reloadPopupDetail } from './signalRHelper';

const handleNewAddRecipient = () => {
  const { issueModel } = store.getState().signalR;
  reloadPopupDetail(issueModel);
};

export default handleNewAddRecipient;
