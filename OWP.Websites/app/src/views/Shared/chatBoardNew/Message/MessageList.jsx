import { Fragment, useLayoutEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../Constants';
import MessageCell from './MessageCell';
import Loading from '../../../components/Loading';
import { isEquals } from 'immutability-helper';
import useStableDispatch from '../../customHook/useStableDispatch';
import { loadMoreMessageThunk } from '../chatBoardThunkNative';
import dayjs from 'dayjs';

const MessageList = ({ policy = 'policy', ticketId }) => {
  const listRef = useRef();
  const lastMessageIdRef = useRef(0);
  const previousScrollDiff = useRef(0);
  const dispatch = useStableDispatch();
  const {
    loading,
    isAllImagesLoaded,
    ticketDetail: { messages, canShowMoreMessage }
  } = useSelector(state => state.chatBoard);
  const { permission } = useSelector(state => state[policy]);

  useLayoutEffect(() => {
    if (messages?.length) {
      const lastPosition = messages.length - 1;
      const lastMessageId = messages[lastPosition].messageId;
      if (!isEquals(lastMessageIdRef.current, lastMessageId)) {
        previousScrollDiff.current = 0;
        lastMessageIdRef.current = lastMessageId;
      }
      if (listRef.current) {
        listRef.current.scrollTop =
          listRef.current?.scrollHeight - previousScrollDiff.current;
      }
    }
  }, [messages, isAllImagesLoaded]);

  if (isEqual(loading, REQUEST_STATUS.pending)) {
    return <Loading />;
  }

  const messageGroupByDate = messages.reduce((result, message) => {
    const date = message.createdDate.split('T')[0];
    if (!result[date]) {
      result[date] = [];
    }
    result[date].push(message);
    return result;
  }, {});

  const renderMessageRow = (message, index, previousMessage) => {
    const key = `${message.messageId}-${index}`;
    const isMyMessage = isEquals(message.userId, permission.ActorId);
    const showUserInfo =
      !previousMessage ||
      previousMessage.userId !== message.userId ||
      dayjs(message.createdDate).diff(
        dayjs(previousMessage.createdDate),
        'minute'
      ) >= 10;

    return (
      <li
        key={key}
        className={
          isMyMessage ? 'message-row my-message' : 'message-row others-user'
        }>
        <MessageCell
          policy={policy}
          data={message}
          ticketId={ticketId}
          isMyMessage={isMyMessage}
          showUserInfo={showUserInfo}
        />
      </li>
    );
  };

  const handleLoadMoreMessage = () => {
    dispatch(
      loadMoreMessageThunk({
        msgOfPage: 20,
        ticketId,
        firstMsgId: messages[0]?.messageId
      })
    );
  };

  const handleScroll = () => {
    previousScrollDiff.current =
      listRef.current.scrollHeight - listRef.current.scrollTop;
  };

  return (
    <>
      <div className='message-list ' ref={listRef} onScroll={handleScroll}>
        {canShowMoreMessage && (
          <a className='load-more' onClick={handleLoadMoreMessage}>
            Load more
          </a>
        )}{' '}
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
      </div>
    </>
  );
};

MessageList.propTypes = {
  policy: PropTypes.string,
  ticketId: PropTypes.number
};
export default MessageList;
