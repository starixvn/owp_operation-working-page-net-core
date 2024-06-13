import { isNil } from 'lodash';
import { baseApiUrl } from '../../Constants';
const basePath = `${baseApiUrl}api/TicketEvent`;

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
