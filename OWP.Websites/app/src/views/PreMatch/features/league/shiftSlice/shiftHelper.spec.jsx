import {
  getUnreadMessagePreMatchLeagueIds,
  mergeMatchesToLeagues,
  changeExpandStateAllLeague,
  changeExpandStateOneLeague,
  sortLeaguesByColumnKey,
  changeOpenNoteStateOneLeague,
  changeOpenNoteStateOneMatch,
  updateLeagueData,
  pasteCopied,
  sortLeaguesByDefault
} from './shiftHelper';

describe('shiftHelper under test', () => {
  it('getUnreadMessagePreMatchLeagueIds under test, get InfoSiteMatchId must hide', () => {
    const matches = [
      {
        PreMatchLeagueId: 1,
        InfoSiteMatchId: 99,
        HasMessages: false,
        IsRead: false
      },
      {
        PreMatchLeagueId: 1,
        InfoSiteMatchId: 98,
        HasMessages: true,
        IsRead: true
      }
    ];
    const leagues = [
      { PreMatchLeagueId: 1, HasMessages: true, IsRead: false },
      { PreMatchLeagueId: 2, HasMessages: false, IsRead: true }
    ];
    const { InfoSiteMatchId } = getUnreadMessagePreMatchLeagueIds(
      matches,
      leagues
    );
    const InfoSiteMatchIdsMustHide = [99, 98];
    expect(InfoSiteMatchId).toEqual(InfoSiteMatchIdsMustHide);
  });
  it('getUnreadMessagePreMatchLeagueIds under test, get PreMatchLeagueIds must show', () => {
    const matches = [
      {
        PreMatchLeagueId: 98,
        InfoSiteMatchId: 2,
        HasMessages: true,
        IsRead: false
      }
    ];
    const leagues = [
      { PreMatchLeagueId: 98, HasMessages: true, IsRead: false },
      { PreMatchLeagueId: 99, HasMessages: true, IsRead: false }
    ];
    const { PreMatchLeagueIds } = getUnreadMessagePreMatchLeagueIds(
      matches,
      leagues
    );
    const PreMatchLeagueIdsMustShow = [98, 99];
    expect(PreMatchLeagueIds).toEqual(PreMatchLeagueIdsMustShow);
  });

  it('mergeMatchesToLeagues under test, merge Match to League have isExpand: true', () => {
    const matches = [
      {
        PreMatchLeagueId: 98,
        InfoSiteMatchId: 2,
        HasMessages: true,
        IsRead: false
      },
      {
        PreMatchLeagueId: 99,
        InfoSiteMatchId: 2,
        HasMessages: true,
        IsRead: false
      }
    ];
    const leagues = [
      {
        PreMatchLeagueId: 98,
        HasMessages: true,
        IsRead: false,
        isExpand: true
      },
      { PreMatchLeagueId: 99, HasMessages: true, IsRead: false }
    ];

    const newArray = [
      {
        PreMatchLeagueId: 98,
        HasMessages: true,
        IsRead: false,
        isExpand: true
      },
      {
        PreMatchLeagueId: 98,
        InfoSiteMatchId: 2,
        HasMessages: true,
        IsRead: false
      },
      { PreMatchLeagueId: 99, HasMessages: true, IsRead: false }
    ];

    const result = mergeMatchesToLeagues(matches, leagues);
    expect(result).toEqual(newArray);
  });

  it('changeExpandStateAllLeague under test,  should change expand state All League ', () => {
    const leagues = [
      {
        PreMatchLeagueId: 98,
        HasMessages: true,
        IsRead: false,
        isExpand: true
      },
      { PreMatchLeagueId: 99, HasMessages: true, IsRead: false }
    ];

    const newArray = [
      {
        PreMatchLeagueId: 98,
        HasMessages: true,
        IsRead: false,
        isExpand: false
      },
      {
        PreMatchLeagueId: 99,
        HasMessages: true,
        IsRead: false,
        isExpand: false
      }
    ];
    changeExpandStateAllLeague(leagues, false);
    expect(leagues).toEqual(newArray);
  });
  it('changeExpandStateOneLeague under test, should change expand state One League ', () => {
    const leagues = [
      {
        PreMatchLeagueId: 98,
        HasMessages: true,
        IsRead: false,
        isExpand: true,
        MatchCount: 1
      },
      { PreMatchLeagueId: 99, HasMessages: true, IsRead: false, MatchCount: 1 }
    ];

    const newArray = [
      {
        PreMatchLeagueId: 98,
        HasMessages: true,
        IsRead: false,
        isExpand: true,
        MatchCount: 1
      },
      { PreMatchLeagueId: 99, HasMessages: true, IsRead: false, MatchCount: 1 }
    ];
    changeExpandStateOneLeague(leagues, 98, true);
    expect(leagues).toEqual(newArray);
  });
  it('sortLeagues under test, should sort Ascending HDP column  ', () => {
    const leagues = [
      { PreMatchLeagueId: 99, HDP: null, MatchCount: 1 },
      { PreMatchLeagueId: 98, HDP: 'z', MatchCount: 1 },
      { PreMatchLeagueId: 100, HDP: 'b', MatchCount: 1 }
    ];

    const newArray = [
      { PreMatchLeagueId: 99, HDP: null, MatchCount: 1 },
      { PreMatchLeagueId: 100, HDP: 'b', MatchCount: 1 },
      { PreMatchLeagueId: 98, HDP: 'z', MatchCount: 1 }
    ];
    const result = sortLeaguesByColumnKey(leagues, 'HDP', 'ORDER_ASC');
    expect(result).toEqual(newArray);
  });
  it('sortLeagues under test, should sort Descending HDP column  ', () => {
    const leagues = [
      { PreMatchLeagueId: 99, HDP: null, MatchCount: 1 },
      { PreMatchLeagueId: 98, HDP: 'z', MatchCount: 1 },
      { PreMatchLeagueId: 100, HDP: 'b', MatchCount: 1 }
    ];

    const newArray = [
      { PreMatchLeagueId: 99, HDP: null, MatchCount: 1 },
      { PreMatchLeagueId: 98, HDP: 'z', MatchCount: 1 },
      { PreMatchLeagueId: 100, HDP: 'b', MatchCount: 1 }
    ];
    const result = sortLeaguesByColumnKey(leagues, 'HDP');
    expect(result).toEqual(newArray);
  });

  it('changeOpenNoteStateOneLeague under test, should change openState of Note League ', () => {
    const leagues = [
      { PreMatchLeagueId: 98, MatchCount: 1 },
      { PreMatchLeagueId: 99, MatchCount: 1 }
    ];
    const matches = [
      { PreMatchLeagueId: 98, InfoSiteMatchId: 1 },
      { PreMatchLeagueId: 99, InfoSiteMatchId: 2 }
    ];

    const newArray = [
      { PreMatchLeagueId: 98, MatchCount: 1, isOpenNote: true },
      { PreMatchLeagueId: 99, MatchCount: 1, isOpenNote: false }
    ];
    changeOpenNoteStateOneLeague(leagues, matches, 98, true);
    expect(leagues).toEqual(newArray);
  });
  it('changeOpenNoteStateOneMatch under test, should change openState of Note League ', () => {
    const leagues = [
      { PreMatchLeagueId: 98, MatchCount: 1 },
      { PreMatchLeagueId: 99, MatchCount: 1 }
    ];
    const matches = [
      { PreMatchLeagueId: 98, InfoSiteMatchId: 1 },
      { PreMatchLeagueId: 99, InfoSiteMatchId: 2 }
    ];

    const newArray = [
      { PreMatchLeagueId: 98, InfoSiteMatchId: 1, isOpenNote: true },
      { PreMatchLeagueId: 99, InfoSiteMatchId: 2, isOpenNote: false }
    ];
    changeOpenNoteStateOneMatch(leagues, matches, 1, true);
    expect(matches).toEqual(newArray);
  });
  it('updateLeagueData under test, should change openState of Note League ', () => {
    const newLeague = {
      index: 0,
      objectKey: 'HDP',
      newValue: 'sa'
    };
    const leagues = [
      { PreMatchLeagueId: 98, MatchCount: 1, HDP: 'esther' },
      { PreMatchLeagueId: 99, MatchCount: 1, HDP: 'carlos' }
    ];
    const entities = [
      { PreMatchLeagueId: 98, MatchCount: 1, HDP: 'esther' },
      { PreMatchLeagueId: 99, MatchCount: 1, HDP: 'carlos' }
    ];
    const prematchIdUpdated = [];

    const updatedLeagues = [
      { PreMatchLeagueId: 98, MatchCount: 1, HDP: 'sa' },
      { PreMatchLeagueId: 99, MatchCount: 1, HDP: 'carlos' }
    ];

    const updatedEntities = [
      { PreMatchLeagueId: 98, MatchCount: 1, HDP: 'sa' },
      { PreMatchLeagueId: 99, MatchCount: 1, HDP: 'carlos' }
    ];
    updateLeagueData(leagues, entities, prematchIdUpdated, newLeague);
    expect(leagues).toEqual(updatedLeagues);
    expect(entities).toEqual(updatedEntities);
  });
  it('pasteCopied under test, should change openState of Note League ', () => {
    const newLeague = {
      rowIndex: 0,
      isTakeOver: true,
      copiedValue: [
        {
          value: {
            MainAssignee1: 'Esther',
            MainAssignee2: 'Jack',
            OtherAssignee1: 'Mira',
            OtherAssignee2: 'Carlos'
          }
        }
      ]
    };
    const leagues = [
      { PreMatchLeagueId: 98, HDP: 'esther' },
      { PreMatchLeagueId: 99, HDP: 'carlos' }
    ];
    const entities = [
      { PreMatchLeagueId: 98, HDP: 'esther' },
      { PreMatchLeagueId: 99, HDP: 'carlos' }
    ];
    const prematchIdUpdated = [];

    const updatedLeagues = [
      {
        PreMatchLeagueId: 98,
        HDP: 'esther',
        HDPTakeOver: 'Esther',
        OUTakeOver: 'Jack',
        OtherHDPTakeOver: 'Mira',
        OtherOUTakeOver: 'Carlos'
      },
      { PreMatchLeagueId: 99, HDP: 'carlos' }
    ];

    const updatedEntities = [
      {
        PreMatchLeagueId: 98,
        HDP: 'esther',
        HDPTakeOver: 'Esther',
        OUTakeOver: 'Jack',
        OtherHDPTakeOver: 'Mira',
        OtherOUTakeOver: 'Carlos'
      },
      { PreMatchLeagueId: 99, HDP: 'carlos' }
    ];
    pasteCopied(leagues, entities, prematchIdUpdated, newLeague);
    expect(leagues).toEqual(updatedLeagues);
    expect(entities).toEqual(updatedEntities);
  });

  it('sortLeaguesByDefault under test, should sort Ascending HDP, OU, HDPTakeOver, OUTakeOver column  ', () => {
    const leagues = [
      {
        PreMatchLeagueId: 98,
        HDP: 'sa',
        OU: 'starix1',
        HDPTakeOver: null,
        OUTakeOver: null
      },
      {
        PreMatchLeagueId: 99,
        HDP: null,
        OU: null,
        HDPTakeOver: null,
        OUTakeOver: null
      },
      {
        PreMatchLeagueId: 100,
        HDP: 'sa',
        OU: 'starix1',
        HDPTakeOver: 'haha',
        OUTakeOver: null
      }
    ];

    const newArray = [
      {
        PreMatchLeagueId: 99,
        HDP: null,
        OU: null,
        HDPTakeOver: null,
        OUTakeOver: null
      },
      {
        PreMatchLeagueId: 100,
        HDP: 'sa',
        OU: 'starix1',
        HDPTakeOver: 'haha',
        OUTakeOver: null
      },
      {
        PreMatchLeagueId: 98,
        HDP: 'sa',
        OU: 'starix1',
        HDPTakeOver: null,
        OUTakeOver: null
      }
    ];
    const result = sortLeaguesByDefault(leagues);
    expect(result).toEqual(newArray);
  });
});
