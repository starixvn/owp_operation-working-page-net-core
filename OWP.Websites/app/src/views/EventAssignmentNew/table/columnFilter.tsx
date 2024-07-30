import { filter, findIndex, includes } from 'lodash';
import Utils from '../../Shared/Utils';
import { TABLE } from '../const.json';
import {
  TAssignment,
  TColumnFilter,
  TColumnFilterByMbc,
  TDataTable
} from '../type/type';
const {
  HEADER: { MATCH_NO, KO_TIME, EVENT, NOTE, IP_TV, PROVIDER, MB, REMARK }
} = TABLE;

const { convertThousandToK, convertSpreadToPercent, toLowerCaseForFilter } =
  Utils;

const filterMatchNo = (events: TDataTable, keyword: string) =>
  filter(events, event => {
    return (
      includes(toLowerCaseForFilter(event.MatchNo), keyword) ||
      includes(toLowerCaseForFilter(event.MatchStatus), keyword)
    );
  });

const filterKickOffTime = (events: TDataTable, keyword: string) => {
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

const filterEvent = (events: TDataTable, keyword: string) => {
  return filter(events, event => {
    const isIncludeEventInfo =
      includes(toLowerCaseForFilter(event.League), keyword) ||
      includes(toLowerCaseForFilter(event.Home), keyword) ||
      includes(toLowerCaseForFilter(event.Away), keyword);
    const isIncludeNoOfHDP =
      event.NoOfHDP && includes(toLowerCaseForFilter(event.NoOfHDP), keyword);

    return (
      isIncludeEventInfo ||
      (isIncludeNoOfHDP ??
        findIndex(event.MatchOffers, mo =>
          includes(toLowerCaseForFilter(mo.MatchOfferValue), keyword)
        ) >= 0)
    );
  });
};

const filterTV = (events: TDataTable, keyword: string) => {
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

const filterProvider = (events: TDataTable, keyword: string) => {
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

const includeMBC = (props: TColumnFilterByMbc) => {
  const { maxbetHDP, spreadHDP, spreadOU, maxBetToDisplay, keyword } = props;
  return (
    includes(toLowerCaseForFilter(maxbetHDP), keyword) ||
    includes(toLowerCaseForFilter(spreadHDP), keyword) ||
    includes(toLowerCaseForFilter(spreadOU), keyword) ||
    includes(toLowerCaseForFilter(maxBetToDisplay), keyword)
  );
};

const filterMaxbetSpread = (events: TDataTable, keyword: string) => {
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
const isContainsAssignment = (assignment: TAssignment, keyword: string) =>
  includes(toLowerCaseForFilter(assignment.Username), keyword) ||
  includes(toLowerCaseForFilter(assignment.AssignmentNote), keyword);

const filterRemark = (events: TDataTable, keyword: string) => {
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
const filterNote = (events: TDataTable, keyword: string) => {
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
const filterLA = (events: TDataTable, keyword: string) => {
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
const filterHandicap = (events: TDataTable, keyword: string) => {
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
const filterOverUnder = (events: TDataTable, keyword: string) => {
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
const filterHandicap1st = (events: TDataTable, keyword: string) => {
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
const filterOverUnder1st = (events: TDataTable, keyword: string) => {
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
const filterOneXTwo = (events: TDataTable, keyword: string) => {
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
const filterOneXTwo1st = (events: TDataTable, keyword: string) => {
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

const filterSpecific15Mins = (events: TDataTable, keyword: string) => {
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

const filterSpecific10Mins = (events: TDataTable, keyword: string) => {
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
const filterFM = (events: TDataTable, keyword: string) => {
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
const filterAIO = (events: TDataTable, keyword: string) => {
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

const filterByColumn = (events: TDataTable, column: TColumnFilter) => { //NOSONAR
  const keyword = toLowerCaseForFilter(column.keyword);
  if (!keyword.trim().length) {
    return events;
  }
  switch (column.columnKey) {
    case MATCH_NO.COLUMN_KEY:
      return filterMatchNo(events, keyword);
    case KO_TIME.COLUMN_KEY:
      return filterKickOffTime(events, keyword);
    case EVENT.COLUMN_KEY:
      return filterEvent(events, keyword);
    case IP_TV.COLUMN_KEY:
      return filterTV(events, keyword);
    case PROVIDER.COLUMN_KEY:
      return filterProvider(events, keyword);
    case MB.COLUMN_KEY:
      return filterMaxbetSpread(events, keyword);
    case REMARK.COLUMN_KEY:
      return filterRemark(events, keyword);
    case NOTE.COLUMN_KEY:
      return filterNote(events, keyword);
    case 'LA':
      return filterLA(events, keyword);
    case 'Handicap':
      return filterHandicap(events, keyword);
    case 'OverUnder':
      return filterOverUnder(events, keyword);
    case 'Handicap1st':
      return filterHandicap1st(events, keyword);
    case 'OverUnder1st':
      return filterOverUnder1st(events, keyword);
    case 'OneXTwo':
      return filterOneXTwo(events, keyword);
    case 'OneXTwo1st':
      return filterOneXTwo1st(events, keyword);
    case 'Specific15Mins':
      return filterSpecific15Mins(events, keyword);
    case 'Specific10Mins':
      return filterSpecific10Mins(events, keyword);
    case 'FM':
      return filterFM(events, keyword);
    case 'AIO':
      return filterAIO(events, keyword);
  }
  return null;
};

const handleFilterByColumn = (
  events: TDataTable,
  columnFilters: Array<TColumnFilter>
) => {
  columnFilters.forEach(column => {
    events = filterByColumn(events, column);
  });
  return events;
};

const handleDataByTicket = (events: TDataTable, isUnread: boolean) =>
  events.filter(x => x.IsRead === false || !isUnread);

const handleFilter =
  (events: TDataTable) =>
    (columnFilters: Array<TColumnFilter>) =>
      (isUnread: boolean) =>
        handleDataByTicket(handleFilterByColumn(events, columnFilters), isUnread);

const updateColumnFilter = (
  currentState: TColumnFilter[],
  newFilter: TColumnFilter
) => {
  const { columnKey, keyword } = newFilter;
  const columnFilter = currentState.find(a => a.columnKey === columnKey);
  if (columnFilter) {
    columnFilter.keyword = keyword;
  } else {
    currentState.push(newFilter);
  }
};

export { handleFilter, handleFilterByColumn, updateColumnFilter };
