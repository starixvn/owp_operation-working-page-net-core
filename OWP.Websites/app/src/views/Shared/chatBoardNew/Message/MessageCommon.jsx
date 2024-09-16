import { isEquals } from 'immutability-helper';
import PropTypes from 'prop-types';
import { Fragment, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { shouldShowUserInfo } from '../helper/chatBoardHelper';
import MessageCell from './MessageCell';
import { setTargetMessageId } from '../chatBoardSlice';
import useStableDispatch from '../../customHook/useStableDispatch';

const MessageCommon = ({
  entities,
  permission,
  consolidationInfo = {},
  messageRefs
}) => {
  const dispatch = useStableDispatch();
  const { targetMessageId } = useSelector(state => state.chatBoard);

  useEffect(() => {
    if (targetMessageId > 0 && entities.length > 0) {
      const messageElement = messageRefs?.current[targetMessageId];
      if (messageElement) {
        messageElement.scrollIntoView({ behavior: 'smooth' });
        highLightMessage(messageElement);
      }
    }
  }, [targetMessageId, entities]);

  const highLightMessage = messageElement => {
    const messageDiv = messageElement.querySelector('.message-div');
    if (messageDiv) {
      messageDiv.classList.add('borderAnimation');
    }
    const timeoutId = setTimeout(() => {
      dispatch(setTargetMessageId(-1));
      messageDiv.classList.remove('borderAnimation');
    }, 2000);

    return () => clearTimeout(timeoutId);
  };

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
  isDisableClickIcon: PropTypes.bool,
  entities: PropTypes.array.isRequired,
  permission: PropTypes.object.isRequired,
  consolidationInfo: PropTypes.object,
  messageRefs: PropTypes.shape({
    current: PropTypes.objectOf(PropTypes.instanceOf(Element))
  })
};

export default MessageCommon;
