import { Component } from 'react';
import PropTypes from 'prop-types';
import MessageService, { SystemMessageTypes } from '../Message/Service';
import StarIcon from 'mdi-react/StarIcon';
import CloudSyncIcon from 'mdi-react/CloudSyncIcon';
import BullhornIcon from 'mdi-react/BullhornIcon';
import AccountIcon from 'mdi-react/AccountIcon';

const propTypes = {
  data: PropTypes.object.isRequired
};

class NotificationItem extends Component {
  renderMessageIcon() {
    const { SystemMessageType } = this.props.data;
    switch (SystemMessageType) {
      case SystemMessageTypes.Notification:
        return (
          <span className='icon-circle'>
            <CloudSyncIcon size={14} />
          </span>
        );
      case SystemMessageTypes.Announcement:
        return (
          <span className='icon-circle'>
            <BullhornIcon size={14} />
          </span>
        );
      default:
        return (
          <span className='icon-circle'>
            <AccountIcon size={14} />
          </span>
        );
    }
  }

  renderMessageContent() {
    const { TimeAgo, MessageType } = this.props.data;
    let messageContent = MessageService.customizeContent(this.props.data);
    messageContent = MessageService.customizeOSContent(
      MessageType,
      messageContent
    );
    messageContent = MessageService.customizePreMatchContent(
      MessageType,
      messageContent
    );
    messageContent = MessageService.customizeTicketCenterContent(
      MessageType,
      messageContent
    );
    messageContent = MessageService.customizeLiveDetailContent(
      MessageType,
      messageContent
    );
    return (
      <div className='notification-content'>
        <p
          className='message-content'
          dangerouslySetInnerHTML={{ __html: messageContent }}
        />
        <p className='time'>{TimeAgo}</p>
      </div>
    );
  }

  render() {
    const { IsImportant } = this.props.data;
    return (
      <div className='noti-item'>
        {this.renderMessageIcon()}
        {this.renderMessageContent()}
        {IsImportant && (
          <StarIcon className='star-icon' size={14} color='#ff6000' />
        )}
      </div>
    );
  }
}

NotificationItem.propTypes = propTypes;
export default NotificationItem;
