import filterByKeyword from './filterByKeyword';

describe('filterByKeyword under test', () => {
  const mockArraySource = [
    {
      matchNo: '123',
      leagueName: 'Premier League',
      matchOffer: 'HDP',
      noHDP: '1.5',
      homeName: 'Man City',
      awayName: 'Liverpool',
      tv: 'TV1',
      iptv: 'IPTV1',
      channel: 'Channel1',
      wb: 'WB1',
      provider: 'Provider1',
      otherProvider: 'OtherProvider1',
      agent: 'Agent1',
      venue: 'Venue1',
      division: 'Division1',
      round: 'Round1',
      etpk: 'ETPK1',
      remark: 'Remark1',
      homeRemark: 'HomeRemark1',
      awayRemark: 'AwayRemark1',
      lstNote: 'Note1',
      infoSiteBlackList: ['blacklist1', 'blacklist2']
    },
    {
      matchNo: '456',
      leagueName: 'La Liga',
      matchOffer: 'OU',
      noHDP: '2.5',
      homeName: 'Barcelona',
      awayName: 'Real Madrid',
      tv: 'TV2',
      iptv: 'IPTV2',
      channel: 'Channel2',
      wb: 'WB2',
      provider: 'Provider2',
      otherProvider: 'OtherProvider2',
      agent: 'Agent2',
      venue: 'Venue2',
      division: 'Division2',
      round: 'Round2',
      etpk: 'ETPK2',
      remark: 'Remark2',
      homeRemark: 'HomeRemark2',
      awayRemark: 'AwayRemark2',
      lstNote: 'Note2',
      infoSiteBlackList: ['blacklist3']
    }
  ];

  describe('when keyword is not provided', () => {
    it('should return original array', () => {
      const result = filterByKeyword(mockArraySource, {});
      expect(result).toEqual(mockArraySource);
    });

    it('should return original array when keyword is undefined', () => {
      const result = filterByKeyword(mockArraySource, { keyword: undefined });
      expect(result).toEqual(mockArraySource);
    });
  });

  describe('filter by "No #" column', () => {
    it('should filter by matchNo', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: '123',
        columnKey: 'No #'
      });
      expect(result).toHaveLength(1);
      expect(result[0].matchNo).toBe('123');
    });

    it('should be case insensitive', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: '456',
        columnKey: 'No #'
      });
      expect(result).toHaveLength(1);
      expect(result[0].matchNo).toBe('456');
    });
  });

  describe('filter by "League" column', () => {
    it('should filter by leagueName', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Premier',
        columnKey: 'League'
      });
      expect(result).toHaveLength(1);
      expect(result[0].leagueName).toBe('Premier League');
    });

    it('should filter by matchOffer', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'HDP',
        columnKey: 'League'
      });
      expect(result).toHaveLength(1);
      expect(result[0].matchOffer).toBe('HDP');
    });

    it('should filter by noHDP', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: '1.5',
        columnKey: 'League'
      });
      expect(result).toHaveLength(1);
      expect(result[0].noHDP).toBe('1.5');
    });
  });

  describe('filter by "Match" column', () => {
    it('should filter by homeName', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Man City',
        columnKey: 'Match'
      });
      expect(result).toHaveLength(1);
      expect(result[0].homeName).toBe('Man City');
    });

    it('should filter by awayName', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Liverpool',
        columnKey: 'Match'
      });
      expect(result).toHaveLength(1);
      expect(result[0].awayName).toBe('Liverpool');
    });
  });

  describe('filter by "TV/IPTV" column', () => {
    it('should filter by tv', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'TV1',
        columnKey: 'TV/IPTV'
      });
      expect(result).toHaveLength(1);
      expect(result[0].tv).toBe('TV1');
    });

    it('should filter by iptv', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'IPTV2',
        columnKey: 'TV/IPTV'
      });
      expect(result).toHaveLength(1);
      expect(result[0].iptv).toBe('IPTV2');
    });

    it('should filter by channel', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Channel1',
        columnKey: 'TV/IPTV'
      });
      expect(result).toHaveLength(1);
      expect(result[0].channel).toBe('Channel1');
    });

    it('should filter by wb', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'WB2',
        columnKey: 'TV/IPTV'
      });
      expect(result).toHaveLength(1);
      expect(result[0].wb).toBe('WB2');
    });
  });

  describe('filter by "Provider" column', () => {
    it('should filter by provider', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Provider1',
        columnKey: 'Provider'
      });
      expect(result).toHaveLength(1);
      expect(result[0].provider).toBe('Provider1');
    });

    it('should filter by otherProvider', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'OtherProvider2',
        columnKey: 'Provider'
      });
      expect(result).toHaveLength(1);
      expect(result[0].otherProvider).toBe('OtherProvider2');
    });

    it('should filter by agent', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Agent1',
        columnKey: 'Provider'
      });
      expect(result).toHaveLength(1);
      expect(result[0].agent).toBe('Agent1');
    });

    it('should filter by venue', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Venue2',
        columnKey: 'Provider'
      });
      expect(result).toHaveLength(1);
      expect(result[0].venue).toBe('Venue2');
    });
  });

  describe('filter by "Round" column', () => {
    it('should filter by division', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Division1',
        columnKey: 'Round'
      });
      expect(result).toHaveLength(1);
      expect(result[0].division).toBe('Division1');
    });

    it('should filter by round', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Round2',
        columnKey: 'Round'
      });
      expect(result).toHaveLength(1);
      expect(result[0].round).toBe('Round2');
    });

    it('should filter by etpk', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'ETPK1',
        columnKey: 'Round'
      });
      expect(result).toHaveLength(1);
      expect(result[0].etpk).toBe('ETPK1');
    });
  });

  describe('filter by "Remark" column', () => {
    it('should filter by lstNote', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Note1',
        columnKey: 'Remark'
      });
      expect(result).toHaveLength(1);
      expect(result[0].lstNote).toBe('Note1');
    });

    it('should filter by remark', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'Remark2',
        columnKey: 'Remark'
      });
      expect(result).toHaveLength(1);
      expect(result[0].remark).toBe('Remark2');
    });

    it('should filter by homeRemark', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'HomeRemark1',
        columnKey: 'Remark'
      });
      expect(result).toHaveLength(1);
      expect(result[0].homeRemark).toBe('HomeRemark1');
    });

    it('should filter by awayRemark', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'AwayRemark2',
        columnKey: 'Remark'
      });
      expect(result).toHaveLength(1);
      expect(result[0].awayRemark).toBe('AwayRemark2');
    });

    it('should filter by infoSiteBlackList', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'blacklist1',
        columnKey: 'Remark'
      });
      expect(result).toHaveLength(1);
      expect(result[0].infoSiteBlackList).toContain('blacklist1');
    });
  });

  describe('when columnKey is not in columnFilter', () => {
    it('should return original array', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'test',
        columnKey: 'UnknownColumn'
      });
      expect(result).toEqual(mockArraySource);
    });
  });

  describe('when arraySource is empty', () => {
    it('should return empty array', () => {
      const result = filterByKeyword([], {
        keyword: 'test',
        columnKey: 'League'
      });
      expect(result).toEqual([]);
    });
  });

  describe('case insensitive filtering', () => {
    it('should filter case insensitively', () => {
      const result = filterByKeyword(mockArraySource, {
        keyword: 'MAN CITY',
        columnKey: 'Match'
      });
      expect(result).toHaveLength(1);
      expect(result[0].homeName).toBe('Man City');
    });
  });
});
