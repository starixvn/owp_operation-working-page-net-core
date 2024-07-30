import { uniqBy } from 'lodash';
import {
  MajorBetTypes,
  MinorBetTypes,
  AssignmentTypes
} from '../../Shared/Constants';

const convertThousandToK = number => {
  const oneThousand = 1000;
  if (number < 1) {
    return number;
  }
  return `${(number / oneThousand).toString()}K`;
};

const convertSpreadToPercent = number => {
  if (!number) {
    return number;
  }
  const percentNumber = 100;
  const parseFloatToFixed = Number.parseFloat(number * percentNumber).toFixed(
    2
  );
  const outputFormat = Number.parseFloat(parseFloatToFixed);
  return `(${outputFormat}%)`;
};

const initializeLiveAssignmentItem = () => {
  return {
    BetTypeId: 0,
    MinorBettypeId: 0,
    Name: '',
    OWPMatchId: 0,
    MaxBetFT: { MaxBet: '', Spread: '' },
    MaxBetHT: { MaxBet: '', Spread: '' },
    FT: { main: {}, others: [], isAllowAdd: true },
    HT: { main: {}, others: [], isAllowAdd: true },
    LA: { main: {}, others: [], isAllowAdd: true }
  };
};

const initializeAssignment = (assignmentType, bettype, minorBettypeId) => {
  return {
    AssignmentId: -1,
    AssignmentNote: null,
    AssignmentType: assignmentType,
    Bettype: bettype,
    OWPMatchId: null,
    DisplayName: null,
    MinorBettypeId: minorBettypeId,
    UserId: null,
    Username: null
  };
};

const hasSettings = matchSetting => matchSetting.Maxbet || matchSetting.Spread;

const isHasMatchSetting = matchSetting =>
  matchSetting &&
  hasSettings(matchSetting) &&
  matchSetting.IsFromMaxbetSetting === false;

const isHasAssignment = (mainAssignment, otherAssignment) =>
  mainAssignment || otherAssignment.length > 0;

const getMatchSettingFT = (matchSettings, minorBettypeId) =>
  matchSettings.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.Handicap
  );

const getMatchSettingHT = (matchSettings, minorBettypeId) =>
  matchSettings.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.Handicap1H
  );

const getMainAssignmentFT = (assignments, minorBettypeId) =>
  assignments.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.Handicap &&
      x.AssignmentType === AssignmentTypes.Main
  );

const getOtherAssignmentFT = (assignments, minorBettypeId) =>
  assignments.filter(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.Handicap &&
      x.AssignmentType === AssignmentTypes.Other
  );

const getMainAssignmentHT = (assignments, minorBettypeId) =>
  assignments.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.Handicap1H &&
      x.AssignmentType === AssignmentTypes.Main
  );

const getOtherAssignmentHT = (assignments, minorBettypeId) =>
  assignments.filter(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.Handicap1H &&
      x.AssignmentType === AssignmentTypes.Other
  );

const getIsHasFTOrHTMatchSetting = (matchSettings, minorBettypeId) => {
  const matchSettingFT = getMatchSettingFT(matchSettings, minorBettypeId);
  const matchSettingHT = getMatchSettingHT(matchSettings, minorBettypeId);
  return isHasMatchSetting(matchSettingFT) || isHasMatchSetting(matchSettingHT);
};

const getIsHasFTOrHTAssignment = (assignments, minorBettypeId) => {
  const otherAssignmentFT = getOtherAssignmentFT(assignments, minorBettypeId);
  const mainAssignmentHT = getMainAssignmentHT(assignments, minorBettypeId);
  const otherAssignmentHT = getOtherAssignmentHT(assignments, minorBettypeId);
  const mainAssignmentFT = getMainAssignmentFT(assignments, minorBettypeId);
  return (
    isHasAssignment(mainAssignmentFT, otherAssignmentFT) ||
    isHasAssignment(mainAssignmentHT, otherAssignmentHT)
  );
};

