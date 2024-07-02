import { getDefaultValueSelected } from './FilterHeaderHelper';
jest.mock('../../../Shared/Utils');

const setUp = (key, localStorageData) => {
  global.localStorage.setItem(key, localStorageData);
  JSON.parse = jest.fn().mockImplementationOnce(() => localStorageData);
};

describe('FilterHeaderHelper underTest', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  describe('getDefaultValueSelected underTest', () => {
    it('should handle when exist localStorageData', () => {
      const storageData = {
        employee: 'sa',
        selectedOfferLives: [1, 2, 3],
        selectedMatchStatuses: [1, 2, 3, 4]
      };
      setUp('anyKey', storageData);
      const expected = {
        defaultOfferLives: [1, 2, 3],
        defaultMatchStatuses: [1, 2, 3, 4],
        defaultLinkedMatches: ['Linked Match', 'Unlinked Match'],
        defaultSports: [
          'Badminton',
          'Baseball',
          'Basketball',
          'Cricket',
          'Darts',
          'Football',
          'Field Hockey',
          'Handball',
          'Ice Hockey',
          'Netball',
          'Pool',
          'Snooker',
          'Table Tennis',
          'Tennis',
          'Volleyball',
          'Other Sports'
        ]
      };

      const result = getDefaultValueSelected('anyKey');

      expect(result.defaultOfferLives).toEqual(expected.defaultOfferLives);
    });
    it('should handle when does not exist localStorageData', () => {
      setUp('anyKey', undefined);
      const expected = {
        defaultOfferLives: ['Live', 'Postponed', 'Canceled'],
        defaultMatchStatuses: ['In-play', 'Completed', 'Prematch', 'Early'],
        defaultLinkedMatches: ['Linked Match', 'Unlinked Match'],
        defaultSports: [
          'Badminton',
          'Baseball',
          'Basketball',
          'Cricket',
          'Darts',
          'Football',
          'Field Hockey',
          'Handball',
          'Ice Hockey',
          'Netball',
          'Pool',
          'Snooker',
          'Table Tennis',
          'Tennis',
          'Volleyball',
          'Other Sports'
        ]
      };

      const result = getDefaultValueSelected('anyKey');
      const defaultOfferLivesResult = result.defaultOfferLives.map(
        x => x.value
      );

      expect(defaultOfferLivesResult).toEqual(expected.defaultOfferLives);
    });
  });
});
