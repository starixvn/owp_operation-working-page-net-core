import { throttle, isUndefined } from 'lodash';
import { refreshEventTickets } from '../../ImportTicketCenterHelperNative';
import {
  getTicketDetail,
  updateSubTypeTicketDetailByTicketId
} from '../../../Shared/chatBoardNew/chatBoardSlice';
import store from '../../app/storeNative';
import dayjs from 'dayjs';
import {
  appendTicketItemIntoDataSource,
  removeTicketItemFromDataSource,
  updateSubTypeByTicketId
} from '../Table/eventSlice/eventCheckingSlice';
const reloadTicketCenterTable = model => {
  const { filterModel, issueModel } = model.signalR;
  const { isOpenPopup } = model.chatBoard;

  const submitThrottle = throttle(
    backupFilter => store.dispatch(refreshEventTickets(backupFilter)),
    500
  );
  isOpenPopup && store.dispatch(getTicketDetail(issueModel));
  submitThrottle(filterModel);
};

const handleCorrectFilter = (elementByTicketId, ticketInfo, isOpenPopup) => {
  if (elementByTicketId) {
    updateTicketInfo(ticketInfo, isOpenPopup);
  } else {
    store.dispatch(appendTicketItemIntoDataSource(ticketInfo));
  }
};

const handleInCorrectFilter = ticketInfo => {
  store.dispatch(removeTicketItemFromDataSource(ticketInfo));
};

const updateTicketInfo = (ticketInfo, isOpenPopup) => {
  store.dispatch(updateSubTypeByTicketId(ticketInfo));
  isOpenPopup &&
    store.dispatch(updateSubTypeTicketDetailByTicketId(ticketInfo));
};

const checkAllFilterCondition = (filterModel, ticketInfo) => {
  const conditions = [
    checkFilterDate(filterModel, ticketInfo),
    checkFilterEvent(filterModel, ticketInfo),
    checkFilterTicketId(filterModel, ticketInfo),
    checkFilterTicketSubTypeId(filterModel, ticketInfo),
    checkFilterStatus(filterModel, ticketInfo)
  ];

  return conditions.every(condition => condition);
};

const checkFilterDate = (filterModel, ticketInfo) => {
  const lastCommentDate = dayjs(ticketInfo.lastCommentDate);
  const fromDateFilter = dayjs(filterModel.FromDateFilter);
  let toDateFilter = dayjs(filterModel.ToDateFilter);
  toDateFilter = toDateFilter.endOf('day');
  return lastCommentDate >= fromDateFilter && lastCommentDate <= toDateFilter;
};

const checkFilterEvent = (filterModel, ticketInfo) => {
  const { Keyword } = filterModel;

  if (isUndefined(Keyword)) {
    return true;
  }

  const fieldsToCheck = [
    'awayName',
    'homeName',
    'eventName',
    'subject',
    'leagueName',
    'owpLeagueName'
  ];

  return fieldsToCheck.some(field =>
    ticketInfo[field]?.toLowerCase().includes(Keyword.toLowerCase())
  );
};

const checkFilterTicketId = (filterModel, ticketInfo) => {
  return isUndefined(filterModel.TicketId)
    ? true
    : ticketInfo.ticketId === filterModel.TicketId;
};

const checkFilterTicketSubTypeId = (filterModel, ticketInfo) => {
  const listSubTypeIdFilterModel = filterModel.TicketTypeId.split(',');
  const ticketSubTypeId = `${ticketInfo.ticketType}-${
    ticketInfo.ticketSubTypeId || ''
  }`;
  return listSubTypeIdFilterModel.some(x => x === ticketSubTypeId);
};

const checkFilterStatus = (filterModel, ticketInfo) =>
  filterModel.TicketStatuses.includes(ticketInfo.ticketStatus);

export {
  reloadTicketCenterTable,
  checkAllFilterCondition,
  handleCorrectFilter,
  handleInCorrectFilter
};