const getCheckMinorBettype = minorBettypeId =>
  minorBettypeId === 0 ||
  minorBettypeId === MinorBetTypes.FastMarket ||
  minorBettypeId === MinorBetTypes.AllInOne;

const getMaxBeSpread = (matchSettingFT, item, matchSettingHT) => {
  const result = item;

  if (matchSettingFT) {
    result.Name = matchSettingFT.DisplayName ? matchSettingFT.DisplayName : '';
    result.OWPMatchId = matchSettingFT.OWPMatchId
      ? matchSettingFT.OWPMatchId
      : 0;
    result.MaxBetFT.MaxBet = convertThousandToK(matchSettingFT.Maxbet);
    result.MaxBetFT.Spread = convertSpreadToPercent(matchSettingFT.Spread);
  }
  if (matchSettingHT) {
    result.MaxBetHT.MaxBet = convertThousandToK(matchSettingHT.Maxbet);
    result.MaxBetHT.Spread = convertSpreadToPercent(matchSettingHT.Spread);
  }

  return result;
};

const getHDPRow = (minorBettypeId, matchSettings, assignments) => {
  const isHasFTOrHTMatchSetting = getIsHasFTOrHTMatchSetting(
    matchSettings,
    minorBettypeId
  );
  const isHasFTOrHTAssignment = getIsHasFTOrHTAssignment(
    assignments,
    minorBettypeId
  );
  const checkMinorBettype = getCheckMinorBettype(minorBettypeId);
  const matchSettingFT = getMatchSettingFT(matchSettings, minorBettypeId);
  const matchSettingHT = getMatchSettingHT(matchSettings, minorBettypeId);
  const mainAssignmentFT = getMainAssignmentFT(assignments, minorBettypeId);
  const otherAssignmentFT = getOtherAssignmentFT(assignments, minorBettypeId);
  const mainAssignmentHT = getMainAssignmentHT(assignments, minorBettypeId);
  const otherAssignmentHT = getOtherAssignmentHT(assignments, minorBettypeId);
  if (
    !(isHasFTOrHTMatchSetting || isHasFTOrHTAssignment || checkMinorBettype)
  ) {
    return null;
  }

  let item = initializeLiveAssignmentItem();
  item.BetTypeId = MajorBetTypes.Handicap;
  item.MinorBettypeId = minorBettypeId;
  item = getMaxBeSpread(matchSettingFT, item, matchSettingHT);

  item.FT.main =
    mainAssignmentFT ||
    initializeAssignment(
      AssignmentTypes.Main,
      MajorBetTypes.Handicap,
      minorBettypeId
    );
  item.FT.others =
    otherAssignmentFT.length > 0
      ? otherAssignmentFT
      : [
          initializeAssignment(
            AssignmentTypes.Other,
            MajorBetTypes.Handicap,
            minorBettypeId
          )
        ];
  item.HT.main =
    mainAssignmentHT ||
    initializeAssignment(
      AssignmentTypes.Main,
      MajorBetTypes.Handicap1H,
      minorBettypeId
    );
  item.HT.others =
    otherAssignmentHT.length > 0
      ? otherAssignmentHT
      : [
          initializeAssignment(
            AssignmentTypes.Other,
            MajorBetTypes.Handicap1H,
            minorBettypeId
          )
        ];

  return item;
};

const updateLiveAssignmentMaxbetFT = (model, matchSettingFT) => {
  model.MaxBetFT.MaxBet = convertThousandToK(matchSettingFT.Maxbet);
  model.MaxBetFT.Spread = convertSpreadToPercent(matchSettingFT.Spread);
};

const updateLiveAssignmentMaxbetHT = (model, matchSettingHT) => {
  model.MaxBetHT.MaxBet = convertThousandToK(matchSettingHT.Maxbet);
  model.MaxBetHT.Spread = convertSpreadToPercent(matchSettingHT.Spread);
};

