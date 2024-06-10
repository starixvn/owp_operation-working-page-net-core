import {
  calculateRowHight,
  buildClassNameHightLightMatch,
  filterHomeOrAwayByKeyword,
  findAndUpdatePropHidenTranderMaxbetByosMatchId,
  updatePropHidenTranderMaxbet,
  selectTable,
  buildStyleSchema
} from './osTableHelper';
import { DefaultTableStyle } from './TableSchema.json';

const { rowHeight } = DefaultTableStyle;

describe('osTableHelper under test', () => {
  it('should return zero assignments null', () => {
    const actual = calculateRowHight(null);
    expect(actual).toEqual(0);
  });

  it('should return default length when maxBets && assignments empty', () => {
    const actual = calculateRowHight({
      traderMaxbetModels: []
    });
    expect(actual).toEqual(rowHeight);
  });

  it('should return row height correct when length larger traderMaxbetModels', () => {
    const actual = calculateRowHight({
      traderMaxbetModels: [
        {
          betTypeName: '1H (HDP)',
          maxbet: {
            osMatchId: 19,
            betTypeName: '1H (HDP)',
            maxBet: 1001
          }
        },
        {
          betTypeName: '1H (OU)',
          maxbet: {
            osMatchId: 19,
            betTypeName: '1H (OU)',
            maxBet: 1001
          }
        }
      ]
    });

    expect(actual).toEqual(100);
  });

  it('when status = postponed, should return class name `yellow-row`', () => {
    expect(buildClassNameHightLightMatch({ matchStatus: 'postponed' })).toEqual(
      'yellow-row'
    );
  });

  it('when status = canceled, should return class name `yellow-row`', () => {
    expect(buildClassNameHightLightMatch({ matchStatus: 'canceled' })).toEqual(
      'yellow-row'
    );
  });

  it('when status = null, should not return class name', () => {
    expect(buildClassNameHightLightMatch()).toEqual('');
  });

  describe('filterHomeOrAwayBykeyword under test', () => {
    let testData = [];
    let expected = [];
    beforeEach(() => {
      testData = [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-04T01:00:00',
          home: 'Portal',
          homeName: 'Portal (N)',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ];

      expected = [
        {
          osMatchId: 10018,
          matchStatus: 'Live',
          sport: 'Basketball',
          dateTime: '2021-01-04T01:00:00',
          home: 'Portal',
          homeName: 'Portal (N)',
          away: 'LA Laker',
          league: 'EstherMiraaaaa23',
          category: '',
          noHDP: '3HDP',
          matchOffer: '15s Accept',
          autoLink: 'YES',
          provider: 'TV/BGTV/Glive53',
          otherProvider: 'BGtv/RB2tv/BETtv/',
          service: 'InPlay',
          iptv: 'Euro40',
          wb: 'IMG-B/Watch & Bet-160939/Glive-65',
          remark: '',
          traderMaxbetModels: [
            {
              betTypeName: 'FT (HDP)',
              traderAssignment: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                mainTrader: 'sa',
                assignmentNote: '#sa noted'
              },
              maxbet: {
                osMatchId: 10018,
                betTypeName: 'FT (HDP)',
                maxBet: 1000
              }
            }
          ]
        }
      ];
    });
    it('should return array origin when keyword empty', () => {
      const acutal = filterHomeOrAwayByKeyword(testData, {});
      expect(acutal).toEqual(expected);
    });

    it('should return array origin when keyword empty and column key', () => {
      const acutal = filterHomeOrAwayByKeyword(testData, 'OABC');
      expect(acutal).toEqual(expected);
    });

    it('should filter correct when league matched', () => {
      const acutal = filterHomeOrAwayByKeyword(
        testData,
        'Miraaaaa23'.toLocaleLowerCase()
      );
      expect(acutal).toEqual(expected);
    });

    it('should filter correct when home matched', () => {
      const acutal = filterHomeOrAwayByKeyword(testData, {
        keyword: 'Portal'.toLocaleLowerCase(),
        columnKey: 'Match'
      });
      expect(acutal).toEqual(expected);
    });

    it('should filter correct when away matched', () => {
      const acutal = filterHomeOrAwayByKeyword(testData, {
        keyword: 'Laker'.toLocaleLowerCase(),
        columnKey: 'Match'
      });
      expect(acutal).toEqual(expected);
    });

    it('should return empty array when league/home/away not match', () => {
      const acutal = filterHomeOrAwayByKeyword(testData, {
        keyword: 'Liam'.toLocaleLowerCase(),
        columnKey: 'Match'
      });
      expect(acutal).toEqual([]);
    });

    it('should find and update prop hiden TranderMaxbet by osMatchId', () => {
      const traderMaxbetModels = [
        {
          hiden: undefined,
          betTypeName: 'All Product (Set Winner)',
          maxbet: {
            betTypeName: 'All Product (Set Winner)',
            maxBet: null,
            osMatchId: 16339
          }
        },
        {
          hiden: undefined,
          betTypeName: 'All Product (Set Winner)1',
          maxbet: {
            betTypeName: 'All Product (Set Winner)1',
            maxBet: null,
            osMatchId: 16339
          }
        }
      ];
      expected = [
        {
          hiden: false,
          betTypeName: 'All Product (Set Winner)',
          maxbet: {
            betTypeName: 'All Product (Set Winner)',
            maxBet: null,
            osMatchId: 16339
          }
        },
        {
          hiden: true,
          betTypeName: 'All Product (Set Winner)1',
          maxbet: {
            betTypeName: 'All Product (Set Winner)1',
            maxBet: null,
            osMatchId: 16339
          }
        }
      ];
      updatePropHidenTranderMaxbet(0, traderMaxbetModels, true);
      expect(traderMaxbetModels).toEqual(expected);
    });
    it('should find and update prop hiden TranderMaxbet by osMatchId', () => {
      const dataSource = [
        {
          autoLink: 'NO',
          away: 'Guangzhou R&F95',
          category: 'Asian 18',
          dateTime: '2021-03-04T15:30:00',
          home: 'Dalian Pro FC (N)95',
          iptv: 'Euro54',
          league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)95',
          matchOffer: '26s Accept',
          matchStatus: 'Live',
          noHDP: '14HDP',
          osMatchId: 16339,
          otherProvider: 'BGtv/RB2tv/BETtv/',
          provider: 'TV/BGtv/Glive61',
          remark: 'test',
          service: 'InPlay',
          sport: 'Badminton',
          traderMaxbetModels: [
            {
              hiden: undefined,
              betTypeName: 'All Product (Set Winner)',
              maxbet: {
                betTypeName: 'All Product (Set Winner)',
                maxBet: null,
                osMatchId: 16339
              }
            },
            {
              hiden: undefined,
              betTypeName: 'All Product (Set Winner)1',
              maxbet: {
                betTypeName: 'All Product (Set Winner)1',
                maxBet: null,
                osMatchId: 16339
              }
            }
          ],
          wb: 'Glive-40'
        }
      ];
      expected = [
        {
          autoLink: 'NO',
          away: 'Guangzhou R&F95',
          category: 'Asian 18',
          dateTime: '2021-03-04T15:30:00',
          home: 'Dalian Pro FC (N)95',
          iptv: 'Euro54',
          league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)95',
          matchOffer: '26s Accept',
          matchStatus: 'Live',
          noHDP: '14HDP',
          osMatchId: 16339,
          otherProvider: 'BGtv/RB2tv/BETtv/',
          provider: 'TV/BGtv/Glive61',
          remark: 'test',
          service: 'InPlay',
          sport: 'Badminton',
          traderMaxbetModels: [
            {
              hiden: false,
              betTypeName: 'All Product (Set Winner)',
              maxbet: {
                betTypeName: 'All Product (Set Winner)',
                maxBet: null,
                osMatchId: 16339
              }
            },
            {
              hiden: true,
              betTypeName: 'All Product (Set Winner)1',
              maxbet: {
                betTypeName: 'All Product (Set Winner)1',
                maxBet: null,
                osMatchId: 16339
              }
            }
          ],
          wb: 'Glive-40'
        }
      ];
      findAndUpdatePropHidenTranderMaxbetByosMatchId(16339, dataSource, true);
      expect(dataSource).toEqual(expected);
    });
  });

  describe('selectTable under unit test', () => {
    it('create correct columnshema', () => {
      const columnSchema = [
        {
          headerName: 'No #',
          columnKey: 'No',
          isCanFilter: undefined,
          isCanSort: undefined,
          isCanCollapseExpand: undefined,
          width: 35.2
        },
        {
          headerName: 'KOTime',
          columnKey: 'KOTime',
          isCanFilter: undefined,
          isCanSort: true,
          isCanCollapseExpand: undefined,
          width: 47.519999999999996
        },
        {
          headerName: 'Sport',
          columnKey: 'Sport',
          isCanFilter: undefined,
          isCanSort: true,
          isCanCollapseExpand: undefined,
          width: 56.32
        },
        {
          headerName: 'League',
          columnKey: 'League',
          isCanFilter: true,
          isCanSort: undefined,
          isCanCollapseExpand: undefined,
          width: 114.4
        },
        {
          headerName: 'Match',
          columnKey: 'Match',
          isCanFilter: true,
          isCanSort: undefined,
          isCanCollapseExpand: undefined,
          width: 140.8
        },
        {
          headerName: 'Remark',
          columnKey: 'Remark',
          isCanFilter: undefined,
          isCanSort: undefined,
          isCanCollapseExpand: undefined,
          width: 70.4
        },
        {
          headerName: 'Trader',
          columnKey: 'Trader',
          isCanFilter: undefined,
          isCanSort: undefined,
          isCanCollapseExpand: true,
          width: 167.2
        },
        {
          headerName: 'MaxBet',
          columnKey: 'MaxBet',
          isCanFilter: undefined,
          isCanSort: undefined,
          isCanCollapseExpand: undefined,
          width: 61.60000000000001
        },
        {
          headerName: 'Spread',
          columnKey: 'Spread',
          isCanFilter: undefined,
          isCanSort: undefined,
          isCanCollapseExpand: undefined,
          width: 61.60000000000001
        },
        {
          headerName: 'IPTV/Streaming',
          columnKey: 'IPTvStreaming',
          isCanFilter: undefined,
          isCanSort: undefined,
          isCanCollapseExpand: undefined,
          width: 61.60000000000001
        },
        {
          headerName: 'Provider',
          columnKey: 'Provider',
          isCanFilter: undefined,
          isCanSort: undefined,
          isCanCollapseExpand: undefined,
          width: 61.60000000000001
        }
      ];
      const table = selectTable(800);

      expect(table.columnSchema).toEqual(columnSchema);
    });

    it('create buildStyleSchema', () => {
      const expected = {
        rowHeight: 100,
        width: 1200,
        defaultHeight: 1200,
        height: 1200,
        maxHeight: 1200,
        headerHeight: 38
      };
      const actual = buildStyleSchema('largePlus');
      expect(actual).toEqual(expected);
    });
  });
});
