import { AnyAction, Dispatch } from '@reduxjs/toolkit';
import { post } from '../../Shared/Service/Service';
import updateNoOfferMatchThunk from './updateNoOfferMatchThunkNative';
import { TNoOfferMatch } from '../types/noOfferMatchType';
import { UPDATE_NO_OFFER_MATCH } from '../const/const.json';
import { netCoreURLEventPicking } from '../const/apiUrl';

jest.mock('../../Shared/Service/Service', () => ({
  post: jest.fn()
}));

const mockedPost = post as jest.Mock;

describe('updateNoOfferMatchThunk under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should dispatch fulfilled action with noOfferMatch on success', async () => {
    const mockNoOfferMatch: TNoOfferMatch[] = [
      {
        infoSiteMatchId: 1,
        childMatchId: null,
        isMovedEventDate: false,
        isCheck: true,
        isNoOffer: true
      },
      {
        infoSiteMatchId: 2,
        childMatchId: 3,
        isMovedEventDate: true,
        isCheck: false,
        isNoOffer: false
      }
    ];

    mockedPost.mockResolvedValueOnce({ status: 200 });

    const dispatch = jest.fn() as Dispatch<AnyAction>;
    const getState = jest.fn();
    const thunkAction = updateNoOfferMatchThunk(mockNoOfferMatch);
    const result = await thunkAction(dispatch, getState, undefined);

    expect(dispatch).toHaveBeenCalledWith(
      expect.objectContaining({ type: 'noOfferMatch/update/pending' })
    );
    expect(dispatch).toHaveBeenCalledWith(
      expect.objectContaining({
        type: 'noOfferMatch/update/fulfilled',
        payload: mockNoOfferMatch
      })
    );
    expect(result.payload).toEqual(mockNoOfferMatch);
    expect(result.type).toBe('noOfferMatch/update/fulfilled');
  });

  it('should call post with correct URL and data', async () => {
    const mockNoOfferMatch: TNoOfferMatch[] = [
      {
        infoSiteMatchId: 1,
        childMatchId: null,
        isMovedEventDate: false,
        isCheck: true,
        isNoOffer: true
      }
    ];

    mockedPost.mockResolvedValueOnce({ status: 200 });

    const dispatch = jest.fn() as Dispatch<AnyAction>;
    const getState = jest.fn();
    const thunkAction = updateNoOfferMatchThunk(mockNoOfferMatch);
    await thunkAction(dispatch, getState, undefined);

    const expectedUrl = `${netCoreURLEventPicking}/${UPDATE_NO_OFFER_MATCH}`;
    expect(mockedPost).toHaveBeenCalledWith({
      url: expectedUrl,
      noOfferMatch: mockNoOfferMatch
    });
  });

  it('should handle empty array', async () => {
    const mockNoOfferMatch: TNoOfferMatch[] = [];

    mockedPost.mockResolvedValueOnce({ status: 200 });

    const dispatch = jest.fn() as Dispatch<AnyAction>;
    const getState = jest.fn();
    const thunkAction = updateNoOfferMatchThunk(mockNoOfferMatch);
    const result = await thunkAction(dispatch, getState, undefined);

    expect(mockedPost).toHaveBeenCalledWith({
      url: `${netCoreURLEventPicking}/${UPDATE_NO_OFFER_MATCH}`,
      noOfferMatch: []
    });
    expect(result.payload).toEqual([]);
    expect(result.type).toBe('noOfferMatch/update/fulfilled');
  });

  it('should handle multiple noOfferMatch items', async () => {
    const mockNoOfferMatch: TNoOfferMatch[] = [
      {
        infoSiteMatchId: 1,
        childMatchId: null,
        isMovedEventDate: false,
        isCheck: true,
        isNoOffer: true
      },
      {
        infoSiteMatchId: 2,
        childMatchId: 10,
        isMovedEventDate: false,
        isCheck: false,
        isNoOffer: false
      },
      {
        infoSiteMatchId: 3,
        childMatchId: 20,
        isMovedEventDate: true,
        isCheck: true,
        isNoOffer: true
      }
    ];

    mockedPost.mockResolvedValueOnce({ status: 200 });

    const dispatch = jest.fn() as Dispatch<AnyAction>;
    const getState = jest.fn();
    const thunkAction = updateNoOfferMatchThunk(mockNoOfferMatch);
    const result = await thunkAction(dispatch, getState, undefined);

    expect(result.payload).toHaveLength(3);
    expect(result.payload).toEqual(mockNoOfferMatch);
    expect(result.type).toBe('noOfferMatch/update/fulfilled');
  });
});
