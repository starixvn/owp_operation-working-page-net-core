import {
  NET_CORE_FILE_UPLOAD,
  NET_FW_FILE_UPLOAD
} from '../../Shared/Constants';

const ENV = process.env.NODE_ENV === 'development';
const BASE_URL = ENV
  ? 'api-netcore/AccountSetting'
  : `${window.netCoreRootUrl}/api/AccountSetting`;

const createUrl = (base: string, endpoint: string) => `${base}/${endpoint}`;

const urlUploadFile = createUrl(
  ENV ? NET_CORE_FILE_UPLOAD : `${window.netCoreRootUrl}/${NET_FW_FILE_UPLOAD}`,
  'upload'
);

const urlGetProfileAvatar = createUrl(BASE_URL, 'Profile/get-user-profile');
const urlSaveProfileAvatar = createUrl(BASE_URL, 'Profile/save-user-profile');
const urlGetQuickMessage = createUrl(
  BASE_URL,
  'QuickMessage/get-quick-message'
);
const urlSaveQuickMessage = createUrl(
  BASE_URL,
  'QuickMessage/save-quick-message'
);
const urlDeleteQuickMessage = createUrl(
  BASE_URL,
  'QuickMessage/delete-quick-message'
);

const getUrlTicketFile = (imageName: string) =>
  process.env.NODE_ENV === 'development'
    ? `Tickets/${imageName}`
    : `${window.netCoreRootUrl}/Tickets/${imageName}`;

export {
  urlUploadFile,
  urlSaveProfileAvatar,
  urlGetProfileAvatar,
  urlGetQuickMessage,
  urlSaveQuickMessage,
  urlDeleteQuickMessage,
  getUrlTicketFile
};
