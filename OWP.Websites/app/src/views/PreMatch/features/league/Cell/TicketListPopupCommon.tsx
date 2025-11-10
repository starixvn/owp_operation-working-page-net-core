import { useEffect, useRef } from 'react';
import Utils from '../../../../../views/Shared/Utils';
import { UncontrolledTooltip } from 'reactstrap';
import { MessageTextOutlineIcon } from 'mdi-react';
import { TPopup } from '../../../../../views/Shared/ticketList/type';

const SUB_TYPE_MAX_LENGTH = 50;

const TicketListPopupCommon = (props: TPopup) => {
  const {
    HasWrite,
    ticketList,
    isMatch,
    handleOpenCreateTicket,
    handleOpenCreateDiscussion,
    handleOpenMessage,
    onCloseTicketList,
    allowQuickAdd,
    handleQuickAddTicket
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
    const abort = new AbortController();
    document.addEventListener('mousedown', handleClickOutside, abort);
    return () => abort.abort();
  }, [wrapperDivRef, onCloseTicketList]);

  return (
    <div className='ticket-list-popup is-show' ref={wrapperDivRef}>
      {HasWrite && (
        <div className='button'>
          {isMatch && ( 
            <button
              className='button-submit'
              onClick={() => {
                handleOpenCreateDiscussion();
              }}>
              Direction Group (RC)
            </button>
          )}
           <button
            className='button-submit'
            onClick={() => {
              handleOpenCreateTicket();
            }}>
            New ticket
          </button>
          {allowQuickAdd && (
            <button
              className='button-submit'
              onClick={() => {
                onCloseTicketList();
                handleQuickAddTicket();
              }}>
              Match Group Ticket
            </button>
          )}
        </div>
      )}
      <table className='ticket-list-table'>
        <tbody>
          {ticketList?.map((x, index) => (
            <tr
              key={`${x.TicketId}-${index}`}
              onClick={e => handleOpenMessage(e, x)}>
              <td>
                {!x.IsRead && <div className='notification-dot'></div>}
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
