import { cloneDeep, isEqual, orderBy } from 'lodash';
import { ORDERBY } from '../../../../Shared/Constants';
import compareValues from '../../../../Shared/Order/compareValues';
import { LeagueGroupColumn } from '../constants/preMatchConst';

const changeValue = (element, key, value) => (element[key] = value);

const getUnreadMessagePreMatchLeagueIds = (matches, leagues) => {
  const leaguesMustShow = [];
  const matchesMustHide = [];

  let unReadMessageLeagues = [];
  let unReadMessageMatches = [];
  let readMessageMatches = [];

  unReadMessageLeagues = leagues.filter(l => l.HasMessages && !l.IsRead);
  unReadMessageLeagues.forEach(l => leaguesMustShow.push(l.PreMatchLeagueId));

  unReadMessageMatches = matches.filter(m => m.HasMessages && !m.IsRead);
  unReadMessageMatches.forEach(m => leaguesMustShow.push(m.PreMatchLeagueId));

  readMessageMatches = matches.filter(
    m => !m.HasMessages || (m.HasMessages && m.IsRead)
  );
  readMessageMatches.forEach(m => matchesMustHide.push(m.InfoSiteMatchId));

  const PreMatchLeagueIds = [...new Set(leaguesMustShow)];
  const InfoSiteMatchId = [...new Set(matchesMustHide)];
  return { PreMatchLeagueIds, InfoSiteMatchId };
};

const mergeMatchesToLeagues = (
  matches,
  leagues,
  filterUnreadMessage,
  filterByTimeCondition
) => {
  let leaguesCloned = cloneDeep(leagues);
  const matchesCloned = cloneDeep(matches);
  matchesCloned.reverse().forEach(m => {
    const index = leaguesCloned.findIndex(
      l => isEqual(l.PreMatchLeagueId, m.PreMatchLeagueId) && l.isExpand
    );
    if (index !== -1) {
      leaguesCloned.splice(index + 1, 0, m);
    }
  });

  if (filterUnreadMessage) {
    const { PreMatchLeagueIds, InfoSiteMatchId } =
      getUnreadMessagePreMatchLeagueIds(matches, leagues);
    leaguesCloned = leaguesCloned.filter(
      e =>
        PreMatchLeagueIds.includes(e.PreMatchLeagueId) &&
        !InfoSiteMatchId.includes(e.InfoSiteMatchId)
    );
  }

  if (filterByTimeCondition?.isShow) {
    const { leagueInfos } = filterByTimeCondition;
    const leagueIds = leagueInfos.map(x => x.conditionLeagueId);
    leaguesCloned = leaguesCloned.filter(x =>
      leagueIds.includes(x.PreMatchLeagueId)
    );
  }
  updateRowLeagues(leaguesCloned);
  return leaguesCloned;
};

const getLeagueInfosByTimeCondition = (matches, datetime, isNightShift) => {
  const matchesWithTimeCondition = isNightShift
    ? matches.filter(item => new Date(item.EventDateTime) > datetime)
    : matches.filter(item => new Date(item.EventDateTime) < datetime);
  return Object.values(
    matchesWithTimeCondition.reduce((leagueInfo, item) => {
      if (leagueInfo[item.PreMatchLeagueId]) {
        leagueInfo[item.PreMatchLeagueId].totalActiveMatch++;
      } else {
        leagueInfo[item.PreMatchLeagueId] = {
          conditionLeagueId: item.PreMatchLeagueId,
          totalActiveMatch: 1
        };
      }
      return leagueInfo;
    }, {})
  );
};
const changeExpandStateAllLeague = (leagues, expandState) => {
  leagues.forEach(l => changeValue(l, 'isExpand', expandState));
};

const changeExpandStateOneLeague = (leagues, preMatchLeagueId, expandState) => {
  const leagueIndex = leagues.findIndex(
    l => l.PreMatchLeagueId === preMatchLeagueId && l.MatchCount
  );
  leagues[leagueIndex].isExpand = expandState;
};

const sortLeaguesByColumnKey = (leagues, columnKey, orderType) => {
  const rowsValue = leagues.filter(row => row[columnKey]) || [];
  const rowsValueNull =
    leagues.filter(row => !row[columnKey] && !row[`${columnKey}Hint`]) || [];
  const rowsValueContainHint =
    leagues.filter(row => !row[columnKey] && row[`${columnKey}Hint`]) || [];
  if (isEqual(orderType, 'ORDER_ASC')) {
    rowsValue.sort(compareValues(columnKey, 'asc'));
    rowsValueContainHint.sort(compareValues(`${columnKey}Hint`, 'asc'));
  } else {
    rowsValue.sort(compareValues(columnKey, 'desc'));
    rowsValueContainHint.sort(compareValues(`${columnKey}Hint`, 'desc'));
  }
  if (isEqual(columnKey, LeagueGroupColumn.columnKey)) {
    return [...rowsValueContainHint, ...rowsValue, ...rowsValueNull];
  }
  return [...rowsValueContainHint, ...rowsValueNull, ...rowsValue];
};

