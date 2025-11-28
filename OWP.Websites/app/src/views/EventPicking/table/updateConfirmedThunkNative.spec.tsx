import { AnyAction, Dispatch } from '@reduxjs/toolkit';
import { post } from '../../Shared/Service/Service';
import updateConfirmedThunk from './updateConfirmedThunkNative';
import { TConfirmed } from '../types/confirmed';
import { UPDATE_CONFIRMED } from '../const/const.json';
import { netCoreURLEventPicking } from '../const/apiUrl';

jest.mock('../../Shared/Service/Service', () => ({
  post: jest.fn()
}));

const mockedPost = post as jest.Mock;

describe('updateConfirmedThunk under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should dispatch fulfilled action with confirmInfo on success', async () => {
    const mockConfirmInfo: TConfirmed = {
      infoSiteMatchIds: [1, 2, 3],
      confirmed: true,
      timestampConfirm: 1234567890
    };

    mockedPost.mockResolvedValueOnce({ status: 200 });

    const dispatch = jest.fn() as Dispatch<AnyAction>;
    const getState = jest.fn();
    const thunkAction = updateConfirmedThunk(mockConfirmInfo);
    const result = await thunkAction(dispatch, getState, undefined);

    expect(dispatch).toHaveBeenCalledWith(
      expect.objectContaining({ type: 'confirmed/update/pending' })
    );
    expect(dispatch).toHaveBeenCalledWith(
      expect.objectContaining({
        type: 'confirmed/update/fulfilled',
        payload: mockConfirmInfo
      })
    );
    expect(result.payload).toEqual(mockConfirmInfo);
    expect(result.type).toBe('confirmed/update/fulfilled');
  });

  it('should call post with correct URL and data', async () => {
    const mockConfirmInfo: TConfirmed = {
      infoSiteMatchIds: [1, 2, 3],
      confirmed: true,
      timestampConfirm: 1234567890
    };

    mockedPost.mockResolvedValueOnce({ status: 200 });

    const dispatch = jest.fn() as Dispatch<AnyAction>;
    const getState = jest.fn();
    const thunkAction = updateConfirmedThunk(mockConfirmInfo);
    await thunkAction(dispatch, getState, undefined);

    const expectedUrl = `${netCoreURLEventPicking}/${UPDATE_CONFIRMED}`;
    expect(mockedPost).toHaveBeenCalledWith({
      url: expectedUrl,
      infoSiteMatchIds: '1,2,3',
      confirmed: true,
      timestampConfirm: 1234567890
    });
  });

  it('should handle single infoSiteMatchId', async () => {
    const mockConfirmInfo: TConfirmed = {
      infoSiteMatchIds: [5],
      confirmed: false,
      timestampConfirm: 9876543210
    };

    mockedPost.mockResolvedValueOnce({ status: 200 });

    const dispatch = jest.fn() as Dispatch<AnyAction>;
    const getState = jest.fn();
    const thunkAction = updateConfirmedThunk(mockConfirmInfo);
    await thunkAction(dispatch, getState, undefined);

    expect(mockedPost).toHaveBeenCalledWith(
      expect.objectContaining({
        infoSiteMatchIds: '5'
      })
    );
  });

  it('should handle empty infoSiteMatchIds array', async () => {
    const mockConfirmInfo: TConfirmed = {
      infoSiteMatchIds: [],
      confirmed: true,
      timestampConfirm: 1111111111
    };

    mockedPost.mockResolvedValueOnce({ status: 200 });

    const dispatch = jest.fn() as Dispatch<AnyAction>;
    const getState = jest.fn();
    const thunkAction = updateConfirmedThunk(mockConfirmInfo);
    await thunkAction(dispatch, getState, undefined);

    expect(mockedPost).toHaveBeenCalledWith(
      expect.objectContaining({
        infoSiteMatchIds: ''
      })
    );
  });
});
