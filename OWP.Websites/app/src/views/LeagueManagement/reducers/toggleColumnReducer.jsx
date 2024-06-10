import { createReducer } from './reducerUtilities';
import { TOGGLE_COLUMN } from '../constants/action-type';

const defaultModel = {
  columnKeys: null
};
const handleToggle = (state, action) => {
  return {
    ...state,
    columnKeys: action.payload.columnKeys
  };
};

const toggleColumnReducer = createReducer(defaultModel, {
  [TOGGLE_COLUMN]: handleToggle
});

export default toggleColumnReducer;
