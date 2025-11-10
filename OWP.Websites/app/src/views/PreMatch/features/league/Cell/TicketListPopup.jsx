import PropTypes from 'prop-types';
import {
  getTicketDetail,
  openTicketPopup,
  closeTicketPopup,
  closePopup,
  getModelByRowIndex,
  getTicketSubTypes
} from '../../../../Shared/chatBoardNew/chatBoardSlice';
import {
  changeErrorRecipient,
  closeCreatePopup,
  setSelectedRecipients,
  setSubTypeSelected,
  openPopupCreateDiscussion
} from '../../../../Shared/createTicketNew/createTicketBoardSliceNew';
import store from '../../../app/store';
import { useStableDispatch } from '../../../ImportPrematchHelperNative';
import ToastsWrapper from '../../../../components/ToastrWrapper';
import { getDetailSegmentUrlByEventType } from '../../../../Shared/TicketCenterPrematch/helper/helper';
import { isEqual } from 'lodash';
import {
  EVENT_TYPE,
  TICKET_TYPE
} from '../../../../Shared/TicketCenterPrematch/const/const.json';
import { backupGetIssueModel } from '../../signalR/signalRSlice';
import {
  updatedIconIssueLeagueRow,
  updatedIconIssueMatchRow
} from '../shiftSlice/shiftSlice';
import { useSelector } from 'react-redux';
import TicketListPopupCommon from './TicketListPopupCommon';
import { openNewChattingTab } from '../../../../Shared/Utils';
import { setLeagueInfo } from '../../directionTicket/directionTicketSlice';
const propTypes = {
  rowIndex: PropTypes.number.isRequired,
  ticketList: PropTypes.array.isRequired,
  onCloseTicketList: PropTypes.func.isRequired,
  dataSource: PropTypes.array
};

const { LEAGUE } = EVENT_TYPE;

const TicketListPopup = props => {
  const { ticketList, onCloseTicketList, dataSource, rowIndex } = props;
  const model = dataSource[rowIndex];
  const { PreMatchLeagueId, InfoSiteMatchId } = model;
  const { HasWrite } = useSelector(state => state.policy).permission;
  const stableDispatch = useStableDispatch();

  const handleOpenMessage = async (event, ticket) => {
    const { policy } = store.getState();
    const { permission } = policy;
    const { TicketId, EventType, IsRead } = ticket;

    const requestDetailModel = {
      ticketId: TicketId,
      actorId: permission.ActorId,
      detailSegmentUrl: getDetailSegmentUrlByEventType(EventType),
      eventType: EventType
    };
    if (event.altKey) {
      openNewChattingTab(permission.ActorId, TicketId, EventType);
      return;
    }

    stableDispatch(closeTicketPopup());
    stableDispatch(closeCreatePopup());
    stableDispatch(changeErrorRecipient(false));
    stableDispatch(getTicketSubTypes(TICKET_TYPE.EVENT_CHECKING));
    if (!IsRead) {
      isEqual(EventType, LEAGUE)
        ? stableDispatch(
            updatedIconIssueLeagueRow({ PreMatchLeagueId, TicketId })
          )
        : stableDispatch(
            updatedIconIssueMatchRow({ InfoSiteMatchId, TicketId })
          );
    }
    stableDispatch(backupGetIssueModel(requestDetailModel));

    const actionResult = await stableDispatch(
      getTicketDetail({
        ...requestDetailModel,
        isQueryMessageInLastTwoDay: true
      })
    );
    if (getTicketDetail.rejected.match(actionResult)) {
      ToastsWrapper.showError(actionResult.payload);
    }
  };
  const handleOpenCreateTicket = () => {
    const { defaultRecipients } = store.getState().createTicketBoard;
    stableDispatch(setSelectedRecipients(defaultRecipients));
    stableDispatch(getModelByRowIndex(model));
    stableDispatch(backupGetIssueModel(model));
    stableDispatch(closePopup());
    stableDispatch(closeCreatePopup());
    stableDispatch(openTicketPopup());
    stableDispatch(setSubTypeSelected(...[{ value: 0, label: '' }]));
    stableDispatch(changeErrorRecipient(false));
  };

  const handleOpenCreateDiscussion = () => {
    const { LeagueName, HomeName, AwayName, AdminSiteMatchId, LeagueId } =
      dataSource[rowIndex];
    const { LeagueGroup } = dataSource.find(x => x.LeagueId === LeagueId);
    stableDispatch(
      setLeagueInfo({
        league: LeagueName,
        event: `${HomeName} -vs- ${AwayName}`,
        matchId: AdminSiteMatchId,
        leagueGroup: LeagueGroup
      })
    );
    stableDispatch(openPopupCreateDiscussion());

    const adminSiteMatchId = dataSource[rowIndex].AdminSiteMatchId;
    if (adminSiteMatchId && adminSiteMatchId !== 0) {
      stableDispatch(openPopupCreateDiscussion());
    } else {
      ToastrWrapper.showError(
        'The system could not find the  AdminSiteId to get the reader for  this match to send to  direction ticket. Please check the match again.'
      );
    }
  };

  return (
    <TicketListPopupCommon
      ticketList={ticketList}
      HasWrite={HasWrite}
      isMatch={dataSource[rowIndex].InfoSiteMatchId}
      handleOpenCreateTicket={handleOpenCreateTicket}
      handleOpenMessage={handleOpenMessage}
      onCloseTicketList={onCloseTicketList}
      handleOpenCreateDiscussion={handleOpenCreateDiscussion}
    />
  );
};

TicketListPopup.propTypes = propTypes;
export default TicketListPopup;
