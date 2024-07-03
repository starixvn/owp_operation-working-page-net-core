import store from '../store';
import { reloadPopupDetail } from './signalRHelper';

const handleNewMessageIncome = () => {
  const { issueModel } = store.getState().signalR;
  reloadPopupDetail(issueModel);
};

export default handleNewMessageIncome;
