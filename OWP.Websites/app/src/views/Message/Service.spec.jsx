import axios from 'axios';
import MessageService, {
  MessageTypes,
  TicketCenterMessageTypes,
  OSMessageTypes,
  PreMatchMessageTypes,
  LiveDetailTypes
} from './Service';


jest.mock('axios');

describe('Message service test', () => {
  beforeEach(() => {
    axios.get.mockReset();
    axios.put.mockReset();
  });

  it('return data when query is called', async () => {
    const date = new Date();
    date.setHours(date.getHours() - 2);
    const dateString = date.toString();
    const responseData = {
      data: {
        Messages: [{ CreatedDate: dateString }]
      }
    };
    axios.get.mockResolvedValue(responseData);

    expect.assertions(3);
    const model = {};
    const actualData = await MessageService.query(model);
    const expectedData = {
      Messages: [{ CreatedDate: dateString }]
    };

    expect(actualData).toEqual(expectedData);
    expect(axios.get).toBeCalledTimes(1);
    expect(axios.get).toHaveBeenCalledWith('/api/application/message', {
      params: model
    });
  });


  it('updateIsViewed', async () => {
    const model = {
      isWatcher: false
    };
    await MessageService.updateIsViewed(model);
    expect(axios.put).toHaveBeenCalledWith('/api/application/message', model);
  });

  it('customizeLiveDetailContent should handle correctly', () => {
    let messageTypes = [
      LiveDetailTypes.AssignmentInsert,
      LiveDetailTypes.OtherAssignmentInsert
    ];
    messageTypes.forEach(messageType => {
      const messageContent = 'You have been new ASSIGNED to event';

      const customizedMessage = MessageService.customizeLiveDetailContent(
        messageType,
        messageContent
      );
      const expectedMessageContent =
        'You have been <span class="match-assigned">NEW ASSIGNED</span> to event';
      expect(customizedMessage).toEqual(expectedMessageContent);
    });

    messageTypes = [
      LiveDetailTypes.AssignmentDelete,
      LiveDetailTypes.OtherAssignmentDelete
    ];
    messageTypes.forEach(messageType => {
      const messageContent = 'You have been REMOVED to event';
      const customizedMessage = MessageService.customizeLiveDetailContent(
        messageType,
        messageContent
      );
      const expectedMessageContent =
        'You have been <span class="match-unassigned">REMOVED</span> to event';
      expect(customizedMessage).toEqual(expectedMessageContent);
    });
  });

  it('customizeContent should handle correctly', () => {
    let messageTypes = [MessageTypes.MinorMatchStatus];
    messageTypes.forEach(messageType => {
      const message = {
        MessageType: messageType,
        MessageContent:
          'The match status changed to Cancel or Postponed or Suspended'
      };
      const customizedMessage = MessageService.customizeContent(message);
      const expectedMessageContent =
        'The match status changed to ' +
        '<span class="match-cancel">CANCELED</span> or <span class="match-postponed">POSTPONED</span> or <span class="match-suspended">SUSPENDED</span>';
      expect(customizedMessage).toEqual(expectedMessageContent);
    });

    messageTypes = [MessageTypes.OtherAssignmentUpdate];
    messageTypes.forEach(messageType => {
      const message = {
        MessageType: messageType,
        MessageContent: 'You have been EDITED to event'
      };
      const customizedMessage = MessageService.customizeContent(message);
      const expectedMessageContent =
        'You have been <span class="match-assigned">EDITED</span> to event';
      expect(customizedMessage).toEqual(expectedMessageContent);
    });
  });

  it('should return correct default message customizeContent ', () => {
    const messageContent = ' LIVE';

    const actual = MessageService.customizeContent({
      MessageType: '',
      MessageContent: messageContent
    });

    expect(actual).toEqual(' LIVE');
  });

  it('customize ticket center message for create ticket', () => {
    const messageTypes = TicketCenterMessageTypes.NewTicket;
    const MessageContent = 'New ticket <b>#123</b> Italy - Team A -vs- Team B';
    const customizedMessage = MessageService.customizeTicketCenterContent(
      messageTypes,
      MessageContent
    );
    const expectedMessageContent =
      '<span class="new-league">New ticket</span> <b>#123</b> Italy - Team A -vs- Team B';
    expect(customizedMessage).toEqual(expectedMessageContent);
  });
  it('customize ticket center message for add message', () => {
    const messageTypes = TicketCenterMessageTypes.NewMessage;
    const MessageContent =
      'New message from ticket <b>#432</b> JAPAN J-LEAGUE DIVISION 1 - Team A/Team B: "lmao" by starix3_nickname';
    const customizedMessage = MessageService.customizeTicketCenterContent(
      messageTypes,
      MessageContent
    );
    const expectedMessageContent =
      '<span class="new-message-title">New message</span> from ticket <b>#432</b> JAPAN J-LEAGUE DIVISION 1 - Team A/Team B: "lmao" by starix3_nickname';
    expect(customizedMessage).toEqual(expectedMessageContent);
  });
  it('customize ticket center message for add attachment', () => {
    const messageTypes = TicketCenterMessageTypes.NewAttachment;
    const MessageContent =
      'New attachment from ticket <b>#432</b> JAPAN J-LEAGUE DIVISION 1 - Team A/Team B by starix3_nickname';
    const customizedMessage = MessageService.customizeTicketCenterContent(
      messageTypes,
      MessageContent
    );
    const expectedMessageContent =
      '<span class="new-attachment-title">New attachment</span> from ticket <b>#432</b> JAPAN J-LEAGUE DIVISION 1 - Team A/Team B by starix3_nickname';
    expect(customizedMessage).toEqual(expectedMessageContent);
  });
  it('customize ticket center message default', () => {
    const messageTypes = TicketCenterMessageTypes.AddRecipient;
    const MessageContent =
      'You have been added to ticket <b>#432</b> ARAB WOMEN CUP (IN EGYPT) by Jacky';
    const customizedMessage = MessageService.customizeTicketCenterContent(
      messageTypes,
      MessageContent
    );
    const expectedMessageContent =
      'You have been added to ticket <b>#432</b> ARAB WOMEN CUP (IN EGYPT) by Jacky';
    expect(customizedMessage).toEqual(expectedMessageContent);
  });
});

