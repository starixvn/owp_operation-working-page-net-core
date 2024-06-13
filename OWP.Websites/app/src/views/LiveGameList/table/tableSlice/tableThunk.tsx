import { createAsyncThunk } from '@reduxjs/toolkit';
import { get } from '../../../Shared/Service/Service';
import Utils from '../../../Shared/Utils';
import { baseApiUrl } from '../../../Shared/Constants';

const getUrl = (methodRoute: string) =>
  `${baseApiUrl}/api/LiveGameList/${methodRoute}`;

const getEvents = createAsyncThunk(
  'live-game-list/get-event',
  async ({
    userIds,
    filterDate,
    statusId,
    matchType
  }: {
    userIds: string;
    filterDate: Date;
    statusId: string;
    matchType: string;
  }) => {
    const url = getUrl('query');
    const eventDate = Utils.formatDate(filterDate);
    const response = await get({
      url,
      userIds,
      eventDate,
      statusId,
      matchType
    });
    return response.data;
  }
);

export { getEvents };
