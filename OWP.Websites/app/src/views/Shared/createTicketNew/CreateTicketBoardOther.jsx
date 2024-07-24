import { useEffect, useState } from 'react';
import {
  closeCreatePopup,
  changeErrorRecipient,
  changeErrorOther,
  otherSubject,
  setSelectedRecipients,
  changeErrorTextField,
  setDefaultRecipients
} from './createTicketBoardSliceNew';
import { urlRequestOther, urlRequestSupport } from './const/const';
import Spinner from '../../components/Spinner';
import { post } from '../Service/Service';
import { useSelector } from 'react-redux';
import useStableDispatch from '../customHook/useStableDispatch';
import Other from './Other/Other';
import {
  formatRecipientModel,
  saveFileToBackEnd,
  informResult
} from './Helper/HelperCreateTicket';
import ShareOfChatBoardTicket from './ShareOfChatBoardTicket/ShareOfChatBoardTicket';
import PropTypes from 'prop-types';
import { TICKET_TYPE } from '../TicketCenterPrematch/const/const.json';
import Loading from '../../components/Loading';

const { TICKET_SUPPORT, GROUP_CHAT } = TICKET_TYPE;

const propTypes = {
  creatorInfo: PropTypes.object.isRequired
};

const CreateTicketBoardOther = props => {
  const { creatorInfo } = props;
  const dispatch = useStableDispatch();
  const {
    isOthers,
    valueOther,
    defaultRecipients,
    selectedRecipients,
    typeTicketText,
    subTypeSelected
  } = useSelector(state => state.createTicketBoard);
  const isTicKetSupport = typeTicketText === 'Ticket Support';
  const ticketType = isTicKetSupport ? TICKET_SUPPORT : GROUP_CHAT;
  const [selectedFiles, setSelectedFiles] = useState([]);

  const dispatchActionOther = value => {
    if (value === '') {
      dispatch(changeErrorOther(true));
      dispatch(changeErrorTextField('This field is required'));
    }
    if (value.length > 200) {
      dispatch(changeErrorOther(true));
      dispatch(
        changeErrorTextField(
          'The maximum of characters is 200. Please try again'
        )
      );
    }
  };
  const dispatchActionRecipient = value => {
    if (!value.length) {
      dispatch(changeErrorRecipient(true));
    }
  };
  const createRequestOther = async message => {
    if (
      selectedRecipients.length === 0 ||
      valueOther === '' ||
      valueOther.length > 200
    ) {
      dispatchActionOther(valueOther);
      dispatchActionRecipient(selectedRecipients);
    } else {
      Spinner.show();
      const Recipients = formatRecipientModel(selectedRecipients, creatorInfo);
      const resultSaveFile =
        selectedFiles.length && (await saveFileToBackEnd(selectedFiles));
      const newAddModel = {
        Files: resultSaveFile || [],
        CreatorId: creatorInfo.recipientId,
        Subject: valueOther,
        Recipients,
        Message: message,
        SubType: subTypeSelected.value,
        SubTypeName: subTypeSelected.label,
        TicketType: isTicKetSupport ? TICKET_SUPPORT : GROUP_CHAT,
        TicketTypeName: typeTicketText,
        url: isTicKetSupport ? urlRequestSupport : urlRequestOther
      };
      const result = await post(newAddModel);
      informResult(result);
      handleOnClose();
      dispatch(otherSubject(''));
      dispatch(setSelectedRecipients([]));
      Spinner.hide();
    }
  };
  const handleOnSubmit = async message => {
    message = message || 'Please add';
    await createRequestOther(message);
  };
  const handleOnClose = () => {
    dispatch(closeCreatePopup());
    setSelectedFiles([]);
  };

  useEffect(() => {
    creatorInfo.userName && dispatch(setDefaultRecipients([creatorInfo]));
  }, [creatorInfo, dispatch]);

  if (!creatorInfo.userName) {
    return <Loading />;
  }

  return (
    <>
      {isOthers && (
        <div className='chat-board-popup' id='create-ticket-other-popup'>
          <span className='close-chat-board-popup' onClick={handleOnClose} />
          <Other />
          <ShareOfChatBoardTicket
            defaultRecipients={defaultRecipients}
            selectedRecipients={selectedRecipients}
            handleOnSubmit={handleOnSubmit}
            handleOnClosePopup={handleOnClose}
            selectedFiles={selectedFiles}
            setSelectedFiles={setSelectedFiles}
            ticketType={ticketType}
          />
        </div>
      )}
    </>
  );
};

CreateTicketBoardOther.propTypes = propTypes;
export default CreateTicketBoardOther;
