import NetCoreSignalR from '../../../Shared/NetCoreSignalR/NetCoreSignalR';
import { handleNewMessageIncome } from './newMessageIncomeHandler';
import { handleNewAddRecipient } from './newAddRecipientHandler';
import { handleOpenTicketDetail } from './openTicketDetailHandler';
import { changeMessageIconHandler } from './changeMessageIconHandler';
import handleChangeTicketStatus from './changeStatusIncomeHandler';
import changeSubTypeIncomeHandler from './changeSubTypeIncomeHandler';
import { highlightSubtypeHandler } from './highlightSubtypeHandler';

const useSignalRWrapper = () => {
  const handlers = [
    {
      method: 'NotifyUpdateReactIconMessage',
      handler: changeMessageIconHandler
    },
    {
      method: 'NotifyNewMessageIncome',
      handler: handleNewMessageIncome
    },
    {
      method: 'NotifyNewAddRecipient',
      handler: handleNewAddRecipient
    },
    {
      method: 'NotifyUserOnClickToast',
      handler: handleOpenTicketDetail
    },
    {
      method: 'NotifyChangeStatusIncome',
      handler: handleChangeTicketStatus
    },
    {
      method: 'NotifyChangeSubTypeIncome',
      handler: changeSubTypeIncomeHandler
    },
    {
      method: 'NotifyHighlightSubtype',
      handler: highlightSubtypeHandler
    }
  ];

  return <NetCoreSignalR handlers={handlers} hubName='ticketCenterNotifyHub' />;
};

export default useSignalRWrapper;
