import { Modal, ModalBody, ModalFooter } from 'reactstrap';
import LeagueInfo from './LeagueInfo';
import TicketInfoWrapper from './TicketInfoWrapper';

const DirectionTicketPopup = () => {
  return (
    <Modal
      isOpen
      scrollable
      className='direction-ticket-popup'
      modalClassName='direction-ticket-modal'
      size='lg'
      centered>
      <ModalBody>
        <LeagueInfo
          league='V-League'
          event={'Viet name -vs- Thai Land'}
          matchId={1234567890}
          leagueGroup='League Group 1' />
        <TicketInfoWrapper />
      </ModalBody>
      <ModalFooter>

      </ModalFooter>
    </Modal>
  );
};

export default DirectionTicketPopup;

