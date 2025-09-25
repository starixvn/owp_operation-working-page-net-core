import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { editEventPickings, getEventPickings } from './eventSliceThunkNative';
import updateNoOfferMatchThunk from '../updateNoOfferMatchThunkNative';
import updateConfirmedThunk from '../updateConfirmedThunkNative';
import { Option } from '../../../Shared/MultipleSelection/type';
import {
    updateNoOfferMatch,
    updateConfirmedMatch
} from '../../helper/reducerHelper';
import {
    buildMultipleFilterDefault,
    buildTableMultipleFilterOptions,
    executeFilterTable
} from '../../helper/filterHelper';
import TDataTable, {
    TTableSlice,
    TFilterState,
    FilterAllColumn,
    TColumnKey,
    TOrderKey,
    TOrderDirection,
    TOrder
} from '../../types/dataTableType';
import {
    addOrReplaceFilterState,
    filterDataTable,
    pushOrUpdateObject
} from '../../../Shared/Utils';
import filterByKeyword from '../filterByKeyword';
import type { PayloadAction } from '@reduxjs/toolkit';
import { executeOrderTable } from '../../helper/orderHelper';

const EventPickingTableSlice = createSlice({
    name: 'EventPickingTableSlice',
    initialState: {
        matchSelected: [] as TDataTable[],
        openPopupState: {
            isOpenAddSpecialProduct: false,
            isOpenRemoveSpecialProduct: false
        },
        entitiesByDate: [],
        entities: [],
        entitiesOriginal: [],
        isNoOfferAll: false,
        isConfirmAll: false,
        isSelectedAll: false,
        valueFilterAllColumn: {} as FilterAllColumn,
        columnKeyFilterCurrent: '',
        filterState: [] as TFilterState[],
        loading: REQUEST_STATUS.idle,
        previousNoOfferIndex: null,
        rAndDInfoFilterOriginal: {
            isFilter: false,
            selected: [],
            options: []
        },
        multipleFilter: {
            time: {
                isFilter: false,
                selected: [],
                options: []
            },
            maxbetHdp: {
                isFilter: false,
                selected: [],
                options: []
            },
            leagueName: {
                isFilter: false,
                selected: [],
                options: []
            },
            spreadHdp: {
                isFilter: false,
                selected: [],
                options: []
            },
            spreadOu: {
                isFilter: false,
                selected: [],
                options: []
            },
            noOfHDP: {
                isFilter: false,
                selected: [],
                options: []
            },
            lstNote: {
                isFilter: false,
                selected: [],
                options: []
            },
            rAndDInfo: {
                isFilter: false,
                selected: [],
                options: []
            },
            turnoverPeriod: {
                isFilter: false,
                selected: [],
                options: []
            },
            marginPeriod: {
                isFilter: false,
                selected: [],
                options: []
            }
        },
        order: {
            columnKey: '',
            turnoverPeriod: {},
            marginPeriod: {}
        } as TOrder
    } as TTableSlice,
    reducers: {
        setUpdateTypeFilter: (state, action: PayloadAction<Option[]>) => {
            state.rAndDInfoFilterOriginal = {
                isFilter: false,
                selected: action.payload,
                options: action.payload
            };

            state.multipleFilter = {
                ...state.multipleFilter,
                rAndDInfo: {
                    isFilter: false,
                    selected: action.payload,
                    options: action.payload
                }
            };
        },
        setOrder: (
            state,
            action: PayloadAction<{ columnKey: string, key: TOrderKey; direction: TOrderDirection }>
        ) => {
            const { columnKey, key, direction } = action.payload;
            state.order = {
                ...state.order,
                columnKey,
                [columnKey]: { key, direction }
            };

            state.entities = executeOrderTable(state.entities, state.order);
        },
        setOpenPopupState: (state, action) => {
            const { key, value } = action.payload;
            state.openPopupState = { ...state.openPopupState, [key]: value };
        },
        setColumnKeyValue: (state, action) => {
            state.columnKeyFilterCurrent = action.payload;
        },
        setValueFilter: (state, action) => {
            const { valueFilterAllColumn } = state;
            state.valueFilterAllColumn = pushOrUpdateObject(
                valueFilterAllColumn,
                action.payload
            );
        },
        filterBySearchKey: (state, action: PayloadAction<{ filterState: TFilterState; hasReadPermission?: boolean }>) => {
            const { filterState, hasReadPermission } = action.payload;
            state.filterState = addOrReplaceFilterState(state.filterState, filterState);
            state.entities = executeFilterTable(state, filterByKeyword, hasReadPermission);
            state.entities = executeOrderTable(state.entities, state.order);
            const options = buildTableMultipleFilterOptions(
                state.entities,
                state.rAndDInfoFilterOriginal.options
            );

            state.multipleFilter.time.options = options.time;
            state.multipleFilter.maxbetHdp.options = options.maxbetHdp;
            state.multipleFilter.spreadHdp.options = options.spreadHdp;
            state.multipleFilter.spreadOu.options = options.spreadOu;
            state.multipleFilter.noOfHDP.options = options.noOfHDP;
            state.multipleFilter.rAndDInfo.options = options.rAndDInfo;
            state.multipleFilter.lstNote.options = options.lstNote;

            state.isConfirmAll = state.entities?.every(x => x.isCheck);
            state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
        },
        clearFilter: state => {
            const { entitiesOriginal } = state;
            state.filterState = [];
            state.valueFilterAllColumn = {};
            state.entities = entitiesOriginal;
            state.entities = executeOrderTable(state.entities, state.order);
            state.isConfirmAll = state.entities?.every(x => x.isCheck);
            state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
            state.multipleFilter = {
                ...state.multipleFilter,
                ...buildMultipleFilterDefault(state.entities),
                rAndDInfo: state.rAndDInfoFilterOriginal
            };
        },
        setMultipleFilter: (
            state,
            action: PayloadAction<{
                columnKey: Required<TColumnKey>;
                selected: Option[];
                hasReadPermission?: boolean;
            }>
        ) => {
            const { columnKey, selected, hasReadPermission } = action.payload;
            state.multipleFilter[columnKey] = {
                ...state.multipleFilter[columnKey],
                isFilter: true,
                selected
            };
            state.entities = executeFilterTable(state, filterByKeyword, hasReadPermission);
            state.entities = executeOrderTable(state.entities, state.order);
            const options = buildTableMultipleFilterOptions(
                state.entities,
                state.rAndDInfoFilterOriginal.options
            );
            
            state.multipleFilter.time.options = options.time;
            state.multipleFilter.maxbetHdp.options = options.maxbetHdp;
            state.multipleFilter.spreadHdp.options = options.spreadHdp;
            state.multipleFilter.spreadOu.options = options.spreadOu;
            state.multipleFilter.noOfHDP.options = options.noOfHDP;
            state.multipleFilter.lstNote.options = options.lstNote;
            state.multipleFilter.rAndDInfo.options = options.rAndDInfo;

            state.isConfirmAll = state.entities?.every(x => x.isCheck);
            state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
          
        },
        setPreviousNoOfferRow: (state, action) => {
            state.previousNoOfferIndex = action.payload;
        },
        setValueMaxbetSpread: (
            state,
            action: PayloadAction<{
                key: 'maxbetHdp' | 'spreadOu' | 'spreadHdp';
                infoSiteMatchId: number;
                value: number | undefined;
                minorBettypeId: number;
            }>
        ) => {
            const { key, value, infoSiteMatchId, minorBettypeId } = action.payload;
            const target = state.entities.find(
                s =>
                    s.infoSiteMatchId === infoSiteMatchId &&
                    s.minorBettypeId === minorBettypeId
            );
            if (target) {
                target.isModified = true;
                target[key] = value;
            }
        },
        setValueNoOfHdp: (
            state,
            action: PayloadAction<{
                infoSiteMatchId: number;
                value: string | undefined;
                minorBettypeId: number;
            }>
        ) => {
            const { value, infoSiteMatchId, minorBettypeId } = action.payload;
            const target = state.entities.find(
                s =>
                    s.infoSiteMatchId === infoSiteMatchId &&
                    s.minorBettypeId === minorBettypeId
            );
            if (target) {
                target.noOfHDP = value;
                target.isModified = true;
            }
        },
        setMatchSelected: (
            state,
            action: PayloadAction<{
                match: TDataTable;
                isAdded: boolean;
            }>
        ) => {
            const { match, isAdded } = action.payload;

            state.matchSelected = isAdded
                ? [...state.matchSelected, match]
                : state.matchSelected.filter(
                    s => s.infoSiteMatchId !== match.infoSiteMatchId
                );
        },

        setListMatchSelected: (
            state,
            action: PayloadAction<{
                matches: TDataTable[];
                isAdded: boolean;
            }>
        ) => {
            const { matches, isAdded } = action.payload;
            state.matchSelected = isAdded
                ? [...state.matchSelected, ...matches]
                : state.matchSelected.filter(
                    s => !matches.some(i => s.infoSiteMatchId === i.infoSiteMatchId)
                );
            state.isSelectedAll =
                state.matchSelected.length ===
                state.entities.filter(x => !x.isChildMatch).length;
        },
        setAllMatchSelected: (
            state,
            action: PayloadAction<{
                isAdded: boolean;
            }>
        ) => {
            const { isAdded } = action.payload;
            state.matchSelected = isAdded ? state.entities : [];
            state.isSelectedAll = isAdded;
        }
    },
    extraReducers: builder => {
        builder
            .addCase(updateNoOfferMatchThunk.fulfilled, (state, { payload }) => {
                const noOfferMatch = payload;
                updateNoOfferMatch(state.entitiesOriginal, noOfferMatch);
                updateNoOfferMatch(state.entities, noOfferMatch);
                updateNoOfferMatch(state.entitiesByDate, noOfferMatch);
                state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
            })
            .addCase(updateConfirmedThunk.fulfilled, (state, { payload }) => {
                const confirmInfo = payload;
                updateConfirmedMatch(state.entitiesOriginal, confirmInfo);
                updateConfirmedMatch(state.entities, confirmInfo);
                state.isConfirmAll = state.entities?.every(x => x.isCheck);
            })
            .addCase(getEventPickings.pending, state => {
                state.loading = REQUEST_STATUS.pending;
            })
            .addCase(getEventPickings.fulfilled, (state, { payload }) => {
                const { matchesByDate, filteredMatches } = payload;
                state.loading = REQUEST_STATUS.idle;
                state.entitiesByDate = matchesByDate;
                state.entitiesOriginal = filteredMatches;
                state.entities = filterDataTable(
                    state.entitiesOriginal,
                    state.filterState,
                    filterByKeyword
                );
                state.order = {};

                state.isConfirmAll = state.entities?.every(x => x.isCheck);
                state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
                state.multipleFilter = {
                    ...state.multipleFilter,
                    ...buildMultipleFilterDefault(state.entities),
                    rAndDInfo: state.rAndDInfoFilterOriginal
                };
            })
            .addCase(getEventPickings.rejected, state => {
                state.loading = REQUEST_STATUS.idle;
            })
            .addCase(editEventPickings.pending, state => {
                state.loading = REQUEST_STATUS.pending;
            })
            .addCase(editEventPickings.fulfilled, (state, { payload }) => {
                const { matchesByDate, filteredMatches } = payload;
                state.loading = REQUEST_STATUS.idle;
                state.entitiesByDate = matchesByDate;
                state.entitiesOriginal = filteredMatches;
                state.entities = filterDataTable(
                    state.entitiesOriginal,
                    state.filterState,
                    filterByKeyword
                );

                state.multipleFilter = {
                    ...state.multipleFilter,
                    ...buildMultipleFilterDefault(state.entities),
                    rAndDInfo: state.rAndDInfoFilterOriginal
                };
                state.isConfirmAll = state.entities?.every(x => x.isCheck);
                state.isNoOfferAll = state.entities?.every(x => x.isNoOffer);
            })
            .addCase(editEventPickings.rejected, state => {
                state.loading = REQUEST_STATUS.idle;
            });
    }
});
const { reducer, actions } = EventPickingTableSlice;
const {
    filterBySearchKey,
    clearFilter,
    setMultipleFilter,
    setColumnKeyValue,
    setValueFilter,
    setPreviousNoOfferRow,
    setValueMaxbetSpread,
    setValueNoOfHdp,
    setOpenPopupState,
    setListMatchSelected,
    setAllMatchSelected,
    setOrder,
    setUpdateTypeFilter
} = actions;

export {
    filterBySearchKey,
    clearFilter,
    setColumnKeyValue,
    setValueFilter,
    setPreviousNoOfferRow,
    setMultipleFilter,
    setValueMaxbetSpread,
    setValueNoOfHdp,
    setOpenPopupState,
    setListMatchSelected,
    setAllMatchSelected,
    setOrder,
    setUpdateTypeFilter
};
export default reducer;
