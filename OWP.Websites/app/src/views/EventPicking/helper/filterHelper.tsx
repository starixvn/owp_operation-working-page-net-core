import { netCoreURLEventPicking } from '../const/apiUrl';
import { uniqBy } from 'lodash';
import { formatTime } from '../../Shared/DateTime/dateTimeHelper';
import Utils, { filterDataTable } from '../../Shared/Utils';
import { COLUMN_KEY } from '../table/TableSchema.json';
import { Option } from '../../Shared/MultipleSelection/type';
import TDataTable, {
  TColumnKey,
  TMultipleFilter,
  TTableSlice,
  TMultipleFilterPipe,
  TMultipleFilterKey,
  TSavedMultipleFilter
} from '../types/dataTableType';
import { pipe } from 'lodash/fp';
import { NONE_OPTION } from '../filterArea/filterConst.json';

const KO_TIME = COLUMN_KEY.KO_TIME;
const MB = COLUMN_KEY.MB;

const currentDate = new Date();
const timeRange = {
  maxFeature: 10,
  maxPrevious: 20
};
const maxDate = new Date(
  currentDate.setDate(currentDate.getDate() + timeRange.maxFeature)
);
const minDate = new Date(
  currentDate.setDate(currentDate.getDate() - timeRange.maxPrevious)
);
const buildUrl = (methodRoute: Required<string>) =>
  `${netCoreURLEventPicking}/${methodRoute}`;

const handleCheckButton = (leagueName: string, isChildMatch: boolean) => {
  let url = `LeagueManagement?leagueName=${leagueName}&isEqualLeague=true`;
  if (isChildMatch) {
    url = `LeagueManagement?tab=maxbet-setting`;
  }
  window.open(url, '_blank');
};

const buildKoOptions = (entities: TDataTable[]) =>
  uniqBy(
    [...entities]
      .sort(
        (first, second) =>
          Number(new Date(first.eventDateTime)) -
          Number(new Date(second.eventDateTime))
      )
      .map(m => formatTime(m.eventDateTime, 'minute', 'hh:mm A'))
      .map(o => ({
        value: o,
        label: o ?? ''
      })),
    i => i.value
  );

const buildMbOptions = (entities: TDataTable[]) =>
  uniqBy(
    [...entities]
      .sort(
        (first, second) =>
          (first.maxbetHdp ?? Number.MAX_SAFE_INTEGER) -
          (second.maxbetHdp ?? Number.MAX_SAFE_INTEGER)
      )
      .map(o => ({
        value: o.maxbetHdp || null,
        label: Utils.convertThousandToK(o.maxbetHdp, true) || '(Blanks)'
      })),
    i => i.value
  );

const buildMbOuOptions = (entities: TDataTable[]) =>
  uniqBy(
    [...entities]
      .sort(
        (first, second) =>
          (first.maxbetOu ?? Number.MAX_SAFE_INTEGER) -
          (second.maxbetOu ?? Number.MAX_SAFE_INTEGER)
      )
      .map(o => ({
        value: o.maxbetOu || null,
        label: Utils.convertThousandToK(o.maxbetOu, true) || '(Blanks)'
      })),
    i => i.value
  );

const buildSpreadOptions = (
  entities: TDataTable[],
  property: 'spreadOu' | 'spreadHdp'
) =>
  uniqBy(
    [...entities]
      .sort(
        (first, second) =>
          (first[property] ?? Number.MAX_SAFE_INTEGER) -
          (second[property] ?? Number.MAX_SAFE_INTEGER)
      )
      .map(o => ({
        value: o[property] || null,
        label: Utils.convertSpreadToPercent(o[property], true) || '(Blanks)'
      })),
    i => i.value
  );

const buildLeagueOptions = (entities: TDataTable[]) =>
  uniqBy(
    [...entities]
      .map(o => ({
        value: o.leagueName,
        label: o.leagueName
      }))
      .sort((a, b) => a.value.localeCompare(b.value)),
    i => i.value
  );

const buildNoOfHdpOptions = (entities: TDataTable[]) =>
  uniqBy(
    [...entities]
      .map((o, index) => ({
        index: o.noOfHDP ? index : Number.MAX_SAFE_INTEGER,
        value: o.noOfHDP || null,
        label: o.noOfHDP || '(Blanks)'
      }))
      .sort((a, b) => {
        if (a.label === '(Blanks)') {
          return 1;
        }
        if (b.label === '(Blanks)') {
          return -1;
        }
        return a.label.localeCompare(b.label, undefined, { numeric: true });
      }),
    i => i.label
  );