const changeOpenNoteStateAll = (array, openState) => {
  array.forEach(l => changeValue(l, 'isOpenNote', openState));
};

const changeOpenNoteStateOneLeague = (
  leagues,
  matches,
  preMatchLeagueId,
  openState
) => {
  changeOpenNoteStateAll(leagues, false);
  changeOpenNoteStateAll(matches, false);
  const leagueIndex = leagues.findIndex(
    l => l.PreMatchLeagueId === preMatchLeagueId
  );
  leagues[leagueIndex].isOpenNote = openState;
};

const changeOpenNoteStateOneMatch = (
  leagues,
  matches,
  infoSiteMatchId,
  openState
) => {
  changeOpenNoteStateAll(leagues, false);
  changeOpenNoteStateAll(matches, false);
  const matchIndex = matches.findIndex(
    l => l.InfoSiteMatchId === infoSiteMatchId
  );
  matches[matchIndex].isOpenNote = openState;
};

const updateLeagueData = (leagues, entities, updatedLeagueIds, newLeague) => {
  const { index, objectKey, newValue } = newLeague;

  const updatedLeague = entities[index];
  const league = leagues.find(l =>
    isEqual(l.PreMatchLeagueId, updatedLeague.PreMatchLeagueId)
  );

  const isExistsIdInPreMatchIdUpdated = updatedLeagueIds.some(id =>
    isEqual(id, updatedLeague.PreMatchLeagueId)
  );
  !isExistsIdInPreMatchIdUpdated &&
    updatedLeagueIds.push(updatedLeague.PreMatchLeagueId);

  updatedLeague[objectKey] = newValue;
  league[objectKey] = newValue;
};

const updateLeagues = (leagues, pasteValue, leagueIds) => {
  const { leagueIndex, isTakeOver, copiedValue, rowEnd } = pasteValue;

  let copiedValueIndex = 0;
  for (let index = rowEnd + 1; index <= leagueIndex; index++) {
    const { MainAssignee1, MainAssignee2, OtherAssignee1, OtherAssignee2 } =
      copiedValue[copiedValueIndex].value;

    const league = leagues[index];
    const { LeagueId } = league;
    leagueIds.push(LeagueId);
    if (isTakeOver) {
      league.HDPTakeOver = MainAssignee1;
      league.OUTakeOver = MainAssignee2;
      league.OtherHDPTakeOver = OtherAssignee1;
      league.OtherOUTakeOver = OtherAssignee2;
    } else {
      league.HDP = MainAssignee1;
      league.OU = MainAssignee2;
      league.OtherHDP = OtherAssignee1;
      league.OtherOU = OtherAssignee2;
    }
    copiedValueIndex++;
    if (isEqual(copiedValueIndex, copiedValue.length)) {
      copiedValueIndex = 0;
    }
  }
};

const updateEntity = (entities, pasteValue, leagueIds, updatedLeagueIds) => {
  const { isTakeOver, copiedValue } = pasteValue;
  let copiedValueIndex = 0;
  leagueIds.forEach(leagueId => {
    const { MainAssignee1, MainAssignee2, OtherAssignee1, OtherAssignee2 } =
      copiedValue[copiedValueIndex].value;
    const entity = entities.find(l => l.LeagueId === leagueId);

    const isExistsIdInPreMatchIdUpdated = updatedLeagueIds.some(id =>
      isEqual(id, entity.PreMatchLeagueId)
    );
    !isExistsIdInPreMatchIdUpdated &&
      updatedLeagueIds.push(entity.PreMatchLeagueId);

    if (entity) {
      copiedValueIndex++;
      if (isTakeOver) {
        entity.HDPTakeOver = MainAssignee1;
        entity.OUTakeOver = MainAssignee2;
        entity.OtherHDPTakeOver = OtherAssignee1;
        entity.OtherOUTakeOver = OtherAssignee2;
      } else {
        entity.HDP = MainAssignee1;
        entity.OU = MainAssignee2;
        entity.OtherHDP = OtherAssignee1;
        entity.OtherOU = OtherAssignee2;
      }
      if (isEqual(copiedValueIndex, copiedValue.length)) {
        copiedValueIndex = 0;
      }
    }
  });
};

