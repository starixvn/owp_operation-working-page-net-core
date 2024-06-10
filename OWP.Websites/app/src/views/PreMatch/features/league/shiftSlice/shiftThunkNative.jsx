import { createAsyncThunk } from '@reduxjs/toolkit';
import { post, get } from '../../../ImportPrematchHelperNative';
import {
  QUERY_DELETE_LEAGUE_RECOVER_API_KEY,
  URL_QUERY_LEAGUE_BY_ID,
  URL_QUERY_ASSIGNED_LEAGUE_ID,
  simpleApiUpdateUrl
} from '../../../const/const';

const restoreDeletedLeague = createAsyncThunk(
  'delete/restoreDeletedLeague',
  async (preMatchLeagueId, { getState }) => {
    const { startTimeOriginal } = getState().header;
    const restore = {
      url: simpleApiUpdateUrl,
      apiKey: QUERY_DELETE_LEAGUE_RECOVER_API_KEY,
      PreMatchLeagueId: preMatchLeagueId
    };
    const queryObject = {
      PreMatchLeagueId: preMatchLeagueId,
      url: URL_QUERY_LEAGUE_BY_ID,
      StartTime: startTimeOriginal || null
    };
    const responseRestore = await post(restore);
    if (responseRestore.status === 200) {
      const responseQueryObject = await post(queryObject);
      return responseQueryObject.data;
    }
    return null;
  }
);

const getShiftByDateAndEmployee = createAsyncThunk(
  'dayShift/getShiftByDateAndEmployee',
  async modelFilter => {
    const response = await post(modelFilter);
    return response.data;
  }
);

const getAssignedLeagueId = createAsyncThunk(
  'dayShift/getAssignedLeagueId',
  async actorId => {
    const resp = await get({
      url: URL_QUERY_ASSIGNED_LEAGUE_ID,
      actorId
    });
    return resp.data;
  }
);

export { restoreDeletedLeague, getShiftByDateAndEmployee, getAssignedLeagueId };
