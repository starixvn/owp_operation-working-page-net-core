import NetCoreSignalR from '../../../Shared/NetCoreSignalR/NetCoreSignalR';
import {
  reloadPreMatch,
  reloadStatusOpenTicketHandler,
  reloadSubTypeOpenTicketHandler,
  reloadTicketCellInTable,
  changeMessageIconHandler
} from './reloadPreMatchHandler';

const SignalRWrapper = () => {
  const handlers = [
    {
      method: 'NotifyUpdateReactIconMessage',
      handler: changeMessageIconHandler
    },
    {
      method: 'NotifyNewMessageIncome',
      handler: reloadPreMatch
    },
    {
      method: 'NotifyNewAddRecipient',
      handler: reloadTicketCellInTable
    },
    {
      method: 'NotifyChangeStatusIncome',
      handler: reloadStatusOpenTicketHandler
    },
    {
      method: 'NotifyChangeSybTypeIncome',
      handler: reloadSubTypeOpenTicketHandler
    }
  ];

  return <NetCoreSignalR handlers={handlers} hubName='preMatchNotifyHub' />;
};
export default SignalRWrapper;
