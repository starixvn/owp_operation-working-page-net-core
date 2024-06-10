import dayjs from 'dayjs';
import { isUndefined } from 'lodash';
const checkAllFilterCondition = (filterModel, ticketInfo) => {
  return (
    checkFilterDate(filterModel, ticketInfo) &&
    checkFilterEvent(filterModel, ticketInfo) &&
    checkFilterTicketSubTypeId(filterModel, ticketInfo) &&
    checkFilterStatus(filterModel, ticketInfo)
  );
};

const checkFilterDate = (filterModel, ticketInfo) => {
  const lastCommentDate = dayjs(ticketInfo.lastCommentDate);
  const fromDateFilter = dayjs(filterModel.fromDateFilter);
  let toDateFilter = dayjs(filterModel.toDateFilter);
  toDateFilter = toDateFilter.endOf('day');
  return lastCommentDate >= fromDateFilter && lastCommentDate <= toDateFilter;
};

const checkFilterEvent = (filterModel, ticketInfo) => {
  const { keyword } = filterModel;
  if (isUndefined(keyword)) {
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
    ticketInfo[field]?.toLowerCase().includes(keyword.toLowerCase())
  );
};

const checkFilterTicketSubTypeId = (filterModel, ticketInfo) => {
  const listSubTypeIdFilterModel = filterModel.ticketTypeId.split(',');
  const ticketSubTypeId = `${ticketInfo.ticketType}-${
    ticketInfo.ticketSubTypeId || ''
  }`;
  return listSubTypeIdFilterModel.some(x => x === ticketSubTypeId);
};

const checkFilterStatus = (filterModel, ticketInfo) => {
  return filterModel.ticketStatuses.includes(ticketInfo.ticketStatus);
};

export { checkAllFilterCondition };
