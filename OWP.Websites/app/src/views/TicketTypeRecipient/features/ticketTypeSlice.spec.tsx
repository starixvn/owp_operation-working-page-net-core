import { any } from 'prop-types';
import { REQUEST_STATUS } from '../../Shared/Constants';
import reducer from './ticketTypeSlice';
import {
  getTicketTypeRecipients,
  saveTicketTypeRecipients
} from './ticketTypeThunkNative';
import { TicketTypeRecipientDataSource } from './TypeNative';

type stateType = {
  loading: string;
  entities: TicketTypeRecipientDataSource[];
};

describe('ticketTypeSlice under test', () => {
  it('should return the initial state', () => {
    const initState: stateType = {
      loading: REQUEST_STATUS.pending,
      entities: [] as TicketTypeRecipientDataSource[]
    };
    const action: any = {};

    expect(reducer(undefined, action)).toEqual(initState);
  });

  it('should handle getTicketTypeRecipients.pending', () => {
    const action = {
      type: getTicketTypeRecipients.pending.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      loading: REQUEST_STATUS.pending,
      entities: [] as TicketTypeRecipientDataSource[]
    });
  });

  it('should handle getTicketTypeRecipients.fulfilled', () => {
    const data: TicketTypeRecipientDataSource[] = [
      {
        ticketType: {
          typeId: 1,
          typeName: 'type1'
        },
        recipients: [
          {
            ticketTypeRecipientId: 1,
            recipientId: 1,
            recipientType: 1,
            recipientName: 'sa',
            recipientNickname: 'sa_nick',
            subtypeName: 'Sub1'
          }
        ],
        subtypes: []
      }
    ];

    const action = {
      type: getTicketTypeRecipients.fulfilled.type,
      payload: data
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      loading: REQUEST_STATUS.idle,
      entities: data
    });
  });

  it('should handle getTicketTypeRecipients.reject', () => {
    const action = {
      type: getTicketTypeRecipients.rejected.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      loading: REQUEST_STATUS.idle,
      entities: []
    });
  });

  it('should handle saveTicketTypeRecipients.pending', () => {
    const action = {
      type: saveTicketTypeRecipients.pending.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      loading: REQUEST_STATUS.pending,
      entities: []
    });
  });

  it('should handle saveTicketTypeRecipients.fulfilled', () => {
    const action = {
      type: getTicketTypeRecipients.fulfilled.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      loading: REQUEST_STATUS.idle,
      entities: undefined
    });
  });

  it('should handle saveTicketTypeRecipients.reject', () => {
    const action = {
      type: getTicketTypeRecipients.rejected.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      loading: REQUEST_STATUS.idle,
      entities: []
    });
  });
});
