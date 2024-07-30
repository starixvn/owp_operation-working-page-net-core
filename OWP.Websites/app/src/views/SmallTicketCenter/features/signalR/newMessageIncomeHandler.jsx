import store from '../../app/storeNative';
import { updateTicketItemIntoDataSource } from '../table/slice/ticketSlice';

const handleNewMessageIncome = (_newMsgInfo, ticketInfo) => {
  const { filter } = store.getState();
  const filterInfo = filter?.entities?.filterInfo;
  store.dispatch(updateTicketItemIntoDataSource({ ticketInfo, filterInfo }));
};

export { handleNewMessageIncome };
