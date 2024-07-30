import orderArrayByTime from './orderArrayByTime';
import { direction } from './orderConst.json';

const { asc, desc } = direction;

describe('orderArrayByTime under test', () => {
  it('should return array original when order is not detected', () => {
    const arraySource = [{
      autoLink: 'NO',
      away: 'Guangzhou R&F5',
      category: 'Asian 8',
      dateTime: '2021-03-02T15:30:00',
      home: 'Dalian Pro FC (N)5',
      iptv: 'Euro44',
      league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
      matchOffer: '16s Accept',
      matchStatus: 'Live',
      noHDP: '4HDP',
      osMatchId: 30377,
      otherProvider: 'BGtv/RB2tv/BETtv/',
      provider: 'TV/BGtv/Glive51',
      remark: 'test',
      service: 'InPlay',
      sport: 'Other Sports'
    }, {
      autoLink: 'NO',
      away: 'Guangzhou R&F5',
      category: 'Asian 8',
      dateTime: '2021-03-03T15:30:00',
      home: 'Dalian Pro FC (N)5',
      iptv: 'Euro44',
      league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
      matchOffer: '16s Accept',
      matchStatus: 'Live',
      noHDP: '4HDP',
      osMatchId: 30377,
      otherProvider: 'BGtv/RB2tv/BETtv/',
      provider: 'TV/BGtv/Glive51',
      remark: 'test',
      service: 'InPlay',
      sport: 'Other Sports'
    }];
    const actual = orderArrayByTime(arraySource, 'dateTime', '');
    const expected = [
      {
        autoLink: 'NO',
        away: 'Guangzhou R&F5',
        category: 'Asian 8',
        dateTime: '2021-03-02T15:30:00',
        home: 'Dalian Pro FC (N)5',
        iptv: 'Euro44',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
        matchOffer: '16s Accept',
        matchStatus: 'Live',
        noHDP: '4HDP',
        osMatchId: 30377,
        otherProvider: 'BGtv/RB2tv/BETtv/',
        provider: 'TV/BGtv/Glive51',
        remark: 'test',
        service: 'InPlay',
        sport: 'Other Sports'
      },
      {
        autoLink: 'NO',
        away: 'Guangzhou R&F5',
        category: 'Asian 8',
        dateTime: '2021-03-03T15:30:00',
        home: 'Dalian Pro FC (N)5',
        iptv: 'Euro44',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
        matchOffer: '16s Accept',
        matchStatus: 'Live',
        noHDP: '4HDP',
        osMatchId: 30377,
        otherProvider: 'BGtv/RB2tv/BETtv/',
        provider: 'TV/BGtv/Glive51',
        remark: 'test',
        service: 'InPlay',
        sport: 'Other Sports'
      }
    ];
    expect(actual).toEqual(expected);
  });

  it('should return array order by DESC when orderType=DESC', () => {
    const arraySource = [{
      autoLink: 'NO',
      away: 'Guangzhou R&F5',
      category: 'Asian 8',
      dateTime: '2021-03-02T15:30:00',
      home: 'Dalian Pro FC (N)5',
      iptv: 'Euro44',
      league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
      matchOffer: '16s Accept',
      matchStatus: 'Live',
      noHDP: '4HDP',
      osMatchId: 30377,
      otherProvider: 'BGtv/RB2tv/BETtv/',
      provider: 'TV/BGtv/Glive51',
      remark: 'test',
      service: 'InPlay',
      sport: 'Other Sports'
    }, {
      autoLink: 'NO',
      away: 'Guangzhou R&F5',
      category: 'Asian 8',
      dateTime: '2021-03-03T15:30:00',
      home: 'Dalian Pro FC (N)5',
      iptv: 'Euro44',
      league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
      matchOffer: '16s Accept',
      matchStatus: 'Live',
      noHDP: '4HDP',
      osMatchId: 30377,
      otherProvider: 'BGtv/RB2tv/BETtv/',
      provider: 'TV/BGtv/Glive51',
      remark: 'test',
      service: 'InPlay',
      sport: 'Other Sports'
    }];
    const actual = orderArrayByTime(arraySource, 'dateTime', desc);
    const expected = [
      {
        autoLink: 'NO',
        away: 'Guangzhou R&F5',
        category: 'Asian 8',
        dateTime: '2021-03-03T15:30:00',
        home: 'Dalian Pro FC (N)5',
        iptv: 'Euro44',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
        matchOffer: '16s Accept',
        matchStatus: 'Live',
        noHDP: '4HDP',
        osMatchId: 30377,
        otherProvider: 'BGtv/RB2tv/BETtv/',
        provider: 'TV/BGtv/Glive51',
        remark: 'test',
        service: 'InPlay',
        sport: 'Other Sports'
      },
      {
        autoLink: 'NO',
        away: 'Guangzhou R&F5',
        category: 'Asian 8',
        dateTime: '2021-03-02T15:30:00',
        home: 'Dalian Pro FC (N)5',
        iptv: 'Euro44',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
        matchOffer: '16s Accept',
        matchStatus: 'Live',
        noHDP: '4HDP',
        osMatchId: 30377,
        otherProvider: 'BGtv/RB2tv/BETtv/',
        provider: 'TV/BGtv/Glive51',
        remark: 'test',
        service: 'InPlay',
        sport: 'Other Sports'
      }
    ];
    expect(actual).toEqual(expected);
  });

  it('should return array order by ASC when orderType=ASC', () => {
    const arraySource = [{
      autoLink: 'NO',
      away: 'Guangzhou R&F5',
      category: 'Asian 8',
      dateTime: '2021-03-02T15:30:00',
      home: 'Dalian Pro FC (N)5',
      iptv: 'Euro44',
      league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
      matchOffer: '16s Accept',
      matchStatus: 'Live',
      noHDP: '4HDP',
      osMatchId: 30377,
      otherProvider: 'BGtv/RB2tv/BETtv/',
      provider: 'TV/BGtv/Glive51',
      remark: 'test',
      service: 'InPlay',
      sport: 'Other Sports'
    }, {
      autoLink: 'NO',
      away: 'Guangzhou R&F5',
      category: 'Asian 8',
      dateTime: '2021-03-03T15:30:00',
      home: 'Dalian Pro FC (N)5',
      iptv: 'Euro44',
      league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
      matchOffer: '16s Accept',
      matchStatus: 'Live',
      noHDP: '4HDP',
      osMatchId: 30377,
      otherProvider: 'BGtv/RB2tv/BETtv/',
      provider: 'TV/BGtv/Glive51',
      remark: 'test',
      service: 'InPlay',
      sport: 'Other Sports'
    }];
    const actual = orderArrayByTime(arraySource, 'dateTime', asc);
    const expected = [
      {
        autoLink: 'NO',
        away: 'Guangzhou R&F5',
        category: 'Asian 8',
        dateTime: '2021-03-02T15:30:00',
        home: 'Dalian Pro FC (N)5',
        iptv: 'Euro44',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
        matchOffer: '16s Accept',
        matchStatus: 'Live',
        noHDP: '4HDP',
        osMatchId: 30377,
        otherProvider: 'BGtv/RB2tv/BETtv/',
        provider: 'TV/BGtv/Glive51',
        remark: 'test',
        service: 'InPlay',
        sport: 'Other Sports'
      },
      {
        autoLink: 'NO',
        away: 'Guangzhou R&F5',
        category: 'Asian 8',
        dateTime: '2021-03-03T15:30:00',
        home: 'Dalian Pro FC (N)5',
        iptv: 'Euro44',
        league: 'CHINA FOOTBALL SUPER LEAGUE (PLAY OFF)5',
        matchOffer: '16s Accept',
        matchStatus: 'Live',
        noHDP: '4HDP',
        osMatchId: 30377,
        otherProvider: 'BGtv/RB2tv/BETtv/',
        provider: 'TV/BGtv/Glive51',
        remark: 'test',
        service: 'InPlay',
        sport: 'Other Sports'
      }
    ];
    expect(actual).toEqual(expected);
  });
});
