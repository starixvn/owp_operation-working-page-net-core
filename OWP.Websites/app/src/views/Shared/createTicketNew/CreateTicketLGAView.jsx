import { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import PropTypes from 'prop-types';
import ShareOfChatBoardTicket from './ShareOfChatBoardTicket/ShareOfChatBoardTicket';
import useStableDispatch from '../customHook/useStableDispatch';
import { formatTimeForTicketLGAPopup } from '../DateTime/dateTimeHelper';
import { post } from '../Service/Service';
import Service from './Service';
import Spinner from '../../components/Spinner';
import {
  changeErrorRecipient,
  setSelectedRecipients,
  closePopupCreateTicketInLGA,
  setDefaultRecipientsLGA,
  setSubTypeSelected
} from './createTicketBoardSliceNew';
import {
  formatRecipientModel,
  saveFileToBackEnd,
  informResult,
  getRecipientDefaultBySubType,
  getSubTypeByType
} from './Helper/HelperCreateTicket';
import {
  urlRequestLiveIncident,
  recipientAssigneeDefault,
  providerCheckingSubTypeId
} from './const/const';
import { TICKET_TYPE } from '../TicketCenterPrematch/const/const.json';
import Loading from '../../components/Loading';
import { TicketTypeRecipientEnum } from '../enum/TicketRecipientEnum';
const { LIVE_INCIDENT } = TICKET_TYPE;

const propTypes = {
  creatorInfo: PropTypes.any,
  isProviderVerification: PropTypes.bool
};

const CreateTicketLGAView = props => {
  const { creatorInfo, isProviderVerification = false } = props;
  const dispatch = useStableDispatch();
  const [selectedFiles, setSelectedFiles] = useState([]);
  const { modelByRowIndex } = useSelector(state => state.chatBoard);
  const {
    selectedRecipients,
    subTypeSelected,
    isOpenCreateTicketInLGA,
    defaultRecipientsLGA
  } = useSelector(state => state.createTicketBoard);

  const { OWPMatchId, League, Home, Away, EventDateTime } = modelByRowIndex;
  const renderHeader = () => (
    <span className='close-chat-board-popup' onClick={handleOnClose} />
  );
  const dispatchActionRecipient = () => {
    dispatch(changeErrorRecipient(true));
  };
  const createRequestEvent = async message => {
    const model = {
      owpMatchId: OWPMatchId
    };
    let recipientLGA = [];
    if (
      OWPMatchId &&
      selectedRecipients.some(
        x => x.recipientType === TicketTypeRecipientEnum.Assignee
      )
    ) {
      recipientLGA = await Service.queryAssigneesByOwpMatchId(model);
    }
    const selected = selectedRecipients
      .filter(x => x.recipientType !== TicketTypeRecipientEnum.Assignee)
      .concat(recipientLGA);

    const reformattedArray = formatRecipientModel(selected, creatorInfo);
    const resultSaveFile =
      selectedFiles.length && (await saveFileToBackEnd(selectedFiles));
    const newAddModel = {
      OwpMatchId: OWPMatchId,
      OwpLeagueName: League,
      OwpHomeName: Home,
      OwpAwayName: Away,
      EventDateTimeGmt8: EventDateTime,
      CreatorId: creatorInfo.recipientId,
      Recipients: reformattedArray,
      Message: message,
      TicketType: LIVE_INCIDENT,
      TicketTypeName: 'Live Incident',
      SubType: subTypeSelected.value,
      SubTypeName: subTypeSelected.label,
      Files: resultSaveFile || [],
      url: urlRequestLiveIncident
    };
    if (!selectedRecipients.length) {
      dispatchActionRecipient();
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
    dispatch(closePopupCreateTicketInLGA());
    setSelectedFiles([]);
  };

  useEffect(() => {
    if (creatorInfo.userName) {
      dispatch(
        setDefaultRecipientsLGA([creatorInfo, recipientAssigneeDefault])
      );
    }
  }, [creatorInfo.userName]);

  useEffect(() => {
    const initializeProviderVerification = async () => {
      if (isProviderVerification && modelByRowIndex?.OWPMatchId) {
        const subTypes = await getSubTypeByType(LIVE_INCIDENT);
        const subtypeSelected = subTypes.find(
          x => x.subTypeId === providerCheckingSubTypeId
        );

        dispatch(
          setSubTypeSelected({
            value: subtypeSelected.subTypeId,
            label: subtypeSelected.subTypeName
          })
        );
        const recipient = await getRecipientDefaultBySubType(
          providerCheckingSubTypeId
        );
        dispatch(setSelectedRecipients(recipient.concat(defaultRecipientsLGA)));
      }
    };

    initializeProviderVerification();
  }, [isProviderVerification, modelByRowIndex?.OWPMatchId]);

  if (!creatorInfo.userName) {
    return <Loading />;
  }

  return (
    isOpenCreateTicketInLGA && (
      <div className='chat-board-popup' id='create-ticket-popup'>
        {renderHeader()}
        <div className='header-popup'>
          <h1> Type: Live Incident </h1>
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
            <div className='league-name-create'>{League}</div>
            <div className='league-event-plus'>
              {Home}&nbsp;vs&nbsp;
              {Away}
            </div>
            <div className='league-ko-time'>
              {' '}
              {`${formatTimeForTicketLGAPopup(EventDateTime, 'minute', 'lll')}`}
            </div>
          </div>
        </div>
        <ShareOfChatBoardTicket
          defaultRecipients={defaultRecipientsLGA}
          selectedRecipients={selectedRecipients}
          handleOnSubmit={handleOnSubmit}
          handleOnClosePopup={handleOnClose}
          selectedFiles={selectedFiles}
          setSelectedFiles={setSelectedFiles}
          ticketType={LIVE_INCIDENT}
        />
      </div>
    )
  );
};
CreateTicketLGAView.propTypes = propTypes;
export default CreateTicketLGAView;
