import PropTypes from 'prop-types';
import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Loading from '../../../components/Loading';
import ToastsWrapper from '../../../components/ToastrWrapper';
import {
  onClickIssueModel,
  setOwpMatchId
} from '../signalR/signalREventTicketSlice';
import {
  getTicketDetail,
  getTicketSubTypes
} from '../../chatBoardNew/chatBoardSlice';
import {
  changeErrorRecipient,
  closeCreatePopup
} from '../../createTicketNew/createTicketBoardSliceNew';
import { TICKET_TYPE } from '../../TicketCenterPrematch/const/const.json';
import {
  EVENT_TYPE,
  getDetailSegmentUrlByEventType
} from '../../TicketCenterPrematch/helper/helper';
import TicketListPopupCommon from '../../ticketList/TicketListPopupCommon';
import { getTicketList, setTicketList } from './ticketListSlice';
import { openNewChattingTab } from '../../Utils';

const propTypes = {
  dataSources: PropTypes.array,
  index: PropTypes.number,
  onCloseTicketList: PropTypes.func,
  updateDataSources: PropTypes.func,
  handleOpenCreateTicket: PropTypes.func
};

const TicketList = props => {
  const {
    dataSources,
    index,
    onCloseTicketList,
    updateDataSources,
    handleOpenCreateTicket
  } = props;
  const dataSource = dataSources[index];
  const { ticketList } = useSelector(state => state.ticketList);
  const { owpMatchId } = useSelector(state => state.signalREventTicket);
  const { permission } = useSelector(state => state.pagePolicy);
  const stableDispatch = useDispatch();
  const handleOpenMessage = async (event, ticket) => {
    const { TicketId } = ticket;
    const requestDetailModel = {
      ticketId: TicketId,
      actorId: permission.ActorId,
      detailSegmentUrl: getDetailSegmentUrlByEventType(
        EVENT_TYPE.LIVE_INCIDENT
      ),
      eventType: EVENT_TYPE.LIVE_INCIDENT
    };
    if (event.altKey) {
      openNewChattingTab(
        permission.ActorId,
        TicketId,
        EVENT_TYPE.LIVE_INCIDENT
      );
      return;
    }
    stableDispatch(changeErrorRecipient(false));
    stableDispatch(closeCreatePopup());
    stableDispatch(onClickIssueModel(requestDetailModel));
    stableDispatch(getTicketSubTypes(TICKET_TYPE.LIVE_INCIDENT));
    const actionResult = await stableDispatch(
      getTicketDetail({
        ...requestDetailModel,
        isQueryMessageInLastTwoDay: true
      })
    );
    const newTicketList = ticketList.map(item => {
      if (item.TicketId === TicketId) {
        return {
          ...item,
          IsRead: true
        };
      }
      return item;
    });
    stableDispatch(setTicketList(newTicketList));
    const isRead = newTicketList.every(x => x.IsRead);
    const { ParentMatchId } = dataSources.find(
      d => d.OWPMatchId === owpMatchId
    );

    const newDataSource = dataSources.map(item => {
      if (item.ParentMatchId === ParentMatchId) {
        return {
          ...item,
          IsRead: isRead
        };
      }
      return item;
    });
    updateDataSources(newDataSource);
    if (getTicketDetail.rejected.match(actionResult)) {
      ToastsWrapper.showError(actionResult.payload);
    }
  };

  useEffect(() => {
    const model = {
      OWPMatchId: dataSource.OWPMatchId,
      ActorId: permission.ActorId
    };
    stableDispatch(getTicketList(model));
    stableDispatch(setOwpMatchId(dataSource.OWPMatchId));
  }, [dataSource.OWPMatchId, permission.ActorId, stableDispatch]);

  return (
    <TicketListPopupCommon
      ticketList={ticketList}
      HasWrite={permission.HasWrite}
      handleOpenCreateTicket={handleOpenCreateTicket}
      handleOpenMessage={handleOpenMessage}
      onCloseTicketList={onCloseTicketList}
    />
  );
};

TicketList.propTypes = propTypes;

export default TicketList;
