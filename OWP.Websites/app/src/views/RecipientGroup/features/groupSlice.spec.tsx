import { REQUEST_STATUS } from '../../Shared/Constants';
import reducer from './groupSlice';
import { getRecipientGroup } from './groupThunkNative';
import { GroupDataSource } from './Type';

describe('groupSlice under test', () => {
  it('should return the initial state', () => {
    const initState = {
      loading: REQUEST_STATUS.pending,
      groups: [] as GroupDataSource[]
    };
    const action: any = {};

    expect(reducer(undefined, action)).toEqual(initState);
  });

  it('should handle getRecipientGroup.pending', () => {
    const action = {
      type: getRecipientGroup.pending.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      loading: REQUEST_STATUS.pending,
      groups: [] as GroupDataSource[]
    });
  });

  it('should handle getRecipientGroup.fulfilled', () => {
    const data: GroupDataSource[] = [
      {
        groupId: 1,
        groupName: 'LST',
        recipients: [
          {
            recipientGroupId: 1,
            recipientId: 1,
            recipientName: 'sa',
            recipientType: 1,
            recipientNickname: ''
          }
        ]
      }
    ];

    const action = {
      type: getRecipientGroup.fulfilled.type,
      payload: data
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      loading: REQUEST_STATUS.idle,
      groups: data
    });
  });

  it('should handle getRecipientGroup.reject', () => {
    const action = {
      type: getRecipientGroup.rejected.type
    };
    const actual = reducer(undefined, action);
    expect(actual).toEqual({
      loading: REQUEST_STATUS.idle,
      groups: []
    });
  });
});
