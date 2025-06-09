import PropTypes from 'prop-types';
import CreateTicketHandler from '../handler/CreateTicketHandler';
import OpenTicketListHandler from '../handler/OpenTicketListHandler';
import { useDispatch, useSelector } from 'react-redux';
import { PlusCircleIcon, PlusIcon } from 'mdi-react';
import {
  closePopup,
  getModelByRowIndex
} from '../../chatBoardNew/chatBoardSlice';
import {
  closeCreatePopup,
  closePopupCreateTicketInLGA,
  openPopupCreateTicketInLGA
} from '../../createTicketNew/createTicketBoardSliceNew';
import {
  QUICK_ADD_SUBTYPE,
  urlRequestLiveIncident
} from '../../createTicketNew/const/const';
import Service from '../../createTicketNew/Service';
import {
  formatRecipientModel,
  getRecipientDefaultBySubType,
  getSubTypeByType
} from '../../createTicketNew/Helper/HelperCreateTicket';
import { TICKET_TYPE } from '../../TicketCenterPrematch/const/const.json';
import { post } from '../../Service/Service';
import { EVENT_TYPE } from '../../TicketCenterPrematch/helper/helper';
import { IconRocket } from '../../common/icons/common-icons';
import { openNewChattingTab } from '../../Utils';

const propTypes = {
  allowQuickAdd: PropTypes.bool,
  dataSource: PropTypes.any,
  rowIndex: PropTypes.number,
  onShowTicketList: PropTypes.func,
  updateDataSources: PropTypes.func
};

const { LIVE_INCIDENT } = TICKET_TYPE;

