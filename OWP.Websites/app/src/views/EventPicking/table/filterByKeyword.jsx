const checkLeague = (arraySource, keyWordFormat) =>
  arraySource.filter(
    item =>
      item.leagueName?.toLowerCase().includes(keyWordFormat) ||
      item.matchOffer?.toLowerCase().includes(keyWordFormat) ||
      item.noHDP?.toLowerCase().includes(keyWordFormat)
  );

const checkMatch = (arraySource, keyWordFormat) =>
  arraySource.filter(
    item =>
      item.homeName?.toLowerCase().includes(keyWordFormat) ||
      item.awayName?.toLowerCase().includes(keyWordFormat)
  );

const checkIpTV = (arraySource, keyWordFormat) =>
  arraySource.filter(
    item =>
      item.tv?.toLowerCase().includes(keyWordFormat) ||
      item.iptv?.toLowerCase().includes(keyWordFormat) ||
      item.channel?.toLowerCase().includes(keyWordFormat) ||
      item.wb?.toLowerCase().includes(keyWordFormat)
  );

const checkProvider = (arraySource, keyWordFormat) =>
  arraySource.filter(
    item =>
      item.provider?.toLowerCase().includes(keyWordFormat) ||
      item.otherProvider?.toLowerCase().includes(keyWordFormat) ||
      item.agent?.toLowerCase().includes(keyWordFormat) ||
      item.venue?.toLowerCase().includes(keyWordFormat)
  );

const checkRound = (arraySource, keyWordFormat) =>
  arraySource.filter(
    item =>
      item.division?.toLowerCase().includes(keyWordFormat) ||
      item.round?.toLowerCase().includes(keyWordFormat) ||
      item.etpk?.toLowerCase().includes(keyWordFormat)
  );

const checkRemark = (arraySource, keyWordFormat) => {
  const remark = (item, keyword) =>
    item.remark?.toLowerCase().includes(keyword);

  const matchRemark = (item, keyword) =>
    item.homeRemark?.toLowerCase().includes(keyword) ||
    item.awayRemark?.toLowerCase().includes(keyword);

  const blackListRemark = (item, keyword) =>
    item.infoSiteBlackList.some(x => x.toLowerCase().includes(keyword));

  return arraySource.filter(
    item =>
      item.lstNote?.toLowerCase().includes(keyWordFormat) ||
      remark(item, keyWordFormat) ||
      matchRemark(item, keyWordFormat) ||
      blackListRemark(item, keyWordFormat)
  );
};

const columnFilter = {
  'League': checkLeague,
  'Match': checkMatch,
  'TV/IPTV': checkIpTV,
  'Provider': checkProvider,
  'Round': checkRound,
  'Remark': checkRemark
};

const filterByKeyword = (
  arraySource,
  { keyword = undefined, columnKey = undefined }
) => {
  if (!keyword) {
    return arraySource;
  }
  if (columnKey in columnFilter) {
    const keyWordFormat = keyword.toLowerCase();
    return columnFilter[columnKey](arraySource, keyWordFormat);
  }

  return arraySource || [];
};

export default filterByKeyword;
