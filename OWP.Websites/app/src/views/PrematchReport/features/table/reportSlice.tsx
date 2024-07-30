import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { PreMatchReportData } from '../../models/report-data';
import { ColumnOrder } from '../../typeNative';
import { getReportData } from './reportThunkNative';
import { orderBy } from 'lodash';
import { getKeyValue } from '../helpers/helpers';

const reportSlice = createSlice({
  name: 'prematchReport',
  initialState: {
    status: REQUEST_STATUS.idle,
    reportData: [] as PreMatchReportData[],
    columnOrder: {} as ColumnOrder,
    isOpenTable: false
  },
  reducers: {
    sort: (state, { payload }) => {
      const columnOrder = payload as ColumnOrder;
      state.columnOrder = columnOrder;

      const currentData = state.reportData as PreMatchReportData[];
      const emptyData = currentData.filter(
        item =>
          !+getKeyValue(
            item,
            columnOrder.subColumnId as keyof PreMatchReportData
          )
      );
      const hasData = currentData.filter(
        item =>
          !!+getKeyValue(
            item,
            columnOrder.subColumnId as keyof PreMatchReportData
          )
      );
      const sortedData = orderBy(
        hasData,
        item =>
          +getKeyValue(
            item,
            columnOrder.subColumnId as keyof PreMatchReportData
          ),
        [columnOrder.orderType]
      );

      state.reportData = [...sortedData, ...emptyData];
    },
    displayTable: (state, action) => {
      const statusOpen = action.payload as boolean;
      state.isOpenTable = statusOpen;
    }
  },
  extraReducers: builder => {
    builder.addCase(getReportData.pending, state => {
      state.status = REQUEST_STATUS.pending;
    });
    builder.addCase(getReportData.fulfilled, (state, { payload }) => {
      state.status = REQUEST_STATUS.idle;
      const data = orderBy(payload, (item: PreMatchReportData) =>
        item.traderName.toLowerCase()
      );
      state.reportData = data;
    });
    builder.addCase(getReportData.rejected, state => {
      state.status = REQUEST_STATUS.idle;
    });
  }
});

const { actions, reducer } = reportSlice;

const { sort, displayTable } = actions;
export { sort, displayTable };
export default reducer;
