import editSlice, { changeMode } from './editSlice';

describe('editSlice under test', () => {
  it('should return the initial state', () => {
    expect(editSlice(undefined, {})).toEqual({
      isEdit: false
    });
  });

  describe('changeMode under test', () => {
    it('should handle changeMode reducer', () => {
      const initialState = {
        isEdit: false
      };

      const expectState = {
        isEdit: true
      };

      const action = {
        type: changeMode.type,
        payload: { isEdit: true }
      };

      const state = editSlice(initialState, action);

      expect(state).toEqual(expectState);
    });
  });
});
