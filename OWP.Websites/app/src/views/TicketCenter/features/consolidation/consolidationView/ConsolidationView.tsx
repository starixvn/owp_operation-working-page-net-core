import { useAppSelector } from '../../../app/hooksNative';
import { useLayoutEffect, useRef, useState } from 'react';
import InfiniteScroll from 'react-infinite-scroll-component';
import store from '../../../app/storeNative';
import { useStableDispatch } from '../../../ImportTicketCenterHelperNative';
import MessageCommon from '../../../../Shared/chatBoardNew/Message/MessageCommon';
import { resetNewMessage } from './consolidationSlice';

const ConsolidationView = () => {
  const { subTypeSelected } = store.getState().subtype;
  const { label } = subTypeSelected;
  const { entities, isNewMessage } = useAppSelector(
    state => state.consolidation
  );
  const [hasVerticalScrollbar, setHasVerticalScrollbar] = useState(false);
  const { permission } = useAppSelector(state => state.consolidationPolicy);
  const dispatch = useStableDispatch();
  const scrollRef = useRef<HTMLDivElement>(null);
  const scrollToBottom = () => {
    scrollRef.current &&
      scrollRef.current.scrollIntoView({ behavior: 'smooth' });
    dispatch(resetNewMessage());
  };

  useLayoutEffect(() => {
    const element = document.querySelector('.infinite-scroll-component');
    const check = (element && element?.scrollHeight > 800) || false;
    setHasVerticalScrollbar(check);
  }, [entities]);

  return (
    <div className='consolidation-wrap'>
      <div className='header-consolidation'>
        <span className='title'>{label}</span>
        <span className='describe'>(All messages for last 3 days)</span>
      </div>

      <div className='message-list'>
        <InfiniteScroll
          loader={<h4>Loading...</h4>}
          hasMore={false}
          next={() => entities}
          dataLength={entities.length}>
          {hasVerticalScrollbar && isNewMessage && (
            <button className='btn-new-message-below' onClick={scrollToBottom}>
              New message below
            </button>
          )}
          <MessageCommon
            entities={entities}
            permission={permission}
            consolidationInfo={{
              ...subTypeSelected,
              isConsolidationView: true
            }}
          />
          <div ref={scrollRef} />
        </InfiniteScroll>
      </div>
    </div>
  );
};

export default ConsolidationView;
