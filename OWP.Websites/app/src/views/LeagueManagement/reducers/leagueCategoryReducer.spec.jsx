import { ACTION_CATEGORY_FILTER } from '../constants/action-type';
import leagueCategoryReducer from './leagueCategoryReducer';

describe('leagueCategory reducer', () => {
  it('should return the initial state', () => {
    expect(leagueCategoryReducer(undefined, {})).toEqual({
      leagueCategory: { value: 1, label: 'Live' }
    });
  });

  it('should handle ACTION_CATEGORY_FILTER.UPDATE', () => {
    expect(
      leagueCategoryReducer(undefined, {
        type: ACTION_CATEGORY_FILTER.UPDATE,
        payload: {
          select: { value: 2, label: 'Today' }
        }
      })
    ).toEqual({
      leagueCategory: { value: 2, label: 'Today' }
    });
  });
});
