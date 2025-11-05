import { nanoid } from '@reduxjs/toolkit';
import { TTicketInfo } from '../../type';
import TicketInfo from './TicketInfo';
import { useSelector } from 'react-redux';


const TicketInfoWrapper = () => {
  const { ticketInfo } = useSelector(state => state.directionTicket.data);
  return ticketInfo.map((ticket: TTicketInfo, index: number) => (
    <TicketInfo
      key={nanoid()}
      index={index}
      isRenderRemoveCommand={ticketInfo.length > 1}
      isRenderAddCommand={index === ticketInfo.length - 1}
      ticketInfo={ticket}
    />

  ));
};

export default TicketInfoWrapper;

