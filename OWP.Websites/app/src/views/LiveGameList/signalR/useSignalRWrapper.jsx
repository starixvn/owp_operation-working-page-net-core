import { reloadTicketCellTable } from './signalRHelper';
import { handleChangeStatus } from './changeStatusIncomeHandler';
import { handleChangeSubtype } from './changeSubtypeIncomeHandler';
import { changeMessageIconHandler } from './changeMessageIconHandler';
import SignalRWrapper from '../../Shared/ticketEvent/signalR/signalRWrapper';

const useSignalRWrapper = () => {
  return (
    <SignalRWrapper
      changeMessageIconHandler={changeMessageIconHandler}
      reloadTicketCellTable={reloadTicketCellTable}
      handleChangeStatus={handleChangeStatus}
      handleChangeSubtype={handleChangeSubtype}
    />
  );
};

export default useSignalRWrapper;
