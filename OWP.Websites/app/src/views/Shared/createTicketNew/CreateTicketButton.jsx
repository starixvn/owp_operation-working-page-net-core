import useStableDispatch from '../customHook/useStableDispatch';
import {
  openPopupEventChecking,
  openPopupLiveIncident,
  openPopupOthers,
  setTypeTicketText,
  openPopupLogWork,
  closePopupLogWork
} from './createTicketBoardSliceNew';
import { closePopup } from '../chatBoardNew/chatBoardSlice';
import { BUTTON_TYPE } from './const/const';
import PropTypes from 'prop-types';

const { LIVE_GAME, PRE_MATCH, MY_TICKET } = BUTTON_TYPE;

const propTypes = {
  buttonType: PropTypes.string,
  isCreateLogWork: PropTypes.bool
};

const CreateTicketButton = ({ buttonType, isCreateLogWork = false }) => {
  const isShowEventCheckingButton =
    buttonType === MY_TICKET || buttonType === PRE_MATCH;
  const isShowLiveIncidentButton =
    buttonType === MY_TICKET || buttonType === LIVE_GAME;

  const dispatch = useStableDispatch();

  return (
    <div className='btn-block-add-new'>
      <div className='dropdown-list'>
        <button className='button-submit'>Create</button>
        <div className='dropdown-content-ticket'>
          {isShowEventCheckingButton && (
            <li
              className='button-event'
              onClick={() => {
                dispatch(setTypeTicketText('Event Checking'));
                dispatch(closePopup());
                dispatch(closePopupLogWork());
                dispatch(openPopupEventChecking());
              }}>
              Event Checking
            </li>
          )}
          <li
            className='button-others'
            onClick={() => {
              dispatch(setTypeTicketText('Group Chat'));
              dispatch(closePopup());
              dispatch(closePopupLogWork());
              dispatch(openPopupOthers());
            }}>
            Group Chat
          </li>
          <li
            className='button-support'
            onClick={() => {
              dispatch(setTypeTicketText('Ticket Support'));
              dispatch(closePopup());
              dispatch(closePopupLogWork());
              dispatch(openPopupOthers());
            }}>
            Ticket Support
          </li>
          {isShowLiveIncidentButton && (
            <li
              className='button-live-incident'
              onClick={() => {
                dispatch(setTypeTicketText('Live Incident'));
                dispatch(closePopup());
                dispatch(closePopupLogWork());
                dispatch(openPopupLiveIncident());
              }}>
              Live Incident
            </li>
          )}
          {isCreateLogWork && (
            <li
              className='button-work-log'
              onClick={() => {
                dispatch(closePopup());
                dispatch(openPopupLogWork());
              }}>
              S&VR
            </li>
          )}
        </div>
      </div>
    </div>
  );
};
CreateTicketButton.propTypes = propTypes;
export default CreateTicketButton;
