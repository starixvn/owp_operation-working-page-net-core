import PropTypes from 'prop-types';
import { Fragment } from 'react';
import { isEquals } from 'immutability-helper';
import { shouldShowUserInfo } from '../helper/chatBoardHelper';
import MessageCell from './MessageCell';

const MessageCommon = ({
  entities,
  permission,
  isConsolidationView = false
}) => {
  const messageGroupByDate = entities.reduce((result, message) => {
    const date = new Date(message.createdDate);
    if (!result[date.toDateString()]) {
      result[date.toDateString()] = [];
    }
    result[date.toDateString()].push(message);
    return result;
  }, {});

  const renderMessageRow = (message, index, previousMessage) => {
    const key = `${message.messageId}-${index}`;
    const isMyMessage = isEquals(message.userId, permission.ActorId);
    const showUserInfo = shouldShowUserInfo(previousMessage, message);

    return (
      <li
        key={key}
        className={
          isMyMessage ? 'message-row my-message' : 'message-row others-user'
        }>
        <MessageCell
          policy={'pagePolicy'}
          data={message}
          ticketId={message.ticketId}
          isMyMessage={isMyMessage}
          showUserInfo={showUserInfo}
          isConsolidationView={isConsolidationView}
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
  isConsolidationView: PropTypes.bool
};

export default MessageCommon;
