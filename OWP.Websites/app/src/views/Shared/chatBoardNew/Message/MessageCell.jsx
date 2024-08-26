import { ArrowRightIcon } from 'mdi-react';
import PropTypes from 'prop-types';
import { useEffect, useRef, useState } from 'react';
import { useSelector } from 'react-redux';
import { UncontrolledTooltip } from 'reactstrap';
import GoodIcon from '../../../../assets/svg-element/GoodIcon';
import ReplyIcon from '../../../../assets/svg-element/ReplyIcon';
import { setModelQuoteMessage } from '../../chatBoardNew/chatBoardSlice';
import AvatarDisplay from '../../common/Avatar/AvatarDisplay';
import useStableDispatch from '../../customHook/useStableDispatch';
import openTicketDetailHandler from '../helper/openTicketDetailHandler';
import MessageContents from './MessageContentNative';
import ReactionBar from './ReactionBar';

const propTypes = {
  data: PropTypes.object.isRequired,
  showUserInfo: PropTypes.bool,
  policy: PropTypes.string.isRequired,
  ticketId: PropTypes.number,
  isMyMessage: PropTypes.bool,
  consolidationInfo: PropTypes.object
};

const MessageCell = ({
  data,
  policy = 'policy',
  ticketId,
  isMyMessage,
  showUserInfo,
  consolidationInfo
}) => /* NOSONAR */ {
  const {
    createdDate,
    message,
    username,
    extension,
    fileId,
    messageId,
    quoteMessageId,
    quoteMessage,
    quoteUsername,
    quoteCreatedDate,
    imageFileName,
    messageReactIcons,
    avatarFileName,
    borderColor
  } = data;

  const dispatch = useStableDispatch();
  const ref = useRef(null);
  const [isOpenReactionIcon, setIsOpenReactionIcon] = useState(false);
  const { permission } = useSelector(state => state[policy]);
  const { HasWrite } = permission;
  const { isConsolidationView = false } = consolidationInfo || {};
  const { getCreatedTime, getMessageContent } = MessageContents({
    createdDate,
    fileId,
    messageContent: message,
    quoteMessageId,
    quoteMessage,
    quoteUsername,
    quoteCreatedDate,
    imageFileName,
    messageReactIcons,
    ticketId,
    borderColor
  });

  const handleClickQuoteIcon = () => {
    const model = {
      messageId,
      message,
      createdDate,
      username
    };

    dispatch(setModelQuoteMessage(model));
  };

  const handleOnClickFlyToMessage = async () => {
    const { eventType } = consolidationInfo;
    await openTicketDetailHandler(
      ticketId,
      permission.ActorId,
      eventType,
      dispatch,
      messageId
    );
  };

  const renderActionIcon = () =>
    HasWrite && (
      <span className='icon-hover-wrapper'>
        <span className='icon-reply-message'>
          {!fileId && (
            <ReplyIcon handleOnClick={() => handleClickQuoteIcon()} />
          )}
        </span>
        <span className='icon-react-message'>
          <GoodIcon
            width={17}
            height={17}
            handleOnClick={() => setIsOpenReactionIcon(!isOpenReactionIcon)}
          />
        </span>
      </span>
    );

  const renderFlyToMessageTicketIcon = () =>
    HasWrite && (
      <span className='icon-hover-wrapper'>
        <span
          className='icon-fly-to-ticket'
          onClick={handleOnClickFlyToMessage}
          id={`name${messageId}`}>
          <ArrowRightIcon size={13} />
        </span>
        <UncontrolledTooltip placement='top' target={`name${messageId}`}>
          View on ticket
        </UncontrolledTooltip>
      </span>
    );

  const handleShowIcon = () => {
    if (isConsolidationView) {
      return renderFlyToMessageTicketIcon();
    }
    return renderActionIcon();
  };

  if (message) {
    useEffect(() => {
      const handleClickOutside = event => {
        if (ref.current && !ref.current.contains(event.target)) {
          setIsOpenReactionIcon(false);
        }
      };
      document.addEventListener('click', handleClickOutside);

      return () => {
        document.removeEventListener('click', handleClickOutside);
      };
    }, [setIsOpenReactionIcon]);
    return (
      <div ref={ref}>
        {!isConsolidationView && (
          <ReactionBar
            isOpenReactionIcon={isOpenReactionIcon}
            setIsOpenReactionIcon={setIsOpenReactionIcon}
            messageId={messageId}
            creatorId={permission.ActorId}
            ticketId={ticketId}
          />
        )}
        {showUserInfo && (
          <div className='info-user'>
            <div className='name-user'>
              {!isMyMessage && (
                <>
                  {extension?.length > 0
                    ? `${username} (${extension})`
                    : username}
                  {','}
                </>
              )}{' '}
              {getCreatedTime()}
            </div>
          </div>
        )}

        <div className='message-group' style={{ whiteSpace: 'pre-wrap' }}>
          {handleShowIcon()}
          {getMessageContent()}
          {showUserInfo && !isMyMessage && (
            <AvatarDisplay imageFileName={avatarFileName} />
          )}
          {!showUserInfo && !isMyMessage && (
            <div style={{ width: '27px' }}></div>
          )}
        </div>
      </div>
    );
  }
  return <div>No comments</div>;
};

MessageCell.propTypes = propTypes;
export default MessageCell;
