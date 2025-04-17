import { openCurrentChattingTab } from '../../../Shared/Utils';
import { useAppSelector } from '../../app/hooksNative';
import cellFactory from './cell/cellFactory';
import IconOpenNewTab from '../../../Shared/IconCustom/IconOpenNewTab';

const TicketList = () => {
  const ticketList = useAppSelector(state => state.tickerSlice.entities);
  const actorId = useAppSelector(
    state => state?.pagePolicy?.permission?.ActorId
  );
  const handleOnClick = (event, ticketId: number, ticketType: number) => {
    if (event?.target?.closest('.icon-new-tab')) {
      return;
    }
    openCurrentChattingTab(actorId, ticketId, ticketType);
  };
  return (
    <div className='table__content--wrapper'>
      <table className='table__content'>
        <thead>
          <tr>
            <th>Conversations</th>
          </tr>
        </thead>
        <tbody>
          {ticketList.map((ticket, index) => (
            <tr className='ticket__list--item' key={index}>
              <td
                className='ticket-list-cell-item'
                onClick={event =>
                  handleOnClick(event, ticket.ticketId, ticket.eventType)
                }>
                <div className='cell'>
                  {cellFactory(ticket)}
                  <IconOpenNewTab
                    ticketId={ticket.ticketId}
                    eventType={ticket.eventType}></IconOpenNewTab>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default TicketList;
