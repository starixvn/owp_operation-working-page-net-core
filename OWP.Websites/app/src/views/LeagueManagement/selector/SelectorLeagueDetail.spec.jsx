import {
  selectLeagueDetail,
  selectToggleColum,
  selectCondition
} from './SelectorLeagueDetail';
import { v4 as uuidv4 } from 'uuid';
const tableKey = uuidv4();
describe('test selector', () => {
  it('select selectLeagueDetail ', () => {
    const state = {
      leagueDetailReducer: {
        leagueDetailPending: true,
        leagueDetailData: [],
        tableKey
      }
    };
    expect(selectLeagueDetail(state)).toEqual({
      leagueDetailPending: true,
      leagueDetailData: [],
      tableKey
    });
  });

  it('select selectToggleColum ', () => {
    const state = {
      toggleColumnReducer: {
        columnKeys: []
      }
    };
    expect(selectToggleColum(state)).toEqual({
      toggleColumnLoading: false,
      columnKeys: []
    });
  });

  it('select selectCondition ', () => {
    const state = {
      conditionReducer: {
        conditionPending: true,
        leagueSettingId: 123,
        tv: {
          HasTV: true,
          TV: 'abc'
        },
        round: [],
        division: []
      }
    };
    expect(selectCondition(state)).toEqual({
      conditionPending: true,
      leagueSettingId: 123,
      tv: { HasTV: true, TV: 'abc' },
      round: [],
      division: []
    });
  });
});
