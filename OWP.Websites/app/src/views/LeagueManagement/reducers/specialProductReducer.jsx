import { createReducer } from './reducerUtilities';
import {
  SPECIALPRODUCT_DELETE,
  SPECIALPRODUCT_QUERY,
  SPECIALPRODUCT_UPDATEINFO,
  ACTION_SPECIALPRODUCT,
  EXECUTE_SPECIALPRODUCT_FILTER
} from '../constants/action-type';
import { cloneDeep, sortBy } from 'lodash';

const initState = {
  specialProductPending: true,
  specialProductData: [],
  specialProductError: [],
  specialProductDataSource: [],
  specialProductDuplicateMBFTHDP: null
};

const handlePending = state => {
  return {
    ...state,
    specialProductPending: true
  };
};

const handleQuerySuccess = (state, action) => {
  let specialProductDataSource = action.data;
  let specialProductData = cloneDeep(specialProductDataSource);
  specialProductData = sortBy(specialProductData, x => x.Maxbet_FTHDP);
  specialProductDataSource = sortBy(
    specialProductDataSource,
    x => x.Maxbet_FTHDP
  );

  return {
    ...state,
    specialProductData,
    specialProductDataSource,
    specialProductPending: false,
    specialProductDuplicateMBFTHDP: null
  };
};

const handleError = (state, action) => {
  return {
    ...state,
    specialProductPending: false,
    specialProductError: action.error
  };
};

const handleFilter = state => {
  let specialProductDataSource = cloneDeep(state.specialProductDataSource);
  let specialProductData = cloneDeep(specialProductDataSource);
  specialProductData = sortBy(specialProductData, x => x.Maxbet_FTHDP);
  specialProductDataSource = sortBy(
    specialProductDataSource,
    x => x.Maxbet_FTHDP
  );

  return {
    ...state,
    specialProductData,
    specialProductDataSource,
    specialProductDuplicateMBFTHDP: null
  };
};

const handleUpdateSuccess = (state, action) => {
  const { ProductId, ErrorMessage } = action.data;
  if (ErrorMessage) {
    return {
      ...state,
      specialProductPending: false,
      specialProductDuplicateMBFTHDP: ErrorMessage
    };
  } else {
    const extraModel = action.extraModel;
    const specialProductDataSource = state.specialProductDataSource.slice();
    const upadtedEleIndex = specialProductDataSource.findIndex(
      x => x.ProductId === extraModel.ProductId
    );
    if (upadtedEleIndex >= 0) {
      specialProductDataSource[upadtedEleIndex] = {
        ...specialProductDataSource[upadtedEleIndex],
        ...extraModel
      };
    } else {
      extraModel.ProductId = ProductId;
      specialProductDataSource.push(extraModel);
    }

    const specialProductData = specialProductDataSource;

    return {
      ...state,
      specialProductPending: false,
      specialProductDataSource,
      specialProductData,
      specialProductDuplicateMBFTHDP: null
    };
  }
};

const handleDeleteSuccess = (state, action) => {
  const data = action.data;
  let specialProductDataSource = [];
  specialProductDataSource = state.specialProductDataSource.filter(
    x => x.ProductId !== data.productId
  );
  const specialProductData = specialProductDataSource;
  return {
    ...state,
    specialProductPending: false,
    specialProductData,
    specialProductDataSource,
    specialProductDuplicateMBFTHDP: null
  };
};

const handleAddNewRow = (state, action) => {
  const data = action.payload;
  const specialProductData = cloneDeep(state.specialProductData);
  specialProductData.push(data);

  return {
    ...state,
    specialProductData
  };
};

const handleDeleteNewRow = state => {
  let specialProductData = cloneDeep(state.specialProductData);
  specialProductData = specialProductData.filter(x => x.ProductId !== -1);

  return {
    ...state,
    specialProductData,
    specialProductDuplicateMBFTHDP: null
  };
};

const specialProductReducer = createReducer(initState, {
  [SPECIALPRODUCT_QUERY.PENDING]: handlePending,
  [SPECIALPRODUCT_QUERY.SUCCESS]: handleQuerySuccess,
  [SPECIALPRODUCT_QUERY.ERROR]: handleError,
  [SPECIALPRODUCT_UPDATEINFO.SUCCESS]: handleUpdateSuccess,
  [SPECIALPRODUCT_UPDATEINFO.ERROR]: handleError,
  [SPECIALPRODUCT_DELETE.SUCCESS]: handleDeleteSuccess,
  [SPECIALPRODUCT_DELETE.ERROR]: handleError,
  [EXECUTE_SPECIALPRODUCT_FILTER]: handleFilter,
  [ACTION_SPECIALPRODUCT.ADD]: handleAddNewRow,
  [ACTION_SPECIALPRODUCT.DELETE]: handleDeleteNewRow
});

export default specialProductReducer;
