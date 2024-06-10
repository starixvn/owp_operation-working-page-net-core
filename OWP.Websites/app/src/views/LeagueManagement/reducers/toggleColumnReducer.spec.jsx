import toggleColumnReducer from './toggleColumnReducer';
import { TOGGLE_COLUMN } from '../constants/action-type';

describe('toggleColumnReducer', () => {
  it('Should handle ToggleColumn', () => {
    const actual = toggleColumnReducer(undefined, {
      type: TOGGLE_COLUMN,
      payload: {
        columnKeys: ['LstNote', 'Remark', 'League']
      }
    });
    expect(actual).toEqual({ columnKeys: ['LstNote', 'Remark', 'League'] });
  });
});
