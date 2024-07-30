import store from '../store';
import { reloadPopupDetail } from './signalRHelper';

const changeSubTypeIncomeHandler = () => {
  const { issueModel } = store.getState().signalR;
  reloadPopupDetail(issueModel);
};

export default changeSubTypeIncomeHandler;
