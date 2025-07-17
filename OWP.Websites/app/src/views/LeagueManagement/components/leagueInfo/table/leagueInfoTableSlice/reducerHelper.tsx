import {
  TCondition,
  TDataSource,
  TMultipleFilter
} from '../TableType';
import helper from './calculateFilterOptions';

const buildDefaultTableFilter = (
  dataSource: TDataSource[],
  tableFilter: TMultipleFilter
) => {
  tableFilter.League_Group = {
    selected: helper.League_Group(dataSource),
    isFilter: false
  };
  tableFilter.League = {
    selected: helper.League(dataSource),
    isFilter: false
  };
  tableFilter.Tv = {
    selected: helper.Tv(),
    isFilter: false
  };
  tableFilter.Round = {
    selected: helper.Round(dataSource),
    isFilter: false
  };
  tableFilter.League_Division = {
    selected: helper.League_Division(dataSource),
    isFilter: false
  };
  tableFilter.Remark = {
    selected: helper.Remark(dataSource),
    isFilter: false
  };
  tableFilter.Spread_HDP_OU = {
    selected: helper.Spread_HDP_OU(dataSource),
    isFilter: false
  };
  tableFilter.MB_WD_HDP = {
    selected: helper.MB_WD_HDP(dataSource),
    isFilter: false
  };
  tableFilter.MB_WD_NO_HDP = {
    selected: helper.MB_WD_NO_HDP(dataSource),
    isFilter: false
  };
  tableFilter.MB_WK_HDP = {
    selected: helper.MB_WK_HDP(dataSource),
    isFilter: false
  };
  tableFilter.MB_WK_NO_HDP = {
    selected: helper.MB_WK_NO_HDP(dataSource),
    isFilter: false
  };
  tableFilter.LST_Note = {
    selected: helper.LST_Note(dataSource),
    isFilter: false
  };
  tableFilter.Remark2 = {
    selected: helper.Remark2(dataSource),
    isFilter: false
  };
  tableFilter.Comment = {
    selected: helper.Comment(dataSource),
    isFilter: false
  };
};

const executeFilterTable = (
  dataSource: TDataSource[],
  tableFilter: TMultipleFilter
) => {
  const {
    League_Group,
    League,
    Tv,
    Round,
    League_Division,
    Remark,
    Spread_HDP_OU,
    MB_WK_HDP,
    MB_WK_NO_HDP,
    MB_WD_HDP,
    MB_WD_NO_HDP,
    LST_Note,
    Remark2,
    Comment
  } = tableFilter;
  const result = dataSource.filter(e => //NOSONAR
    (!League_Group.isFilter || League_Group.selected.some(
      m => m.label === buildLabelItemOption(e.leagueGroup)
    )) &&
    (!League.isFilter || League.selected.some(m => m.label === buildLabelItemOption(e.league))) &&
    (!Tv.isFilter || Tv.selected.some(
      m =>
        m.label === buildLabelTvOption(e.condition.hasTv, e.condition.hasIpTv)
    )) &&
    (!Round.isFilter || Round.selected.some(m =>
      buildLabelItemForCondition(e.condition.round, m.label)
    )) &&
    (!League_Division.isFilter || League_Division.selected.some(m =>
      buildLabelItemForCondition(e.condition.division, m.label)
    )) &&
    (!Remark.isFilter || Remark.selected.some(
      m => m.label === buildLabelItemOption(e.condition.remark)
    )) &&
    (!Spread_HDP_OU.isFilter || Spread_HDP_OU.selected.some(
      m => m.label === buildLabelItemOption(e.spreadShow)
    )) &&
    (!MB_WD_HDP.isFilter || MB_WD_HDP.selected.some(
      m => m.label === buildLabelItemOption(e.maxbetWD_HDP_Show)
    )) &&
    (!MB_WD_NO_HDP.isFilter || MB_WD_NO_HDP.selected.some(
      m => m.label === buildLabelItemOption(e.wD_NoOfHDP_Show)
    )) &&
    (!MB_WK_HDP.isFilter || MB_WK_HDP.selected.some(
      m => m.label === buildLabelItemOption(e.maxbetWK_HDP_Show)
    )) &&
    (!MB_WK_NO_HDP.isFilter || MB_WK_NO_HDP.selected.some(
      m => m.label === buildLabelItemOption(e.wK_NoOfHDP_Show)
    )) &&
    (!LST_Note.isFilter || LST_Note.selected.some(
      m => m.label === buildLabelItemOption(e.lstNote)
    )) &&
    (!Remark2.isFilter || Remark2.selected.some(m => m.label === buildLabelItemOption(e.remark2))) &&
    (!Comment.isFilter || Comment.selected.some(m => m.label === buildLabelItemOption(e.comment)))
  );
  return result;
};
const buildLabelTvOption = (hasTv: boolean, hasIpTv: boolean) => {
  return hasIpTv && hasTv
    ? 'TV/IPTV'
    : hasIpTv
      ? 'IPTV'
      : hasTv
        ? 'TV'
        : '(Blanks)';
};
const buildLabelItemOption = (value: string | undefined | null) =>
  value?.trim().length ? value : '(Blanks)';

const buildLabelItemForCondition = (
  conditions: TCondition[],
  selected: string | number | null
) => {
  if (conditions.length === 0 && selected === '(Blanks)') {
    return true;
  }
  return conditions.some(x => buildValue(x.value, x.type) === selected);
};

const buildValue = (value: string, type: string) =>
  value.length > 0 ? (type === 'Contains' ? `*${value}*` : value) : null;
export { buildDefaultTableFilter, executeFilterTable };
