import NetCoreSignalR from '../../../Shared/NetCoreSignalR/NetCoreSignalR';
import PropTypes from 'prop-types';

const propTypes = {
  reloadTicketCellTable: PropTypes.func,
  handleChangeStatus: PropTypes.func,
  handleChangeSubtype: PropTypes.func,
  changeMessageIconHandler: PropTypes.func
};

const SignalRWrapper = props => {
  const handlers = [
    {
      method: 'NotifyUpdateReactIconMessage',
      handler: props.changeMessageIconHandler
    },
    {
      method: 'NotifyNewMessageLga',
      handler: props.reloadTicketCellTable
    },
    {
      method: 'NotifyNewAddRecipientLga',
      handler: props.reloadTicketCellTable
    },
    {
      method: 'NotifyChangeStatusIncome',
      handler: props.handleChangeStatus
    },
    {
      method: 'NotifyChangeSubTypeIncome',
      handler: props.handleChangeSubtype
    }
  ];
  return <NetCoreSignalR handlers={handlers} hubName='ticketCenterNotifyHub' />;
};

SignalRWrapper.propTypes = propTypes;
export default SignalRWrapper;
