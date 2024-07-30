import {
  getFlagFromStore,
  flagFactory,
  createFlag,
  testRegex,
  checkLeague,
  Flag
} from './FlagLogic';
import { FIFA_IMAGE_STORE } from './FlagStore/FIFA';

describe('FlagLogic under test', () => {
  describe('getFlagFromStore under test', () => {
    it('have Flag in store, it should return 1 flagString', () => {
      const props = {
        flagName: 'AC Milan (V)',
        flagNameLibrary: FIFA_IMAGE_STORE,
        flagURL: '/images/Flags/FIFA'
      };
      const result = getFlagFromStore(props);
      const expectString = '/images/Flags/FIFA/AC Milan (V).jpg';

      expect(result).toEqual(expectString);
    });

    it('not have Flag in store, it should return null', () => {
      const props = {
        flagName: 'no name',
        flagNameLibrary: FIFA_IMAGE_STORE,
        flagURL: '/images/Flags/FIFA'
      };
      const result = getFlagFromStore(props);
      const expectString = null;

      expect(result).toEqual(expectString);
    });
  });
  describe('flagFactory under test', () => {
    it('should return exact flag base on flagInfo (PES)', () => {
      const props = { flagName: 'AC Milan (V)', flagInfo: 'PES' };
      const result = flagFactory(props);
      const expectString = '/images/Flags/PES/AC Milan (V).jpg';

      expect(result).toEqual(expectString);
    });

    it('should return exact flag base on flagInfo (FIFA)', () => {
      const props = { flagName: 'AC Milan (V)', flagInfo: 'FIFA' };
      const result = flagFactory(props);
      const expectString = '/images/Flags/FIFA/AC Milan (V).jpg';

      expect(result).toEqual(expectString);
    });

    it('should return exact flag base on flagInfo (FIFA)', () => {
      const props = { flagName: 'AC Milan (V)', flagInfo: 'new type' };
      const result = flagFactory(props);
      const expectString = null;

      expect(result).toEqual(expectString);
    });
  });
  describe('createFlag under test', () => {
    it('matchName condition: do not have (v), return null`', () => {
      const props = { flagName: 'AC Milan', flagInfo: 'PES' };
      const result = createFlag(props);
      const expectString = null;

      expect(result).toEqual(expectString);
    });

    it('matchName condition: have `(v)`, do not have string after `(v)`', () => {
      const props = { flagName: 'AC Milan (V)', flagInfo: 'PES' };
      const result = createFlag(props);
      const expectString = '/images/Flags/PES/AC Milan (V).jpg';

      expect(result).toEqual(expectString);
    });

    it('matchName condition: have `(v)`,  string after `(v)` only contain number, return flagString', () => {
      const props = { flagName: 'AC Milan (V) 3', flagInfo: 'PES' };
      const result = createFlag(props);
      const expectString = '/images/Flags/PES/AC Milan (V).jpg';

      expect(result).toEqual(expectString);
    });

    it('matchName condition: have `(v)`,  string after `(v)` contain char, return null', () => {
      const props = { flagName: 'AC Milan (V) 3d', flagInfo: 'PES' };
      const result = createFlag(props);
      const expectString = null;

      expect(result).toEqual(expectString);
    });
  });
  describe('testRegex under test', () => {
    it('leagueName condition: match regex`, return type of Image`', () => {
      const props = {
        leagueName: 'SABA CLUB FRIENDLY Virtual  FIFA 20 FUTSAL',
        type: 'SABA',
        condition: 'SABA'
      };
      const result = testRegex(props);
      const expectString = 'SABA';
      expect(result).toEqual(expectString);
    });

    it('leagueName condition: do not match regex `(v)`, return null', () => {
      const props = {
        leagueName: 'CLUB FRIENDLY Virtual  FIFA 20 FUTSAL',
        type: 'SABA',
        condition: 'SABA'
      };
      const result = testRegex(props);
      const expectString = null;
      expect(result).toEqual(expectString);
    });
  });

  describe('checkLeague under test', () => {
    it('leagueName condition: have SABA, PES 20, FIFA 20`, return null`', () => {
      const props = {
        leagueName: 'SAba CLUB FRIENDLY Virtual PES 20 FIFA 20 FUTSAL'
      };
      const result = checkLeague(props);
      const expectString = { isHaveFlag: null, flagInfo: null };
      expect(result).toEqual(expectString);
    });

    it('leagueName condition: have  SABA, PES 20`, return PES`', () => {
      const props = {
        leagueName: 'SAba CLUB FRIENDLY Virtual  PES 20 FUTSAL'
      };
      const result = checkLeague(props);
      const expectString = { isHaveFlag: true, flagInfo: 'PES' };
      expect(result).toEqual(expectString);
    });

    it('leagueName condition: have SABA, FIFA 20`, return FIFA`', () => {
      const props = {
        leagueName: 'SAba CLUB FRIENDLY Virtual  FIFA 20 FUTSAL'
      };
      const result = checkLeague(props);
      const expectString = { isHaveFlag: true, flagInfo: 'FIFA' };
      expect(result).toEqual(expectString);
    });
  });
  describe('Flag under test', () => {
    it('flag condition: leagueName do not have flag, return null`', () => {
      const props = {
        awayName: 'AC Milan (V) 145',
        homeName: 'Arsenal (V) 12',
        leagueName: 'CLUB FRIENDLY Virtual  FIFA 20 FUTSAL'
      };
      const result = Flag(props);
      const expectString = null;
      expect(result).toEqual(expectString);
    });

    it('flag condition: leagueName have flag, return object { awayFlag,homeFlag,flagInfo}`', () => {
      const props = {
        awayName: 'AC Milan (V) 145',
        homeName: 'Arsenal (V) 12',
        leagueName: 'SABA CLUB FRIENDLY Virtual  FIFA 20 FUTSAL'
      };
      const result = Flag(props);
      const expectString = {
        awayFlag: '/images/Flags/FIFA/AC Milan (V).jpg',
        homeFlag: '/images/Flags/FIFA/Arsenal (V).jpg',
        flagInfo: 'FIFA'
      };
      expect(result).toEqual(expectString);
    });
    it('flag condition: leagueName have flag PES 21, return object { awayFlag,homeFlag,flagInfo}`', () => {
      const props = {
        awayName: 'AC Milan (V) 145',
        homeName: 'Arsenal (V) 12',
        leagueName: 'SABA CLUB FRIENDLY Virtual PES 21 FUTSAL'
      };
      const result = Flag(props);
      const expectString = {
        awayFlag: '/images/Flags/PES21/AC Milan (V).jpg',
        homeFlag: '/images/Flags/PES21/Arsenal (V).jpg',
        flagInfo: 'PES 21'
      };
      expect(result).toEqual(expectString);
    });
  });
});