const updateGeneralInfoLiveAssignment = (model, info) => {
  const { minorBettypeId, displayName, OWPMatchId } = info;
  model.MinorBettypeId = minorBettypeId;
  model.Name = displayName;
  model.OWPMatchId = OWPMatchId;
};

const getOURow = (minorBettypeId, matchSettings, assignments) => {
  const matchSettingFT = matchSettings.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OverUnder
  );
  const matchSettingHT = matchSettings.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OverUnder1H
  );
  const mainAssignmentFT = assignments.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OverUnder &&
      x.AssignmentType === AssignmentTypes.Main
  );
  const otherAssignmentFT = assignments.filter(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OverUnder &&
      x.AssignmentType === AssignmentTypes.Other
  );
  const mainAssignmentHT = assignments.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OverUnder1H &&
      x.AssignmentType === AssignmentTypes.Main
  );
  const otherAssignmentHT = assignments.filter(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OverUnder1H &&
      x.AssignmentType === AssignmentTypes.Other
  );

  const isHasFTOrHTMatchSetting =
    isHasMatchSetting(matchSettingFT) || isHasMatchSetting(matchSettingHT);
  const isHasFTOrHTAssignment =
    isHasAssignment(mainAssignmentFT, otherAssignmentFT) ||
    isHasAssignment(mainAssignmentHT, otherAssignmentHT);

  if (
    !(isHasFTOrHTMatchSetting || isHasFTOrHTAssignment || minorBettypeId === 0)
  ) {
    return null;
  }

  const item = initializeLiveAssignmentItem();
  item.BetTypeId = MajorBetTypes.OverUnder;
  const generalInfo = {
    minorBettypeId,
    displayName: matchSettingFT.DisplayName,
    OWPMatchId: matchSettingFT.OWPMatchId
  };
  updateGeneralInfoLiveAssignment(item, generalInfo);
  updateLiveAssignmentMaxbetFT(item, matchSettingFT);
  updateLiveAssignmentMaxbetHT(item, matchSettingHT);
  item.FT.main =
    mainAssignmentFT ||
    initializeAssignment(
      AssignmentTypes.Main,
      MajorBetTypes.OverUnder,
      minorBettypeId
    );
  item.FT.others =
    otherAssignmentFT.length > 0
      ? otherAssignmentFT
      : [
          initializeAssignment(
            AssignmentTypes.Other,
            MajorBetTypes.OverUnder,
            minorBettypeId
          )
        ];
  item.HT.main =
    mainAssignmentHT ||
    initializeAssignment(
      AssignmentTypes.Main,
      MajorBetTypes.OverUnder1H,
      minorBettypeId
    );
  item.HT.others =
    otherAssignmentHT.length > 0
      ? otherAssignmentHT
      : [
          initializeAssignment(
            AssignmentTypes.Other,
            MajorBetTypes.OverUnder1H,
            minorBettypeId
          )
        ];

  return item;
};

