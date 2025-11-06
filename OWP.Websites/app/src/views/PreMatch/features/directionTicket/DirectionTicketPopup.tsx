import { Modal, ModalBody, ModalFooter, ModalHeader, Button } from 'reactstrap';
import LeagueInfo from './LeagueInfo';
import TicketInfoWrapper from './TicketInfoWrapper';
import { useSelector, useDispatch } from 'react-redux';
import { closePopupCreateDiscussion } from '../../../Shared/createTicketNew/createTicketBoardSliceNew';
import { clearPopupData } from './directionTicketSlice';
import { validateTicketInfo } from './validationThunk';
import store from '../../app/store';

const DirectionTicketPopup = () => {
  const { isOpenCreateDiscussion } = useSelector(state => state.createTicketBoard);
  const { league, event, matchId, leagueGroup } = store.getState().directionTicket.data;
  debugger
  const dispatch = useDispatch();
  const handleSubmit = async () => {
    const ticketInfo = store.getState().directionTicket.data.ticketInfo;
    const resultAction = await dispatch(validateTicketInfo(ticketInfo)).unwrap();

    if (resultAction?.valid) {

    }
    console.log('success');
  };

  const handleCancel = () => {
    dispatch(closePopupCreateDiscussion({}));
    dispatch(clearPopupData());
  };

  return (
    <Modal
      isOpen={isOpenCreateDiscussion}
      scrollable
      className='direction-ticket-popup'
      modalClassName='direction-ticket-modal'
      size='lg'
      centered>
      <ModalHeader>
        <div className='title'>League Information</div>
      </ModalHeader>
      <ModalBody>
        <LeagueInfo
          league={league}
          event={event}
          matchId={matchId}
          leagueGroup={leagueGroup} />
        <TicketInfoWrapper />
      </ModalBody>
      <ModalFooter>
        <ModalFooter>
          <Button
            color='primary'
            className='btn-add'
            onClick={handleSubmit}>
            Submit
          </Button>
          <Button
            color='secondary'
            className='btn-cancel'
            onClick={handleCancel}>
            Cancel
          </Button>
        </ModalFooter>
      </ModalFooter>
    </Modal>
  );
};

export default DirectionTicketPopup;

