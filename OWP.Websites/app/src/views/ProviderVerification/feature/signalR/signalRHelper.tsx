import store from '../../app/storeNative';
import { setDataSignalR } from '../table/tableSlice';
import { handleReloadEventAssignment } from '../../../Shared/ticketEvent/signalR/reLoadHelper';

const reloadTicketCellTable = (parentMatchId: number, actorId: number) => {
  const userId = store.getState().pagePolicy.permission.ActorId;
  handleReloadEventAssignment(
    store,
    actorId,
    userId,
    parentMatchId,
    setDataSignalR
  );
};

export { reloadTicketCellTable };
