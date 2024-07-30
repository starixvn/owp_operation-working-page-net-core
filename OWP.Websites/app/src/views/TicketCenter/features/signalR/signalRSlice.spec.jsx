import signalRSlice, {
  onSubmitFilterModel,
  onClickIssueModel
} from './signalRSlice';

describe('signalRSlice under test', () => {
  it('should return initial state', () => {
    expect(signalRSlice(undefined, {})).toEqual({
      filterModel: null,
      issueModel: null
    });
  });
  it('should handle onSubmitFilterModel', () => {
    const initialState = {
      filterModel: null
    };
    const action = {
      type: onSubmitFilterModel.type,
      payload: { MarketKey: 1, ActorId: 169368 }
    };
    const expectState = {
      filterModel: { MarketKey: 1, ActorId: 169368 }
    };
    const state = signalRSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
  it('should handle onClickIssueModel', () => {
    const initialState = {
      issueModel: null
    };
    const action = {
      type: onClickIssueModel.type,
      payload: {
        ticketId: 566,
        actorId: 169368,
        detailSegmentUrl: 'ticket-detail/get-other',
        eventType: 3
      }
    };
    const expectState = {
      issueModel: {
        ticketId: 566,
        actorId: 169368,
        detailSegmentUrl: 'ticket-detail/get-other',
        eventType: 3
      }
    };
    const state = signalRSlice(initialState, action);
    expect(state).toEqual(expectState);
  });
});
