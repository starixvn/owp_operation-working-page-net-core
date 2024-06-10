import { createReducer } from './reducerUtilities';
import { UPDATE_STATUS_DISPLAY_POPUP_FILTER } from '../constants/action-type';

const defaultModel = {
  filterKey: '',
  isDisplay: false
};

const handleUpdateStatus = (state, action) => {
  const { filterKey, isDisplay } = action.payload;
  return {
    ...state,
    filterKey,
    isDisplay
  };
};

const showHidePopupFilterReducer = createReducer(defaultModel, {
  [UPDATE_STATUS_DISPLAY_POPUP_FILTER]: handleUpdateStatus
});

export default showHidePopupFilterReducer;
