import { useAppSelector } from '../../../app/hooksNative';
import { isEquals } from 'immutability-helper';
import { Fragment } from 'react';
import InfiniteScroll from 'react-infinite-scroll-component';
import { shouldShowUserInfo } from '../../../../Shared/chatBoardNew/helper/chatBoardHelper';
import MessageCell from '../../../../Shared/chatBoardNew/Message/MessageCell';
import store from '../../../app/storeNative';

const ConsolidationView = () => {
  const {
    subtype: {
      subTypeSelected: { label }
    }
  } = store.getState();
  const { entities } = useAppSelector(state => state.consolidation);

  const messageGroupByDate = entities.reduce((result: any, message) => {
    const date = new Date(message.createdDate);
    if (!result[date.toDateString()]) {
      result[date.toDateString()] = [];
    }
    result[date.toDateString()].push(message);
    return result;
  }, {});

  const renderMessageRow = (
    message: any,
    index: number,
    previousMessage: any
  ) => {
    const key = `${message.messageId}-${index}`;
    const isMyMessage = isEquals(message.userId, 8);
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
          isConsolidationView={true}
        />
      </li>
    );
  };

  return (
    <div className='consolidation-wrap'>
      <div className='header-consolidation'>
        <span className='title'>{label}</span>
        <span className='describe'>(All messages for 3 last days)</span>
      </div>
      <div className='message-list'>
        <InfiniteScroll
          loader={<h4>Loading...</h4>}
          hasMore={false}
          next={() => entities}
          dataLength={entities.length}>
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
                {messageGroupByDate[date].map((message: any, index: number) =>
                  renderMessageRow(
                    message,
                    index,
                    messageGroupByDate[date][index - 1]
                  )
                )}
              </ul>
            </Fragment>
          ))}
        </InfiniteScroll>
      </div>
    </div>
  );
};

export default ConsolidationView;