const buildLstNoteOptions = (entities: TDataTable[]) =>
  uniqBy(
    [...entities]
      .map(o => ({
        value: o.lstNote || null,
        label: o.lstNote || '(Blanks)'
      }))
      .sort((a, b) => {
        if (a.label === '(Blanks)') {
          return 1;
        }
        if (b.label === '(Blanks)') {
          return -1;
        }

        return a.label.localeCompare(b.label);
      }),
    i => i.label
  );

const buildDefaultRAndDInfoOptions = (updateTypeOption: Option[]) =>
  updateTypeOption
    .map(type => ({
      value: type.value || null,
      label: type.label
    }))
    .concat(NONE_OPTION);

const buildRAndDInfoOptions = (
  entities: TDataTable[],
  rAndDInfoOriginalOption: Option[]
) => {
  const allRAndDInfo = entities
    .map(entity =>
      entity.rAndDInfo?.length ? entity.rAndDInfo : [{ group: null }]
    )
    .flat(1) as Array<{ group: string | null }>;

  const rAndDInfo = uniqBy(allRAndDInfo, i => i.group);

  return rAndDInfoOriginalOption.filter(o =>
    rAndDInfo.some(r => r.group === o.value)
  );
};

const buildTurnoverPeriodOptions = (entities: TDataTable[]) => {
  const allPeriods = entities.flatMap(entity =>
    Array.isArray(entity.turnoverPeriod)
      ? entity.turnoverPeriod.map(period => ({
          value: period.value,
          label: String(period.valueDisplay || '(Blanks)')
        }))
      : []
  );
  const periods = uniqBy(allPeriods, i => i.value);

  return periods.sort((a, b) => {
    if (a.value === null && b.value === null) {
      return 0;
    }
    if (a.value === null) {
      return 1;
    }
    if (b.value === null) {
      return -1;
    }
    return a.value - b.value;
  });
};

const buildMarginPeriodOptions = (entities: TDataTable[]) => {
  const allPeriods = entities.flatMap(entity =>
    Array.isArray(entity.marginPeriod)
      ? entity.marginPeriod.map(period => ({
          value: period.value,
          label: String(period.value || '(Blanks)')
        }))
      : []
  );
  const periods = uniqBy(allPeriods, i => i.value);

  return periods.sort((a, b) => {
    if (a.value === null && b.value === null) {
      return 0;
    }
    if (a.value === null) {
      return 1;
    }
    if (b.value === null) {
      return -1;
    }
    return a.value - b.value;
  });
};

const buildRoundOptions = (entities: TDataTable[]) => {
  const allRoundValues = entities.flatMap(entity => {
    const { round } = entity;

    return [round];
  });

  const options = uniqBy(
    allRoundValues
      .map(round => ({
        value: round.trim(),
        label: round.trim() || '(Blanks)'
      }))
      .sort((a, b) => {
        if (a.label === '(Blanks)') {
          return 1;
        }
        if (b.label === '(Blanks)') {
          return -1;
        }
        return a.label.localeCompare(b.label);
      }),
    i => i.value
  );

  return options;
};

const buildProviderOptions = (entities: TDataTable[]) => {
  const allOtherProviderValues = entities.flatMap(entity => {
    const { otherProvider } = entity;
    const otherProviderValues = otherProvider?.split('/').filter(Boolean) || [];

    return [...otherProviderValues];
  });

  const options = uniqBy(
    allOtherProviderValues
      .map(otherProvider => ({
        value: otherProvider.trim(),
        label: otherProvider.trim() || '(Blanks)'
      }))
      .sort((a, b) => a.label.localeCompare(b.label)),
    i => i.value
  );

  options.push({ value: null as unknown as string, label: '(Blanks)' });

  return options;
};

const buildTvOptions = (entities: TDataTable[]) => {
  const allTvValues = entities.flatMap(entity => {
    const { tv, iptv } = entity;
    const tvValues = tv?.split('/').filter(Boolean) || [];
    const iptvValues = iptv?.split('/').filter(Boolean) || [];

    return [...tvValues, ...iptvValues];
  });

  const options = uniqBy(
    allTvValues
      .map(tv => ({
        value: tv.trim(),
        label: tv.trim() || '(Blanks)'
      }))
      .sort((a, b) => a.label.localeCompare(b.label)),
    i => i.value
  );

  options.push({ value: null as unknown as string, label: '(Blanks)' });

  return options;
};

