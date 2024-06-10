import axios from 'axios';
export const SystemMessageTypes = {
  Notification: 1,
  Announcement: 2,
  Message: 3
};

export const MessageTypes = {
  AssignmentInsert: 1,
  AssignmentDelete: 2,
  MatchNoteInsert: 3,
  MatchNoteUpdate: 4,
  MatchNoteDelete: 5,
  MatchStatus: 6,
  MinorMatchStatus: 7,
  OtherAssignmentInsert: 8,
  OtherAssignmentDelete: 9,
  OtherAssignmentUpdate: 10,
  PreMatchIssue: 42,
  EventCheckingResponse: 43,
  NewAddRequest: 44,
  NewResponse: 45,
  Attachment: 46
};

export const LiveDetailTypes = {
  AssignmentInsert: 1,
  AssignmentDelete: 2,
  MatchStatus: 6,
  OtherAssignmentInsert: 8,
  OtherAssignmentDelete: 9
};

export const TicketCenterMessageTypes = {
  NewTicket: 220,
  AddRecipient: 221,
  NewMessage: 222,
  NewAttachment: 224
};

export const OSMessageTypes = {
  OSAssignmentInsert: 101,
  OSOtherAssignmentInsert: 102,
  OSAssignmentDelete: 103,
  OSOtherAssignmentDelete: 104,
  OSAssignmentUpdate: 105,
  OSOtherAssignmentUpdate: 106,
  OSEditMatchStatus: 108
};

export const PreMatchMessageTypes = {
  DayShiftAssignmentInsert: 201,
  DayShiftAssignmentDelete: 202,
  DayShiftOtherAssignmentUpdate: 203,
  NightShiftAssignmentInsert: 204,
  NightShiftAssignmentDelete: 205,
  NightShiftOtherAssignmentUpdate: 206,
  NewLeague: 207,
  LGAAddLiveMatch: 208,
  LGACancelLiveMatch: 209,
  LGAChangeMatchStatus: 210,
  UpdateHome: 211,
  UpdateAway: 212,
  UpdateKOTime: 213
};

export const OpenPreMatchIssueURL =
  'api/Prematch-Assignment/on-click-issue-notification';

export const openTicketCenterIssueURL =
  process.env.NODE_ENV === 'development'
    ? 'api-netcore/TicketCenter/on-click-issue-notification'
    : `api/TicketCenter/on-click-issue-notification`;

const buildMessageUrl = () =>
  process.env.NODE_ENV === 'development'
    ? 'api-netcore/application/message'
    : 'api/application/message';

const query = async (model = null) => {
  const response = await axios.get(buildMessageUrl(), {
    params: model
  });

  return response.data;
};

const updateIsViewed = async model => {
  await axios.put(buildMessageUrl(), model);
};

const removedSpan = '<span class="match-unassigned">REMOVED</span>';
const canceledSpan = '<span class="match-cancel">CANCELED</span>';
const postponedSpan = '<span class="match-postponed">POSTPONED</span>';

const customizeTicketCenterContent = (messageType, messageContent) => {
  switch (messageType) {
    case TicketCenterMessageTypes.NewTicket:
      return messageContent.replace(
        'New ticket',
        '<span class="new-league">New ticket</span>'
      );
    case TicketCenterMessageTypes.NewMessage:
      return messageContent.replace(
        'New message',
        '<span class="new-message-title">New message</span>'
      );
    case TicketCenterMessageTypes.NewAttachment:
      return messageContent.replace(
        'New attachment',
        '<span class="new-attachment-title">New attachment</span>'
      );
    default:
      return messageContent;
  }
};

