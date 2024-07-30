import { useEffect, useState } from 'react';
import PropTypes from 'prop-types';
import MessageList from './Message/MessageList';
import InputMessage from './Message/InputMessage';
import useStableDispatch from '../customHook/useStableDispatch';
import { useSelector } from 'react-redux';
import {
  closePopup,
  createNewMessage,
  setMessage,
  setModelQuoteMessage
} from './chatBoardSlice';
import TicketDetailHeader from './TicketDetailHeader';
import Axios from 'axios';
import { URL, TicketStatus as TicketStatusConst } from './const/index.json';
import {
  getTicketTitle,
  validateFileExtension,
  validateFileSize
} from './helper/chatBoardHelper';
import DragAndDrop from '../common/DragAndDrop/DragAndDrop';
import ToastsWrapper from '../../components/ToastrWrapper';
import { NET_CORE_FILE_UPLOAD } from '../../Shared/Constants';
import { updateTicketStatus } from './chatBoardThunkNative';
import { backToSmallTicket } from '../../Shared/Utils';
import isEqual from 'lodash.isequal';
import TicketDetailInformation from './TicketDetailInformation';
import { KeyboardArrowDownIcon, KeyboardArrowUpIcon } from 'mdi-react';

const TicketDetail = ({ policy = 'policy', isSmallBoard = false }) => {
  const dispatch = useStableDispatch();
  const { permission } = useSelector(state => state[policy]);
  const { HasWrite } = permission;
  const [isExpandInfo, setIsExpandInfo] = useState(false);
  const {
    ticketDetail,
    inputMessage,
    ticketSubTypeOptions,
    modelQuoteMessage,
    isDisplayCloseButton
  } = useSelector(state => state.chatBoard);
  const { ticketId } = ticketDetail;
  const { messageId } = modelQuoteMessage;

  const handleOnClickClosePopUp = () => {
    dispatch(closePopup());
  };

  const renderCloseBtn = () => (
    <span
      data-testid='close-testId'
      className='close-chat-board-popup'
      onClick={handleOnClickClosePopUp}
    />
  );

  const handleSendMessage = async (event, fileId, fileName) => {
    event.preventDefault();
    const {
      ticketId,
      eventType,
      ticketType,
      ticketStatus,
      typeName,
      ticketSubTypeId,
      ticketSubTypeName,
      lastCommentDate,
      isRead,
      isPinned,
      owpLeagueId,
      owpLeagueName,
      owpHomeId,
      owpAwayId,
      homeName,
      awayName,
      koTime,
      subject,
      eventName,
      leagueName
    } = ticketDetail;
    let newMessage = {
      ticketId,
      ticketEventType: eventType,
      ticketTitle: getTicketTitle(ticketDetail),
      message: inputMessage,
      actorId: permission.ActorId,
      fileId: '',
      quoteMessageId: messageId
    };

    if (fileId) {
      newMessage = {
        ...newMessage,
        fileId,
        message: fileName
      };
    }

    newMessage = {
      ...newMessage,
      ticketInfo: {
        ticketId,
        eventType,
        ticketType,
        ticketStatus,
        typeName,
        ticketSubTypeId,
        ticketSubTypeName,
        lastCommentDate,
        isRead,
        isPinned,
        owpLeagueId,
        owpLeagueName,
        owpHomeId,
        owpAwayId,
        homeName,
        awayName,
        koTime,
        subject,
        eventName,
        leagueName
      }
    };

    if (!newMessage.message.trim()) {
      return;
    }

    dispatch(setMessage(''));
    dispatch(setModelQuoteMessage({}));
    const actionResult = await dispatch(createNewMessage(newMessage));

    if (createNewMessage.rejected.match(actionResult)) {
      ToastsWrapper.showError(actionResult.payload);
      return;
    }
    if (isEqual(ticketDetail.ticketStatus, TicketStatusConst.Closed)) {
      const model = {
        ticketId: ticketDetail.ticketId,
        ticketStatus: TicketStatusConst.Open,
        actorId: permission.ActorId,
        ticketInfo: ticketDetail
      };
      await dispatch(updateTicketStatus(model));
    }
  };

  const handleSendFile = async (event, file) => {
    if (!validateFileExtension(file.name)) {
      ToastsWrapper.showError('File extension not allowed!');
    } else if (!validateFileSize(file.size)) {
      ToastsWrapper.showError(
        "The file's size should not larger than 5MB. Try another one"
      );
    } else {
      const formData = new FormData();
      formData.append('file', file, file.name);

      const url = `${NET_CORE_FILE_UPLOAD}/${URL.UPLOAD_FILE}`;

      const { data } = await Axios.post(url, formData);

      await handleSendMessage(event, data, file.name);
    }
  };

  useEffect(() => {
    setIsExpandInfo(false);
    dispatch(setModelQuoteMessage({}));
  }, [ticketId]);

  return (
    <div className='chat-board-popup'>
      <DragAndDrop onDrop={handleSendFile}>
        <div className='chatBoard-Header'>
          {isDisplayCloseButton && renderCloseBtn()}
          <div className='ticket-header'>
            <TicketDetailHeader
              {...ticketDetail}
              isSmallBoard={isSmallBoard}
              backToSmallTicket={backToSmallTicket}
            />
          </div>
        </div>
        <div className='chatBoard-info header-filter'>
          <div className='filter-right'>
            {isExpandInfo ? (
              <>
                <div
                  className='show-info'
                  onClick={() => setIsExpandInfo(false)}>
                  <KeyboardArrowUpIcon />
                  <p>Show less ticket information</p>
                </div>
                <TicketDetailInformation
                  policy={policy}
                  ticketSubTypeOptions={ticketSubTypeOptions}
                  permission={permission}
                  ticketDetail={ticketDetail}
                />
              </>
            ) : (
              <div className='show-info' onClick={() => setIsExpandInfo(true)}>
                <KeyboardArrowDownIcon />
                <p>Show more ticket information</p>
              </div>
            )}
          </div>
        </div>
        <MessageList policy={policy} ticketId={ticketId} />
        {HasWrite && (
          <InputMessage
            handleSendFile={handleSendFile}
            handleSendMessage={handleSendMessage}
          />
        )}
      </DragAndDrop>
    </div>
  );
};
TicketDetail.propTypes = {
  policy: PropTypes.string,
  isSmallBoard: PropTypes.bool
};

export default TicketDetail;