const pasteMultipleValue = (
  leagues,
  entities,
  updatedLeagueIds,
  pasteValue
) => {
  const leagueIds = [];
  const { highlightedCell } = pasteValue;

  if (!isEqual(highlightedCell.isTakeOver, pasteValue.isTakeOver)) {
    return;
  }
  updateLeagues(leagues, pasteValue, leagueIds);
  updateEntity(entities, pasteValue, leagueIds, updatedLeagueIds);
};

const pasteCopied = (leagues, entities, updatedLeagueIds, pasteValue) => {
  const { rowIndex, isTakeOver, copiedValue } = pasteValue;

  if (copiedValue.length > 1) {
    return;
  }
  const { MainAssignee1, MainAssignee2, OtherAssignee1, OtherAssignee2 } =
    copiedValue[0].value;
  const entity = entities[rowIndex];
  const league = leagues.find(l =>
    isEqual(l.PreMatchLeagueId, entity.PreMatchLeagueId)
  );

  const isExistsIdInPreMatchIdUpdated = updatedLeagueIds.some(id =>
    isEqual(id, entity.PreMatchLeagueId)
  );
  !isExistsIdInPreMatchIdUpdated &&
    updatedLeagueIds.push(entity.PreMatchLeagueId);

  if (isTakeOver) {
    entity.HDPTakeOver = MainAssignee1;
    league.HDPTakeOver = MainAssignee1;

    entity.OUTakeOver = MainAssignee2;
    league.OUTakeOver = MainAssignee2;

    entity.OtherHDPTakeOver = OtherAssignee1;
    league.OtherHDPTakeOver = OtherAssignee1;

    league.OtherOUTakeOver = OtherAssignee2;
    entity.OtherOUTakeOver = OtherAssignee2;
  } else {
    entity.HDP = MainAssignee1;
    league.HDP = MainAssignee1;

    entity.OU = MainAssignee2;
    league.OU = MainAssignee2;

    entity.OtherHDP = OtherAssignee1;
    league.OtherHDP = OtherAssignee1;

    entity.OtherOU = OtherAssignee2;
    league.OtherOU = OtherAssignee2;
  }
};

const hasAssigneeLeague = row =>
  row.HDP || row.OU || row.HDPTakeOver || row.OUTakeOver;

const getValueToLower = value => (value ? value.toLowerCase() : null);

const sortLeaguesByDefault = data => {
  const leaguesHasNotAssignee = data.filter(row => !hasAssigneeLeague(row));
  const leaguesHasAssignee = data.filter(row => hasAssigneeLeague(row));
  const sortedHasAssigneeLeagues = orderBy(
    leaguesHasAssignee,
    [
      u => getValueToLower(u.HDP),
      u => getValueToLower(u.OU),
      u => getValueToLower(u.HDPTakeOver),
      u => getValueToLower(u.OUTakeOver),
      u => getValueToLower(u.LeagueName)
    ],
    [ORDERBY.ASC, ORDERBY.ASC, ORDERBY.ASC, ORDERBY.ASC, ORDERBY.ASC]
  );
  return [...leaguesHasNotAssignee, ...sortedHasAssigneeLeagues];
};

const clearState = state => {
  state.prematchIdUpdated.length = 0;
  state.prematchIdDeleted.length = 0;
  state.sortState = {};
};
const getMatchActiveByCondition = (
  leagueWithTimeCondition,
  preMatchLeagueId
) => {
  if (leagueWithTimeCondition.leagueInfos.length > 0) {
    return (
      leagueWithTimeCondition.leagueInfos.find(
        x => x.conditionLeagueId === preMatchLeagueId
      )?.totalActiveMatch ?? null
    );
  }
  return null;
};
const getTotalLeagues = data => data.filter(x => x?.No)?.length;

const updateRowLeagues = data => {
  let index = 1;
  data.forEach(x => {
    if (x.No) {
      x.No = index;
      index++;
    }
  });
};

export {
  getUnreadMessagePreMatchLeagueIds,
  mergeMatchesToLeagues,
  changeExpandStateAllLeague,
  changeExpandStateOneLeague,
  sortLeaguesByColumnKey,
  changeOpenNoteStateOneLeague,
  changeOpenNoteStateOneMatch,
  updateLeagueData,
  pasteCopied,
  pasteMultipleValue,
  sortLeaguesByDefault,
  clearState,
  getLeagueInfosByTimeCondition,
  getMatchActiveByCondition,
  getTotalLeagues,
  updateRowLeagues
};