const customizeContent = message => {
  const { MessageType, MessageContent } = message;
  switch (MessageType) {
    case MessageTypes.AssignmentInsert:
    case MessageTypes.OtherAssignmentInsert:
      return MessageContent.replace(
        'new ASSIGNED',
        '<span class="match-assigned">NEW ASSIGNED</span>'
      );
    case MessageTypes.AssignmentDelete:
    case MessageTypes.OtherAssignmentDelete:
      return MessageContent.replace('REMOVED', removedSpan);
    case MessageTypes.MatchStatus:
    case MessageTypes.MinorMatchStatus:
      return MessageContent.replace('Cancel', canceledSpan)
        .replace('Postponed', postponedSpan)
        .replace('Suspended', '<span class="match-suspended">SUSPENDED</span>');
    case MessageTypes.OtherAssignmentUpdate:
      return MessageContent.replace(
        'EDITED',
        '<span class="match-assigned">EDITED</span>'
      );
    default:
      return MessageContent;
  }
};
const customizeLiveDetailContent = (messageType, messageContent) => {
  switch (messageType) {
    case LiveDetailTypes.AssignmentInsert:
    case LiveDetailTypes.OtherAssignmentInsert:
      return messageContent.replace(
        'new ASSIGNED',
        '<span class="match-assigned">NEW ASSIGNED</span>'
      );
    case LiveDetailTypes.AssignmentDelete:
    case LiveDetailTypes.OtherAssignmentDelete:
      return messageContent.replace('REMOVED', removedSpan);
    case LiveDetailTypes.MatchStatus:
      return messageContent
        .replace('Cancel', '<span class="match-cancel">CANCEL</span>')
        .replace('Postponed', '<span class="match-postponed">POSTPONED</span>')
        .replace('Suspended', '<span class="match-suspended">SUSPENDED</span>');
    default:
      return messageContent;
  }
};

const customizeOSContent = (messageType, messageContent) => {
  switch (messageType) {
    case OSMessageTypes.OSAssignmentInsert:
    case OSMessageTypes.OSOtherAssignmentInsert:
      return messageContent.replace(
        'NEW ASSIGNMENT',
        '<span class="match-assigned">NEW ASSIGNMENT</span>'
      );
    case OSMessageTypes.OSAssignmentDelete:
    case OSMessageTypes.OSOtherAssignmentDelete:
      return messageContent.replace('REMOVED', removedSpan);
    case OSMessageTypes.OSEditMatchStatus:
      return messageContent
        .replace('Canceled', canceledSpan)
        .replace('Postponed', postponedSpan)
        .replace('Live', '<span class="match-live">LIVE</span>');
    case OSMessageTypes.OSAssignmentUpdate:
    case OSMessageTypes.OSOtherAssignmentUpdate:
      return messageContent.replace(
        'UPDATED ASSIGNEE',
        '<span class="match-assigned">UPDATED ASSIGNEE</span>'
      );
    default:
      return messageContent;
  }
};

const customizePreMatchContent = (messageType, messageContent) => {
  switch (messageType) {
    case PreMatchMessageTypes.DayShiftAssignmentInsert:
    case PreMatchMessageTypes.NightShiftAssignmentInsert:
      return messageContent.replace(
        'NEW ASSIGNMENT',
        '<span class="match-assigned">NEW ASSIGNMENT</span>'
      );
    case PreMatchMessageTypes.DayShiftAssignmentDelete:
    case PreMatchMessageTypes.NightShiftAssignmentDelete:
      return messageContent.replace('REMOVED', removedSpan);
    case PreMatchMessageTypes.NewLeague:
      return messageContent.replace(
        'New Add',
        '<span class="new-league">New Add</span>'
      );
    case PreMatchMessageTypes.LGAChangeMatchStatus:
      return messageContent
        .replace('Cancel', canceledSpan)
        .replace('Postponed', postponedSpan)
        .replace('Suspended', '<span class="match-suspended">SUSPENDED</span>')
        .replace('Live', '<span class="match-live">LIVE</span>')
        .replace(
          'MATCH STATUS',
          '<span class="match-status-change">MATCH STATUS</span>'
        );
    case PreMatchMessageTypes.LGAAddLiveMatch:
      return messageContent.replace(
        'ADD LIVE',
        '<span class="add-live">ADD LIVE</span>'
      );
    case PreMatchMessageTypes.LGACancelLiveMatch:
      return messageContent.replace(
        'CANCEL LIVE',
        '<span class="cancel-live">CANCEL LIVE</span>'
      );
    default:
      return messageContent;
  }
};

const service = {
  query,
  updateIsViewed,
  customizeContent,
  customizeOSContent,
  customizePreMatchContent,
  customizeTicketCenterContent,
  customizeLiveDetailContent
};
export default service;
