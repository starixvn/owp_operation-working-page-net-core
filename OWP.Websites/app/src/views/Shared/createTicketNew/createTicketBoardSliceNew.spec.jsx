import createTicketBoardSlice, {
  closeCreatePopup,
  openPopupEventChecking,
  openPopupOthers,
  selectedEvent,
  otherSubject,
  setSelectedRecipients,
  changeErrorInput,
  changeErrorRecipient,
  changeErrorOther,
  changeErrorTextField,
  setEventChecking,
  setTypeTicketText,
  setSubTypeOption,
  setSubTypeSelected,
  openPopupLiveIncident
} from './createTicketBoardSliceNew';

describe('createTicketBoardSlice under test', () => {
  it('should return initial state', () => {
    expect(createTicketBoardSlice(undefined, {})).toEqual({
      isOpenPopup: false,
      isEventChecking: false,
      isOthers: false,
      valueEvent: null,
      valueOther: '',
      selectedRecipients: [],
      errorOther: false,
      errorInput: false,
      errorRecipient: false,
      errorTextField: '',
      isLiveIncident: false,
      isOpenLogWork: false,
      eventChecking: [],
      recipientAssigned: [],
      isOpenCreateTicketInLGA: false,
      typeTicketText: '',
      defaultRecipients: [],
      defaultRecipientsLGA: [],
      subTypeOption: [],
      subTypeSelected: [],
      'creatorInfo': null
    });
  });
  it('should handle setSubTypeSelected', () => {
    const initialState = {
      subTypeSelected: null
    };
    const action = {
      type: setSubTypeSelected.type,
      payload: 1
    };
    const expectState = {
      subTypeSelected: 1
    };
    const state = createTicketBoardSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
  it('should handle setTypeOption', () => {
    const initialState = {
      subTypeOption: []
    };
    const action = {
      type: setSubTypeOption.type,
      payload: [
        { value: null, label: '' },
        { value: 1, label: 'abc' },
        { value: 2, label: 'xyz' }
      ]
    };
    const expectState = {
      subTypeOption: [
        { value: null, label: '' },
        { value: 1, label: 'abc' },
        { value: 2, label: 'xyz' }
      ]
    };
    const state = createTicketBoardSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
  it('should handle setEventChecking', () => {
    const initialState = {
      eventChecking: []
    };
    const action = {
      type: setEventChecking.type,
      payload: [
        {
          owpLeagueId: 32459,
          leagueName: '*UEFA EUROPA LEAGUE',
          homeId: 123,
          awayId: 141,
          homeName: 'A',
          awayName: 'B',
          eventDateTimeGmt8: '2021-10-01T00:45:00',
          isLeague: false
        }
      ]
    };
    const expectState = {
      eventChecking: [
        {
          owpLeagueId: 32459,
          leagueName: '*UEFA EUROPA LEAGUE',
          homeId: 123,
          awayId: 141,
          homeName: 'A',
          awayName: 'B',
          eventDateTimeGmt8: '2021-10-01T00:45:00',
          isLeague: false
        }
      ]
    };
    const state = createTicketBoardSlice(initialState, action);
    expect(state).toEqual(expectState);
  });

  it('should handle change error textfield', () => {
    const initialState = {
      errorTextField: ''
    };
    const action = {
      type: changeErrorTextField.type,
      payload: 'abc'
    };
    const expectState = {
      errorTextField: 'abc'
    };
    const state = createTicketBoardSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
  it('should handle selectedRecipient', () => {
    const initialState = {
      selectedRecipients: []
    };
    const action = {
      type: setSelectedRecipients.type,
      payload: [
        {
          ticketTypeRecipientId: 1,
          recipientId: 1,
          recipientType: 1,
          recipientName: 'sa',
          recipientNickname: 'sa_nick'
        }
      ]
    };
    const expectState = {
      selectedRecipients: [
        {
          ticketTypeRecipientId: 1,
          recipientId: 1,
          recipientType: 1,
          recipientName: 'sa',
          recipientNickname: 'sa_nick'
        }
      ]
    };
    const state = createTicketBoardSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
  it('should handle open event checking popup', () => {
    const initialState = {
      subTypeSelected: [],
      errorInput: true,
      errorRecipient: true,
      valueEvent: null,
      selectedRecipients: [],
      isOpenPopup: false,
      isEventChecking: false,
      isOthers: false,
      isLiveIncident: false
    };
    const expectEventCheckingState = {
      subTypeSelected: [{ value: 0, label: '' }],
      errorInput: false,
      errorRecipient: false,
      valueEvent: null,

      isOpenPopup: true,
      isEventChecking: true,
      isOthers: false,
      isLiveIncident: false
    };

    const action = {
      type: openPopupEventChecking.type
    };

    const state = createTicketBoardSlice(initialState, action);

    expect(state).toEqual(expectEventCheckingState);
  });

  it('should handle open event live incident popup', () => {
    const initialState = {
      subTypeSelected: [],
      errorInput: true,
      errorRecipient: true,
      valueEvent: null,
      selectedRecipients: [],
      isOpenPopup: false,
      isEventChecking: false,
      isOthers: false,
      isLiveIncident: false
    };
    const expectEventCheckingState = {
      subTypeSelected: [{ value: 0, label: '' }],
      errorInput: false,
      errorRecipient: false,
      valueEvent: null,

      isOpenPopup: true,
      isEventChecking: false,
      isOthers: false,
      isLiveIncident: true
    };

    const action = {
      type: openPopupLiveIncident.type
    };

    const state = createTicketBoardSlice(initialState, action);

    expect(state).toEqual(expectEventCheckingState);
  });

  it('should handle change other error', () => {
    const initialState = {
      errorOther: false
    };
    const action = {
      type: changeErrorOther.type,
      payload: true
    };
    const expectState = {
      errorOther: true
    };
    const state = createTicketBoardSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
  it('should handle change event error', () => {
    const initialState = {
      errorInput: false
    };
    const action = {
      type: changeErrorInput.type,
      payload: true
    };
    const expectState = {
      errorInput: true
    };
    const state = createTicketBoardSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
  it('should handle change recipient error', () => {
    const initialState = {
      errorRecipient: false
    };
    const action = {
      type: changeErrorRecipient.type,
      payload: true
    };
    const expectState = {
      errorRecipient: true
    };
    const state = createTicketBoardSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
  it('should handle other value', () => {
    const initialState = {
      valueOther: ''
    };
    const action = {
      type: otherSubject.type,
      payload: 'johnny'
    };
    const expectState = {
      valueOther: 'johnny'
    };
    const state = createTicketBoardSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
  it('should handle event selected', () => {
    const initialState = {
      valueEvent: {}
    };
    const action = {
      type: selectedEvent.type,
      payload: {
        owpLeagueId: 12312,
        leagueName: '*UEFA EUROPA LEAGUE',
        homeId: 0,
        awayId: 0,
        homeName: null,
        awayName: null,
        eventDateTimeGmt8: '2021-10-01T00:45:00',
        isLeague: false
      }
    };
    const expectState = {
      valueEvent: {
        owpLeagueId: 12312,
        leagueName: '*UEFA EUROPA LEAGUE',
        homeId: 0,
        awayId: 0,
        homeName: null,
        awayName: null,
        eventDateTimeGmt8: '2021-10-01T00:45:00',
        isLeague: false
      }
    };
    const state = createTicketBoardSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
  it('should handle open others popup', () => {
    const initialState = {
      subTypeSelected: [],
      errorOther: false,
      errorRecipient: false,
      valueOther: '',
      selectedRecipients: [],
      isOpenPopup: false,
      isEventChecking: false,
      isOthers: false,
      isLiveIncident: false
    };
    const expectOthersState = {
      subTypeSelected: [{ value: 0, label: '' }],
      errorOther: false,
      errorRecipient: false,
      valueOther: '',
      isOpenPopup: true,
      isEventChecking: false,
      isOthers: true,
      isLiveIncident: false
    };
    const action = {
      type: openPopupOthers.type
    };

    const state = createTicketBoardSlice(initialState, action);

    expect(state).toEqual(expectOthersState);
  });
  it('should handle set state typeTicketText', () => {
    const initialState = {
      typeTicketText: ''
    };
    const expectOthersState = {
      typeTicketText: 'Ticket Support'
    };
    const action = {
      type: setTypeTicketText.type,
      payload: 'Ticket Support'
    };

    const state = createTicketBoardSlice(initialState, action);

    expect(state).toEqual(expectOthersState);
  });

  it('should handle close popup', () => {
    const initialState = {
      isOpenPopup: true,
      isEventChecking: true,
      isLiveIncident: true,
      isOthers: false,
      isOpenLogWork: false
    };
    const expectEventCheckingState = {
      isOpenPopup: false,
      isEventChecking: false,
      isLiveIncident: false,
      isOthers: false,
      isOpenLogWork: false
    };
    const action = {
      type: closeCreatePopup.type
    };

    const state = createTicketBoardSlice(initialState, action);

    expect(state).toEqual(expectEventCheckingState);
  });
});
