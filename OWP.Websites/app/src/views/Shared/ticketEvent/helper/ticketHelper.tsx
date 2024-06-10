import { isNil } from 'lodash';
const basePath =
  process.env.NODE_ENV === 'development'
    ? `api-netcore/TicketEvent`
    : `${window.netCoreRootUrl}/api/TicketEvent`;

const buildIsReadValue = (
  actorId: number,
  userId: number,
  isRead: boolean | null
) => {
  if (actorId !== userId) {
    return false;
  }
  return isNil(isRead) ? true : isRead;
};

export { basePath, buildIsReadValue };