const get1X2Row = (minorBettypeId, matchSettings, assignments) => {
  const matchSettingFT = matchSettings.find(
    x =>
      x.MinorBettypeId === minorBettypeId && x.Bettype === MajorBetTypes.OneXTwo
  );
  const matchSettingHT = matchSettings.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OneXTwo1H
  );
  const mainAssignmentFT = assignments.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OneXTwo &&
      x.AssignmentType === AssignmentTypes.Main
  );
  const otherAssignmentFT = assignments.filter(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OneXTwo &&
      x.AssignmentType === AssignmentTypes.Other
  );
  const mainAssignmentHT = assignments.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OneXTwo1H &&
      x.AssignmentType === AssignmentTypes.Main
  );
  const otherAssignmentHT = assignments.filter(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.OneXTwo1H &&
      x.AssignmentType === AssignmentTypes.Other
  );

  const isHasFTOrHTMatchSetting =
    isHasMatchSetting(matchSettingFT) || isHasMatchSetting(matchSettingHT);
  const isHasFTOrHTAssignment =
    isHasAssignment(mainAssignmentFT, otherAssignmentFT) ||
    isHasAssignment(mainAssignmentHT, otherAssignmentHT);

  if (
    !(isHasFTOrHTMatchSetting || isHasFTOrHTAssignment || minorBettypeId === 0)
  ) {
    return null;
  }

  const item = initializeLiveAssignmentItem();
  item.BetTypeId = MajorBetTypes.OneXTwo;
  const generalInfo = {
    minorBettypeId,
    displayName: matchSettingFT.DisplayName,
    OWPMatchId: matchSettingFT.OWPMatchId
  };
  updateGeneralInfoLiveAssignment(item, generalInfo);
  updateLiveAssignmentMaxbetFT(item, matchSettingFT);
  updateLiveAssignmentMaxbetHT(item, matchSettingHT);

  item.FT.main =
    mainAssignmentFT ||
    initializeAssignment(
      AssignmentTypes.Main,
      MajorBetTypes.OneXTwo,
      minorBettypeId
    );
  item.FT.others =
    otherAssignmentFT.length > 0
      ? otherAssignmentFT
      : [
          initializeAssignment(
            AssignmentTypes.Other,
            MajorBetTypes.OneXTwo,
            minorBettypeId
          )
        ];
  item.HT.main =
    mainAssignmentHT ||
    initializeAssignment(
      AssignmentTypes.Main,
      MajorBetTypes.OneXTwo1H,
      minorBettypeId
    );
  item.HT.others =
    otherAssignmentHT.length > 0
      ? otherAssignmentHT
      : [
          initializeAssignment(
            AssignmentTypes.Other,
            MajorBetTypes.OneXTwo1H,
            minorBettypeId
          )
        ];

  return item;
};

const getEmptyHDPRow = (minorBettypeId, matchSettings) => {
  const matchSettingFT = matchSettings.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.Handicap
  );
  const item = initializeLiveAssignmentItem();
  item.BetTypeId = MajorBetTypes.Handicap;
  item.MinorBettypeId = minorBettypeId;

  if (matchSettingFT) {
    item.Name = matchSettingFT.DisplayName;
    item.OWPMatchId = matchSettingFT.OWPMatchId;
  }

  item.FT.main = initializeAssignment(
    AssignmentTypes.Main,
    MajorBetTypes.Handicap,
    minorBettypeId
  );
  item.FT.others = [
    initializeAssignment(
      AssignmentTypes.Other,
      MajorBetTypes.Handicap,
      minorBettypeId
    )
  ];
  item.HT.main = initializeAssignment(
    AssignmentTypes.Main,
    MajorBetTypes.Handicap1H,
    minorBettypeId
  );
  item.HT.others = [
    initializeAssignment(
      AssignmentTypes.Other,
      MajorBetTypes.Handicap1H,
      minorBettypeId
    )
  ];

  return item;
};

const getMainAssignmentLA = (minorBettypeId, assignments) => {
  return assignments.find(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.LA &&
      x.AssignmentType === AssignmentTypes.Main
  );
};

const getOtherAssignmentLA = (minorBettypeId, assignments) => {
  return assignments.filter(
    x =>
      x.MinorBettypeId === minorBettypeId &&
      x.Bettype === MajorBetTypes.LA &&
      x.AssignmentType === AssignmentTypes.Other
  );
};

const getAssignmentLA = (minorBettypeId, mainLA, othersLA) => {
  return {
    main:
      mainLA ||
      initializeAssignment(
        AssignmentTypes.Main,
        MajorBetTypes.LA,
        minorBettypeId
      ),
    others:
      othersLA.length > 0
        ? othersLA
        : [
            initializeAssignment(
              AssignmentTypes.Other,
              MajorBetTypes.LA,
              minorBettypeId
            )
          ],
    isAllowAdd: true
  };
};

