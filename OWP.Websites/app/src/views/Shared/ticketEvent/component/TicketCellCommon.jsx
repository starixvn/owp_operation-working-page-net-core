import PropTypes from 'prop-types';
import CreateTicketHandler from '../handler/CreateTicketHandler';
import OpenTicketListHandler from '../handler/OpenTicketListHandler';
import { useDispatch, useSelector } from 'react-redux';
import {
  closePopup,
  getModelByRowIndex,
  getTicketDetail
} from '../../chatBoardNew/chatBoardSlice';
import {
  closeCreatePopup,
  openPopupCreateTicketInLGA
} from '../../createTicketNew/createTicketBoardSliceNew';
import {
  RECIPIENT_TYPE,
  urlRequestLiveIncident
} from '../../createTicketNew/const/const';
import Service from '../../createTicketNew/Service';
import { formatRecipientModel } from '../../createTicketNew/Helper/HelperCreateTicket';
import usePermission from '../../customHook/usePermission';
import { FunctionKeys } from '../../Constants';
import { TICKET_TYPE } from '../../TicketCenterPrematch/const/const.json';
import { post } from '../../Service/Service';
import {
  EVENT_TYPE,
  getDetailSegmentUrlByEventType
} from '../../TicketCenterPrematch/helper/helper';

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
      SubType: 0,
      SubTypeName: '',
      Files: [],
      IsQuickAdd: true,
      url: urlRequestLiveIncident
    };
    debugger;
    const { data } = await post(newAddModel);

    const requestDetailModel = {
      ticketId: data,
      actorId: ActorId,
      detailSegmentUrl: getDetailSegmentUrlByEventType(
        EVENT_TYPE.LIVE_INCIDENT
      ),
      eventType: EVENT_TYPE.LIVE_INCIDENT
    };

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
    />
  );

  const renderQuickAddTicketHandler = () => (
    <div className='btn-block-add-new'>
      <div className='dropdown-list'>
        <button className='button-submit'>Create</button>
        <div className='dropdown-content-ticket'>
          <li className='button-event' onClick={handleQuickAddTicket}>
            Match Group Ticket
          </li>
          <li className='button-event' onClick={handleOpenCreateTicket}>
            New Ticket{' '}
          </li>
        </div>
      </div>
    </div>
  );

  const renderCreateTicketHandler = () => (
    <CreateTicketHandler
      hasWrite={hasWrite}
      handleOpenCreateTicket={handleOpenCreateTicket}
    />
  );

  return (
    <div className='ticket-cell'>
      {dataSource[rowIndex].HasTicket
        ? renderOpenTicketListHandler()
        : allowQuickAdd && hasWrite
        ? renderQuickAddTicketHandler()
        : renderCreateTicketHandler()}
    </div>
  );
};

TicketCellCommon.propTypes = propTypes;

export default TicketCellCommon;
