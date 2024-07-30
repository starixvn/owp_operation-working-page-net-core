import { useEffect, useRef } from 'react';
import Utils from '../Utils';
import { UncontrolledTooltip } from 'reactstrap';
import { MessageTextOutlineIcon } from 'mdi-react';
import { TPopup } from './type';

const SUB_TYPE_MAX_LENGTH = 50;

const TicketListPopupCommon = (props: TPopup) => {
  const {
    HasWrite,
    ticketList,
    handleOpenCreateTicket,
    handleOpenMessage,
    onCloseTicketList
  } = props;

  const wrapperDivRef = useRef<HTMLDivElement>(null);

  const color = (isRead: boolean) =>
    isRead ? 'icon-message' : 'icon-unread-message';

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        wrapperDivRef.current &&
        !wrapperDivRef.current.contains(event.target as Node)
      ) {
        onCloseTicketList();
      }
    };
    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, [wrapperDivRef, onCloseTicketList]);

  return (
    <div className='ticket-list-popup is-show' ref={wrapperDivRef}>
      {HasWrite && (
        <div className='button'>
          <button
            className='button-submit'
            onClick={() => {
              handleOpenCreateTicket();
            }}>
            Add Ticket
          </button>
        </div>
      )}
      <table className='ticket-list-table'>
        <tbody>
          {ticketList?.map((x, index) => (
            <tr
              key={`${x.TicketId}-${index}`}
              onClick={e => handleOpenMessage(e, x)}>
              <td>
                <MessageTextOutlineIcon
                  className={color(x.IsRead)}
                  color={color(x.IsRead)}
                />
              </td>
              <td>#{x.TicketId}</td>
              <td>
                <div className='line-subtype' id={`subtype-${x.TicketId}`}>
                  {x.SubtypeName?.length > SUB_TYPE_MAX_LENGTH
                    ? x.SubtypeName.substring(0, SUB_TYPE_MAX_LENGTH) + '...'
                    : x.SubtypeName}
                </div>
                {x.SubtypeName?.length > SUB_TYPE_MAX_LENGTH && (
                  <UncontrolledTooltip
                    placement='bottom'
                    target={`subtype-${x.TicketId}`}>
                    {x.SubtypeName}
                  </UncontrolledTooltip>
                )}
              </td>
              <td>
                <div className='line-message'>
                  <span className='username'>{x.LatestUsername}</span>
                  <span className='time'>
                    {' '}
                    - {Utils.formatDateTimeString(x.LatestCreatedDate)} :{' '}
                  </span>
                  {x.LatestMessage}
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default TicketListPopupCommon;
