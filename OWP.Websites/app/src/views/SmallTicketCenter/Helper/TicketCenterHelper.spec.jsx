import {
  buildValueTicketsFilter,
  getDetailSegmentUrlByEventType,
  EVENT_TYPE,
  URL
} from './TicketCenterHelper';
import { getValueHrefByReversedIndex } from '../../Shared/Utils';

const { LEAGUE, MATCH, OTHER } = EVENT_TYPE;

const {
  GET_TICKET_DETAIL_LEAGUE,
  GET_TICKET_DETAIL_MATCH,
  GET_TICKET_DETAIL_OTHER
} = URL;

jest.mock('../../Shared/Utils');
const setUp = url => {
  getValueHrefByReversedIndex.mockReturnValue(url);
};

describe('TicketCenterHelper underTest', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  describe('buildValueMyTicketFilter underTest', () => {
    it('should handle when advance flow (from PreMatchPage)', () => {
      setUp('MyTicket');
      const expectResult = {
        Keyword: 'Liam',
        ActorId: 1,
        MarketKey: 1
      };

      const result = buildValueTicketsFilter({ ActorId: 1, Username: 'Liam' });

      expect(result).toEqual(expectResult);
    });
    it('should handle when basic flow', () => {
      setUp('TicketCenter');
      const expectResult = {
        ActorId: 8,
        MarketKey: 1
      };

      const result = buildValueTicketsFilter({
        ActorId: 8,
        Username: 'Johnny'
      });

      expect(result).toEqual(expectResult);
    });

    it('should return segmentUrl league when input param = league', () => {
      const actual = getDetailSegmentUrlByEventType(LEAGUE);
      expect(actual).toEqual(GET_TICKET_DETAIL_LEAGUE);
    });

    it('should return segmentUrl match when input param = match', () => {
      const actual = getDetailSegmentUrlByEventType(MATCH);
      expect(actual).toEqual(GET_TICKET_DETAIL_MATCH);
    });

    it('should return segmentUrl other when input param = other', () => {
      const actual = getDetailSegmentUrlByEventType(OTHER);
      expect(actual).toEqual(GET_TICKET_DETAIL_OTHER);
    });
  });
});
