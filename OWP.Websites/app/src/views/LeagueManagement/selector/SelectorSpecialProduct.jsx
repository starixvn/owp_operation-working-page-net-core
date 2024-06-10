import { createSelector } from 'reselect';

const selectSpecialProductData = createSelector(
  state => state.specialProductReducer.specialProductPending,
  state => state.specialProductReducer.specialProductData,
  state => state.specialProductReducer.specialProductError,
  state => state.specialProductReducer.specialProductDuplicateMBFTHDP,
  (
    specialProductPending,
    specialProductData,
    specialProductError,
    specialProductDuplicateMBFTHDP
  ) => ({
    specialProductPending: specialProductPending ?? true,
    specialProductData,
    specialProductError,
    specialProductDuplicateMBFTHDP
  })
);
const selectLogHistoryState = createSelector(
  state => state.logHistoryReducer.Pending,
  state => state.logHistoryReducer.Data,
  (Pending, Data) => ({
    Pending: Pending ?? true,
    Data
  })
);

export { selectSpecialProductData, selectLogHistoryState };
