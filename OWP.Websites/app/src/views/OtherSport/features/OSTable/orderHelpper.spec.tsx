import { orderBy } from './orderHelpper';

describe('orderHelper under unit test', () => {
  it('order by with sport correct', () => {
    const dataSource = [
      {
        osMatchId: 19,
        matchStatus: 'LIVE',
        sport: 'Basketball',
        dateTime: '2021-01-01T01:00:00',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: 'Asian 4',
        noHDP: '3HDP',
        matchOffer: '15s Accept',
        autoLink: 'YES',
        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        remark: '',
        iptv: '',
        matchNo: 1,
        reporter: '',
        traderMaxbetModels: []
      },
      {
        osMatchId: 20,
        matchStatus: 'LIVE',
        sport: 'CDBasketball',
        dateTime: '2021-02-01T01:00:00',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: 'Asian 4',
        noHDP: '3HDP',
        matchOffer: '15s Accept',
        autoLink: 'YES',
        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        remark: '',
        iptv: '',
        matchNo: 1,
        reporter: '',
        traderMaxbetModels: []
      }
    ];
    const columnKey = 'Sport';
    const orderType = 'ORDER_DESC';

    const expected = [
      {
        osMatchId: 20,
        matchStatus: 'LIVE',
        sport: 'CDBasketball',
        dateTime: '2021-02-01T01:00:00',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: 'Asian 4',
        noHDP: '3HDP',
        matchOffer: '15s Accept',
        autoLink: 'YES',
        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        remark: '',
        iptv: '',
        matchNo: 1,
        reporter: '',
        traderMaxbetModels: []
      },
      {
        osMatchId: 19,
        matchStatus: 'LIVE',
        sport: 'Basketball',
        dateTime: '2021-01-01T01:00:00',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: 'Asian 4',
        noHDP: '3HDP',
        matchOffer: '15s Accept',
        autoLink: 'YES',
        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        remark: '',
        iptv: '',
        matchNo: 1,
        reporter: '',
        traderMaxbetModels: []
      }
    ];

    const actual = orderBy(dataSource, columnKey, orderType);

    expect(actual).toEqual(expected);
  });

  it('order by with KOTime correct', () => {
    const dataSource = [
      {
        osMatchId: 19,
        matchStatus: 'LIVE',
        sport: 'Basketball',
        dateTime: '2021-01-01T01:00:00',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: 'Asian 4',
        noHDP: '3HDP',
        matchOffer: '15s Accept',
        autoLink: 'YES',
        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        remark: '',
        iptv: '',
        matchNo: 1,
        reporter: '',
        traderMaxbetModels: []
      },
      {
        osMatchId: 20,
        matchStatus: 'LIVE',
        sport: 'CDBasketball',
        dateTime: '2021-02-01T01:00:00',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: 'Asian 4',
        noHDP: '3HDP',
        matchOffer: '15s Accept',
        autoLink: 'YES',
        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        remark: '',
        iptv: '',
        matchNo: 1,
        reporter: '',
        traderMaxbetModels: []
      }
    ];
    const columnKey = 'KOTime';
    const orderType = 'ORDER_DESC';

    const expected = [
      {
        osMatchId: 20,
        matchStatus: 'LIVE',
        sport: 'CDBasketball',
        dateTime: '2021-02-01T01:00:00',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: 'Asian 4',
        noHDP: '3HDP',
        matchOffer: '15s Accept',
        autoLink: 'YES',
        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        remark: '',
        iptv: '',
        matchNo: 1,
        reporter: '',
        traderMaxbetModels: []
      },
      {
        osMatchId: 19,
        matchStatus: 'LIVE',
        sport: 'Basketball',
        dateTime: '2021-01-01T01:00:00',
        home: 'Dalian Pro FC (N)',
        away: 'Guangzhou R&F',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)',
        category: 'Asian 4',
        noHDP: '3HDP',
        matchOffer: '15s Accept',
        autoLink: 'YES',
        provider: 'TV/BGTV/Glive53',
        otherProvider: 'BGtv/RB2tv/BETtv/',
        service: 'InPlay',
        remark: '',
        iptv: '',
        matchNo: 1,
        reporter: '',
        traderMaxbetModels: []
      }
    ];

    const actual = orderBy(dataSource, columnKey, orderType);

    expect(actual).toEqual(expected);
  });
});
