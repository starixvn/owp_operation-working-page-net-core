import { useEffect, useRef } from 'react';
import Utils from '../Utils';
import { UncontrolledTooltip } from 'reactstrap';
import { MessageTextOutlineIcon } from 'mdi-react';
import { TPopup } from './type';
import IconOpenNewTab from '../IconCustom/IconOpenNewTab';
import { EVENT_TYPE } from '../TicketCenterPrematch/helper/helper';
import { IconRocketChat } from '../common/icons/common-icons';

const SUB_TYPE_MAX_LENGTH = 50;

const TicketListPopupCommon = (props: TPopup) => {
  const {
    HasWrite,
    ticketList,
    handleOpenCreateTicket,
    handleOpenMessage,
    onCloseTicketList,
    allowQuickAdd,
    handleQuickAddTicket,
    handleCreateDiscussionRC,
    authToken
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
          {allowQuickAdd && (
            <>
              <button
                className='button-submit'
                onClick={() => {
                  onCloseTicketList();
                  handleCreateDiscussionRC();
                }}>
                OWP Chat Discussion
              </button>
              <button
                className='button-submit'
                onClick={() => {
                  onCloseTicketList();
                  handleQuickAddTicket();
                }}>
                Match Group Ticket
              </button>
            </>
          )}
          <button
            className='button-submit'
            onClick={() => {
              handleOpenCreateTicket();
            }}>
            New ticket
          </button>
        </div>
      )}
      <table className='ticket-list-table'>
        <tbody>
          {ticketList?.map((x, index) => (
            <tr
              key={`${x.TicketId}-${index}`}
              onClick={e => (x.IsRC === true ? null : handleOpenMessage(e, x))}>
              <td>
                <div className='icon-container'>
                  {!x.IsRead && <div className='notification-dot'></div>}
                  {x.IsRC ? (
                    <IconRocketChat className={'mdi-icon icon-message'} />
                  ) : (
                    <MessageTextOutlineIcon
                      className={color(x.IsRead)}
                      color={color(x.IsRead)}
                    />
                  )}
                </div>
              </td>
              <td className='ticket-info'>
                <div className='event-name'>
                  {x.Home} -vs- {x.Away}
                </div>
                <div className='event-date'>
                  {Utils.formatDateTimeDisplay(x.EventDate)}
                </div>
                {x.SubtypeName && (
                  <>
                    <div className='line-subtype' id={`subtype-${x.TicketId}`}>
                      <span className='tag' />
                      {x.SubtypeName.length > SUB_TYPE_MAX_LENGTH
                        ? x.SubtypeName.substring(0, SUB_TYPE_MAX_LENGTH) +
                          '...'
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
              <td>
                <IconOpenNewTab
                  eventType={EVENT_TYPE.LIVE_INCIDENT}
                  ticketId={x.TicketId}
                  isRC={x.IsRC}
                  discussionName={x.DiscussionName}
                  authToken={authToken}></IconOpenNewTab>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default TicketListPopupCommon;
