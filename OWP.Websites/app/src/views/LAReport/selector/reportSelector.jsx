import { createSelector } from 'reselect';

const reportSelector = createSelector(
  [
    state => state.reportReducer
  ],
  reportReducer => ({
    pending: reportReducer.pending ?? true,
    isDisplayTable: reportReducer.isDisplayTable ?? false,
    laInfo: reportReducer.laInfo,
    orderType: reportReducer.orderType ?? '',
    columnId: reportReducer.columnId ?? '',
    isClearAll: reportReducer.isClearAll ?? false,
    keyWordLAColumn: reportReducer.keyWordLAColumn,
    keyWordSectionColumn: reportReducer.keyWordSectionColumn
  })
);

export { reportSelector };
