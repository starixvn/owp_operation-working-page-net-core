import quickMessageReducer, {
  setEditIndex,
  closeEditMode,
  setEditModel,
  addQuickMessage,
  deleteAction
} from './quickMessageSlice';
import { TQuickMessage } from '../../../Type';
import { REQUEST_STATUS } from '../../../../Shared/Constants';
import { queryQuickMessageThunk } from './quickMessageThunk';

describe('quickMessageSlice', () => {
  const initialState = {
    editIndex: -1,
    quickMessage: [] as Array<TQuickMessage>,
    editModel: {} as TQuickMessage,
    isCreating: false,
    loading: REQUEST_STATUS.pending,
  };

  test('should handle initial state', () => {
    expect(quickMessageReducer(undefined, { type: 'unknown' })).toEqual(initialState);
  });

  test('should handle setEditIndex', () => {
    const previousState = {
      ...initialState,
      quickMessage: [{ id: 1, shortcut: '/a', message: 'Message A' }],
    };
    const action = setEditIndex(0);
    const nextState = quickMessageReducer(previousState, action);
    expect(nextState.editIndex).toEqual(0);
    expect(nextState.editModel).toEqual(previousState.quickMessage[0]);
  });

  test('should handle closeEditMode', () => {
    const previousState = {
      ...initialState,
      editIndex: 0,
      isCreating: true,
      quickMessage: [{ id: 1, shortcut: '/a', message: 'Message A' }],
    };
    const action = closeEditMode();
    const nextState = quickMessageReducer(previousState, action);
    expect(nextState.editIndex).toEqual(-1);
    expect(nextState.quickMessage).toEqual([]);
  });

  test('should handle setEditModel', () => {
    const previousState = {
      ...initialState,
      editModel: { id: 1, shortcut: '/a', message: 'Message A' },
    };
    const action = setEditModel({ property: 'message', value: 'Updated Message' });
    const nextState = quickMessageReducer(previousState, action);
    expect(nextState.editModel.message).toEqual('Updated Message');
  });

  test('should handle addQuickMessage', () => {
    const previousState = {
      ...initialState,
      quickMessage: [{ id: 1, shortcut: '/a', message: 'Message A' }],
    };
    const action = addQuickMessage();
    const nextState = quickMessageReducer(previousState, action);
    expect(nextState.editIndex).toEqual(1);
    expect(nextState.editModel).toEqual({ id: 0, shortcut: '', message: '' });
    expect(nextState.quickMessage).toHaveLength(2);
    expect(nextState.isCreating).toEqual(true);
  });

  test('should handle queryQuickMessageThunk.pending', () => {
    const action = { type: queryQuickMessageThunk.pending.type };
    const nextState = quickMessageReducer(initialState, action);
    expect(nextState.loading).toEqual(REQUEST_STATUS.pending);
  });

  test('should handle queryQuickMessageThunk.fulfilled', () => {
    const mockPayload = [{ id: 1, shortcut: '/a', message: 'Message A' }];
    const action = { type: queryQuickMessageThunk.fulfilled.type, payload: mockPayload };
    const nextState = quickMessageReducer(initialState, action);
    expect(nextState.loading).toEqual(REQUEST_STATUS.idle);
    expect(nextState.quickMessage).toEqual(mockPayload);
  });

  test('should handle queryQuickMessageThunk.rejected', () => {
    const action = { type: queryQuickMessageThunk.rejected.type };
    const nextState = quickMessageReducer(initialState, action);
    expect(nextState.loading).toEqual(REQUEST_STATUS.pending);
  });
});
