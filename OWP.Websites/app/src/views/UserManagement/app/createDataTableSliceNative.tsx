import { StateCreator } from 'zustand';
import { REQUEST_STATUS } from '../../Shared/Constants';
import { TDataTable, TDataTableResponse } from '../type';
import { post } from '../../Shared/Service/Service';
import update from 'immutability-helper';

const createDataTableSliceNative: StateCreator<
  TDataTable,
  [],
  [],
  TDataTable
> = set => ({
  queryDataTableStatus: REQUEST_STATUS.idle,
  dataSource: [],
  async queryUserInfo(value) {
    set(state =>
      update(state, {
        queryDataTableStatus: { $set: REQUEST_STATUS.pending }
      })
    );

    try {
      const url = `/api/UserManagement/query-user`;
      const dataSource = await post({
        url,
        ...value
      }).then(response => response.data as TDataTableResponse);

      set(state =>
        update(state, {
          queryDataTableStatus: { $set: REQUEST_STATUS.idle },
          dataSource: { $set: dataSource }
        })
      );
    } catch (error) {
      set(state =>
        update(state, {
          queryDataTableStatus: { $set: REQUEST_STATUS.idle }
        })
      );
    }
  },
  updateValue(newValue, key, rowIndex) {
    set(state =>
      update(state, {
        dataSource: {
          [rowIndex]: {
            [key]: { $set: newValue }
          }
        }
      })
    );
  }
});

export default createDataTableSliceNative;
