import { get, post } from '../Shared/Service/Service';
import Utils from '../Shared/Utils';
import { netCoreURL } from './const.json';
import { Target } from './feature/type/TableType.native';

const buildRequestURL = (key: string | null) => `${netCoreURL}/${key}`;

const queryDataTable = async (
  filterFromDate: Date,
  filterToDate: Date,
  sectionIds: string,
  activeUser: boolean,
  resignUser: boolean
) => {
  const url = buildRequestURL('');
  const fromDate = Utils.formatDate(filterFromDate);
  const toDate = Utils.formatDate(filterToDate);

  const response = await get({
    url,
    fromDate,
    toDate,
    sectionIds,
    activeUser,
    resignUser
  });
  return response.data;
};

const exportDataTable = async (
  filterFromDate: Date,
  filterToDate: Date,
  sectionIds: string,
  activeUser: boolean,
  resignUser: boolean
) => {
  const url = buildRequestURL('export');
  const fromDate = Utils.formatDate(filterFromDate);
  const toDate = Utils.formatDate(filterToDate);
  window.location.href = `${url}?fromDate=${fromDate}&toDate=${toDate}&sectionIds=${sectionIds}&activeUser=${activeUser}&resignUser=${resignUser}`;
};

const queryConditionData = async () => {
  const url = buildRequestURL('get-condition');
  const response = await get({
    url
  });

  return response.data;
};

const queryLevelOption = async () => {
  const url = buildRequestURL('get-level-option');
  const response = await get({ url });

  return response.data;
};

const updateTargetLevel = async (model: Target[]) => {
  const url = buildRequestURL('update-target-level');
  const response = await post({ url, targetLevel: model });

  return response.data;
};

const getSectionOption = async (userId: number) => {
  const url = buildRequestURL('get-all-section');
  const response = await get({
    url,
    userId
  });
  return response.data;
};

const getLogHistory = async (userId: number) => {
  const url = buildRequestURL('get-log-target-level');
  const response = await get({ url, userId });
  return response.data;
};

export {
  queryDataTable,
  queryConditionData,
  queryLevelOption,
  updateTargetLevel,
  exportDataTable,
  getSectionOption,
  getLogHistory
};
