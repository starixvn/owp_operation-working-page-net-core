import PropTypes from 'prop-types';
import MessageContents from './MessageContentNative';
import useStableDispatch from '../../customHook/useStableDispatch';
import { UncontrolledTooltip } from 'reactstrap';
import { ContentSaveOutlineIcon } from 'mdi-react';
import { URL } from '../const/index.json';
import { saveMessage } from '../chatBoardThunkNative';
import { setModelQuoteMessage } from '../../chatBoardNew/chatBoardSlice';
import { useSelector } from 'react-redux';
import ToastrWrapper from '../../../components/ToastrWrapper';
import ReplyIcon from '../../../../assets/svg-element/ReplyIcon';
import ReactionBar from './ReactionBar';
import { useState, useRef, useEffect } from 'react';
import GoodIcon from '../../../../assets/svg-element/GoodIcon';
import AvatarDisplay from '../../common/Avatar/AvatarDisplay';
import { netCoreURLTicKetCenter } from '../const/apiUrl';

const url = `${netCoreURLTicKetCenter}/${URL.UPDATE_SAVE_MESSAGE}`;

const propTypes = {
  data: PropTypes.object.isRequired,
  showUserInfo: PropTypes.bool,
  policy: PropTypes.string.isRequired,
  ticketId: PropTypes.number,
  isMyMessage: PropTypes.bool,
  isConsolidationView: PropTypes.bool
};

const MessageCell = ({
  data,
  policy = 'policy',
  ticketId,
  isMyMessage,
  showUserInfo,
  isConsolidationView = false
}) => {
  const {
    createdDate,
    message,
    username,
    extension,
    fileId,
    messageId,
    isSaved,
    quoteMessageId,
    quoteMessage,
    quoteUsername,
    quoteCreatedDate,
    imageFileName,
    messageReactIcons,
    avatarFileName
  } = data;
  const dispatch = useStableDispatch();
  const ref = useRef(null);
  const [isOpenReactionIcon, setIsOpenReactionIcon] = useState(false);
  const { permission } = useSelector(state => state[policy]);
  const { HasWrite } = permission;
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
    ticketId
  });

  const handleSaveMessages = async value => {
    const model = {
      url,
      messageId,
      isSaved: value,
      ticketId,
      actorId: permission.ActorId
    };
    const actionResult = dispatch(saveMessage(model));
    if (saveMessage.rejected.match(actionResult)) {
      ToastrWrapper.showError(actionResult.payload);
    }
  };

  const handleClickQuoteIcon = () => {
    const model = {
      messageId,
      message,
      createdDate,
      username
    };

    dispatch(setModelQuoteMessage(model));
  };

  const renderSaveIcon = () =>
    HasWrite && (
      <span className='icon-hover-wrapper'>
        <span className='icon-save-message'>
          <ContentSaveOutlineIcon
            onClick={() => handleSaveMessages(true)}
            id={`name${messageId}`}
          />
        </span>
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
        <UncontrolledTooltip placement='top' target={`name${messageId}`}>
          Click to save your message or it is deleted after 30 days
        </UncontrolledTooltip>
      </span>
    );
  const renderUnSaveIcon = () =>
    HasWrite && (
      <span className='icon-hover-wrapper-unsave'>
        <span className='icon-save-message unsave-message'>
          <span className='text-saved'> Saved </span>
          <span className='icon-unsave-message'>
            <ContentSaveOutlineIcon onClick={() => handleSaveMessages(false)} />
          </span>
        </span>
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

  const handleShowIcon = () => {
    if (isConsolidationView) {
      return <>icon scroll to message</>;
    }
    return isSaved ? renderUnSaveIcon() : renderSaveIcon();
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
