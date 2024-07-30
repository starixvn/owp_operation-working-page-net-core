import { AnyAction, Dispatch } from '@reduxjs/toolkit';
import { get, post } from '../../../../Shared/Service/Service';
import { deleteQuickMessageThunk, queryQuickMessageThunk, saveQuickMessageThunk } from './quickMessageThunk';

jest.mock('../../../../Shared/Service/Service', () => ({
  get: jest.fn(),
  post: jest.fn(),
}));

const mockedGet = get as jest.Mock;
const mockedPost = post as jest.Mock;

describe('quickMessage thunks', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  describe('queryQuickMessageThunk', () => {
    test('dispatches fulfilled action with response data on success', async () => {
      const mockData = [{ id: 1, shortcut: '/a', message: 'Message A' }];
      mockedGet.mockResolvedValueOnce({ data: mockData });

      const dispatch = jest.fn() as Dispatch<AnyAction>;
      const getState = jest.fn();
      const thunkAction = queryQuickMessageThunk();
      const result = await thunkAction(dispatch, getState, undefined);

      expect(dispatch).toHaveBeenCalledWith(expect.objectContaining({ type: 'quickMessage/query/pending' }));
      expect(dispatch).toHaveBeenCalledWith(expect.objectContaining({ type: 'quickMessage/query/fulfilled', payload: mockData }));
    });
  });

  describe('saveQuickMessageThunk', () => {
    test('dispatches fulfilled action with status on success', async () => {
      const mockModel = { id: 1, shortcut: '/a', message: 'Message A' };
      const mockStatus = 200;
      mockedPost.mockResolvedValueOnce({ status: mockStatus });

      const dispatch = jest.fn() as Dispatch<AnyAction>;
      const getState = jest.fn();
      const thunkAction = saveQuickMessageThunk(mockModel);
      const result = await thunkAction(dispatch, getState, undefined);

      expect(dispatch).toHaveBeenCalledWith(expect.objectContaining({ type: 'quickMessage/save/pending' }));
      expect(dispatch).toHaveBeenCalledWith(expect.objectContaining({ type: 'quickMessage/save/fulfilled', payload: mockStatus }));
    });
  });

  describe('deleteQuickMessageThunk', () => {
    test('dispatches fulfilled action with status on success', async () => {
      const mockModel = { id: 1 };
      const mockStatus = 200;
      mockedPost.mockResolvedValueOnce({ status: mockStatus });

      const dispatch = jest.fn() as Dispatch<AnyAction>;
      const getState = jest.fn();
      const thunkAction = deleteQuickMessageThunk(mockModel);
      const result = await thunkAction(dispatch, getState, undefined);

      expect(dispatch).toHaveBeenCalledWith(expect.objectContaining({ type: 'quickMessage/delete/pending' }));
      expect(dispatch).toHaveBeenCalledWith(expect.objectContaining({ type: 'quickMessage/delete/fulfilled', payload: mockStatus }));
    });
  });
});
