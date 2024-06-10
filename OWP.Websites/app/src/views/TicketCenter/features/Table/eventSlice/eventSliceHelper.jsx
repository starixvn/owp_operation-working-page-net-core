const filterIssueByUnreadMessageValue = state => {
  const { entitiesOrigin, isShowUnreadMessOnly } = state;
  if (isShowUnreadMessOnly) {
    return entitiesOrigin.filter(d => !d.isRead);
  }
  return entitiesOrigin;
};

const formatTicketTypeOption = ticketTypes =>
  ticketTypes.map(({ id: value, name: label }) => ({
    value,
    label
  }));

const sortTickets = entities => {
  entities.sort((a, b) => {
    if (a.isPinned !== b.isPinned) {
      return a.isPinned ? -1 : 1;
    } else {
      return new Date(b.lastCommentDate) - new Date(a.lastCommentDate);
    }
  });
};

const removeTickets = (entities, ticketId) =>
  entities.filter(item => item.ticketId !== ticketId);

const updateTicketsStatus = (entities, ticketInfo) => {
  const { ticketId, ticketStatus } = ticketInfo;
  const ticketIndex = entities.findIndex(e => e.ticketId === ticketId);

  if (ticketIndex !== -1) {
    entities[ticketIndex].ticketStatus = ticketStatus;
  } else {
    entities.unshift(ticketInfo);
  }
};

export {
  filterIssueByUnreadMessageValue,
  formatTicketTypeOption,
  sortTickets,
  removeTickets,
  updateTicketsStatus
};
