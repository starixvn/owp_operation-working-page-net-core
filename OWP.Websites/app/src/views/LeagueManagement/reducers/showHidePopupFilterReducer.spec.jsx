import { UPDATE_STATUS_DISPLAY_POPUP_FILTER } from '../constants/action-type';
import showHidePopupFilterReducer from './showHidePopupFilterReducer';

describe('showHidePopupFilter reducer', () => {
  it('should return the initial state', () => {
    expect(showHidePopupFilterReducer(undefined, {})).toEqual({
      filterKey: '',
      isDisplay: false
    });
  });

  it('should handle UPDATE_STATUS_DISPLAY_POPUP_FILTER', () => {
    expect(
      showHidePopupFilterReducer(undefined, {
        type: UPDATE_STATUS_DISPLAY_POPUP_FILTER,
        payload: {
          filterKey: 'League',
          isDisplay: true
        }
      })
    ).toEqual({
      filterKey: 'League',
      isDisplay: true
    });
  });
});
