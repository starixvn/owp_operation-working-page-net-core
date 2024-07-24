import { useState } from 'react';
import PropTypes from 'prop-types';
import ShareOfChatBoardTicket from '../../../Shared/createTicketNew/ShareOfChatBoardTicket/ShareOfChatBoardTicket';
import { closeTicketPopup } from '../../../Shared/chatBoardNew/chatBoardSlice';
import useStableDispatch from '../../../Shared/customHook/useStableDispatch';
import { formatTime } from '../../../Shared/DateTime/dateTimeHelper';
import Service from './Service/Service';
import { post } from '../../../Shared/Service/Service';
import Spinner from '../../../components/Spinner';
import ToastrWrapper from '../../../components/ToastrWrapper';
import {
  changeErrorRecipient,
  setSelectedRecipients
} from '../../../Shared/createTicketNew/createTicketBoardSliceNew';
import {
  formatRecipientModel,
  saveFileToBackEnd
} from '../../../Shared/createTicketNew/Helper/HelperCreateTicket';
import {
  urlRequestEvent,
  urlRequestLeague
} from '../../../Shared/createTicketNew/const/const';
import { TICKET_TYPE } from '../../../Shared/TicketCenterPrematch/const/const.json';

const { EVENT_CHECKING } = TICKET_TYPE;
const propTypes = {
  selectedRecipients: PropTypes.any,
  defaultRecipients: PropTypes.any,
  modelByRowIndex: PropTypes.any,
  creatorInfo: PropTypes.any,
  subTypeSelected: PropTypes.any
};
const informResult = result => {
  if (result) {
    ToastrWrapper.showInfo(
      <h4 className='eventTicket-createTicket-success'>
        Your ticket has been created
      </h4>
    );
  } else {
    ToastrWrapper.showError(
      <h4 className='eventTicket-createTicket-failure'>Please try again</h4>
    );
  }
};
const ViewCreateTicketBoard = props => {
  const dispatch = useStableDispatch();
  const [selectedFiles, setSelectedFiles] = useState([]);
  const {
    selectedRecipients,
    modelByRowIndex,
    creatorInfo,
    subTypeSelected,
    defaultRecipients
  } = props;
  const { InfoSiteMatchId, PreMatchLeagueId, LeagueId } = modelByRowIndex;

  const dispatchActionRecipient = value => {
    if (value.length === 0) {
      dispatch(changeErrorRecipient(true));
    }
  };

  const createRequestEvent = async message => {
    const recipientPreMatch = await Service.GetRecipientPreMatch(
      PreMatchLeagueId
    );
    const selected = selectedRecipients.concat(recipientPreMatch.data);
    const reformattedArray = formatRecipientModel(selected, creatorInfo);
    const resultSaveFile =
      selectedFiles.length && (await saveFileToBackEnd(selectedFiles));
    let newAddModel;

    if (InfoSiteMatchId) {
      const infoMatchById = await Service.GetInfoSiteMatch(InfoSiteMatchId);
      newAddModel = {
        InfoSiteMatchId,
        CreatorId: creatorInfo.recipientId,
        OwpAwayId: infoMatchById.data.OwpAwayId,
        OwpHomeId: infoMatchById.data.OwpHomeId,
        OwpLeagueId: infoMatchById.data.OwpLeagueId,
        Recipients: reformattedArray,
        Message: message,
        TicketType: EVENT_CHECKING,
        SubType: subTypeSelected.value,
        Files: resultSaveFile || [],
        OwpHomeName: modelByRowIndex.HomeName,
        OwpAwayName: modelByRowIndex.AwayName,
        OwpLeagueName: modelByRowIndex.LeagueName,
        EventDateTimeGmt8: modelByRowIndex.EventDateTime,
        SubTypeName: subTypeSelected.label,
        TicketTypeName: 'Event Checking',
        url: urlRequestEvent
      };
    } else {
      newAddModel = {
        InfoSiteMatchId: 0,
        CreatorId: creatorInfo.recipientId,
        OwpAwayId: null,
        OwpHomeId: null,
        OwpLeagueId: LeagueId,
        Recipients: reformattedArray,
        Message: message,
        TicketType: EVENT_CHECKING,
        SubType: subTypeSelected.value,
        Files: resultSaveFile || [],
        OwpLeagueName: modelByRowIndex.LeagueName,
        SubTypeName: subTypeSelected.label,
        TicketTypeName: 'Event Checking',
        url: urlRequestLeague
      };
    }
    if (selectedRecipients.length === 0) {
      dispatchActionRecipient(selectedRecipients);
    } else {
      Spinner.show();
      const result = await post(newAddModel);
      informResult(result);
      handleOnClose();
      dispatch(setSelectedRecipients([]));
      Spinner.hide();
    }
  };

  const handleOnSubmit = async message => {
    message = message || 'Please add';
    await createRequestEvent(message);
  };
  const handleOnClose = () => {
    dispatch(closeTicketPopup());
    setSelectedFiles([]);
  };

  return (
    <div className='chat-board-popup' id='create-ticket-popup'>
      <span className='close-chat-board-popup' onClick={handleOnClose} />
      <div className='header-popup'>
        <h1> Type: Event Checking </h1>
      </div>
      <div className='form-createTicket'>
        <div className='left-form'>
          <label
            className='label'
            style={{ marginTop: '10px' }}
            htmlFor='subject'>
            Event<span className='required'>*</span>
          </label>
        </div>
        <div className='right-form'>
          {InfoSiteMatchId ? (
            <>
              <div className='league-name-create'>
                {modelByRowIndex.LeagueName}
              </div>
              <div className='league-event-plus'>
                {modelByRowIndex.HomeName}&nbsp;vs&nbsp;
                {modelByRowIndex.AwayName}
              </div>
              <div className='league-ko-time'>
                {' '}
                {`${formatTime(
                  modelByRowIndex.EventDateTime,
                  'minute',
                  'lll'
                )}`}
              </div>
            </>
          ) : (
            <div className='league-name-create'>
              {modelByRowIndex.LeagueName}
            </div>
          )}
        </div>
      </div>
      <ShareOfChatBoardTicket
        defaultRecipients={defaultRecipients}
        selectedRecipients={selectedRecipients}
        handleOnSubmit={handleOnSubmit}
        handleOnClosePopup={handleOnClose}
        selectedFiles={selectedFiles}
        setSelectedFiles={setSelectedFiles}
        ticketType={EVENT_CHECKING}
      />
    </div>
  );
};

ViewCreateTicketBoard.propTypes = propTypes;
export default ViewCreateTicketBoard;
