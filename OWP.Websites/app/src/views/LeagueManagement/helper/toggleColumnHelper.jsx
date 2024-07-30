import { FunctionKeys } from '../../Shared/Constants';
import ColumnConst from '../constants/ColumnConst';
import isEqual from 'lodash.isequal';

const buildLocalStoreKey = policyData => {
  let localstoreKey = policyData.generalPolicy.Username;

  if (policyData.generalPolicy.HasRead) {
    localstoreKey = `${localstoreKey}_${FunctionKeys.LeagueManagement_General}`;
  }
  if (policyData.conditionPolicy.HasRead) {
    localstoreKey = `${localstoreKey}_${FunctionKeys.LeagueManagement_EditConditions}`;
  }
  if (policyData.lstNotePolicy.HasRead) {
    localstoreKey = `${localstoreKey}_${FunctionKeys.LeagueManagement_LSTNote}`;
  }
  if (policyData.tvListPolicy.HasRead) {
    localstoreKey = `${localstoreKey}_${FunctionKeys.LeagueManagement_TVList}`;
  }
  return localstoreKey;
};

const buildOptionDropdownToggleColumn = policyData => {
  const ignoreColumn = [
    'category',
    'time',
    'condition',
    'isApplied',
    'lastUpdated',
    'action'
  ];
  !policyData.lstNotePolicy.HasRead && ignoreColumn.push('lstNote');
  const options = [];
  const { columnLeagueDetail } = ColumnConst;
  Object.keys(columnLeagueDetail).forEach(key => {
    if (
      !ignoreColumn.some(col => isEqual(columnLeagueDetail[key].columnKey, col))
    ) {
      options.push({
        value: columnLeagueDetail[key].columnKey,
        label: columnLeagueDetail[key].headerName
      });
    }
  });
  return options;
};

const columnExists = (columnKeys, key) =>
  columnKeys.some(item => isEqual(item, key));

export { buildLocalStoreKey, buildOptionDropdownToggleColumn, columnExists };
