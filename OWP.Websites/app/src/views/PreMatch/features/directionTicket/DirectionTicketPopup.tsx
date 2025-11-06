import { Modal, ModalBody, ModalFooter, ModalHeader, Button } from 'reactstrap';
import LeagueInfo from './LeagueInfo';
import TicketInfoWrapper from './TicketInfoWrapper';
import { useSelector, useDispatch } from 'react-redux';
import { closePopupCreateDiscussion } from '../../../Shared/createTicketNew/createTicketBoardSliceNew';
import { clearPopupData } from './directionTicketSlice';

const DirectionTicketPopup = () => {
  const dispatch = useDispatch();
  const { isOpenCreateDiscussion } = useSelector(state => state.createTicketBoard);
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
          league='V-League'
          event={'Viet name -vs- Thai Land'}
          matchId={1234567890}
          leagueGroup='League Group 1' />
        <TicketInfoWrapper />
      </ModalBody>
      <ModalFooter>
        <ModalFooter>
          <Button
            color='primary'
            className='btn-add'
            onClick={() => { }}>
            Submit
          </Button>
          <Button
            color='secondary'
            className='btn-cancel'
            onClick={() => {
              dispatch(closePopupCreateDiscussion({}));
              dispatch(clearPopupData());
            }}>
            Cancel
          </Button>
        </ModalFooter>
      </ModalFooter>
    </Modal>
  );
};

export default DirectionTicketPopup;

