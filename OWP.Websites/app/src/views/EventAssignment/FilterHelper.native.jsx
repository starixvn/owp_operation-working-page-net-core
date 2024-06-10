import Utils from '../Shared/Utils';

import {
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
  filterAIO
} from './FilterHelper';

const { toLowerCaseForFilter } = Utils;

const filterByColumn = (events, item) => {
  const keyword = toLowerCaseForFilter(item.keyword);
  if (!keyword.trim().length) {
    return events;
  }
  switch (item.name) {
    case 'MatchNo':
      return filterMatchNo(events, keyword);
    case 'KickOffTime':
      return filterKickOffTime(events, keyword);
    case 'Event':
      return filterEvent(events, keyword);
    case 'TV':
      return filterTV(events, keyword);
    case 'Provider':
      return filterProvider(events, keyword);
    case 'MaxBet_Spread':
      return filterMaxbetSpread(events, keyword);
    case 'Remark':
      return filterRemark(events, keyword);
    case 'Note':
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
  return undefined;
};

export { filterByColumn };
