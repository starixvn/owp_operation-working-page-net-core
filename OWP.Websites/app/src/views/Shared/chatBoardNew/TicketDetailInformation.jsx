import { useCallback } from 'react';
import PropTypes from 'prop-types';
import { TicketSubType } from './TicketSubType';
import TicketStatus from './TicketStatus';
import RecipientsSelect from '../recipientSelect/RecipientsSelect';
import { getMessageList, getTicketTitle } from './helper/chatBoardHelper';
import Axios from 'axios';
import { URL } from './const/index.json';

const TicketDetailInformation = ({
  policy,
  ticketSubTypeOptions,
  permission,
  ticketDetail
}) => {
  const { HasWrite, ActorId } = permission;

  const { ticketId, eventType, messageId, messages } = ticketDetail;

  const handleRemoveRecipient = useCallback(
    async recipient => {
      const { recipientId, recipientType } = recipient;

      const requestModel = {
        ticketId,
        recipientId,
        recipientType,
        ActorId,
        ticketEventType: eventType
      };
      const url =
        process.env.NODE_ENV === 'development'
          ? `${URL.NET_Core_TICKET_CENTER}/${URL.REMOVE_SINGLE_RECIPIENT}`
          : `${window.netCoreRootUrl}/${URL.NET_FW_TICKET_CENTER}/${URL.REMOVE_SINGLE_RECIPIENT}`;

      const { status, data } = await Axios.post(url, requestModel);
      return { status, data };
    },
    [ActorId, ticketId, eventType]
  );

  const handleAddRecipient = useCallback(
    async recipient => {
      const messageList =
        messages.length === 0
          ? await getMessageList(ticketId, messageId, messages)
          : messages;

      const modelTicket = {
        ...ticketDetail,
        messages: messageList,
        lastCommentDate: messageList[0]?.createdDate
      };
      const requestModel = {
        ticketId,
        ticketTitle: getTicketTitle(ticketDetail),
        ticketEventType: eventType,
        actorId: ActorId,
        recipientId: recipient.recipientId,
        recipientType: recipient.recipientType,
        ticketInfo: modelTicket
      };

      const url =
        process.env.NODE_ENV === 'development'
          ? `${URL.NET_Core_TICKET_CENTER}/${URL.ADD_SINGLE_RECIPIENT}`
          : `${window.netCoreRootUrl}/${URL.NET_FW_TICKET_CENTER}/${URL.ADD_SINGLE_RECIPIENT}`;

      const { status, data } = await Axios.post(url, requestModel);
      return { status, data };
    },
    [ActorId, ticketDetail]
  );

  return (
    <>
      <div className='ticket-type'>
        <div className='event-type '>
          <label>Ticket Type:</label>
          <br />
          <span>{ticketDetail.typeName}</span>
        </div>
        <div className='ticket-subtype'>
          <label>Subtype:</label>
          <div className='subtype select'>
            <TicketSubType options={ticketSubTypeOptions} policy={policy} />
          </div>
        </div>
        <div className='ticket-status'>
          <TicketStatus policy={policy} />
        </div>
      
      <div className='recipient'>
        <label>
          Recipient:<span className='required'>*</span>
        </label>
        <br />
        <div className='recipient-input'>
          <RecipientsSelect
            defaultValue={ticketDetail.recipients}
            onAdd={handleAddRecipient}
            onRemove={handleRemoveRecipient}
            edit
            hasWrite={HasWrite}
          />
        </div>
      </div>
      </div>
    </>
  );
};
TicketDetailInformation.propTypes = {
  policy: PropTypes.string,
  ticketSubTypeOptions: PropTypes.array,
  permission: PropTypes.object,
  ticketDetail: PropTypes.object
};
export default TicketDetailInformation;