const getDataForGroupMajor = (matchSettings, assignments) => {
  const groupName = 'Major';
  const listAssignmentItem = [];
  const dataHDPRow = getHDPRow(0, matchSettings, assignments);
  dataHDPRow.LA = getAssignmentLA(
    0,
    getMainAssignmentLA(0, assignments),
    getOtherAssignmentLA(0, assignments)
  );
  const dataOURow = getOURow(0, matchSettings, assignments);
  const data1X2Row = get1X2Row(0, matchSettings, assignments);
  const data15minsRow = getHDPRow(
    MinorBetTypes.Specific15mins,
    matchSettings,
    assignments
  );
  const data10minsRow = getHDPRow(
    MinorBetTypes.Specific10mins,
    matchSettings,
    assignments
  );

  listAssignmentItem.push(dataHDPRow);
  listAssignmentItem.push(dataOURow);
  listAssignmentItem.push(data1X2Row);
  if (data15minsRow) {
    data15minsRow.HT.isAllowAdd = false;
    data15minsRow.LA.isAllowAdd = false;
    listAssignmentItem.push(data15minsRow);
  }

  if (data10minsRow) {
    data10minsRow.HT.isAllowAdd = false;
    data10minsRow.LA.isAllowAdd = false;
    listAssignmentItem.push(data10minsRow);
  }

  return {
    Group: groupName,
    DataSource: listAssignmentItem,
    TotalRows: listAssignmentItem.length
  };
};

const getDataForGroupNoOfCorners = (matchSettings, assignments) => {
  const groupName = 'NoOfCorners';
  const dataNoOfCornersHDP = getHDPRow(
    MinorBetTypes.NoOfCorners,
    matchSettings,
    assignments
  );
  const dataNoOfCornersOU = getOURow(
    MinorBetTypes.NoOfCorners,
    matchSettings,
    assignments
  );
  const dataNoOfCorners1X2 = get1X2Row(
    MinorBetTypes.NoOfCorners,
    matchSettings,
    assignments
  );
  const data15minsNoOfCorners = getHDPRow(
    MinorBetTypes.Specific15minsNoOfCorners,
    matchSettings,
    assignments
  );

  const dataAllInOneNoOfCorners = getHDPRow(
    MinorBetTypes.AllInOneCorners,
    matchSettings,
    assignments
  );

  const dataFastMarketNoOfCorners = getHDPRow(
    MinorBetTypes.FastMarketCorners,
    matchSettings,
    assignments
  );

  let listAssignmentItem = [
    dataNoOfCornersHDP,
    dataNoOfCornersOU,
    dataNoOfCorners1X2
  ];

  listAssignmentItem = listAssignmentItem.filter(x => x !== null);

  const mainLA = getMainAssignmentLA(MinorBetTypes.NoOfCorners, assignments);
  const othersLA = getOtherAssignmentLA(MinorBetTypes.NoOfCorners, assignments);
  if (listAssignmentItem.length > 0) {
    listAssignmentItem[0].LA = getAssignmentLA(
      MinorBetTypes.NoOfCorners,
      mainLA,
      othersLA
    );
  } else {
    if (mainLA) {
      const emptyDataHDP = getEmptyHDPRow(
        MinorBetTypes.NoOfCorners,
        matchSettings
      );
      emptyDataHDP.LA = getAssignmentLA(
        MinorBetTypes.NoOfCorners,
        mainLA,
        othersLA
      );
      listAssignmentItem.push(emptyDataHDP);
    }
  }

  if (data15minsNoOfCorners) {
    data15minsNoOfCorners.HT.isAllowAdd = false;
    data15minsNoOfCorners.LA.isAllowAdd = false;
    listAssignmentItem.push(data15minsNoOfCorners);
  }

  if (dataAllInOneNoOfCorners) {
    dataAllInOneNoOfCorners.HT.isAllowAdd = false;
    dataAllInOneNoOfCorners.LA.isAllowAdd = false;
    listAssignmentItem.push(dataAllInOneNoOfCorners);
  }

  if (dataFastMarketNoOfCorners) {
    dataFastMarketNoOfCorners.HT.isAllowAdd = false;
    dataFastMarketNoOfCorners.LA.isAllowAdd = false;
    listAssignmentItem.push(dataFastMarketNoOfCorners);
  }

  return {
    Group: groupName,
    DataSource: listAssignmentItem,
    TotalRows: listAssignmentItem.length
  };
};

