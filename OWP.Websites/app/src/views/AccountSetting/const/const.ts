import { NET_CORE_FILE_UPLOAD } from '../../Shared/Constants';

const BASE_URL = `api/AccountSetting`;

const createUrl = (base: string, endpoint: string) => `${base}/${endpoint}`;

const urlUploadFile = createUrl(NET_CORE_FILE_UPLOAD, 'upload');

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

const getUrlTicketFile = (imageName: string) => `Tickets/${imageName}`;

export {
  urlUploadFile,
  urlSaveProfileAvatar,
  urlGetProfileAvatar,
  urlGetQuickMessage,
  urlSaveQuickMessage,
  urlDeleteQuickMessage,
  getUrlTicketFile
};
