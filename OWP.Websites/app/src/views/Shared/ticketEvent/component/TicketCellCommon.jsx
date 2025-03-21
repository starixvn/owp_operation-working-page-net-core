import PropTypes from 'prop-types';
import CreateTicketHandler from '../handler/CreateTicketHandler';
import OpenTicketListHandler from '../handler/OpenTicketListHandler';
import { useDispatch, useSelector } from 'react-redux';
import { PlusCircleIcon, PlusIcon } from 'mdi-react';
import {
  closePopup,
  getModelByRowIndex,
  getTicketDetail,
  getTicketSubTypes
} from '../../chatBoardNew/chatBoardSlice';
import {
  closeCreatePopup,
  openPopupCreateTicketInLGA
} from '../../createTicketNew/createTicketBoardSliceNew';
import {
  QUICK_ADD_SUBTYPE,
  RECIPIENT_TYPE,
  urlRequestLiveIncident
} from '../../createTicketNew/const/const';
import Service from '../../createTicketNew/Service';
import { getSubTypeByType } from '../../createTicketNew/Helper/HelperCreateTicket';
import usePermission from '../../customHook/usePermission';
import { FunctionKeys } from '../../Constants';
import { TICKET_TYPE } from '../../TicketCenterPrematch/const/const.json';
import { post } from '../../Service/Service';
import {
  EVENT_TYPE,
  getDetailSegmentUrlByEventType
} from '../../TicketCenterPrematch/helper/helper';
import { IconRocket } from '../../common/icons/common-icons';

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
    updateDataSources
  } = props;

  const hasWrite = useSelector(state => state.pagePolicy).permission.HasWrite;
  const pagePermissionModel = usePermission(
    FunctionKeys.TicketCenter,
    'pagePolicy'
  );
  const { permission } = pagePermissionModel;
  const { Username, ActorId } = permission;
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

    const newAddModel = {
      OwpMatchId: OWPMatchId,
      OwpLeagueName: League,
      OwpHomeName: Home,
      OwpAwayName: Away,
      EventDateTimeGmt8: EventDateTime,
      CreatorId: creatorInfo.recipientId,
      Recipients: [
        {
          RecipientType: RECIPIENT_TYPE.INDIVIDUAL,
          RecipientIds: recipients.map(r => r.RecipientId)
        }
      ],
      Message: 'Please add',
      TicketType: LIVE_INCIDENT,
      TicketTypeName: 'Live Incident',
      SubType: subTypeMatchGroupChat.subTypeId,
      SubTypeName: subTypeMatchGroupChat.subTypeName,
      Files: [],
      IsQuickAdd: true,
      url: urlRequestLiveIncident
    };

    const { data } = await post(newAddModel);

    const requestDetailModel = {
      ticketId: data,
      actorId: ActorId,
      detailSegmentUrl: getDetailSegmentUrlByEventType(
        EVENT_TYPE.LIVE_INCIDENT
      ),
      eventType: EVENT_TYPE.LIVE_INCIDENT
    };

    stableDispatch(getTicketSubTypes(LIVE_INCIDENT));
    stableDispatch(
      getTicketDetail({
        ...requestDetailModel,
        isQueryMessageInLastTwoDay: true
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
      hasWrite={hasWrite}
      handleOpenCreateTicket={handleOpenCreateTicket}
    />
  );

  return (
    <>
      {dataSource[rowIndex].HasTicket
        ? renderOpenTicketListHandler()
        : allowQuickAdd && hasWrite
        ? renderQuickAddTicketHandler()
        : renderCreateTicketHandler()}
    </>
  );
};

TicketCellCommon.propTypes = propTypes;

export default TicketCellCommon;
