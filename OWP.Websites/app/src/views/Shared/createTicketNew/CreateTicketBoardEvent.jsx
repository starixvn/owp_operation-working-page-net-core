import { useState } from 'react';
import {
  closeCreatePopup,
  changeErrorRecipient,
  changeErrorInput,
  selectedEvent,
  setSelectedRecipients
} from './createTicketBoardSliceNew';
import Spinner from '../../components/Spinner';
import { post } from '../Service/Service';
import { useSelector } from 'react-redux';
import useStableDispatch from '../customHook/useStableDispatch';
import Event from './Event/Event';
import {
  formatRecipientModel,
  saveFileToBackEnd,
  informResult
} from './Helper/HelperCreateTicket';
import ShareOfChatBoardTicket from './ShareOfChatBoardTicket/ShareOfChatBoardTicket';
import PropTypes from 'prop-types';
import Service from './Service';
import { TicketTypeRecipientEnum } from '../enum/TicketRecipientEnum';

const propTypes = {
  creatorInfo: PropTypes.object.isRequired,
  handleBuildModel: PropTypes.func.isRequired,
  ticketType: PropTypes.number.isRequired
};

const CreateTicketBoardEvent = props => {
  const { creatorInfo, handleBuildModel, ticketType } = props;
  const dispatch = useStableDispatch();
  const {
    valueEvent,
    defaultRecipientsLGA,
    selectedRecipients,
    recipientAssigned,
    subTypeSelected,
    typeTicketText
  } = useSelector(state => state.createTicketBoard);

  const [selectedFiles, setSelectedFiles] = useState([]);

  const renderHeader = () => (
    <span className='close-chat-board-popup' onClick={handleOnClose} />
  );

  const dispatchActionInput = value => {
    !value && dispatch(changeErrorInput(true));
  };
  const dispatchActionRecipient = value => {
    !value.length && dispatch(changeErrorRecipient(true));
  };
  const buildModel = () => handleBuildModel();
  const createRequestEvent = async message => {
    if (!selectedRecipients.length || !valueEvent) {
      dispatchActionInput(valueEvent);
      dispatchActionRecipient(selectedRecipients);
    } else {
      Spinner.show();
      const modelByTicketType = buildModel();
      let recipientLGA = [];
      if (
        valueEvent.owpMatchId &&
        selectedRecipients.some(
          x => x.recipientType === TicketTypeRecipientEnum.Assignee
        )
      ) {
        const model = {
          owpMatchId: valueEvent.owpMatchId
        };
        recipientLGA = await Service.queryAssigneesByOwpMatchId(model);
      }
      const selected = selectedRecipients
        .concat(recipientAssigned)
        .concat(recipientLGA);

      const recipients = formatRecipientModel(selected, creatorInfo);

      const resultSaveFile =
        selectedFiles.length && (await saveFileToBackEnd(selectedFiles));
      const addModel = {
        CreatorId: creatorInfo.recipientId,
        Recipients: recipients,
        Message: message,
        TicketType: ticketType,
        SubType: subTypeSelected.value,
        Files: resultSaveFile || []
      };
      const newAddModel = { ...modelByTicketType, ...addModel };
      const result = await post(newAddModel);
      informResult(result);
      handleOnClose();
      dispatch(selectedEvent(null));
      dispatch(setSelectedRecipients([]));
      Spinner.hide();
    }
  };
  const handleOnSubmit = async message => {
    message = message || 'Please add';
    await createRequestEvent(message);
  };
  const handleOnClose = () => {
    dispatch(closeCreatePopup());
    setSelectedFiles([]);
  };
  return (
    <div className='chat-board-popup' id='create-ticket-popup'>
      {renderHeader()}
      <Event ticketType={ticketType} typeTicketText={typeTicketText} />
      <ShareOfChatBoardTicket
        defaultRecipients={defaultRecipientsLGA}
        selectedRecipients={selectedRecipients}
        handleOnSubmit={handleOnSubmit}
        handleOnClosePopup={handleOnClose}
        selectedFiles={selectedFiles}
        setSelectedFiles={setSelectedFiles}
        ticketType={ticketType}
      />
    </div>
  );
};

CreateTicketBoardEvent.propTypes = propTypes;
export default CreateTicketBoardEvent;
