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
import { IconRocket, IconRocketChat } from '../../common/icons/common-icons';
import { openNewChattingTab } from '../../Utils';
import { createDiscussionThunk } from '../../../TaskList/features/table/tableThunk';
import { setDataSignalR } from '../../../TaskList/features/table/taskListSlice';
import { getServerUrl } from '../../RcChat/Service';

const propTypes = {
  allowQuickAdd: PropTypes.bool,
  dataSource: PropTypes.any,
  rowIndex: PropTypes.number,
  onShowTicketList: PropTypes.func,
  updateDataSources: PropTypes.func,
  authToken: PropTypes.string
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

    const recipients = await Service.queryAssigneesByParentMatchId(OWPMatchId);
    const userNames = recipients.map(item => item.userName);

    const discussionName = await stableDispatch(
      createDiscussionThunk({
        t_name: `${KickOffTimeDisplay} - ${Home} -vs- ${Away}`,
        reply: replyMessage,
        users: userNames,
        owpMatchId: OWPMatchId
      })
    ).unwrap();

    const width = window.innerWidth / 2;
    const height = window.innerHeight / 2;
    const left = (window.innerWidth - width) / 2;
    const top = (window.innerHeight - height) / 2;

    const baseUrl = `${await getServerUrl()}/group/${discussionName}`;
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

    stableDispatch(
      setDataSignalR({
        actorId: creatorInfo.recipientId,
        parentMatchId: OWPMatchId
      })
    );
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
      authToken={authToken}
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
          <IconRocketChat className={'mdi-icon icon-plus icon-rocket'} />
          <span className='btn-name'>Match Group Discussion</span>
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

  const renderTicketContent = () => {
    if (dataSource[rowIndex].HasTicket) {
      return renderOpenTicketListHandler();
    }

    if (allowQuickAdd && HasWrite) {
      return renderQuickAddTicketHandler();
    }

    return renderCreateTicketHandler();
  };

  return <div >{renderTicketContent()}</div>;
};

TicketCellCommon.propTypes = propTypes;

export default TicketCellCommon;
