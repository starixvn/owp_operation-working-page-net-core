import { uniqBy } from 'lodash';
import { formatTime } from '../../Shared/DateTime/dateTimeHelper';
import Utils, { filterDataTable } from '../../Shared/Utils';
import { COLUMN_KEY } from '../table/TableSchema.json';
import { TConfirmed } from '../types/confirmed';
import TDataTable, {
  MultipleFilterProperties,
  TColumnKey,
  TMultipleFilter,
  TTableSlice
} from '../types/dataTableType';
import { TNoOfferMatch } from '../types/noOfferMatchType';

const KO_TIME = COLUMN_KEY.KO_TIME;
const MB = COLUMN_KEY.MB;

const updateNoOfferMatch = (
  entities: TDataTable[],
  noOfferMatch: TNoOfferMatch[]
) => {
  entities.forEach(element => {
    const noOfferMatchTarget = noOfferMatch.find(
      x => x.infoSiteMatchId === element.infoSiteMatchId
    );

    if (noOfferMatchTarget) {
      element.isNoOffer = noOfferMatchTarget.isNoOffer;
    }
  });
};

const updateConfirmedMatch = (
  entities: TDataTable[],
  confirmInfo: TConfirmed
) => {
  entities.forEach(element => {
    if (
      confirmInfo.infoSiteMatchIds.some(id => id === element.infoSiteMatchId)
    ) {
      element.isCheck = confirmInfo.confirmed;
    }
  });
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

const buildMultipleFilterDefault = (entities: TDataTable[]) => {
  const koTimeOptions = buildKoOptions(entities);
  const mbOptions = buildMbOptions(entities);
  const leagueOptions = buildLeagueOptions(entities);
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
    [COLUMN_KEY.LEAGUE]: {
      isFilter: false,
      selected: leagueOptions,
      options: leagueOptions
    }
  } as TMultipleFilter;
};

const filterByProperty = (
  entities: TDataTable[],
  selectedValue: (number | string | null)[],
  property: keyof TDataTable
) => {
  const selectedSet = new Set(selectedValue);
  return entities.filter(e => {
    const value = e[property] ?? null;
    if (typeof value === 'string' || typeof value === 'number' || value === null) {
      return selectedSet.has(value);
    }
    return entities.filter(e => selectedValue.some(s => e[property] === s));
  });
};

const executeFilterTable = (
  state: TTableSlice,
  filterByKeyword: (
    arraySource: any, //NOSONAR
    { keyword, columnKey }: { keyword: undefined; columnKey: undefined }
  ) => any //NOSONAR
) => {
  state.entities = filterDataTable(
    state.entitiesOriginal,
    state.filterState,
    filterByKeyword
  );
  if (state.multipleFilter.KO_Time) {
    state.entities = filterByProperty(
      state.entities,
      state.multipleFilter.KO_Time.selected.map(x => x.value),
      'time'
    );
  }
  if (state.multipleFilter.MB) {
    state.entities = filterByProperty(
      state.entities,
      state.multipleFilter.MB.selected.map(x => x.value),
      'maxbetHdp'
    );
  }
  if (state.multipleFilter.League) {
    state.entities = filterByProperty(
      state.entities,
      state.multipleFilter.League.selected.map(x => x.value),
      'leagueName'
    );
  }
  return state.entities;
};

export {
  buildKoOptions,
  buildMbOptions,
  buildMultipleFilterDefault,
  executeFilterTable,
  updateConfirmedMatch,
  updateNoOfferMatch
};

