import { createAsyncThunk } from '@reduxjs/toolkit';
import { post } from '../../../Shared/Service/Service';
import Utils from '../../../Shared/Utils';
import {
  TQueryTaskList,
  TResponseReadyStatus,
  TResponseTable,
  TUpdateReadyStatus,
  TResponseDiscussion,
  TQueryDiscussion
} from '../../type/TTaskListTable';
import {
  TASK_LIST_URL,
  UPDATE_TRADE_STATUS,
  CREATE_RC_DISCUSSION
} from '../const/url.json';
import { buildRequestURL } from '../utils/utils';

const taskListThunk = createAsyncThunk<TResponseTable, TQueryTaskList>(
  'taskListThunk/queryDataTable',
  async (props: TQueryTaskList) => {
    const { eventDateTime } = props;
    const model = {
      url: buildRequestURL(TASK_LIST_URL),
      eventDateTime: Utils.formatDate(eventDateTime)
    };
    const response = await post(model);
    return response.data as TResponseTable;
  }
);

const createDiscussionThunk = createAsyncThunk<
  TResponseDiscussion,
  TQueryDiscussion
>('taskListThunk/createDiscussion', async (props: TQueryDiscussion) => {
  const { t_name, reply, users, owpMatchId } = props;
  const model = {
    url: buildRequestURL(CREATE_RC_DISCUSSION),
    t_name,
    reply,
    users,
    owpMatchId
  };
  const response = await post(model);
  return response.data;
});

const updateReadyStatusThunk = createAsyncThunk<
  TResponseReadyStatus,
  TUpdateReadyStatus
>('taskListThunk/updateReady', async props => {
  const { rowIndex, owpMatchId } = props;
  const model = {
    url: buildRequestURL(UPDATE_TRADE_STATUS),
    owpMatchId
  };
  const response = await post(model);
  return {
    readyTime: response.data,
    rowIndex
  };
});
export { taskListThunk, updateReadyStatusThunk, createDiscussionThunk };
