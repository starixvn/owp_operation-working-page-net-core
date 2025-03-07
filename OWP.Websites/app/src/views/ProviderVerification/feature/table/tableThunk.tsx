import { createAsyncThunk } from '@reduxjs/toolkit';
import { get, post } from '../../../Shared/Service/Service';
import Utils from '../../../Shared/Utils';
import { TOptionResponse } from '../../type/confirmProviderOption';

const getUrl = (methodRoute: string) =>
  `/api/ProviderVerification/${methodRoute}`;

const getEvents = createAsyncThunk(
  'provider-verification/get-event',
  async ({
    eventDate,
    onlyShowIsMismatchEvent,
    confirmProvider
  }: {
    eventDate: Date;
    onlyShowIsMismatchEvent: boolean;
    confirmProvider: number | string | null;
  }) => {
    const url = getUrl('get-provider-verification');
    const response = await get({
      url,
      eventDate: Utils.formatDate(eventDate),
      onlyShowIsMismatchEvent,
      confirmProvider
    });
    return response.data;
  }
);

const getConfirmProvider = createAsyncThunk(
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

const saveConfirmProvider = createAsyncThunk(
  'provider-verification/save-provider-confirm',
  async (data: { owpMatchId: number; agentSelected: string; }) => {
    const url = getUrl('save-provider-confirm');
    const prop = {
      url,
      ...data
    };
    return post(prop);
  }
);

const saveRemark = createAsyncThunk(
  'provider-verification/save-remark',
  async (data: { owpMatchId: number; remark: string | undefined; }) => {
    const url = getUrl('save-remark');
    const prop = {
      url,
      ...data
    };
    return post(prop);
  }
);

const saveRemarkBy = createAsyncThunk(
  'provider-verification/save-remark-by',
  async (data: { owpMatchId: number; remarkBy: string | undefined; }) => {
    const url = getUrl('save-remark-by');
    const prop = {
      url,
      ...data
    };
    return post(prop);
  }
);

export {
  getEvents,
  getConfirmProvider,
  saveConfirmProvider,
  saveRemark,
  saveRemarkBy
};
