import { formatTime } from "../../../Shared/DateTime/dateTimeHelper";
import { Option } from "../../../Shared/MultipleSelection/type";
import { TDataSource, TMaxbetSetting, TMultipleFilter } from "../../type/table";
import helper from "./calculateFilterOption";

const buildMaxbetItemInOption = (maxbetSetting: TMaxbetSetting) => {
  const { HandicapSetting, OverUnderSetting } = maxbetSetting;
  if (!HandicapSetting && !OverUnderSetting) {
    return '(Blanks)';
  }
  const isDisplayHDPSetting =
    HandicapSetting?.SpreadToDisplay ?? OverUnderSetting?.SpreadToDisplay;
  const isHDPSpreadExisting = HandicapSetting?.SpreadToDisplay;
  return `${HandicapSetting?.MaxBetToDisplay}${isDisplayHDPSetting ? '(' + HandicapSetting?.SpreadToDisplay : ''}
${OverUnderSetting?.SpreadToDisplay ? (isHDPSpreadExisting ? '-' : '') + OverUnderSetting.SpreadToDisplay
      : ''}
  ${isDisplayHDPSetting ? ')' : ''}`;
};

const buildDefaultTableFilter = (dataSource: TDataSource[], agentConfirmOption: Option[], tableFilter: TMultipleFilter) => {
  tableFilter.MatchNo = {
    selected: helper.MatchNo(dataSource),
    isFilter: false
  };
  tableFilter.MatchStatus = {
    selected: helper.MatchStatus(dataSource),
    isFilter: false
  };
  tableFilter.KoTime = {
    selected: helper.KoTime(dataSource),
    isFilter: false
  };
  tableFilter.League = {
    selected: helper.League(dataSource),
    isFilter: false
  };
  tableFilter.Match = {
    selected: helper.Match(dataSource),
    isFilter: false
  };
  tableFilter.Provider = {
    selected: helper.Provider(dataSource),
    isFilter: false
  };
  tableFilter.OtherProvider = {
    selected: helper.OtherProvider(dataSource),
    isFilter: false
  };
  tableFilter.AgentEst = {
    selected: helper.AgentEst(dataSource),
    isFilter: false
  };
  tableFilter.Mb = {
    selected: helper.Mb(dataSource),
    isFilter: false
  };
  tableFilter.RemarkEst = {
    selected: helper.RemarkEst(dataSource),
    isFilter: false
  };
  tableFilter.RemarkBy = {
    selected: helper.RemarkBy(dataSource),
    isFilter: false
  };
  tableFilter.AgentConfirm = {
    selected: [...agentConfirmOption, { value: -1, label: '(Blanks)' }],
    isFilter: false
  };
  tableFilter.EstNote = {
    selected: helper.EstNote(dataSource),
    isFilter: false
  };
};

const executeFilterTable = (dataSource: TDataSource[], tableFilter: TMultipleFilter) => {
  const { MatchNo,
    MatchStatus,
    KoTime,
    League,
    Match,
    Provider,
    OtherProvider,
    AgentEst,
    Mb,
    RemarkEst,
    RemarkBy,
    EstNote
  } = tableFilter;
  const result = dataSource.filter(e => MatchNo.selected.some(m => m.label === buildLabelItemOption(e.MatchNo))
    && MatchStatus.selected.some(m => m.label === buildLabelItemOption(e.MatchStatus))
    && Match.selected.some(m => m.label === `${e.Home} -vs- ${e.Away}`)
    && KoTime.selected.some(m => m.label === formatTime(e.EventDateTime, 'minute', 'hh:mm A'))
    && League.selected.some(m => m.label === e.League)
    && Provider.selected.some(m => m.label === buildLabelItemOption(e.Provider))
    && OtherProvider.selected.some(m => m.label === buildLabelItemOption(e.OtherProvider))
    && AgentEst.selected.some(m => m.label === buildLabelItemOption(e.AgentEst))
    && Mb.selected.some(m => m.label === (buildMaxbetItemInOption({ HandicapSetting: e.HandicapSetting, OverUnderSetting: e.OverUnderSetting }) ?? '(Blanks)'))
    && RemarkEst.selected.some(m => m.label === buildLabelItemOption(e.RemarkEst))
    && RemarkBy.selected.some(m => m.label === buildLabelItemOption(e.RemarkBy))
    && EstNote.selected.some(m => m.label === buildLabelItemOption(e.EstNote)));

  return filterByAgentConfirm(result, tableFilter);
};

const filterByAgentConfirm = (dataSource: TDataSource[], tableFilter: TMultipleFilter) => {
  return dataSource.filter(e => {
    const { AgentConfirm } = e;
    const { selected } = tableFilter.AgentConfirm;
    if (!AgentConfirm.length && selected.some(s => s.label === '(Blanks)')) {
      return true;
    }
    return AgentConfirm.some(a =>
      selected.some(s =>
        s.label === a.Label
      ));
  });
};

const buildLabelItemOption = (value: string | undefined | null) => value?.trim().length ? value : '(Blanks)';


export { buildMaxbetItemInOption, buildDefaultTableFilter, executeFilterTable, buildLabelItemOption };
