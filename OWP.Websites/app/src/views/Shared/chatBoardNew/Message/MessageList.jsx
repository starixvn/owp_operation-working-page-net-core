import { useLayoutEffect, useRef } from 'react';
import PropTypes from 'prop-types';
import { useSelector } from 'react-redux';
import { isEqual } from 'lodash';
import { REQUEST_STATUS } from '../../Constants';
import Loading from '../../../components/Loading';
import { isEquals } from 'immutability-helper';
import useStableDispatch from '../../customHook/useStableDispatch';
import { loadMoreMessageThunk } from '../chatBoardThunkNative';
import MessageCommon from './MessageCommon';

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
        <MessageCommon entities={messages} permission={permission} />
      </div>
    </>
  );
};

MessageList.propTypes = {
  policy: PropTypes.string,
  ticketId: PropTypes.number
};
export default MessageList;