const filterTableExceptColumn = (
  entities: TDataTable[],
  exceptColumnKey: TColumnKey,
  multipleFilter: TMultipleFilter,
  hasReadPermission: boolean
) => {
  return pipe(
    (props: TMultipleFilterPipe) =>
      props.filterInfo.time?.isFilter && exceptColumnKey !== COLUMN_KEY.KO_TIME
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.KO_TIME as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.leagueName?.isFilter &&
      exceptColumnKey !== COLUMN_KEY.LEAGUE
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.LEAGUE as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.maxbetHdp?.isFilter && exceptColumnKey !== COLUMN_KEY.MB
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.MB as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.maxbetOu?.isFilter &&
      exceptColumnKey !== COLUMN_KEY.MB_OU
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.MB_OU as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.spreadHdp?.isFilter &&
      exceptColumnKey !== COLUMN_KEY.SPREAD_HDP
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.SPREAD_HDP as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.spreadOu?.isFilter &&
      exceptColumnKey !== COLUMN_KEY.SPREAD_OU
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.SPREAD_OU as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.lstNote?.isFilter &&
      exceptColumnKey !== COLUMN_KEY.LST_NOTE
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.LST_NOTE as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.turnoverPeriod?.isFilter &&
      exceptColumnKey !== COLUMN_KEY.TURNOVER_PERIOD
        ? filterByTurnoverPeriod(
            props.entities,
            props.filterInfo,
            hasReadPermission
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.marginPeriod?.isFilter &&
      exceptColumnKey !== COLUMN_KEY.MARGIN_PERIOD
        ? filterByMarginPeriod(
            props.entities,
            props.filterInfo,
            hasReadPermission
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.noOfHDP?.isFilter &&
      exceptColumnKey !== COLUMN_KEY['NO.OF_HDP']
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY['NO.OF_HDP'] as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.tv?.isFilter && exceptColumnKey !== COLUMN_KEY.TV
        ? filterByTv(props.entities, props.filterInfo)
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.round?.isFilter && exceptColumnKey !== COLUMN_KEY.ROUND
        ? filterByRound(props.entities, props.filterInfo)
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.provider?.isFilter &&
      exceptColumnKey !== COLUMN_KEY.PROVIDER
        ? filterByProvider(props.entities, props.filterInfo)
        : props,
    (props: TMultipleFilterPipe) => {
      if (
        props.filterInfo.rAndDInfo?.isFilter &&
        exceptColumnKey !== COLUMN_KEY.RAndD &&
        props.filterInfo.rAndDInfo.selected
      ) {
        return filterByRAndD(
          props.entities,
          props.filterInfo.rAndDInfo.selected.map(s => s.value)
        );
      }
      return props.entities;
    }
  )({
    entities,
    filterInfo: multipleFilter
  });
};

const updateMultipleFilterOptionsAndSelected = (
  state: TTableSlice,
  hasReadPermission: boolean = false,
  filterByKeyword: any
) => {
  // Build options cho tất cả cột bằng cách filter data trừ từng cột ra
  const options = buildTableMultipleFilterOptionsExceptColumn(
    state.entitiesOriginal,
    state.multipleFilter,
    state.rAndDInfoFilterOriginal.options,
    hasReadPermission,
    state.filterState,
    filterByKeyword
  );

  // Update options cho tất cả cột
  state.multipleFilter.leagueName.options = options.leagueName;
  state.multipleFilter.time.options = options.time;
  state.multipleFilter.maxbetHdp.options = options.maxbetHdp;
  state.multipleFilter.maxbetOu.options = options.maxbetOu;
  state.multipleFilter.spreadHdp.options = options.spreadHdp;
  state.multipleFilter.spreadOu.options = options.spreadOu;
  state.multipleFilter.noOfHDP.options = options.noOfHDP;
  state.multipleFilter.lstNote.options = options.lstNote;
  state.multipleFilter.rAndDInfo.options = options.rAndDInfo;
  state.multipleFilter.tv.options = options.tv;
  state.multipleFilter.provider.options = options.provider;
  state.multipleFilter.round.options = options.round;
  state.multipleFilter.turnoverPeriod.options = options.turnoverPeriod;
  state.multipleFilter.marginPeriod.options = options.marginPeriod;

  // Update selected values theo Excel behavior:
  // - Cột được clear: set selected = all options (để user có thể filter lại)
  // - Các cột khác: giữ nguyên selected nếu đang active filter, hoặc set = all options nếu không active
  state.multipleFilter.leagueName.selected = state.multipleFilter.leagueName
    .isFilter
    ? state.multipleFilter.leagueName.selected
    : options.leagueName;
  state.multipleFilter.time.selected = state.multipleFilter.time.isFilter
    ? state.multipleFilter.time.selected
    : options.time;
  state.multipleFilter.maxbetHdp.selected = state.multipleFilter.maxbetHdp
    .isFilter
    ? state.multipleFilter.maxbetHdp.selected
    : options.maxbetHdp;
  state.multipleFilter.maxbetOu.selected = state.multipleFilter.maxbetOu
    .isFilter
    ? state.multipleFilter.maxbetOu.selected
    : options.maxbetOu;
  state.multipleFilter.spreadHdp.selected = state.multipleFilter.spreadHdp
    .isFilter
    ? state.multipleFilter.spreadHdp.selected
    : options.spreadHdp;
  state.multipleFilter.spreadOu.selected = state.multipleFilter.spreadOu
    .isFilter
    ? state.multipleFilter.spreadOu.selected
    : options.spreadOu;
  state.multipleFilter.noOfHDP.selected = state.multipleFilter.noOfHDP.isFilter
    ? state.multipleFilter.noOfHDP.selected
    : options.noOfHDP;
  state.multipleFilter.lstNote.selected = state.multipleFilter.lstNote.isFilter
    ? state.multipleFilter.lstNote.selected
    : options.lstNote;
  state.multipleFilter.rAndDInfo.selected = state.multipleFilter.rAndDInfo
    .isFilter
    ? state.multipleFilter.rAndDInfo.selected
    : options.rAndDInfo;
  state.multipleFilter.tv.selected = state.multipleFilter.tv.isFilter
    ? state.multipleFilter.tv.selected
    : options.tv;
  state.multipleFilter.provider.selected = state.multipleFilter.provider
    .isFilter
    ? state.multipleFilter.provider.selected
    : options.provider;
  state.multipleFilter.round.selected = state.multipleFilter.round.isFilter
    ? state.multipleFilter.round.selected
    : options.round;
  state.multipleFilter.turnoverPeriod.selected = state.multipleFilter
    .turnoverPeriod.isFilter
    ? state.multipleFilter.turnoverPeriod.selected
    : options.turnoverPeriod;
  state.multipleFilter.marginPeriod.selected = state.multipleFilter.marginPeriod
    .isFilter
    ? state.multipleFilter.marginPeriod.selected
    : options.marginPeriod;
};

const buildTableMultipleFilterOptionsExceptColumn = (
  entitiesOriginal: TDataTable[],
  multipleFilter: TMultipleFilter,
  rAndDInfoOriginalOption: Option[],
  hasReadPermission: boolean = false,
  filterState: any,
  filterByKeyword: any
) => {
  const columnsKeyArray = [
    COLUMN_KEY.KO_TIME,
    COLUMN_KEY.LEAGUE,
    COLUMN_KEY.MB,
    COLUMN_KEY.MB_OU,
    COLUMN_KEY.SPREAD_HDP,
    COLUMN_KEY.SPREAD_OU,
    COLUMN_KEY['NO.OF_HDP'],
    COLUMN_KEY.LST_NOTE,
    COLUMN_KEY.TURNOVER_PERIOD,
    COLUMN_KEY.MARGIN_PERIOD,
    COLUMN_KEY.RAndD,
    COLUMN_KEY.TV,
    COLUMN_KEY.ROUND,
    COLUMN_KEY.PROVIDER
  ];

  const result = {
    time: [] as Option[],
    maxbetHdp: [] as Option[],
    maxbetOu: [] as Option[],
    leagueName: [] as Option[],
    spreadHdp: [] as Option[],
    spreadOu: [] as Option[],
    noOfHDP: [] as Option[],
    lstNote: [] as Option[],
    rAndDInfo: [] as Option[],
    turnoverPeriod: [] as Option[],
    marginPeriod: [] as Option[],
    tv: [] as Option[],
    round: [] as Option[],
    provider: [] as Option[]
  };

  // Filter data by keyword filter first
  const entitiesFilteredByKeyword = filterDataTable(
    entitiesOriginal,
    filterState,
    filterByKeyword
  );

  // Build options for each column by filtering data excluding that column
  for (const columnKey of columnsKeyArray) {
    const filteredData = filterTableExceptColumn(
      entitiesFilteredByKeyword,
      columnKey as TColumnKey,
      multipleFilter,
      hasReadPermission
    );

    const options = buildTableMultipleFilterOptions(
      filteredData,
      rAndDInfoOriginalOption
    );

    // Map column key to result property
    switch (columnKey) {
      case COLUMN_KEY.KO_TIME:
        result.time = options.time;
        break;
      case COLUMN_KEY.LEAGUE:
        result.leagueName = options.leagueName;
        break;
      case COLUMN_KEY.MB:
        result.maxbetHdp = options.maxbetHdp;
        break;
      case COLUMN_KEY.MB_OU:
        result.maxbetOu = options.maxbetOu;
        break;
      case COLUMN_KEY.SPREAD_HDP:
        result.spreadHdp = options.spreadHdp;
        break;
      case COLUMN_KEY.SPREAD_OU:
        result.spreadOu = options.spreadOu;
        break;
      case COLUMN_KEY['NO.OF_HDP']:
        result.noOfHDP = options.noOfHDP;
        break;
      case COLUMN_KEY.LST_NOTE:
        result.lstNote = options.lstNote;
        break;
      case COLUMN_KEY.TURNOVER_PERIOD:
        result.turnoverPeriod = options.turnoverPeriod;
        break;
      case COLUMN_KEY.MARGIN_PERIOD:
        result.marginPeriod = options.marginPeriod;
        break;
      case COLUMN_KEY.RAndD:
        result.rAndDInfo = options.rAndDInfo;
        break;
      case COLUMN_KEY.TV:
        result.tv = options.tv;
        break;
      case COLUMN_KEY.ROUND:
        result.round = options.round;
        break;
      case COLUMN_KEY.PROVIDER:
        result.provider = options.provider;
        break;
    }
  }

  return result;
};

const singleColumnFilterOptionsFactory = (
  columnKey: TColumnKey,
  entities: TDataTable[],
  rAndDInfoOriginalOption?: Option[]
) => {
  switch (columnKey) {
    case KO_TIME:
      return buildKoOptions(entities);
    case MB:
      return buildMbOptions(entities);
    case COLUMN_KEY.MB_OU:
      return buildMbOuOptions(entities);
    case COLUMN_KEY.LEAGUE:
      return buildLeagueOptions(entities);
    case COLUMN_KEY.SPREAD_HDP:
      return buildSpreadOptions(entities, 'spreadHdp');
    case COLUMN_KEY.SPREAD_OU:
      return buildSpreadOptions(entities, 'spreadOu');
    case COLUMN_KEY['NO.OF_HDP']:
      return buildNoOfHdpOptions(entities);
    case COLUMN_KEY.LST_NOTE:
      return buildLstNoteOptions(entities);
    case COLUMN_KEY.TURNOVER_PERIOD:
      return buildTurnoverPeriodOptions(entities);
    case COLUMN_KEY.MARGIN_PERIOD:
      return buildMarginPeriodOptions(entities);
    case COLUMN_KEY.RAndD:
      return buildRAndDInfoOptions(entities, rAndDInfoOriginalOption || []);
    default:
      return [];
  }
};

const buildTableMultipleFilterOptions = (
  entities: TDataTable[],
  rAndDInfoOriginalOption: Option[]
) => {
  const koTimeOptions = buildKoOptions(entities);
  const mbOptions = buildMbOptions(entities);
  const mbOuOptions = buildMbOuOptions(entities);
  const leagueOptions = buildLeagueOptions(entities);
  const spreadHdpOptions = buildSpreadOptions(entities, 'spreadHdp');
  const spreadOuOptions = buildSpreadOptions(entities, 'spreadOu');
  const noOfHdpOptions = buildNoOfHdpOptions(entities);
  const lstNoteOptions = buildLstNoteOptions(entities);
  const rAndDInfoOptions = buildRAndDInfoOptions(
    entities,
    rAndDInfoOriginalOption
  );
  const turnoverPeriodOptions = buildTurnoverPeriodOptions(entities);
  const marginPeriodOptions = buildMarginPeriodOptions(entities);
  const tvOptions = buildTvOptions(entities);
  const roundOptions = buildRoundOptions(entities);
  const providerOptions = buildProviderOptions(entities);

  return {
    time: koTimeOptions,
    maxbetHdp: mbOptions,
    maxbetOu: mbOuOptions,
    leagueName: leagueOptions,
    spreadHdp: spreadHdpOptions,
    spreadOu: spreadOuOptions,
    noOfHDP: noOfHdpOptions,
    lstNote: lstNoteOptions,
    rAndDInfo: rAndDInfoOptions,
    turnoverPeriod: turnoverPeriodOptions,
    marginPeriod: marginPeriodOptions,
    tv: tvOptions,
    round: roundOptions,
    provider: providerOptions
  };
};

const buildMultipleFilterDefault = (entities: TDataTable[]) => {
  const koTimeOptions = buildKoOptions(entities);
  const mbOptions = buildMbOptions(entities);
  const mbOuOptions = buildMbOuOptions(entities);
  const leagueOptions = buildLeagueOptions(entities);
  const spreadHdpOptions = buildSpreadOptions(entities, 'spreadHdp');
  const spreadOuOptions = buildSpreadOptions(entities, 'spreadOu');
  const noOfHdpOptions = buildNoOfHdpOptions(entities);
  const lstNoteOptions = buildLstNoteOptions(entities);
  const turnoverPeriodOptions = buildTurnoverPeriodOptions(entities);
  const marginPeriodOptions = buildMarginPeriodOptions(entities);
  const tvOptions = buildTvOptions(entities);
  const roundOptions = buildRoundOptions(entities);
  const providerOptions = buildProviderOptions(entities);
  return {
    [KO_TIME]: {
      isFilter: false,
      selected: koTimeOptions,
      options: koTimeOptions
    },
    [MB]: {
      isFilter: false,
      selected: mbOptions,
      options: mbOptions
    },
    [COLUMN_KEY.MB_OU]: {
      isFilter: false,
      selected: mbOuOptions,
      options: mbOuOptions
    },
    [COLUMN_KEY.LEAGUE]: {
      isFilter: false,
      selected: leagueOptions,
      options: leagueOptions
    },
    [COLUMN_KEY.SPREAD_HDP]: {
      isFilter: false,
      selected: spreadHdpOptions,
      options: spreadHdpOptions
    },
    [COLUMN_KEY.SPREAD_OU]: {
      isFilter: false,
      selected: spreadOuOptions,
      options: spreadOuOptions
    },
    [COLUMN_KEY['NO.OF_HDP']]: {
      isFilter: false,
      selected: noOfHdpOptions,
      options: noOfHdpOptions
    },
    [COLUMN_KEY.LST_NOTE]: {
      isFilter: false,
      selected: lstNoteOptions,
      options: lstNoteOptions
    },
    [COLUMN_KEY.TURNOVER_PERIOD]: {
      isFilter: false,
      selected: turnoverPeriodOptions,
      options: turnoverPeriodOptions
    },
    [COLUMN_KEY.MARGIN_PERIOD]: {
      isFilter: false,
      selected: marginPeriodOptions,
      options: marginPeriodOptions
    },
    [COLUMN_KEY.TV]: {
      isFilter: false,
      selected: tvOptions,
      options: tvOptions
    },
    [COLUMN_KEY.ROUND]: {
      isFilter: false,
      selected: roundOptions,
      options: roundOptions
    },
    [COLUMN_KEY.PROVIDER]: {
      isFilter: false,
      selected: providerOptions,
      options: providerOptions
    }
  } as TMultipleFilter;
};

const multipleFilterCommon = (
  entities: TDataTable[],
  multipleFilter: TMultipleFilter,
  filterProps: TColumnKey
) => {
  if (!multipleFilter || !multipleFilter[filterProps]) {
    return {
      multipleFilter,
      entities
    };
  }
  const selectedValue = multipleFilter[filterProps].selected.map(x => x.value);
  if (selectedValue.length === 0) {
    return {
      multipleFilter,
      entities
    };
  }

  return {
    filterInfo: multipleFilter,
    entities: entities.filter(e =>
      selectedValue.some(s => e[filterProps] === s || (!e[filterProps] && !s))
    )
  };
};

const filterByRAndD = (
  entities: TDataTable[],
  selectedValue: (number | string | null)[]
) => {
  return entities.filter(
    e =>
      e.rAndDInfo?.some(r => selectedValue?.some(s => s === r.group)) ||
      (!e.rAndDInfo?.length && selectedValue?.some(s => !s))
  );
};

const filterByTurnoverPeriod = (
  entities: TDataTable[],
  filterInfo: TMultipleFilter,
  hasReadPermission: boolean = true
) => {
  if (!hasReadPermission) {
    return {
      filterInfo,
      entities
    };
  }
  if (
    !filterInfo ||
    !filterInfo.turnoverPeriod ||
    !filterInfo.turnoverPeriod.selected
  ) {
    return {
      filterInfo,
      entities
    };
  }
  const selectedValue = filterInfo.turnoverPeriod.selected.map(s => s.value);
  if (!selectedValue) {
    return {
      filterInfo,
      entities
    };
  }
  return {
    filterInfo,
    entities: entities.filter(
      e =>
        e.turnoverPeriod?.some(period =>
          selectedValue.some(s => s === period.value)
        ) || false
    )
  };
};

const filterByMarginPeriod = (
  entities: TDataTable[],
  filterInfo: TMultipleFilter,
  hasReadPermission: boolean = true
) => {
  if (!hasReadPermission) {
    return {
      filterInfo,
      entities
    };
  }
  if (
    !filterInfo ||
    !filterInfo.marginPeriod ||
    !filterInfo.marginPeriod.selected
  ) {
    return {
      filterInfo,
      entities
    };
  }
  const selectedValue = filterInfo.marginPeriod.selected.map(s => s.value);

  if (!selectedValue) {
    return {
      filterInfo,
      entities
    };
  }
  return {
    filterInfo,
    entities: entities.filter(
      e =>
        e.marginPeriod?.some(period =>
          selectedValue.some(s => s === period.value)
        ) || false
    )
  };
};

const filterByTv = (entities: TDataTable[], filterInfo: TMultipleFilter) => {
  if (!filterInfo || !filterInfo.tv || !filterInfo.tv.selected) {
    return {
      filterInfo,
      entities
    };
  }

  const selected = filterInfo.tv.selected.map(s => s.value);

  if (selected.length === 0) {
    return {
      filterInfo,
      entities
    };
  }
  const selectedSet = new Set(selected.filter(Boolean));

  const filtered = entities.filter(e => {
    const all = [e.tv, e.iptv].filter(Boolean).flatMap(v =>
      v
        .split('/')
        .map(s => s.trim())
        .filter(Boolean)
    );

    const isBlankSelected = selected.includes(null);
    const isEmpty = all.length === 0;

    if (isBlankSelected && isEmpty) {
      return true;
    }

    return all.some(tv => selectedSet.has(tv));
  });

  return { filterInfo, entities: filtered };
};

const filterByRound = (entities: TDataTable[], filterInfo: TMultipleFilter) => {
  if (!filterInfo || !filterInfo.round || !filterInfo.round.selected) {
    return {
      filterInfo,
      entities
    };
  }

  const selected = filterInfo.round.selected.map(s => s.value);

  if (selected.length === 0) {
    return {
      filterInfo,
      entities
    };
  }
  return {
    filterInfo,
    entities: entities.filter(e => selected.some(s => s === e.round))
  };
};

const filterByProvider = (
  entities: TDataTable[],
  filterInfo: TMultipleFilter
) => {
  //user change requirement to filter by other provider
  if (!filterInfo || !filterInfo.provider || !filterInfo.provider.selected) {
    return {
      filterInfo,
      entities
    };
  }

  const selected = filterInfo.provider.selected.map(s => s.value);

  if (selected.length === 0) {
    return {
      filterInfo,
      entities
    };
  }
  const selectedSet = new Set(selected.filter(Boolean));
  const filtered = entities.filter(e => {
    //change here to apply filter by other provider
    const all = [e.otherProvider].filter(Boolean).flatMap(v =>
      v
        .split('/')
        .map(s => s.trim())
        .filter(Boolean)
    );

    const isBlankSelected = selected.includes(null);
    const isEmpty = all.length === 0;

    if (isBlankSelected && isEmpty) {
      return true;
    }

    return all.some(otherProvider => selectedSet.has(otherProvider));
  });
  return { filterInfo, entities: filtered };
};

/**
 * Preserves and restores multipleFilter state when loading new data from server
 * This ensures user's filter selections are maintained across data refreshes
 * @param currentMultipleFilter - Current multipleFilter state
 * @param newFilteredMatches - New data from server
 * @param rAndDInfoFilterOriginal - Original R&D Info filter options
 * @returns Restored multipleFilter with preserved user selections
 */
const preserveAndRestoreMultipleFilter = (
  currentMultipleFilter: TMultipleFilter,
  newFilteredMatches: TDataTable[],
  rAndDInfoFilterOriginal: {
    isFilter: boolean;
    selected: Option[];
    options: Option[];
  }
): TMultipleFilter => {
  const newFilterOptions = buildTableMultipleFilterOptions(
    newFilteredMatches,
    rAndDInfoFilterOriginal.options
  );
  const newMultipleFilter = { ...currentMultipleFilter };

  // Iterate through each key in newMultipleFilter
  Object.keys(newMultipleFilter).forEach(key => {
    const currentFilter = newMultipleFilter[key as keyof TMultipleFilter];

    if (!currentFilter.isFilter && key !== 'rAndDInfo') {
      // If isFilter is false, update the filter object
      const updatedOptions =
        newFilterOptions[key as keyof typeof newFilterOptions] || [];

      newMultipleFilter[key as keyof TMultipleFilter] = {
        isFilter: false,
        options: updatedOptions,
        selected: updatedOptions // Select all options
      };
    }
    // If isFilter is true, keep the current object unchanged
  });

  return newMultipleFilter;
};

const executeFilterTable = (
  state: TTableSlice,
  filterByKeyword: (
    arraySource: any, //NOSONAR
    { keyword, columnKey }: { keyword: undefined; columnKey: undefined }
  ) => any, //NOSONAR
  hasReadPermission: boolean = true
) => {
  state.entities = filterDataTable(
    state.entitiesOriginal,
    state.filterState,
    filterByKeyword
  );

  return pipe(
    (props: TMultipleFilterPipe) =>
      props.filterInfo.time?.isFilter
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.KO_TIME as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.leagueName?.isFilter
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.LEAGUE as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.maxbetHdp?.isFilter
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.MB as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.maxbetOu?.isFilter
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.MB_OU as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.spreadHdp?.isFilter
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.SPREAD_HDP as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.spreadOu?.isFilter
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.SPREAD_OU as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.lstNote?.isFilter
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY.LST_NOTE as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.turnoverPeriod?.isFilter
        ? filterByTurnoverPeriod(
            props.entities,
            props.filterInfo,
            hasReadPermission
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.marginPeriod?.isFilter
        ? filterByMarginPeriod(
            props.entities,
            props.filterInfo,
            hasReadPermission
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.noOfHDP?.isFilter
        ? multipleFilterCommon(
            props.entities,
            props.filterInfo,
            COLUMN_KEY['NO.OF_HDP'] as TColumnKey
          )
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.tv?.isFilter
        ? filterByTv(props.entities, props.filterInfo)
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.round?.isFilter
        ? filterByRound(props.entities, props.filterInfo)
        : props,
    (props: TMultipleFilterPipe) =>
      props.filterInfo.provider?.isFilter
        ? filterByProvider(props.entities, props.filterInfo)
        : props,
    (props: TMultipleFilterPipe) => {
      if (
        !props.filterInfo ||
        !props.filterInfo.rAndDInfo ||
        !props.filterInfo.rAndDInfo.isFilter ||
        !props.filterInfo.rAndDInfo.selected
      ) {
        return props.entities;
      }
      return filterByRAndD(
        props.entities,
        props.filterInfo.rAndDInfo.selected.map(s => s.value)
      );
    }
  )({
    entities: state.entities,
    filterInfo: state.multipleFilter
  });
};

export {
  maxDate,
  minDate,
  buildUrl,
  handleCheckButton,
  buildMultipleFilterDefault,
  executeFilterTable,
  buildTableMultipleFilterOptions,
  buildTableMultipleFilterOptionsExceptColumn,
  updateMultipleFilterOptionsAndSelected,
  buildDefaultRAndDInfoOptions,
  preserveAndRestoreMultipleFilter,
  singleColumnFilterOptionsFactory,
  filterTableExceptColumn
};
