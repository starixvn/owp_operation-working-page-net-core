import { Component } from 'react';
import PropTypes from 'prop-types';
import StarIcon from 'mdi-react/StarIcon';
import CloudSyncIcon from 'mdi-react/CloudSyncIcon';
import BullhornIcon from 'mdi-react/BullhornIcon';
import AccountIcon from 'mdi-react/AccountIcon';
import { SystemMessageTypes } from '../Message/Service';
import dayjs from 'dayjs';

const propTypes = {
  onClick: PropTypes.func,
  message: PropTypes.shape({
    SystemMessageType: PropTypes.number,
    MessageContent: PropTypes.string,
    IsImportant: PropTypes.bool,
    IsViewed: PropTypes.bool,
    CreatedDate: PropTypes.string
  }).isRequired
};

class MessageItem extends Component {
  constructor(props) {
    super(props);
    this.state = {
      isUnreadClassName: true
    };
  }

  componentDidMount() {
    const { message } = this.props;
    if (message.IsViewed) {
      this.setState({ isUnreadClassName: false });
    }
  }

  componentDidUpdate(prevProps) {
    if (this.props.message.IsViewed !== prevProps.message.IsViewed) {
      this.onUpdate(function callback() {
        this.setState({ isUnreadClassName: false });
      });
    }
  }

  onClickHandle(evt) {
    evt.preventDefault();
    const { message } = this.props;
    if (!message.IsViewed) {
      this.props.onClick(message);
      this.setState({ isUnreadClassName: false });
    }
  }

  render() {
    const {
      SystemMessageType,
      MessageContent,
      IsImportant,
      IsViewed,
      CreatedDate
    } = this.props.message;
    const renderIconImportant = IsImportant ? <StarIcon size={13} /> : null;
    let iconClass = null;
    switch (SystemMessageType) {
      case SystemMessageTypes.Notification:
        iconClass = <CloudSyncIcon size={14} />;
        break;
      case SystemMessageTypes.Announcement:
        iconClass = <BullhornIcon size={14} />;
        break;
      default:
        iconClass = <AccountIcon size={14} />;
        break;
    }
    const { isUnreadClassName } = this.state;
    const getClassName = isUnreadClassName
      ? 'message-item unread'
      : 'message-item';
    return (
      <>
        <div
          className={getClassName}
          data-readed={IsViewed}
          onClick={evt => this.onClickHandle(evt)}>
          <div className='icon'>{iconClass}</div>
          <span className='content'>{MessageContent}</span>
          <span className='iconImportant'>{renderIconImportant}</span>
          <span className='date'>
            {dayjs(CreatedDate)
              .add(12, 'hour')
              .format('MM/DD/YYYY hh:mm A')}
          </span>
        </div>
      </>
    );
  }
}

MessageItem.propTypes = propTypes;
export default MessageItem;
