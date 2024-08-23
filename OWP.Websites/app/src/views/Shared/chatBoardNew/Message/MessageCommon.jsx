import { isEquals } from 'immutability-helper';
import PropTypes from 'prop-types';
import { Fragment, useEffect, useRef } from 'react';
import { useSelector } from 'react-redux';
import { shouldShowUserInfo } from '../helper/chatBoardHelper';
import MessageCell from './MessageCell';

const MessageCommon = ({
  entities,
  permission,
  consolidationInfo = {},
  messageRefs
}) => {
  const { targetMessageId } = useSelector(state => state.chatBoard);
  //console.log('re-render', targetMessageId);

  useEffect(() => {
    if (targetMessageId > 0 && entities.length > 0) {
      const messageElement = messageRefs?.current[targetMessageId];
      if (messageElement) {
        messageElement && messageElement.scrollIntoView({ behavior: 'smooth' });
        const messageDiv = messageElement.querySelector('.message-div');
        if (messageDiv) {
          messageDiv.classList.add('borderAnimation');
        }
      }
    }
  }, [targetMessageId, entities]);

  const messageGroupByDate = entities.reduce((result, message) => {
    const date = new Date(message.createdDate);
    if (!result[date.toDateString()]) {
      result[date.toDateString()] = [];
    }
    result[date.toDateString()].push(message);
    return result;
  }, {});
  const setMessageRef = (messageId, element) => {
    if (element && messageRefs && messageRefs.current) {
      messageRefs.current[messageId] = element;
    }
  };

  const renderMessageRow = (message, index, previousMessage) => {
    const key = `${message.messageId}-${index}`;
    const isMyMessage = isEquals(message.userId, permission.ActorId);
    const showUserInfo = shouldShowUserInfo(previousMessage, message);

    return (
      <li
        key={key}
        ref={el => setMessageRef(message.messageId, el)}
        className={
          isMyMessage ? 'message-row my-message' : 'message-row others-user'
        }>
        <MessageCell
          policy={'pagePolicy'}
          data={message}
          ticketId={message.ticketId}
          isMyMessage={isMyMessage}
          showUserInfo={showUserInfo}
          consolidationInfo={consolidationInfo}
        />
      </li>
    );
  };

  return (
    <>
      {Object.keys(messageGroupByDate).map((date, key) => (
        <Fragment key={`${date}-${key}`}>
          <div>
            <h2 className='date-time-line'>
              <span>
                {new Date(date).toLocaleDateString('en-US', {
                  month: 'long',
                  day: 'numeric',
                  year: 'numeric'
                })}
              </span>
            </h2>
          </div>

          <ul>
            {messageGroupByDate[date].map((message, index) =>
              renderMessageRow(
                message,
                index,
                messageGroupByDate[date][index - 1]
              )
            )}
          </ul>
        </Fragment>
      ))}
    </>
  );
};

MessageCommon.propTypes = {
  entities: PropTypes.array.isRequired,
  permission: PropTypes.object.isRequired,
  consolidationInfo: PropTypes.object,
  messageRefs: PropTypes.shape({
    current: PropTypes.objectOf(PropTypes.instanceOf(Element))
  })
};

export default MessageCommon;
