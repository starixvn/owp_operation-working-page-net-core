import { Modal, ModalBody, ModalFooter } from 'reactstrap';
import LeagueInfo from './LeagueInfo';
import TicketInfo from './TicketInfo';

const DirectionTicketPopup = () => {
  // Example: Get data from Redux store (adjust based on your actual store structure)
  // const dispatch = useDispatch();
  // const { directionTicketData } = useSelector(state => state.directionTicketSlice);

  // useEffect(() => {
  //   // Fetch data if needed
  //   // dispatch(fetchDirectionTicketDataThunk());
  // }, [dispatch]);

  return (
    <Modal
      isOpen
      scrollable
      className='direction-ticket-popup'
      modalClassName='direction-ticket-modal'
      centered>
      <ModalBody>
        <LeagueInfo
          league='V-League'
          event={'Viet name -vs- Thai Land'}
          matchId={1234567890}
          leagueGroup='League Group 1' />
        <TicketInfo />
      </ModalBody>
      <ModalFooter>

      </ModalFooter>
    </Modal>
  );
};

export default DirectionTicketPopup;

