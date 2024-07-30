import { create } from 'zustand';
import update from 'immutability-helper';
import {
  DataSource,
  SortState,
  TAction,
  TSection,
  TSectionOption,
  TState,
  Target
} from '../feature/type/TableType.native';
import { REQUEST_STATUS } from '../../Shared/Constants';
import {
  queryDataTable,
  queryConditionData,
  queryLevelOption,
  updateTargetLevel,
  exportDataTable,
  getSectionOption
} from '../service';
import { filterData, sortData } from '../helper/LaKpiHelper';
import getPermission from '../../Shared/permission/getPermission';
import { USER_STATUS } from '../const.json';

const useStore = create<TState & TAction>((set, get) => ({
  dataTable: {
    loadingStatus: REQUEST_STATUS.idle,
    entities: [],
    dataSourcesFiltered: [],
    isGap: true
  },
  filterHeaderValue: '',
  permission: null,
  conditionData: [],
  levelOption: [],
  sectionOption: [],
  sectionSelected: [],
  userStatusOption: USER_STATUS,
  userStatusSelected: [USER_STATUS[0]],
  loading: REQUEST_STATUS.idle,
  sortState: {
    columnKeySelected: '',
    orderType: '',
    propertySelected: ''
  },
  queryConditionData: async () => {
    const data = await queryConditionData();
    set({
      conditionData: data
    });
  },
  exportDataTable: async (
    fromDate: Required<Date>,
    toDate: Required<Date>,
    section: string,
    userActive: boolean,
    userResign: boolean
  ) => {
    await exportDataTable(fromDate, toDate, section, userActive, userResign);
  },
  queryLevelOption: async () => {
    const data = await queryLevelOption();
    set({
      levelOption: data
    });
  },
  updateTargetLevel: async (model: Target[]) => {
    await updateTargetLevel(model);
  },
  queryDataTable: async (
    fromDate: Required<Date>,
    toDate: Required<Date>,
    section: string,
    activeUser: boolean,
    resignUser: boolean
  ) => {
    const { filterHeaderValue } = get();
    set((state: TState) =>
      update(state, {
        dataTable: {
          loadingStatus: { $set: REQUEST_STATUS.pending }
        }
      })
    );

    const data = await queryDataTable(fromDate, toDate, section, activeUser, resignUser);
    const dataFiltered = filterData(data.kpiLa, filterHeaderValue);
    set((state: TState) =>
      update(state, {
        dataTable: {
          loadingStatus: { $set: REQUEST_STATUS.idle },
          entities: { $set: data.kpiLa },
          dataSourcesFiltered: { $set: dataFiltered },
          isGap: { $set: data.isGap }
        }
      })
    );
  },
  setDataSourcesFiltered: (data: DataSource[]) => {
    set((state: TState) =>
      update(state, {
        dataTable: {
          dataSourcesFiltered: { $set: data }
        }
      })
    );
  },
  sortDataSources: (sortState: SortState) => {
    const { dataTable } = get();
    const dataSorted = sortData(
      dataTable.dataSourcesFiltered,
      sortState.propertySelected,
      sortState.columnKeySelected,
      sortState.orderType
    );
    set((state: TState) =>
      update(state, {
        sortState: { $set: sortState },
        dataTable: {
          dataSourcesFiltered: { $set: dataSorted }
        }
      })
    );
  },
  setFilterHeader: (value: string) => {
    set((state: TState) =>
      update(state, {
        filterHeaderValue: { $set: value }
      })
    );
  },
  setSectionSelected: (value: Array<TSectionOption>) => {
    set((state: TState) =>
      update(state, {
        sectionSelected: { $set: value }
      })
    );
  },
  setUserStatusSelected(value: Array<TSectionOption>) {
    set((state: TState) =>
      update(state, {
        userStatusSelected: { $set: value }
      })
    );
  },
  queryPermissionsAndSections: async () => {
    set((state: TState) =>
      update(state, {
        loading: { $set: REQUEST_STATUS.pending }
      })
    );
    const permission = await getPermission('LAKpi');
    const section = await getSectionOption(permission.ActorId);
    const sectionOption = await section.map((x: TSection) => ({
      label: x.sectionName,
      value: x.sectionId
    }));
    const sectionSelected = section
      .filter((x: TSection) => x.isContainUser)
      .map((x: TSection) => ({
        label: x.sectionName,
        value: x.sectionId
      }));
    set((state: TState) =>
      update(state, {
        permission: { $set: permission },
        sectionOption: {
          $set: sectionOption
        },
        sectionSelected: { $set: sectionSelected },
        loading: { $set: REQUEST_STATUS.idle }
      })
    );
  },
  getPermission: () => get().permission
}));

export default useStore;
