import store from '../app/store';
import { setDataSignalR } from '../table/eventAssignmentTableSlice';
import { handleReloadEventAssignment } from '../../Shared/ticketEvent/signalR/reLoadHelper';

const reloadTicketCellTable = (parentMatchId, actorId) => {
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
