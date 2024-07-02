import {
  selectLeagueGroup,
  selectTVList,
  selectMaxbetGeneralData,
  selectDisplayPopupFilter,
  selectTeamInfo
} from './SelectorMaxbetGeneral';

describe('test selector', () => {
  it('select TeamInfo', () => {
    const teamInfoReducer = {
      loading: 'idle',
      entity: []
    };
    expect(
      selectTeamInfo({
        teamInfoReducer
      })
    ).toEqual({
      loading: 'idle',
      entity: []
    });
  })

  it('select LeagueGroup ', () => {
    const leagueGroupReducer = {
      leagueGroupPending: true,
      leagueGroupOption: [],
      leagueGroupError: []
    };
    expect(
      selectLeagueGroup({
        leagueGroupReducer
      })
    ).toEqual({
      leagueGroupPending: true,
      leagueGroupOption: [
        {
          value: -1,
          label: 'All'
        },
        {
          value: 0,
          label: 'Unknown'
        }
      ],
      leagueGroupError: []
    });
  });

  it('select TVList ', () => {
    const tvListReducer = {
      tvListPending: true,
      tvListExpression: []
    };
    expect(
      selectTVList({
        tvListReducer
      })
    ).toEqual(tvListReducer);
  });

  it('select maxbetGenaralData ', () => {
    const maxbetGeneralReducer = {
      maxbetGeneralPending: true,
      maxbetGeneralData: [],
      maxbetGeneralError: [],
      filterInfo: []
    };
    expect(
      selectMaxbetGeneralData({
        maxbetGeneralReducer
      })
    ).toEqual(maxbetGeneralReducer);
  });

  it('select displayPopupFilter ', () => {
    const showHidePopupFilterReducer = {
      filterKey: 'league',
      isDisplay: true
    };
    expect(
      selectDisplayPopupFilter({
        showHidePopupFilterReducer
      })
    ).toEqual(showHidePopupFilterReducer);
  });
});