const getDataForGroupTotalBooking = (matchSettings, assignments) => {
  const groupName = 'TotalBooking';
  const dataTotalBookingHDP = getHDPRow(
    MinorBetTypes.TotalBooking,
    matchSettings,
    assignments
  );
  const dataTotalBookingOU = getOURow(
    MinorBetTypes.TotalBooking,
    matchSettings,
    assignments
  );
  const dataTotalBooking1X2 = get1X2Row(
    MinorBetTypes.TotalBooking,
    matchSettings,
    assignments
  );
  const data15minsTotalBooking = getHDPRow(
    MinorBetTypes.Specific15minsTotalBooking,
    matchSettings,
    assignments
  );

  const dataAllInOneTotalBooking = getHDPRow(
    MinorBetTypes.AllInOneTotalBookings,
    matchSettings,
    assignments
  );

  const dataFastMarketTotalBooking = getHDPRow(
    MinorBetTypes.FastMarketTotalBookings,
    matchSettings,
    assignments
  );

  let listAssignmentItem = [
    dataTotalBookingHDP,
    dataTotalBookingOU,
    dataTotalBooking1X2
  ];

  listAssignmentItem = listAssignmentItem.filter(x => x !== null);

  const mainLA = getMainAssignmentLA(MinorBetTypes.TotalBooking, assignments);
  const othersLA = getOtherAssignmentLA(
    MinorBetTypes.TotalBooking,
    assignments
  );
  if (listAssignmentItem.length > 0) {
    listAssignmentItem[0].LA = getAssignmentLA(
      MinorBetTypes.TotalBooking,
      mainLA,
      othersLA
    );
  } else {
    if (mainLA) {
      const emptyDataHDP = getEmptyHDPRow(
        MinorBetTypes.TotalBooking,
        matchSettings
      );
      emptyDataHDP.LA = getAssignmentLA(
        MinorBetTypes.TotalBooking,
        mainLA,
        othersLA
      );
      listAssignmentItem.push(emptyDataHDP);
    }
  }

  if (data15minsTotalBooking) {
    data15minsTotalBooking.HT.isAllowAdd = false;
    data15minsTotalBooking.LA.isAllowAdd = false;
    listAssignmentItem.push(data15minsTotalBooking);
  }

  if (dataAllInOneTotalBooking) {
    dataAllInOneTotalBooking.HT.isAllowAdd = false;
    dataAllInOneTotalBooking.LA.isAllowAdd = false;
    listAssignmentItem.push(dataAllInOneTotalBooking);
  }

  if (dataFastMarketTotalBooking) {
    dataFastMarketTotalBooking.HT.isAllowAdd = false;
    dataFastMarketTotalBooking.LA.isAllowAdd = false;
    listAssignmentItem.push(dataFastMarketTotalBooking);
  }

  return {
    Group: groupName,
    DataSource: listAssignmentItem,
    TotalRows: listAssignmentItem.length
  };
};

const getDataForGroupFastMarket = (matchSettings, assignments) => {
  const groupName = 'FM';
  const dataFM = getHDPRow(
    MinorBetTypes.FastMarket,
    matchSettings,
    assignments
  );

  const listAssignmentItem = [];

  if (dataFM) {
    dataFM.HT.isAllowAdd = false;
    dataFM.LA.isAllowAdd = false;
    listAssignmentItem.push(dataFM);
  }

  return {
    Group: groupName,
    DataSource: listAssignmentItem,
    TotalRows: listAssignmentItem.length
  };
};

