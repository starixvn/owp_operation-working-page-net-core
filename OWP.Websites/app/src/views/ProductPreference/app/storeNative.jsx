import { configureStore } from '@reduxjs/toolkit';
import { isEqual } from 'lodash';
import productReducer from '../features/productPreference/productSlice';
import filterReducer from '../features/filter/filterSlice';
import policyReducer from '../../Shared/permission/policySlice';

const store = configureStore({
  reducer: {
    policy: policyReducer,
    productAssignment: productReducer,
    filter: filterReducer
  },
  devTools: !isEqual(process.env.NODE_ENV, 'production')
});

export default store;
