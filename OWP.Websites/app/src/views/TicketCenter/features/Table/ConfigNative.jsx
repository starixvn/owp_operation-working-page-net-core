import { calculateCustomTableWidth } from '../../../Shared/common/CustomTable/Helper';
import DynamicCell from './Cell/DynamicCell';
import { sort } from './eventSlice/eventCheckingSlice';
import store from '../../app/storeNative';
import {
  getTicketDetail,
  getTicketSubTypes
} from '../../../Shared/chatBoardNew/chatBoardSlice';
import ToastrWrapper from '../../../components/ToastrWrapper';
import { getDetailSegmentUrlByEventType } from '../../Helper/TicketCenterHelper';
import { updatedIsReadOneRow } from '../../ImportTicketCenterHelperNative';
import {
  closeCreatePopup,
  changeErrorRecipient
} from '../../../Shared/createTicketNew/createTicketBoardSliceNew';
import { onClickIssueModel } from '../signalR/signalRSlice';
import { openNewChattingTab } from '../../../Shared/Utils';

const DefaultTableStyle = {
  rowHeight: 70,
  filterOffset: 190,
  height: 750,
  headerHeight: 38
};

const LeagueColumn = {
  order: 1,
  headerName: 'Conversations',
  columnKey: 'eventName',
  width: 350,
  isCanSort: true,
  isFreeze: true,
  isOpenTab: true
};

const TicketIdColumn = {
  headerName: 'ID',
  columnKey: 'ticketId',
  isCanSort: true,
  width: 60,
  isFreeze: true
};

const LastCommentColumn = {
  order: 2,
  headerName: 'Last Comment',
  columnKey: 'lastCommentDate',
  width: 450,
  isCanSort: true
};

const TicketTypeColumn = {
  headerName: 'Ticket Type - Subtype',
  columnKey: 'ticketType',
  isCanSort: true,
  width: 180
};

const StatusColumn = {
  headerName: 'Status',
  columnKey: 'status',
  isCanSort: false,
  width: 100
};

const ActionColumn = {
  headerName: 'Action',
  columnKey: 'action',
  isCanSort: false,
  width: 80
};

const ColumnModels = {
  LeagueColumn,
  TicketIdColumn,
  LastCommentColumn,
  TicketTypeColumn,
  StatusColumn,
  ActionColumn
};

const bonusWidth = columnSchema => calculateCustomTableWidth(columnSchema, 16);

const getConfigTicketsTab = containerWidth => {
  const ratio = containerWidth <= 1000 ? 1000 / 1614 : containerWidth / 1614;

  return {
    columnSchema: [
      { ...TicketIdColumn, width: TicketIdColumn.width * ratio },
      { ...LeagueColumn, width: LeagueColumn.width * ratio },
      { ...TicketTypeColumn, width: TicketTypeColumn.width * ratio },
      { ...StatusColumn, width: StatusColumn.width * ratio },
      { ...LastCommentColumn, width: LastCommentColumn.width * ratio },
      { ...ActionColumn, width: ActionColumn.width * ratio }
    ],
    tableStyle: DefaultTableStyle,
    dynamicCell: DynamicCell,
    sortFunc: sort,
    tableWidth: bonusWidth,
    onRowClick: async (event, rowIndex) => {
      if (event?.target?.className?.includes('icon-new-tab')) {
        return;
      }
      const { pagePolicy, eventChecking } = store.getState();
      const { dispatch } = store;
      const { permission } = pagePolicy;
      const { ticketId, eventType, ticketType } =
        eventChecking.entities[rowIndex];
      if (event.altKey) {
        openNewChattingTab(permission.ActorId, ticketId, eventType);
        return;
      }
      const requestDetailModel = {
        ticketId,
        actorId: permission.ActorId,
        detailSegmentUrl: getDetailSegmentUrlByEventType(eventType),
        eventType
      };

      dispatch(changeErrorRecipient(false));
      dispatch(closeCreatePopup());
      dispatch(updatedIsReadOneRow(rowIndex));
      dispatch(onClickIssueModel(requestDetailModel));
      dispatch(getTicketSubTypes(ticketType));
      const actionResult = await dispatch(
        getTicketDetail({
          ...requestDetailModel,
          isQueryMessageInLastTwoDay: true
        })
      );
      if (getTicketDetail.rejected.match(actionResult)) {
        ToastrWrapper.showError(actionResult.payload);
      }
    }
  };
};

export { getConfigTicketsTab, ColumnModels, DefaultTableStyle };
