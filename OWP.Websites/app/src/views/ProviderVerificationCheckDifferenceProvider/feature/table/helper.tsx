import { formatTime } from "../../../Shared/DateTime/dateTimeHelper";
import { Option } from "../../../Shared/MultipleSelection/type";
import { TDataSource, TMultipleFilter } from "../../type/table";
import helper from "./calculateFilterOption";

const buildDefaultTableFilter = (dataSource: TDataSource[], confirmProviderOption: Option[], tableFilter: TMultipleFilter) => {
  tableFilter.MatchNo = {
    selected: helper.MatchNo(dataSource),
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
  tableFilter.LstBookProvider = {
    selected: helper.LstBookProvider(dataSource),
    isFilter: false
  };
  tableFilter.Provider = {
    selected: helper.Provider(dataSource),
    isFilter: false
  };
  tableFilter.ConfirmProvider = {
    selected: [...confirmProviderOption, { value: -1, label: '(Blanks)' }],
    isFilter: false
  };
};

const executeFilterTable = (dataSource: TDataSource[], tableFilter: TMultipleFilter) => {
  const { MatchNo,
    KoTime,
    League,
    Match,
    LstBookProvider,
    Provider
  } = tableFilter;
  const result = dataSource.filter(e => MatchNo.selected.some(m => m.label === buildLabelItemOption(e.MatchNo)) //NOSONAR
    && Match.selected.some(m => m.label === `${e.Home} -vs- ${e.Away}`)
    && KoTime.selected.some(m => m.label === formatTime(e.EventDateTime, 'minute', 'hh:mm A'))
    && League.selected.some(m => m.label === e.League)
    && LstBookProvider.selected.some(m => m.label === buildLabelItemOption(e.LstBookProvider))
    && Provider.selected.some(m => m.label === buildLabelItemOption(e.Provider)));

  return filterByConfirmProvider(result, tableFilter);
};

const filterByConfirmProvider = (dataSource: TDataSource[], tableFilter: TMultipleFilter) => {
  return dataSource.filter(e => {
    const ConfirmProvider = e.AgentConfirm;
    const { selected } = tableFilter.ConfirmProvider;
    if (!ConfirmProvider.length && selected.some(s => s.label === '(Blanks)')) {
      return true;
    }
    return ConfirmProvider.some(a =>
      selected.some(s =>
        s.label === a.Label
      ));
  });
};

const buildLabelItemOption = (value: string | undefined | null) => value?.trim().length ? value : '(Blanks)';


export { buildDefaultTableFilter, executeFilterTable, buildLabelItemOption };
