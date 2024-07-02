import { REQUEST_STATUS } from '../Constants';
import slice, {
  closePopup,
  getTicketDetail,
  saveMessage,
  openTicketPopup,
  closeTicketPopup,
  getTicketSubTypes,
  getModelByRowIndex,
  setMessage,
  updateRecipient
} from './chatBoardSlice';

describe('unit test for chatBoardSlice', () => {
  it('should return init state', () => {
    const action = {};
    const expectedState = {
      isOpenPopup: false,
      isOpenTicket: false,
      modelByRowIndex: {},
      loading: REQUEST_STATUS.idle,
      loadMoreMessage: REQUEST_STATUS.idle,
      error: null,
      isShowAllMessage: false,
      ticketDetail: {
        ticketId: '',
        eventType: 0,
        ticketTypeId: '',
        ticketTypeName: '',
        ticketSubTypeId: 0,
        ticketSubTypeName: '',
        canShowMoreMessage: false,
        messages: [],
        recipients: [],
        owpLeagueId: '',
        owpLeagueName: '',
        owpHomeId: '',
        owpAwayId: '',
        homeName: '',
        awayName: '',
        subject: '',
        ticketStatus: '',
        eventName: '',
        creatorInfo: {
          id: 0,
          userName: '',
          nickName: ''
        }
      },
      ticketSubTypeOptions: [],
      inputMessage: '',
      modelQuoteMessage: {},
      imagesLoaded: 0,
      isAllImagesLoaded: false
    };

    const newState = slice(undefined, action);

    expect(newState).toEqual(expectedState);
  });
  it('should handle updateRecipient', () => {
    const state = {
      ticketDetail: {}
    };
    const expectedState = {
      ticketDetail: {
        recipients: [
          {
            recipientId: 4,
            groupName: 'abc',
            userName: null,
            nickName: null,
            recipientType: 2
          }
        ]
      }
    };
    const action = {
      type: updateRecipient.type,
      payload: [
        {
          recipientId: 4,
          groupName: 'abc',
          userName: null,
          nickName: null,
          recipientType: 2
        }
      ]
    };
    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });
  it('should handle setMessage', () => {
    const state = {
      inputMessage: ''
    };
    const expectedState = {
      inputMessage: 'sa'
    };
    const action = {
      type: setMessage.type,
      payload: 'sa'
    };
    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });
  it('should handle closePopup', () => {
    const state = {
      isOpenPopup: true
    };
    const expectedState = {
      isOpenPopup: false
    };
    const action = {
      type: closePopup.type
    };
    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });

  it('should handle getTicketDetail.fulfilled', () => {
    const state = {
      isOpenPopup: false,
      ticketDetail: {}
    };
    const expectedState = {
      imagesLoaded: 0,
      isAllImagesLoaded: false,
      isOpenPopup: true,
      loading: REQUEST_STATUS.idle,
      ticketDetail: {
        ticketId: '12',
        ticketTypeId: '1',
        ticketTypeName: 'a',
        messages: [],
        owpLeagueId: 'a',
        owpLeagueName: 'a',
        owpHomeId: 'a',
        owpAwayId: 'a',
        homeName: 'a',
        awayName: 'a',
        subject: '',
        creatorInfo: null
      }
    };
    const action = {
      type: getTicketDetail.fulfilled,
      payload: {
        data: {
          ticketId: '12',
          ticketTypeId: '1',
          ticketTypeName: 'a',
          messages: [],
          owpLeagueId: 'a',
          owpLeagueName: 'a',
          owpHomeId: 'a',
          owpAwayId: 'a',
          homeName: 'a',
          awayName: 'a',
          subject: '',
          creatorInfo: null
        }
      }
    };

    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });

  it('should handle saveMessage.fulfilled', () => {
    const state = {
      ticketDetail: {
        messages: [
          {
            isSaved: false
          }
        ]
      }
    };
    const expectedState = {
      ticketDetail: {
        messages: [
          {
            isSaved: true
          }
        ]
      }
    };
    const action = {
      type: saveMessage.fulfilled.type,
      payload: {
        index: 0,
        isSaved: true
      }
    };

    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });

  it('should handle saveMessage.rejected', () => {
    const state = {
      error: 'error',
      loading: 'idle'
    };
    const expectedState = {
      loading: REQUEST_STATUS.idle,
      error: 'error'
    };
    const action = {
      type: saveMessage.rejected.type,
      error: 'error'
    };

    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });

  it('should update state open popup', () => {
    const state = {
      isOpenTicket: false
    };
    const expectedState = {
      isOpenTicket: true
    };
    const action = {
      type: openTicketPopup.type
    };
    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });

  it('should update state close popup', () => {
    const state = {
      isOpenTicket: true
    };
    const expectedState = {
      isOpenTicket: false
    };
    const action = {
      type: closeTicketPopup.type
    };
    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });

  it('should handle getTicketSubTypes.fulfilled', () => {
    const state = {
      ticketSubTypeOptions: []
    };
    const expectedState = {
      ticketSubTypeOptions: [{ value: 'test', label: 'test' }]
    };
    const action = {
      type: getTicketSubTypes.fulfilled.type,
      payload: [{ value: 'test', label: 'test' }]
    };

    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });

  it('should handle getModelByRowIndex.pending', () => {
    const state = {
      loading: REQUEST_STATUS.idle
    };
    const expectedState = {
      loading: REQUEST_STATUS.pending
    };
    const action = {
      type: getModelByRowIndex.pending.type
    };

    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });
  it('should handle getModelByRowIndex.fulfilled', () => {
    const state = {
      modelByRowIndex: {},
      loading: REQUEST_STATUS.idle
    };
    const expectedState = {
      modelByRowIndex: {
        test: 'anything'
      },
      loading: REQUEST_STATUS.idle
    };
    const action = {
      type: getModelByRowIndex.fulfilled.type,
      payload: { test: 'anything' }
    };

    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });
  it('should handle getModelByRowIndex.rejected', () => {
    const state = {
      error: null,
      loading: REQUEST_STATUS.idle
    };
    const expectedState = {
      error: 'error',
      loading: REQUEST_STATUS.idle
    };
    const action = {
      type: getModelByRowIndex.rejected.type,
      error: 'error'
    };

    const newState = slice(state, action);

    expect(newState).toEqual(expectedState);
  });
});
