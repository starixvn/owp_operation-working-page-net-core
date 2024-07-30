import { createSlice } from '@reduxjs/toolkit';
import { REQUEST_STATUS } from '../../../Shared/Constants';
import { ReportData } from '../../models/report-data';
import { ColumnOrder } from '../../typeNative';
import { sortDataReport } from '../../helper/helper';
import { getReportData } from './reportThunkNative';

const reportSlice = createSlice({
  name: 'report',
  initialState: {
    status: REQUEST_STATUS.idle,
    reportData: [] as ReportData[],
    columnOrder: {} as ColumnOrder,
    isOpenTable: false
  },
  reducers: {
    sort: (state, { payload }) => {
      const columnOrder = payload as ColumnOrder;
      state.columnOrder = columnOrder;

      const currentData = state.reportData as ReportData[];

      const realData = currentData.filter((x: ReportData) => !x.isSaba && x.no);
      const sabaData = currentData.filter((x: ReportData) => x.isSaba && x.no);

      const sortedRealData = sortDataReport(realData, columnOrder);
      const sortedSabaData = sortDataReport(sabaData, columnOrder);

      const realMainIndex = currentData.findIndex(
        (element: ReportData) => !element.no && !element.isSaba
      );
      const sabaMainIndex = currentData.findIndex(
        (element: ReportData) => !element.no && element.isSaba
      );

      sortedRealData.unshift(currentData[realMainIndex]);
      sortedSabaData.unshift(currentData[sabaMainIndex]);

      state.reportData = [...sortedRealData, ...sortedSabaData];
    },
    displayTable: (state, action) => {
      const statusOpen = action.payload as boolean;
      state.isOpenTable = statusOpen;
    }
  },
  extraReducers: builder => {
    builder.addCase(getReportData.pending, state => {
      state.status = REQUEST_STATUS.pending;
    }).addCase(getReportData.fulfilled, (state, { payload }) => {
      state.status = REQUEST_STATUS.idle;
      state.reportData = payload;
    }).addCase(getReportData.rejected, state => {
      state.status = REQUEST_STATUS.idle;
    });
  }
});

const { actions, reducer } = reportSlice;

const { sort, displayTable } = actions;
export { sort, displayTable };
export default reducer;
