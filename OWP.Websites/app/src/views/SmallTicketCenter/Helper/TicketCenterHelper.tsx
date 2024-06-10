import { isEqual } from 'lodash';
import { TicketCenterPageURL } from '../const/const.json';
import { getValueHrefByReversedIndex } from '../../Shared/Utils';
import {
  getDetailSegmentUrlByEventType,
  EVENT_TYPE,
  URL
} from '../../Shared/TicketCenterPrematch/helper/helper';
import { TChildrenResponse, TParentsResponse } from '../type';

const getDefaultDateRange = () => {
  const dateTimeGmtPlus8String = new Date().toLocaleString('en-US', {
    timeZone: 'Asia/Manila'
  });
  const dateTimeGmtPlus8 = new Date(dateTimeGmtPlus8String);

  const fromDateDefault = new Date(dateTimeGmtPlus8String);
  fromDateDefault.setDate(dateTimeGmtPlus8.getDate() - 1);

  const minDate = new Date(dateTimeGmtPlus8String);
  minDate.setDate(dateTimeGmtPlus8.getDate() - 30);

  return {
    fromDateDefault,
    toDateDefault: dateTimeGmtPlus8,
    minDate
  };
};

const flowFromPreMatch = (userId, searchKey) => {
  return {
    Keyword: searchKey,
    ActorId: userId,
    MarketKey: MARKET[0].value
  };
};

const flowBasic = userId => {
  return {
    MarketKey: MARKET[0].value,
    ActorId: userId
  };
};

const buildValueTicketsFilter = policy => {
  const { ActorId, Username } = policy;
  const pathName = getValueHrefByReversedIndex(1);
  if (isEqual(pathName, TicketCenterPageURL.MyTicket)) {
    return flowFromPreMatch(ActorId, Username);
  }
  return flowBasic(ActorId);
};

const buildTicketTypeFilter = (
  parents: Array<TParentsResponse>,
  children: Array<TChildrenResponse>
) => {
  const parentOption = parents.map(({ id, name }) => ({
    value: `p-${id}`,
    title: name,
    key: `p-${id}`,
    showTitle: name
  }));

  const getParent = (id: number) => parents.find(x => x.id === id);

  const childrenOption = children.map(
    ({ parentId, subTypeId, subTypeName }) => ({
      value: `${parentId}-${subTypeId}`,
      title: subTypeName,
      key: `${parentId}-${subTypeId}`,
      pId: `p-${parentId}`,
      showTitle: `${getParent(parentId)?.name}-${subTypeName}`
    })
  );

  const emptyOption = parents.map(({ id, name }) => ({
    value: `${id}-`,
    title: `None`,
    key: `p-${id}-None`,
    pId: `p-${id}`,
    showTitle: `${name}-None`
  }));

  return { parentOption, childrenOption, emptyOption };
};

const removeTickets = (entities, ticketId) =>
  entities.filter(item => item.ticketId !== ticketId);

const sortTickets = entities => {
  entities.sort((a, b) => {
    if (a.isPinned !== b.isPinned) {
      return a.isPinned ? -1 : 1;
    } else {
      return new Date(b.lastCommentDate) - new Date(a.lastCommentDate);
    }
  });
};

export {
  buildValueTicketsFilter,
  getDetailSegmentUrlByEventType,
  buildTicketTypeFilter,
  removeTickets,
  sortTickets,
  EVENT_TYPE,
  URL,
  getDefaultDateRange
};
