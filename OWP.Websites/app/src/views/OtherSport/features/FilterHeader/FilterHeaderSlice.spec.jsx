import filterHeaderSlice, {
  onSubmitFilterModel
} from './filterHeaderSlice';

describe('filterHeaderSlice under test', () => {
  it('should return the initial state', () => {
    const initState = { model: {} };
    expect(filterHeaderSlice(initState, {})).toEqual({
      model: {}
    });
  });

  describe('onSubmitFilterModel under test', () => {
    it('should update state', () => {
      const initState = { model: {} };

      const expectState = {
        model: {
          From: new Date('Mon Jan 18 2021 10:34:03 GMT+0700 (Indochina Time)'),
          To: new Date('Tue Jan 19 2021 10:34:03 GMT+0700 (Indochina Time)'),
          Employee: 'sa',
          Sports: ['Ball', 'Dart'],
          MatchStatuses: ['Live']
        }
      };

      const action = {
        type: onSubmitFilterModel.type,
        payload: {
          From: new Date('Mon Jan 18 2021 10:34:03 GMT+0700 (Indochina Time)'),
          To: new Date('Tue Jan 19 2021 10:34:03 GMT+0700 (Indochina Time)'),
          Employee: 'sa',
          Sports: ['Ball', 'Dart'],
          MatchStatuses: ['Live']
        }
      };

      const state = filterHeaderSlice(initState, action);
      expect(state).toEqual(expectState);
    });
  });
});
