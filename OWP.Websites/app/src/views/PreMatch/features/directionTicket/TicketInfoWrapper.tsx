import TicketInfo from './TicketInfo';
import { useSelector } from 'react-redux';
import type { RootState } from '../../app/store';

const TicketInfoWrapper = () => {
  const ticketInfo = useSelector((state: RootState) => state.directionTicket.directionTicket.ticketInfo);

  return ticketInfo.map((ticket, index) => (
    <TicketInfo key={index} {...ticket} />
  ));
};

export default TicketInfoWrapper;

