import { uniqBy } from "lodash";
import { formatTime } from "../../../Shared/DateTime/dateTimeHelper";
import { buildLabelItemOption } from "./helper";
import { TDataSource } from "../../type/table";
import store from "../../app/storeNative";
import { Option } from "../../../Shared/MultipleSelection/type";

const calculateMatchNoOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.owpMatchId,
    label: buildLabelItemOption(x.MatchNo)
  })), v => v.label));
};

const calculateKoOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource]
    .sort((first, second) =>
      Number(new Date(first.EventDateTime)) - Number(new Date(second.EventDateTime))
    )
    .map(x => ({
      value: x.OWPMatchId,
      label: formatTime(x.eventDateTime, 'minute', 'hh:mm A') ?? '(Blanks)'
    })), v => v.label));
};

const calculateLeagueOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: x.league ?? '(Blanks)'
  })), v => v.label));
};

const calculateMatchOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: `${x.home} -vs- ${x.away}`
  })), v => v.label));
};

const calculateLSTBookProviderOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: buildLabelItemOption(x.lstBookProvider)
  })), v => v.label));
};

const calculateProviderOption = (dataSource: TDataSource[]) => {
  return swapBlanks(uniqBy([...dataSource].map(x => ({
    value: x.OWPMatchId,
    label: buildLabelItemOption(x.inHouseProviderId)
  })), v => v.label));
};

const calculateConfirmProviderOption = () => {
  const { confirmProviderOption } = store.getState().events;
  return [...confirmProviderOption, { value: -1, label: '(Blanks)' }];
};

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
  KoTime: (dataSource: TDataSource[]) => calculateKoOption(dataSource),
  League: (dataSource: TDataSource[]) => calculateLeagueOption(dataSource),
  Match: (dataSource: TDataSource[]) => calculateMatchOption(dataSource),
  LstBookProvider: (dataSource: TDataSource[]) => calculateLSTBookProviderOption(dataSource),
  Provider: (dataSource: TDataSource[]) => calculateProviderOption(dataSource),
  ConfirmProvider: (dataSource: TDataSource[]) => calculateConfirmProviderOption()
};

export default helper;

