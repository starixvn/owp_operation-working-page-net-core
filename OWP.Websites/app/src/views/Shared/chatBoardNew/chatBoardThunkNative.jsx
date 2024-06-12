import { post } from '../Service/Service';
import { createAsyncThunk } from '@reduxjs/toolkit';
import Axios from 'axios';
import { URL, TICKET_TYPE } from './const/index.json';
import { buildURL } from './helper/chatBoardHelper';

const handleMessage = name =>
  createAsyncThunk(name, async model => {
    const response = await post(model);
    return response.data;
  });

const getTicketDetail = createAsyncThunk(
  'message/GetTicketDetail',
  async (model, { rejectWithValue, getState }) => {
    const { detailSegmentUrl, isQueryMessageInLastTwoDay, ...rest } = model;
    const state = getState();

    const { ticketDetail } = state.chatBoard;

    const { messages } = ticketDetail;

    let postModel = rest;

    if (!isQueryMessageInLastTwoDay) {
      postModel = {
        ...rest,
        firstMessageId: messages[0]?.messageId
      };
    }

    const url = `${URL.NET_Core_TICKET_CENTER}/${detailSegmentUrl}`;

    const { data, status } = await post({
      url,
      ...postModel
    });

    return status === 203
      ? rejectWithValue(data)
      : {
          data,
          status
        };
  }
);

const loadMoreMessageThunk = createAsyncThunk(
  'message/loadMore',
  async model => {
    const url = `${URL.NET_Core_TICKET_CENTER}/${URL.LOAD_MORE_MESSAGE}`;
    const response = await post({
      url,
      ...model
    });
    return response.data;
  }
);

const createNewMessage = createAsyncThunk(
  'message/CreateNewMessage',
  async (model, { rejectWithValue }) => {
    const url = `${URL.NET_Core_TICKET_CENTER}/${URL.ADD_MESSAGE}`;

    const { data, status } = await Axios.post(url, model);
    if (status === 203) {
      return rejectWithValue(data);
    }
    return data;
  }
);

const getMessage = createAsyncThunk('message/GetMessage', async model => {
  const { url, PreMatchIssueId, IsShowAllMessage, ...rest } = model;
  const response = await post({
    url,
    PreMatchIssueId,
    IsShowAllMessage
  });
  return {
    ...rest,
    PreMatchIssueId,
    IsShowAllMessage,
    data: response.data
  };
});
const getModelByRowIndex = createAsyncThunk(
  'getModelByRowIndex',
  async model => {
    const { PreMatchIssueId, IsShowAllMessage, ...rest } = model;
    if (!PreMatchIssueId) {
      return {
        ...rest
      };
    }

    return {
      ...rest,
      PreMatchIssueId,
      IsShowAllMessage
    };
  }
);
const updateTicketStatus = createAsyncThunk(
  'saveStatus',
  async (model, { rejectWithValue }) => {
    const url = `${URL.NET_Core_TICKET_CENTER}/${URL.UPDATE_TICKET_STATUS}`;

    const { data, status } = await Axios.post(url, model);
    if (status === 203) {
      return rejectWithValue(data);
    }
    return data;
  }
);

const updateTicketSubType = createAsyncThunk(
  'changeSubType',
  async (request, { rejectWithValue }) => {
    const url = `${URL.NET_Core_TICKET_CENTER}/${URL.UPDATE_TICKET_SUBTYPE}`;

    const { data, status } = await Axios.post(url, request);
    if (status === 203) {
      return rejectWithValue(data);
    }
    return data;
  }
);

const createIssue = handleMessage('createIssue');

const saveMessage = createAsyncThunk(
  'save-message',
  async (model, { rejectWithValue }) => {
    const { status, data } = await post(model);
    if (status === 203) {
      return rejectWithValue(data);
    }
    return { ...model };
  }
);

const saveDoneIssue = handleMessage('saveDoneIssue');

const getTicketSubTypes = createAsyncThunk(
  'getTicketSubTypes',
  async ticketTypeId => {
    const url = `${URL.NET_Core_TICKET_CENTER}/get-subtype`;

    const { data, status } = await Axios.post(url, {
      ticketTypeId
    });
    if (status === 200) {
      const options = data.map(d => ({
        value: d.subTypeId,
        label: d.subTypeName
      }));
      return ticketTypeId !== TICKET_TYPE.LOG_WORK
        ? [...options, { value: 0, label: '' }]
        : [...options];
    }
    return [];
  }
);

const updateReactIconMessage = createAsyncThunk(
  'updateReactIconMessage',
  async model => {
    const url = buildURL('update-message-icon');

    const response = Axios.post(url, model);
    return response.data;
  }
);

export {
  getMessage,
  getModelByRowIndex,
  createIssue,
  saveMessage,
  saveDoneIssue,
  getTicketDetail,
  createNewMessage,
  updateTicketStatus,
  updateTicketSubType,
  getTicketSubTypes,
  loadMoreMessageThunk,
  updateReactIconMessage
};