const TicketCellCommon = props => {
  const {
    allowQuickAdd,
    dataSource,
    rowIndex,
    onShowTicketList,
    updateDataSources,
    authToken
  } = props;
  const { permission } = useSelector(state => state.pagePolicy);
  const { Username, ActorId, HasWrite } = permission;

  const creatorInfo = {
    groupName: null,
    nickName: Username,
    recipientId: ActorId,
    recipientType: 1,
    userName: Username
  };

  const stableDispatch = useDispatch();

  const closeAnotherPopup = () => {
    stableDispatch(closePopup());
    stableDispatch(closeCreatePopup());
    stableDispatch(closePopupCreateTicketInLGA());
  };

  const handleOpenCreateTicket = () => {
    closeAnotherPopup();
    stableDispatch(getModelByRowIndex(dataSource[rowIndex]));
    stableDispatch(openPopupCreateTicketInLGA());
  };

  const handleQuickAddTicket = async () => {
    closeAnotherPopup();
    const { OWPMatchId, League, Home, Away, EventDateTime } =
      dataSource[rowIndex];

    const recipients = await Service.queryAssigneesByParentMatchId(OWPMatchId);
    const subTypesByLiveIncidentType = await getSubTypeByType(LIVE_INCIDENT);
    const subTypeMatchGroupChat = subTypesByLiveIncidentType.find(
      x => x.subTypeName === QUICK_ADD_SUBTYPE.MATCH_GROUP_CHAT
    );
    const recipientsBySubType = await getRecipientDefaultBySubType(2016);
    const Recipients = formatRecipientModel(
      recipients.concat(recipientsBySubType),
      creatorInfo
    );

    const newAddModel = {
      OwpMatchId: OWPMatchId,
      OwpLeagueName: League,
      OwpHomeName: Home,
      OwpAwayName: Away,
      EventDateTimeGmt8: EventDateTime,
      CreatorId: creatorInfo.recipientId,
      Recipients,
      Message:
        'Hi all, I have just created this group chat for the live matter discussion.',
      TicketType: LIVE_INCIDENT,
      TicketTypeName: 'Live Incident',
      SubType: subTypeMatchGroupChat.subTypeId,
      SubTypeName: subTypeMatchGroupChat.subTypeName,
      Files: [],
      IsQuickAdd: true,
      url: urlRequestLiveIncident
    };

    const { data } = await post(newAddModel);

    openNewChattingTab(ActorId, data, EVENT_TYPE.LIVE_INCIDENT);
  };

  const handleCreateDiscussionRC = async () => {
    closeAnotherPopup();
    const {
      OWPMatchId,
      League,
      Home,
      Away,
      KickOffTimeDisplay,
      AdminSiteId,
      IPTVCopy,
      Provider
    } = dataSource[rowIndex];

    const replyMessage = `
  K.O Time: ${KickOffTimeDisplay}
  League: ${League}
  Match: ${Home} -vs- ${Away}
  Match ID: ${AdminSiteId ?? ''}
  TV/IPTV: ${IPTVCopy ?? ''}
  Provider: ${Provider ?? ''}
    `.trim();

    try {
      const recipients = await Service.queryAssigneesByParentMatchId(
        OWPMatchId
      );
      const userNames = recipients.map(item => item.userName);

      const response = await fetch(
        `${import.meta.env.VITE_CHAT_SEVER_URL}/api/v1/rooms.createDiscussion`,
        {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'X-Auth-Token': import.meta.env.VITE_OWP_BOT_TOKEN,
            'X-User-Id': import.meta.env.VITE_OWP_BOT_ID
          },
          body: JSON.stringify({
            prid: import.meta.env.VITE_LA_TODAY_MATCH_CHANNEL_ID,
            t_name: `${KickOffTimeDisplay} - ${Home} -vs- ${Away}`,
            users: userNames,
            topic: 'Live Soccer',
            reply: replyMessage
          })
        }
      );

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || 'Failed to create discussion');
      }

      const data = await response.json();

      const width = window.innerWidth / 2;
      const height = window.innerHeight / 2;
      const left = (window.innerWidth - width) / 2;
      const top = (window.innerHeight - height) / 2;

      const baseUrl = `${import.meta.env.VITE_CHAT_SEVER_URL}/group/${
        data.discussion.rid
      }`;
      const params = new URLSearchParams({
        layout: 'embedded'
      });
      if (authToken) {
        params.set('resumeToken', authToken);
      }

      const chatUrl = `${baseUrl}?${params.toString()}`;

      window.open(
        chatUrl,
        '_blank',
        `toolbar=no,location=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=${width},height=${height},top=${top},left=${left}`
      );

      return true;
    } catch (err) {
      console.error('Create discussion failed:', err);
      return false;
    }
  };

  const renderOpenTicketListHandler = () => (
    <OpenTicketListHandler
      dataSources={dataSource}
      index={rowIndex}
      onShowTicketList={onShowTicketList}
      updateDataSources={updateDataSources}
      handleOpenCreateTicket={handleOpenCreateTicket}
      allowQuickAdd={allowQuickAdd}
      handleQuickAddTicket={handleQuickAddTicket}
      handleCreateDiscussionRC={handleCreateDiscussionRC}
    />
  );

  const renderQuickAddTicketHandler = () => (
    <>
      <span style={{ margin: '10px' }}>
        <PlusCircleIcon className='icon-plus' />
      </span>
      <div className='popup-add-ticket'>
        <div
          className='button-add-ticket btn-quick'
          onClick={handleCreateDiscussionRC}>
          <IconRocket className={'mdi-icon icon-plus icon-rocket'} />
          <span className='btn-name'>OWPChat Discussion</span>
        </div>
        <div
          className='button-add-ticket btn-quick'
          onClick={handleQuickAddTicket}>
          <IconRocket className={'mdi-icon icon-plus icon-rocket'} />
          <span className='btn-name'>Match Group Ticket</span>
        </div>
        <div
          className='button-add-ticket btn-add'
          onClick={handleOpenCreateTicket}>
          <PlusIcon className='icon-plus' />
          <span className='btn-name'>New Ticket</span>
        </div>
      </div>
    </>
  );

  const renderCreateTicketHandler = () => (
    <CreateTicketHandler
      hasWrite={HasWrite}
      handleOpenCreateTicket={handleOpenCreateTicket}
    />
  );

  return (
    <div className='ticket-cell'>
      {dataSource[rowIndex].HasTicket
        ? renderOpenTicketListHandler()
        : allowQuickAdd && HasWrite
        ? renderQuickAddTicketHandler()
        : renderCreateTicketHandler()}
    </div>
  );
};

TicketCellCommon.propTypes = propTypes;

export default TicketCellCommon;