describe('customizeOSContent test', () => {
  it('should return correct message when messageType is OSAssignmentInsert', () => {
    const messageContent = 'You have been NEW ASSIGNMENT to event';

    const actual = MessageService.customizeOSContent(
      OSMessageTypes.OSAssignmentInsert,
      messageContent
    );

    expect(actual).toEqual(
      'You have been <span class="match-assigned">NEW ASSIGNMENT</span> to event'
    );
  });

  it('should return correct message when messageType is OSAssignmentDelete', () => {
    const messageContent = 'You have been REMOVED';

    const actual = MessageService.customizeOSContent(
      OSMessageTypes.OSAssignmentDelete,
      messageContent
    );

    expect(actual).toEqual(
      'You have been <span class="match-unassigned">REMOVED</span>'
    );
  });

  it('should return correct message when messageType is OSEditMatchStatus', () => {
    const messageContent = 'Live';

    const actual = MessageService.customizeOSContent(
      OSMessageTypes.OSEditMatchStatus,
      messageContent
    );

    expect(actual).toEqual('<span class="match-live">LIVE</span>');
  });

  it('should return correct message when messageType is OSAssignmentUpdate', () => {
    const messageContent = 'UPDATED ASSIGNEE';

    const actual = MessageService.customizeOSContent(
      OSMessageTypes.OSAssignmentUpdate,
      messageContent
    );

    expect(actual).toEqual(
      '<span class="match-assigned">UPDATED ASSIGNEE</span>'
    );
  });

  it('should return default message', () => {
    const messageContent = 'UPDATED ASSIGNEE';

    const actual = MessageService.customizeOSContent('', messageContent);

    expect(actual).toEqual('UPDATED ASSIGNEE');
  });
});

describe('customizePreMatchContent test', () => {
  it('should return correct message when messageType is DayShiftAssignmentInsert', () => {
    const messageContent = 'NEW ASSIGNMENT';

    const actual = MessageService.customizePreMatchContent(
      PreMatchMessageTypes.DayShiftAssignmentInsert,
      messageContent
    );

    expect(actual).toEqual(
      '<span class="match-assigned">NEW ASSIGNMENT</span>'
    );
  });

  it('should return correct message when messageType is DayShiftAssignmentDelete', () => {
    const messageContent = 'REMOVED';

    const actual = MessageService.customizePreMatchContent(
      PreMatchMessageTypes.DayShiftAssignmentDelete,
      messageContent
    );

    expect(actual).toEqual('<span class="match-unassigned">REMOVED</span>');
  });

  it('should return correct message when messageType is NewLeague', () => {
    const messageContent = 'New Add';

    const actual = MessageService.customizePreMatchContent(
      PreMatchMessageTypes.NewLeague,
      messageContent
    );

    expect(actual).toEqual('<span class="new-league">New Add</span>');
  });

  it('should return correct message when messageType is LGAChangeMatchStatus', () => {
    const messageContent = 'Cancel';

    const actual = MessageService.customizePreMatchContent(
      PreMatchMessageTypes.LGAChangeMatchStatus,
      messageContent
    );

    expect(actual).toEqual('<span class="match-cancel">CANCELED</span>');
  });

  it('should return correct message when messageType is LGAAddLiveMatch', () => {
    const messageContent = 'ADD LIVE';

    const actual = MessageService.customizePreMatchContent(
      PreMatchMessageTypes.LGAAddLiveMatch,
      messageContent
    );

    expect(actual).toEqual('<span class="add-live">ADD LIVE</span>');
  });

  it('should return correct message when messageType is LGACancelLiveMatch', () => {
    const messageContent = 'CANCEL LIVE';

    const actual = MessageService.customizePreMatchContent(
      PreMatchMessageTypes.LGACancelLiveMatch,
      messageContent
    );

    expect(actual).toEqual('<span class="cancel-live">CANCEL LIVE</span>');
  });

  it('should return correct default message', () => {
    const messageContent = 'CANCEL LIVE';

    const actual = MessageService.customizePreMatchContent('', messageContent);

    expect(actual).toEqual('CANCEL LIVE');
  });
});
