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
              <td className='ticket-info'>
                <div className='event-name'>{x.Home} -vs- {x.Away}</div>
                <div className='event-date'>
                  {Utils.formatDateTimeDisplay(x.EventDate)}
                </div>
                {x.SubtypeName && (
                  <>
                    <div className='line-subtype' id={`subtype-${x.TicketId}`}>
                      <span className='tag' />
                      {x.SubtypeName.length > SUB_TYPE_MAX_LENGTH
                        ? x.SubtypeName.substring(0, SUB_TYPE_MAX_LENGTH) + '...'
                        : x.SubtypeName}
                    </div>
                    {x.SubtypeName.length > SUB_TYPE_MAX_LENGTH && (
                      <UncontrolledTooltip
                        placement='bottom'
                        target={`subtype-${x.TicketId}`}>
                        {x.SubtypeName}
                      </UncontrolledTooltip>
                    )}
                  </>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default TicketListPopupCommon;
