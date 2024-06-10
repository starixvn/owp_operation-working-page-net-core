import {
  validExtension,
  imageExtension,
  EVENT_TYPE,
  URL
} from '../const/index.json';
import Axios from 'axios';

const buildMessageListClassName = (height, isExpandInfo) => {
  const className = 'message-list';
  switch (true) {
    case height <= 350:
      return `${className}-100`;
    case height <= 540:
      return `${className}-150`;
    case height <= 590:
      return `${className}-200`;
    case height <= 640:
      return `${className}-220`;
    case height <= 690:
      return `${className}-240`;
    case height <= 750:
      return `${className}-320`;
    case height <= 970:
      if (isExpandInfo) {
        return `${className}-550`;
      }
      return `${className}-700`;
    default:
      if (isExpandInfo) {
        return `${className}-750`;
      }
      return `${className}-900`;
  }
};

const getMessageList = async (ticketId, messageId, messages) => {
  const urlGetLastMessage =
    process.env.NODE_ENV === 'development'
      ? `${URL.NET_Core_TICKET_CENTER}/${URL.LOAD_MORE_MESSAGE}`
      : `${URL.NET_FW_TICKET_CENTER}/${URL.LOAD_MORE_MESSAGE}`;
  const modelLoadMoreMessage = {
    msgOfPage: 20,
    ticketId,
    firstMsgId: messageId
  };
  const response = await Axios.post(urlGetLastMessage, modelLoadMoreMessage);
  const messageList = response.data.messages.concat(messages);
  return messageList.sort(
    (a, b) => new Date(b.createdDate) - new Date(a.createdDate)
  );
};

const getExtensionFile = fileName =>
  fileName.substring(fileName.lastIndexOf('.')).toLowerCase().trim();

const isImage = fileName => isContain(imageExtension, fileName);

const validateFileExtension = fileName => isContain(validExtension, fileName);

const isContain = (validExtensionList, fileName) => {
  const fileType = getExtensionFile(fileName);
  return validExtensionList.indexOf(fileType) >= 0;
};

const validateFileSize = size => size / (1024 * 1024) <= 5;

const getTicketTitle = ticket => {
  if (ticket.eventType === EVENT_TYPE.OTHER) {
    return ticket.subject;
  }
  if (ticket.eventType === EVENT_TYPE.MATCH) {
    return `${ticket.owpLeagueName} - ${ticket.homeName}/${ticket.awayName}`;
  }
  if (ticket.eventType === EVENT_TYPE.LOG_WORK) {
    return ticket.eventName
      ? `[${ticket.adminSiteId}] ${ticket.eventName} - ${ticket.homeName}/${ticket.awayName}`
      : ticket.subject;
  }
  return ticket.owpLeagueName;
};
const getUrlTicketFile = imageName =>
  process.env.NODE_ENV === 'development'
    ? `${URL.NET_CORE_TICKET_CENTER_FILE}/${imageName}`
    : `${URL.NET_CORE_TICKET_CENTER_FILE}/${imageName}`;

const buildURL = urlName =>
  process.env.NODE_ENV === 'development'
    ? `${URL.NET_Core_TICKET_CENTER}/${urlName}`
    : `${URL.NET_FW_TICKET_CENTER}/${urlName}`;

export {
  getMessageList,
  buildMessageListClassName,
  getUrlTicketFile,
  validateFileExtension,
  validateFileSize,
  getTicketTitle,
  isImage,
  buildURL
};
