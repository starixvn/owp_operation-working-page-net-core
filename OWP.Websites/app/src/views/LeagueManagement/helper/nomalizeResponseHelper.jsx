import Utils from '../../Shared/Utils';
const { convertThousandToK, convertSpreadToPercent, convertToPercent } = Utils;

const convertSpreadToPercentWrapper = value =>
  value === 0 ? '0%' : convertSpreadToPercent(value, true);

const convertThousandToKWrapper = value =>
  value === 0 ? '0K' : convertThousandToK(value, true);

const nomalizeMaxbetSpreadResponse = dataSource => {
  return dataSource.map(item => {
    return {
      ...item,
      GroupValue: item.GroupValue ?? 0,
      GroupLabel: item.GroupLabel ?? '',
      MinSpread: convertSpreadToPercentWrapper(item.MinSpread),
      MaxSpread: convertSpreadToPercentWrapper(item.MaxSpread),
      MinMaxbetWD: convertThousandToKWrapper(item.MinMaxbetWD),
      MaxMaxbetWD: convertThousandToKWrapper(item.MaxMaxbetWD),
      MinMaxbetWK: convertThousandToKWrapper(item.MinMaxbetWK),
      MaxMaxbetWK: convertThousandToKWrapper(item.MaxMaxbetWK),
      MinFT1X2: convertThousandToKWrapper(item.MinFT1X2),
      MaxFT1X2: convertThousandToKWrapper(item.MaxFT1X2)
    };
  });
};

const nomalizeArraySpecialProductMaxbetSpreadResponse = dataSource =>
  dataSource.map(item => nomalizeSpecialProductMaxbetSpreadResponse(item));

const normalizeMaxbet = maxbet =>
  maxbet ? convertThousandToK(maxbet, false) : null;

const normalizeSpread = spread => (spread ? convertToPercent(spread) : null);

const nomalizeSpecialProductMaxbetSpreadResponse = dataSource => {
  return {
    ...dataSource,
    Maxbet_10_15Mins: normalizeMaxbet(dataSource.Maxbet_10_15Mins),
    Maxbet_FTHDP: normalizeMaxbet(dataSource.Maxbet_FTHDP),
    Maxbet_NextCorner: normalizeMaxbet(dataSource.Maxbet_NextCorner),
    Maxbet_NextTB: normalizeMaxbet(dataSource.Maxbet_NextTB),
    Maxbet_NoOfCorner: normalizeMaxbet(dataSource.Maxbet_NoOfCorner),
    Maxbet_PS: normalizeMaxbet(dataSource.Maxbet_PS),
    Maxbet_TB: normalizeMaxbet(dataSource.Maxbet_TB),
    Maxbet_Winner: normalizeMaxbet(dataSource.Maxbet_Winner),
    Spread_10_15Mins: normalizeSpread(dataSource.Spread_10_15Mins),
    Spread_NextCorner: normalizeSpread(dataSource.Spread_NextCorner),
    Spread_NextTB: normalizeSpread(dataSource.Spread_NextTB),
    Spread_NoOfCorner: normalizeSpread(dataSource.Spread_NoOfCorner),
    Spread_PS: normalizeSpread(dataSource.Spread_PS),
    Spread_TB: normalizeSpread(dataSource.Spread_TB),
    Spread_Winner: normalizeSpread(dataSource.Spread_Winner)
  };
};

export {
  nomalizeMaxbetSpreadResponse,
  nomalizeArraySpecialProductMaxbetSpreadResponse,
  nomalizeSpecialProductMaxbetSpreadResponse
};
