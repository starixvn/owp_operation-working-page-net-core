import { uniqBy } from "lodash";
import { formatTime } from "../../../Shared/DateTime/dateTimeHelper";
import { buildLabelItemOption, buildMaxbetItemInOption } from "./helper";
import { TDataSource } from "../../type/table";
import store from "../../app/storeNative";
import { Option } from "../../../Shared/MultipleSelection/type";

const calculateMatchNoOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: buildLabelItemOption(x.MatchNo)
  })), v => v.label));
};
const calculateMatchStatusOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: buildLabelItemOption(x.MatchStatus)
  })), v => v.label));
};
const calculateKoOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource]
    .sort((first, second) =>
      Number(new Date(first.EventDateTime)) - Number(new Date(second.EventDateTime))
    )
    .map(x => ({
      value: x.OWPMatchId,
      label: formatTime(x.EventDateTime, 'minute', 'hh:mm A') ?? '(Blanks)'
    })), v => v.label));
};
const calculateLeagueOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: x.League ?? '(Blanks)'
  })), v => v.label));
};
const calculateMatchOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: `${x.Home} -vs- ${x.Away}`
  })), v => v.label));
};
const calculateProviderOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: buildLabelItemOption(x.Provider)
  })), v => v.label));
};
const calculateOtherProviderOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: buildLabelItemOption(x.OtherProvider)
  })), v => v.label));
};
const calculateAgentOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.AgentEst ?? '',
    label: buildLabelItemOption(x.AgentEst)
  })), v => v.value));
};

const calculateMbOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource]
    .sort((first, second) => (first.HandicapSetting?.MaxBet ?? Number.MAX_SAFE_INTEGER) - (second.HandicapSetting?.MaxBet ?? Number.MAX_SAFE_INTEGER))
    .map(x => ({
      value: x.OWPMatchId,
      label: buildMaxbetItemInOption({ HandicapSetting: x.HandicapSetting, OverUnderSetting: x.OverUnderSetting }) ?? '(Blanks)'
    })), v => v.label));
};
const calculateRemarkOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: buildLabelItemOption(x.RemarkEst)
  })), v => v.label));
};
const calculateRemarkByOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: buildLabelItemOption(x.RemarkBy)
  })), v => v.label));
};

const calculateAgentConfirmOption = () => {
  const { agentConfirmOption } = store.getState().events;
  return [...agentConfirmOption, { value: -1, label: '(Blanks)' }];
};

const calculateEstNoteOption = (dataSource: TDataSource[]) => swapBlanks(uniqBy([...dataSource].map(x => ({
  value: x.EstNote ?? '',
  label: buildLabelItemOption(x.EstNote)
})), v => v.value));

const swapBlanks = (options: Option[]) => {
  const indexBlanks = options.findIndex(o => o.label === '(Blanks)');
  if (indexBlanks > -1) {
    const { length } = options;
    const temp = options[indexBlanks];
    options[indexBlanks] = options[length - 1];
    options[length - 1] = temp;
  }
  return options;
};

const helper = {
  MatchNo: (dataSource: TDataSource[]) => calculateMatchNoOption(dataSource),
  MatchStatus: (dataSource: TDataSource[]) => calculateMatchStatusOption(dataSource),
  KoTime: (dataSource: TDataSource[]) => calculateKoOption(dataSource),
  League: (dataSource: TDataSource[]) => calculateLeagueOption(dataSource),
  Match: (dataSource: TDataSource[]) => calculateMatchOption(dataSource),
  Provider: (dataSource: TDataSource[]) => calculateProviderOption(dataSource),
  AgentEst: (dataSource: TDataSource[]) => calculateAgentOption(dataSource),
  Mb: (dataSource: TDataSource[]) => calculateMbOption(dataSource),
  RemarkEst: (dataSource: TDataSource[]) => calculateRemarkOption(dataSource),
  RemarkBy: (dataSource: TDataSource[]) => calculateRemarkByOption(dataSource),
  AgentConfirm: (dataSource: TDataSource[]) => calculateAgentConfirmOption(),
  EstNote: (dataSource: TDataSource[]) => calculateEstNoteOption(dataSource),
  OtherProvider: (dataSource: TDataSource[]) => calculateOtherProviderOption(dataSource)
};

export default helper;

