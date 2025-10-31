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
        label: o.lstNote?.trim() || '(Blanks)'
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

const singleColumnFilterOptionsFactory = (
  columnKey: TColumnKey,
  entities: TDataTable[],
  rAndDInfoOriginalOption?: Option[]) => {
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
    marginPeriod: marginPeriodOptions
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
  if (!filterInfo.turnoverPeriod || !filterInfo.turnoverPeriod.selected) {
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
  if (!filterInfo.marginPeriod || !filterInfo.marginPeriod.selected) {
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
  // Step 1: Save current filter state (isFilter and selected)
  const savedMultipleFilter: TSavedMultipleFilter = {};
  (Object.keys(currentMultipleFilter) as TMultipleFilterKey[]).forEach(key => {
    savedMultipleFilter[key] = {
      isFilter: currentMultipleFilter[key].isFilter,
      selected: currentMultipleFilter[key].selected
    };
  });

  // Step 2: Build new filter options from new data
  const newFilterOptions = buildMultipleFilterDefault(newFilteredMatches);

  // Step 3: Restore filter state
  (Object.keys(savedMultipleFilter) as TMultipleFilterKey[]).forEach(key => {
    if (newFilterOptions[key] && savedMultipleFilter[key]) {
      newFilterOptions[key] = {
        ...newFilterOptions[key],
        isFilter: savedMultipleFilter[key]!.isFilter,
        selected: savedMultipleFilter[key]!.isFilter
          ? savedMultipleFilter[key]!.selected
          : newFilterOptions[key].selected
      };
    }
  });

  return {
    ...currentMultipleFilter,
    ...newFilterOptions,
    rAndDInfo: rAndDInfoFilterOriginal
  };
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
      multipleFilterCommon(
        props.entities,
        props.filterInfo,
        COLUMN_KEY.KO_TIME as TColumnKey
      ),
    (props: TMultipleFilterPipe) =>
      multipleFilterCommon(
        props.entities,
        props.filterInfo,
        COLUMN_KEY.LEAGUE as TColumnKey
      ),
    (props: TMultipleFilterPipe) =>
      multipleFilterCommon(
        props.entities,
        props.filterInfo,
        COLUMN_KEY.MB as TColumnKey
      ),
    (props: TMultipleFilterPipe) =>
      multipleFilterCommon(
        props.entities,
        props.filterInfo,
        COLUMN_KEY.MB_OU as TColumnKey
      ),
    (props: TMultipleFilterPipe) =>
      multipleFilterCommon(
        props.entities,
        props.filterInfo,
        COLUMN_KEY.SPREAD_HDP as TColumnKey
      ),
    (props: TMultipleFilterPipe) =>
      multipleFilterCommon(
        props.entities,
        props.filterInfo,
        COLUMN_KEY.SPREAD_OU as TColumnKey
      ),
    (props: TMultipleFilterPipe) =>
      multipleFilterCommon(
        props.entities,
        props.filterInfo,
        COLUMN_KEY.LST_NOTE as TColumnKey
      ),
    (props: TMultipleFilterPipe) =>
      filterByTurnoverPeriod(
        props.entities,
        props.filterInfo,
        hasReadPermission
      ),
    (props: TMultipleFilterPipe) =>
      filterByMarginPeriod(props.entities, props.filterInfo, hasReadPermission),
    (props: TMultipleFilterPipe) =>
      multipleFilterCommon(
        props.entities,
        props.filterInfo,
        COLUMN_KEY['NO.OF_HDP'] as TColumnKey
      ),
    (props: TMultipleFilterPipe) => {
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
  buildDefaultRAndDInfoOptions,
  buildTableMultipleFilterOptions,
  preserveAndRestoreMultipleFilter,
  singleColumnFilterOptionsFactory
};
