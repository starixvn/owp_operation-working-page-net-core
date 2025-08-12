import reducer, { updateFilter } from './FilterSlice';
import { buildDefaultFilterDateProviderVerification } from '../../../Shared/DateTime/dateTimeHelper';

describe('filterSlice', () => {
  const initialState = {
    filter: {
      eventDate: buildDefaultFilterDateProviderVerification(),
      onlyShowIsMismatchEvent: false,
      confirmProvider: [],
      lstBookProvider: []
    }
  };

  it('should return the initial state', () => {
    expect(reducer(undefined, { type: undefined })).toEqual(initialState);
  });

  it('should handle updateFilter action', () => {
    const previousState = initialState;
    const newEventDate = '2024-07-09';

    const action = updateFilter({ key: 'eventDate', value: newEventDate });
    const newState = reducer(previousState, action);

    expect(newState.filter.eventDate).toEqual(newEventDate);
  });
});
