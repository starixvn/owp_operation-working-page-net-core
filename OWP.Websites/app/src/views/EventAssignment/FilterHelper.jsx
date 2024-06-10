import Utils from '../Shared/Utils';
import { findIndex, includes, filter, find } from 'lodash';
import isEqual from 'lodash.isequal';

const { convertThousandToK, convertSpreadToPercent, toLowerCaseForFilter } =
  Utils;
const filterMatchNo = (events, keyword) =>
  filter(events, event => {
    return (
      includes(toLowerCaseForFilter(event.MatchNo), keyword) ||
      includes(toLowerCaseForFilter(event.MatchStatus), keyword)
    );
  });

const filterKickOffTime = (events, keyword) => {
  return filter(events, event => {
    if (event.KickOffTime || event.OtherTime) {
      return (
        includes(toLowerCaseForFilter(event.KickOffTime), keyword) ||
        includes(toLowerCaseForFilter(event.OtherTime), keyword)
      );
    }
    return false;
  });
};

const filterEvent = (events, keyword) => {
  return filter(events, event => {
    const isIncludeEventInfo =
      includes(toLowerCaseForFilter(event.League), keyword) ||
      includes(toLowerCaseForFilter(event.Home), keyword) ||
      includes(toLowerCaseForFilter(event.Away), keyword);
    const isIncludeNoOfHDP =
      event.NoOfHDP && includes(toLowerCaseForFilter(event.NoOfHDP), keyword);

    return (
      isIncludeEventInfo ||
      isIncludeNoOfHDP ||
      findIndex(event.MatchOffers, mo =>
        includes(toLowerCaseForFilter(mo.MatchOfferValue), keyword)
      ) >= 0
    );
  });
};

const filterTV = (events, keyword) => {
  return filter(events, event => {
    return (
      (event.Channel &&
        includes(toLowerCaseForFilter(event.Channel), keyword)) ||
      findIndex(event.Televisions, tv =>
        includes(toLowerCaseForFilter(tv.TVCode), keyword)
      ) >= 0
    );
  });
};

const filterProvider = (events, keyword) => {
  return filter(events, event => {
    if (
      event.Provider ||
      event.OtherProvider ||
      event.WatchBet ||
      event.Phone
    ) {
      return (
        includes(toLowerCaseForFilter(event.Provider), keyword) ||
        includes(toLowerCaseForFilter(event.OtherProvider), keyword) ||
        includes(toLowerCaseForFilter(event.WatchBet), keyword) ||
        includes(toLowerCaseForFilter(event.Phone), keyword)
      );
    }
    return false;
  });
};

const includeMBC = props => {
  const { maxbetHDP, spreadHDP, spreadOU, maxBetToDisplay, keyword } = props;
  return (
    includes(toLowerCaseForFilter(maxbetHDP), keyword) ||
    includes(toLowerCaseForFilter(spreadHDP), keyword) ||
    includes(toLowerCaseForFilter(spreadOU), keyword) ||
    includes(toLowerCaseForFilter(maxBetToDisplay), keyword)
  );
};

const filterMaxbetSpread = (events, keyword) => {
  return filter(events, event => {
    let maxbetHDP = '';
    let spreadHDP = '';
    let spreadOU = '';
    let maxBetToDisplay = '';
    if (event.HandicapSetting) {
      maxbetHDP = convertThousandToK(event.HandicapSetting.Maxbet, true);
      spreadHDP = convertSpreadToPercent(event.HandicapSetting.Spread, true);

      maxBetToDisplay = event.HandicapSetting.MaxBetToDisplay;
    }

    if (event.OverUnderSetting) {
      spreadOU = convertSpreadToPercent(event.OverUnderSetting.Spread, true);
    }

    const isMBCValid = maxbetHDP || spreadHDP || spreadOU;

    const propMbc = {
      maxbetHDP,
      spreadHDP,
      spreadOU,
      maxBetToDisplay,
      keyword
    };
    const isIncludeMBC = includeMBC(propMbc);

    if (isMBCValid) {
      return isIncludeMBC;
    }

    return false;
  });
};
const isContainsAssignment = (assignment, keyword) =>
  includes(toLowerCaseForFilter(assignment.Username), keyword) ||
  includes(toLowerCaseForFilter(assignment.AssignmentNote), keyword);

const filterRemark = (events, keyword) => {
  return filter(events, event => {
    if (event.Remark || event.Division || event.Round) {
      return (
        includes(toLowerCaseForFilter(event.Remark), keyword) ||
        includes(toLowerCaseForFilter(event.Division), keyword) ||
        includes(toLowerCaseForFilter(event.Round), keyword)
      );
    }
    return false;
  });
};
const filterNote = (events, keyword) => {
  return filter(events, event => {
    if (event.Notes) {
      const notes = filter(event.Notes, note =>
        includes(toLowerCaseForFilter(note.Note), keyword)
      ).map(x => toLowerCaseForFilter(x.Note));
      return notes.length > 0;
    }
    return false;
  });
};
const filterLA = (events, keyword) => {
  return filter(events, event => {
    if (event.LA) {
      const assignments = filter(event.LA, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};
const filterHandicap = (events, keyword) => {
  return filter(events, event => {
    if (event.Handicap) {
      const assignments = filter(event.Handicap, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};
const filterOverUnder = (events, keyword) => {
  return filter(events, event => {
    if (event.OverUnder) {
      const assignments = filter(event.OverUnder, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};
const filterHandicap1st = (events, keyword) => {
  return filter(events, event => {
    if (event.Handicap1st) {
      const assignments = filter(event.Handicap1st, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};
const filterOverUnder1st = (events, keyword) => {
  return filter(events, event => {
    if (event.OverUnder1st) {
      const assignments = filter(event.OverUnder1st, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};
const filterOneXTwo = (events, keyword) => {
  return filter(events, event => {
    if (event.OneXTwo) {
      const assignments = filter(event.OneXTwo, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};
const filterOneXTwo1st = (events, keyword) => {
  return filter(events, event => {
    if (event.OneXTwo1st) {
      const assignments = filter(event.OneXTwo1st, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};

const filterSpecific15Mins = (events, keyword) => {
  return filter(events, event => {
    if (event.Specific15Mins) {
      const assignments = filter(event.Specific15Mins, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};

const filterSpecific10Mins = (events, keyword) => {
  return filter(events, event => {
    if (event.Specific10Mins) {
      const assignments = filter(event.Specific10Mins, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};
const filterFM = (events, keyword) => {
  return filter(events, event => {
    if (event.FM) {
      const assignments = filter(event.FM, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};
const filterAIO = (events, keyword) => {
  return filter(events, event => {
    if (event.AIO) {
      const assignments = filter(event.AIO, assignment =>
        isContainsAssignment(assignment, keyword)
      );
      return assignments.length > 0;
    }
    return false;
  });
};

const getKeyword = (columnFilters, columnName) => {
  const result = find(columnFilters, col => isEqual(col.name, columnName));
  return result ? result.keyword : '';
};

const selectOffVROptions = matchOffers =>
  matchOffers.filter(x => x.Value.includes('OFF VR'));

export {
  filterMatchNo,
  filterKickOffTime,
  filterEvent,
  filterTV,
  filterProvider,
  filterMaxbetSpread,
  filterRemark,
  filterNote,
  filterLA,
  filterHandicap,
  filterOverUnder,
  filterHandicap1st,
  filterOverUnder1st,
  filterOneXTwo,
  filterOneXTwo1st,
  filterSpecific15Mins,
  filterSpecific10Mins,
  filterFM,
  filterAIO,
  getKeyword,
  selectOffVROptions
};
