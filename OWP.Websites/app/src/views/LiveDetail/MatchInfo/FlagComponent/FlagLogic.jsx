import { FIFA_IMAGE_STORE } from './FlagStore/FIFA';
import {
  FIFA_FLAG,
  PES_FLAG,
  PES_FLAG_21,
  FIFA_FLAG_23,
  PES_FLAG_23,
  SABA,
  FC_24,
  V_LEAGUE
} from './FlagStore/FlagsCondition';
import { FC_24_IMAGE_STORE } from './FlagStore/FC24';
import { PES_IMAGE_STORE } from './FlagStore/PES';
import { PES_21_IMAGE_STORE } from './FlagStore/PES21';
import { FIFA_23_IMAGE_STORE } from './FlagStore/FIFA23';
import { PES_23_IMAGE_STORE } from './FlagStore/PES23';

export const getFlagFromStore = props => {
  const { flagName, flagNameLibrary, flagURL, flagExtension = '.jpg' } = props;
  const flagInStore = flagNameLibrary.find(
    f => f.toLowerCase().replace(/\s+/g, ' ').trim() === flagName.toLowerCase().replace(/\s+/g, ' ').trim()
  );

  if (flagInStore) {
    return `${flagURL}/${flagInStore}${flagExtension}`;
  }

  return null;
};

export const flagFactory = props => {
  const { flagName, flagInfo } = props;
  switch (flagInfo) {
    case V_LEAGUE.type:
      return getFlagFromStore({
        flagName,
        flagNameLibrary: V_LEAGUE_IMAGE_STORE,
        flagURL: V_LEAGUE.rootPath,
        flagExtension: '.png'
      });
    case 'FIFA':
      return getFlagFromStore({
        flagName,
        flagNameLibrary: FIFA_IMAGE_STORE,
        flagURL: FIFA_FLAG.rootPath
      });
    case 'PES':
      return getFlagFromStore({
        flagName,
        flagNameLibrary: PES_IMAGE_STORE,
        flagURL: PES_FLAG.rootPath
      });
    case 'PES 21':
      return getFlagFromStore({
        flagName,
        flagNameLibrary: PES_21_IMAGE_STORE,
        flagURL: PES_FLAG_21.rootPath
      });
    case 'PES 23':
      return getFlagFromStore({
        flagName,
        flagNameLibrary: PES_23_IMAGE_STORE,
        flagURL: PES_FLAG_23.rootPath
      });
    case 'FIFA 23':
      return getFlagFromStore({
        flagName,
        flagNameLibrary: FIFA_23_IMAGE_STORE,
        flagURL: FIFA_FLAG_23.rootPath
      });
    case 'FC 24':
      return getFlagFromStore({
        flagName,
        flagNameLibrary: FC_24_IMAGE_STORE,
        flagURL: FC_24.rootPath
      });
    default:
      return null;
  }
};

export const createFlag = props => {
  const { flagName, flagInfo } = props;

  if (flagInfo === V_LEAGUE.type) {
    return flagFactory({ flagName, flagInfo });
  }
  const vStringCondition = /[(]V[)]/im;

  const vString = vStringCondition.exec(flagName);
  if (vString) {
    const vStringRight = flagName
      .substring(vString.index + 3, flagName.length)
      .trim();
    const vStringLeft = flagName.substring(0, vString.index + 3).trim();

    if (vStringRight) {
      const notContainChar = /[^0-9]/im;
      const isContainChar = notContainChar.test(vStringRight);
      return isContainChar
        ? null
        : flagFactory({ flagName: vStringLeft, flagInfo });
    }
    return flagFactory({ flagName: vStringLeft, flagInfo });
  }

  return null;
};

export const testRegex = props => {
  const { leagueName, type, condition } = props;
  const regex = RegExp(condition, 'mi');
  const value = regex.test(leagueName);
  if (value) {
    return type;
  }
  return null;
};

// eslint-disable-next-line no-warning-comments
export const checkLeague = props => /* NOSONAR */ {
  const { leagueName } = props;
  const isSABA = testRegex({ leagueName, ...SABA });
  const pes = testRegex({ leagueName, ...PES_FLAG });
  const pes21 = testRegex({ leagueName, ...PES_FLAG_21 });
  const fifa = testRegex({ leagueName, ...FIFA_FLAG });
  const fifa23 = testRegex({ leagueName, ...FIFA_FLAG_23 });
  const pes23 = testRegex({ leagueName, ...PES_FLAG_23 });
  const fc24 = testRegex({ leagueName, ...FC_24 });

  if (leagueName === V_LEAGUE.condition) {
    return { isHaveFlag: true, flagInfo: V_LEAGUE.type };
  }
  if (pes && fifa) {
    return { isHaveFlag: null, flagInfo: null };
  }

  if (pes21) {
    return { isHaveFlag: true, flagInfo: PES_FLAG_21.type };
  }

  if (pes23) {
    return { isHaveFlag: true, flagInfo: PES_FLAG_23.type };
  }

  if (isSABA && pes) {
    return { isHaveFlag: true, flagInfo: PES_FLAG.type };
  }

  if (isSABA && fifa) {
    return { isHaveFlag: true, flagInfo: FIFA_FLAG.type };
  }

  if (isSABA && fifa23) {
    return { isHaveFlag: true, flagInfo: FIFA_FLAG_23.type };
  }
  if (isSABA && fc24) {
    return { isHaveFlag: true, flagInfo: FC_24.type };
  }

  return { isHaveFlag: null, flagInfo: null };
};

export const Flag = props => {
  const { leagueName, homeName, awayName } = props;
  const { isHaveFlag, flagInfo } = checkLeague({ leagueName });

  const newAwayName = awayName.replace(/\u00A0/, ' ');
  const newHomeName = homeName.replace(/\u00A0/, ' ');

  const result = isHaveFlag
    ? {
        awayFlag: createFlag({
          flagName: newAwayName,
          flagInfo
        }),
        homeFlag: createFlag({ flagName: newHomeName, flagInfo }),
        flagInfo
      }
    : null;

  return result;
};
