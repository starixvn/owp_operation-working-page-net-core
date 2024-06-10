import { v4 as uuidv4 } from 'uuid';
import isEqual from 'lodash.isequal';

const generateKeyTVList = tvList =>
  tvList.map(item => ({
    ...item,
    key: uuidv4()
  }));

const buildModelFilter = state => {
  const [League, LeagueGroupId, IsActive, Category] = [
    state.leagueReducer.league,
    state.leagueGroupReducer.leagueGroupBackup.value,
    state.statusReducer.status.value === 1,
    state.leagueCategoryReducer.leagueCategory.label
  ];
  return {
    League,
    LeagueGroupId,
    IsActive,
    Category
  };
};

const rebuildLeagueGroupOption = option =>
  option.concat([{ value: 0, label: '' }]);

const buildModelTVList = state => {
  let { tvListExpression } = state.tvListReducer;
  tvListExpression = tvListExpression.filter(
    item => item.value.trim().length > 0
  );
  return tvListExpression.map(item => {
    return {
      Label: item.expression.label,
      Value: item.value
    };
  });
};

const buildModelRound = model => {
  return model.map(round => {
    return {
      Type: round.expression.label,
      Value: round.value
    };
  });
};

const buildModelCondition = model => {
  let { leagueSettingId, tv, round, division } = model;
  round = round.filter(item => item.value.trim().length > 0);
  division = division.filter(item => item.value.trim().length > 0);
  return {
    LeagueSettingId: leagueSettingId,
    HasTV: tv.hasTV,
    TV: tv.tv,
    Rounds: buildModelRound(round),
    Divisions: buildModelRound(division)
  };
};

const calculateNumberOfDaysBetweenTwoDates = (fromDate, toDate) => {
  const differenceInTime = toDate.getTime() - fromDate.getTime();
  return differenceInTime / (1e3 * 3600 * 24);
};

const excludeOldLeague = dataSource => {
  const currentDate = new Date();
  return dataSource.filter(item => {
    if (item.LastUse) {
      const lastUseDate = new Date(item.LastUse);
      return (
        calculateNumberOfDaysBetweenTwoDates(lastUseDate, currentDate) < 365
      );
    }
    return false;
  });
};

const updateLeagueGroup = (dataSource, extraModel) => {
  const { rowIndex, GroupValue, GroupLabel } = extraModel;
  return dataSource.map((item, index) => {
    if (index !== rowIndex) {
      return item;
    }
    return {
      ...item,
      GroupValue,
      GroupLabel
    };
  });
};

const updateActive = (dataSource, extraModel) => {
  const { rowIndex, IsActive } = extraModel;
  return dataSource.map((item, index) => {
    if (index !== rowIndex) {
      return item;
    }
    return {
      ...item,
      IsActive
    };
  });
};

const updateBlackList = (dataSource, extraModel) => {
  const { rowIndex, IsBlackList } = extraModel;
  return dataSource.map((item, index) => {
    if (index !== rowIndex) {
      return item;
    }
    return {
      ...item,
      IsBlackList
    };
  });
};

const updateLeagueDetail = (dataSource, extraModel) => {
  const { rowIndex, key, value } = extraModel;
  return dataSource.map((item, index) => {
    if (index !== rowIndex) {
      return item;
    }
    return {
      ...item,
      [key]: value
    };
  });
};

const removeLeagueDetail = (dataSource, extraModel) => {
  const { rowIndex } = extraModel;
  return dataSource.filter((_, index) => !isEqual(index, rowIndex));
};

const buildPropsMaxbetSpreadCell = (data, rowIndex, modelKey) => ({
  data,
  rowIndex,
  modelKey
});

const hasWriteOnPageMaxbetGeneral = policyData =>
  policyData.generalPolicy.HasWrite ||
  policyData.conditionPolicy.HasWrite ||
  policyData.lstNotePolicy.HasWrite ||
  policyData.tvListPolicy.HasWrite;

const segmentPrefix = [
  'V1',
  'V2',
  'V3',
  'V4',
  'V5',
  'V6',
  'V7',
  'Main',
  'v1',
  'v2',
  'OWP-Staging',
  'OWP'
];

const buildLeagueDetailLink = (id, leagueName, isActive) => {
  if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
    return `/${id}/${leagueName}/${isActive}`;
  }
  const segmentFirst = window.location.pathname.split('/')[1];
  return segmentPrefix.some(seg => isEqual(seg, segmentFirst))
    ? `/${segmentFirst}/LeagueManagement/${id}/${leagueName}/${isActive}`
    : `/LeagueManagement/${id}/${leagueName}/${isActive}`;
};

const buildMaxbetUrl = () => {
  let maxbetGeneralUrl = '';
  let maxbetDetailUrl = '';
  if (!process.env.NODE_ENV || process.env.NODE_ENV === 'development') {
    maxbetGeneralUrl = '/';
    maxbetDetailUrl = '/:id/:leagueName/:isActive';
  } else {
    const segmentFirst = window.location.pathname.split('/')[1];
    if (segmentPrefix.some(seg => isEqual(seg, segmentFirst))) {
      maxbetGeneralUrl = `/${segmentFirst}/LeagueManagement`;
      maxbetDetailUrl = `/${segmentFirst}/LeagueManagement/:id/:leagueName/:isActive`;
    } else {
      maxbetGeneralUrl = '/LeagueManagement';
      maxbetDetailUrl = '/LeagueManagement/:id/:leagueName/:isActive';
    }
  }
  return {
    maxbetGeneralUrl,
    maxbetDetailUrl
  };
};

export {
  generateKeyTVList,
  buildModelFilter,
  buildModelTVList,
  buildModelCondition,
  rebuildLeagueGroupOption,
  updateLeagueGroup,
  updateActive,
  updateBlackList,
  calculateNumberOfDaysBetweenTwoDates,
  excludeOldLeague,
  buildMaxbetUrl,
  hasWriteOnPageMaxbetGeneral,
  updateLeagueDetail,
  removeLeagueDetail,
  buildPropsMaxbetSpreadCell,
  buildLeagueDetailLink
};
