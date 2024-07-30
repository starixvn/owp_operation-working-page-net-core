import store from '../app/storeNative';
import { setDataSignalR } from '../features/table/taskListSlice';
import { handleReload } from '../../Shared/ticketEvent/signalR/reLoadHelper';

const reloadTicketCellTable = (parentMatchId, actorId) => {
  handleReload(store, actorId, parentMatchId, setDataSignalR);
};

export { reloadTicketCellTable };
