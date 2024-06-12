import { createAsyncThunk } from '@reduxjs/toolkit';
import { get, post } from '../../../Shared/Service/Service';
import Utils from '../../../Shared/Utils';
import { netCoreURL } from './TableSchema.json';
import { TOptionResponse } from '../../type/agentConfirmOption';

const getUrl = (methodRoute: string) => `${netCoreURL}/${methodRoute}`;

const getEvents = createAsyncThunk(
  'provider-verification/get-event',
  async (eventDate: Date) => {
    const url = getUrl('get-provider-verification');
    const response = await get({
      url,
      eventDate: Utils.formatDate(eventDate)
    });
    return response.data;
  }
);

const getAgentConfirm = createAsyncThunk(
  'provider-verification/get-agent-confirm',
  async () => {
    const url = getUrl('get-agent-option');
    const response = await get({
      url
    });
    return (response.data as TOptionResponse[]).map(data => ({
      value: data.agentId,
      label: data.agentName
    }));
  }
);

const saveAgentConfirm = createAsyncThunk(
  'provider-verification/save-agent-confirm',
  async (data: { owpMatchId: number; agentSelected: string }) => {
    const url = getUrl('save-agent-confirm');
    const prop = {
      url,
      ...data
    };
    return await post(prop);
  }
);

const saveESTNote = createAsyncThunk(
  'provider-verification/save-est-note',
  async (data: { owpMatchId: number; estNote: string | undefined }) => {
    const url = getUrl('save-est-note');
    const prop = {
      url,
      ...data
    };
    return await post(prop);
  }
);

const saveESTAgent = createAsyncThunk(
  'provider-verification/save-est-agent',
  async (data: { owpMatchId: number; estAgent: string | undefined }) => {
    const url = getUrl('create-est-agent');
    const prop = {
      url,
      ...data
    };
    return await post(prop);
  }
);

export {
  getEvents,
  getAgentConfirm,
  saveAgentConfirm,
  saveESTNote,
  saveESTAgent
};
