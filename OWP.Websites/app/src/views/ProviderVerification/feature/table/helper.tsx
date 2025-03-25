import { formatTime } from "../../../Shared/DateTime/dateTimeHelper";
import { Option } from "../../../Shared/MultipleSelection/type";
import { TDataSource, TMultipleFilter } from "../../type/table";
import helper from "./calculateFilterOption";

const buildDefaultTableFilter = (dataSource: TDataSource[], confirmProviderOption: Option[], tableFilter: TMultipleFilter) => {
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
  tableFilter.LstBookProvider = {
    selected: helper.LstBookProvider(dataSource),
    isFilter: false
  };
  tableFilter.InHouseProviderId = {
    selected: helper.InHouseProviderId(dataSource),
    isFilter: false
  };
  tableFilter.Remark = {
    selected: helper.Remark(dataSource),
    isFilter: false
  };
  tableFilter.RemarkBy = {
    selected: helper.RemarkBy(dataSource),
    isFilter: false
  };
  tableFilter.ConfirmProvider = {
    selected: [...confirmProviderOption, { value: -1, label: '(Blanks)' }],
    isFilter: false
  };
};

const executeFilterTable = (dataSource: TDataSource[], tableFilter: TMultipleFilter) => {
  const { MatchNo,
    MatchStatus,
    KoTime,
    League,
    Match,
    LstBookProvider,
    InHouseProviderId,
    Remark,
    RemarkBy
  } = tableFilter;
  const result = dataSource.filter(e => MatchNo.selected.some(m => m.label === buildLabelItemOption(e.matchNo)) //NOSONAR
    && MatchStatus.selected.some(m => m.label === buildLabelItemOption(e.matchStatus))
    && Match.selected.some(m => m.label === `${e.home} -vs- ${e.away}`)
    && KoTime.selected.some(m => m.label === formatTime(e.eventDateTime, 'minute', 'hh:mm A'))
    && League.selected.some(m => m.label === e.league)
    && LstBookProvider.selected.some(m => m.label === buildLabelItemOption(e.lstBookProvider))
    && InHouseProviderId.selected.some(m => m.label === buildLabelItemOption(e.inHouseProviderId))
    && Remark.selected.some(m => m.label === buildLabelItemOption(e.remark))
    && RemarkBy.selected.some(m => m.label === buildLabelItemOption(e.remarkBy)));

  return filterByConfirmProvider(result, tableFilter);
};

const filterByConfirmProvider = (dataSource: TDataSource[], tableFilter: TMultipleFilter) => {
  return dataSource.filter(e => {
    const ConfirmProvider = e.agentConfirm;
    const { selected } = tableFilter.ConfirmProvider;
    if (!ConfirmProvider.length && selected.some(s => s.label === '(Blanks)')) {
      return true;
    }
    return ConfirmProvider.some(a =>
      selected.some(s =>
        s.label === a.label
      ));
  });
};

const buildLabelItemOption = (value: string | undefined | null) => value?.trim().length ? value : '(Blanks)';


export { buildDefaultTableFilter, executeFilterTable, buildLabelItemOption };
