import { isNil } from 'lodash';
const basePath = `/api/TicketEvent`;

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
