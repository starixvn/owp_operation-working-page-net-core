const netCoreURL = 'api-netcore/TicketEvent';
const netFwURL = 'api/TicketEvent';
const buildTicketEventURL = keyword =>
  process.env.NODE_ENV === 'development'
    ? `${netCoreURL}/${keyword}`
    : `${netFwURL}/${keyword}`;

export { buildTicketEventURL };
