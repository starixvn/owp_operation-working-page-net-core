import { formatTime } from '../../Shared/DateTime/dateTimeHelper';
import { TConfirmed } from '../types/confirmed';
import TDataTable, {
  TMultipleFilter,
  TTableSlice
} from '../types/dataTableType';
import { TNoOfferMatch } from '../types/noOfferMatchType';
import { uniqBy } from 'lodash';
import { COLUMN_KEY } from '../table/TableSchema.json';
import { isEquals } from 'immutability-helper';
import Utils, { filterDataTable } from '../../Shared/Utils';

const KO_TIME = COLUMN_KEY.KO_TIME;
const MB = COLUMN_KEY.MB;

const updateNoOfferMatch = (
  entities: TDataTable[],
  noOfferMatch: TNoOfferMatch[]
) => {
  entities.forEach(element => {
    const noOfferMatchTarget = noOfferMatch.find(
      x =>
        x.infoSiteMatchId === element.infoSiteMatchId &&
        x.childMatchId === element.childMatchId
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

const buildMultipleFilterDefault = (entities: TDataTable[]) => {
  const koTimeOptions = buildKoOptions(entities);
  const mbOptions = buildMbOptions(entities);
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
    }
  } as TMultipleFilter;
};

const filterByKo = (
  entities: TDataTable[],
  selectedValue: (number | string | null)[]
) =>
  entities.filter(e =>
    selectedValue.some(
      s =>
        s &&
        isEquals(formatTime(e.eventDateTime, 'minute', 'hh:mm A'), s.toString())
    )
  );

const filterByMb = (
  entities: TDataTable[],
  selectedValue: (number | string | null)[]
) => entities.filter(e => selectedValue.some(s => e.maxbetHdp === s));

const executeFilterTable = (
  state: TTableSlice,
  filterByKeyword: (
    arraySource: any,
    { keyword, columnKey }: { keyword: undefined; columnKey: undefined }
  ) => any
) => {
  state.entities = filterDataTable(
    state.entitiesOriginal,
    state.filterState,
    filterByKeyword
  );
  state.entities = filterByKo(
    state.entities,
    state.multipleFilter.KO_Time.selected.map(x => x.value)
  );
  state.entities = filterByMb(
    state.entities,
    state.multipleFilter.MB.selected.map(x => x.value)
  );
  return state.entities;
};

export {
  updateNoOfferMatch,
  updateConfirmedMatch,
  buildMultipleFilterDefault,
  filterByKo,
  filterByMb,
  buildMbOptions,
  buildKoOptions,
  executeFilterTable
};