const getDataForGroupAllInOne = (matchSettings, assignments) => {
  const groupName = 'AIO';
  const dataAIO = getHDPRow(MinorBetTypes.AllInOne, matchSettings, assignments);

  const listAssignmentItem = [];

  if (dataAIO) {
    dataAIO.MaxBetFT.MaxBet = '';
    dataAIO.MaxBetFT.Spread = '';
    dataAIO.HT.isAllowAdd = false;
    dataAIO.LA.isAllowAdd = false;
    listAssignmentItem.push(dataAIO);
  }

  return {
    Group: groupName,
    DataSource: listAssignmentItem,
    TotalRows: listAssignmentItem.length
  };
};

const getDataForGroupMinor = (
  groupName,
  minorBettypeId,
  matchSettings,
  assignments
) => {
  const dataHDP = getHDPRow(minorBettypeId, matchSettings, assignments);
  const dataOU = getOURow(minorBettypeId, matchSettings, assignments);
  const data1X2 = get1X2Row(minorBettypeId, matchSettings, assignments);

  let listAssignmentItem = [dataHDP, dataOU, data1X2];

  listAssignmentItem = listAssignmentItem.filter(x => x !== null);

  const mainLA = getMainAssignmentLA(minorBettypeId, assignments);
  const othersLA = getOtherAssignmentLA(minorBettypeId, assignments);
  if (listAssignmentItem.length > 0) {
    listAssignmentItem[0].LA = getAssignmentLA(
      minorBettypeId,
      mainLA,
      othersLA
    );
  } else {
    if (mainLA) {
      const emptyDataHDP = getEmptyHDPRow(minorBettypeId, matchSettings);
      emptyDataHDP.LA = getAssignmentLA(minorBettypeId, mainLA, othersLA);
      listAssignmentItem.push(emptyDataHDP);
    }
  }

  return {
    Group: groupName,
    DataSource: listAssignmentItem,
    TotalRows: listAssignmentItem.length
  };
};

const getAll = (matchSettings, assignments) => {
  const excludeMinorBettypes = [
    0,
    MinorBetTypes.Specific15mins,
    MinorBetTypes.Specific10mins,
    MinorBetTypes.NoOfCorners,
    MinorBetTypes.Specific15minsNoOfCorners,
    MinorBetTypes.TotalBooking,
    MinorBetTypes.Specific15minsTotalBooking,
    MinorBetTypes.FastMarket,
    MinorBetTypes.FastMarketCorners,
    MinorBetTypes.FastMarketTotalBookings,
    MinorBetTypes.AllInOne,
    MinorBetTypes.AllInOneCorners,
    MinorBetTypes.AllInOneTotalBookings
  ];

  const minorBettypes = matchSettings
    .map(x => ({ MinorBettypeId: x.MinorBettypeId }))
    .filter(x => excludeMinorBettypes.indexOf(x.MinorBettypeId) === -1);
  const uniqueMinorBettypes = uniqBy(minorBettypes, 'MinorBettypeId');

  const newDataSource = [];
  newDataSource.push(getDataForGroupMajor(matchSettings, assignments));
  newDataSource.push(getDataForGroupNoOfCorners(matchSettings, assignments));
  newDataSource.push(getDataForGroupTotalBooking(matchSettings, assignments));
  uniqueMinorBettypes.forEach((element, index) => {
    const groupName = `Minor${index}`;
    const dataGroupMinor = getDataForGroupMinor(
      groupName,
      element.MinorBettypeId,
      matchSettings,
      assignments
    );

    newDataSource.push(dataGroupMinor);
  });

  newDataSource.push(getDataForGroupFastMarket(matchSettings, assignments));
  newDataSource.push(getDataForGroupAllInOne(matchSettings, assignments));

  return newDataSource;
};

const LiveDetailService = {
  getAll
};
export default LiveDetailService;
