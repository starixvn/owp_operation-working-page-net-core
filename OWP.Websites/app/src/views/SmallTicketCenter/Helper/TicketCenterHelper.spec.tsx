import {
  buildTicketTypeFilter,
  removeTickets,
  sortTickets,
  getDefaultDateRange
} from './TicketCenterHelper';
import {
  getDetailSegmentUrlByEventType,
  EVENT_TYPE,
  URL
} from '../../Shared/TicketCenterPrematch/helper/helper';

import { TChildrenResponse, TParentsResponse } from '../type';

describe('Utility Function Tests', () => {
  describe('getDetailSegmentUrlByEventType', () => {
    it('returns correct URL for a valid event type', () => {
      const eventType = EVENT_TYPE.SOME_EVENT_TYPE;
      const expectedUrl = URL[eventType];

      const result = getDetailSegmentUrlByEventType(eventType);

      expect(result).toEqual(expectedUrl);
    });

    it('returns undefined for an invalid event type', () => {
      const eventType = 'InvalidEventType';

      const result = getDetailSegmentUrlByEventType(eventType);

      expect(result).toBeUndefined();
    });
  });

  describe('buildTicketTypeFilter', () => {
    it('correctly builds filter options from parents and children arrays', () => {
      const parents: TParentsResponse[] = [{ id: 1, name: 'Parent1' }];
      const children: TChildrenResponse[] = [
        { parentId: 1, subTypeId: 1, subTypeName: 'Child1' }
      ];

      const { parentOption, childrenOption, emptyOption } =
        buildTicketTypeFilter(parents, children);

      expect(parentOption).toEqual([
        {
          value: 'p-1',
          title: 'Parent1',
          key: 'p-1',
          showTitle: 'Parent1'
        }
      ]);

      expect(childrenOption).toEqual([
        {
          value: '1-1',
          title: 'Child1',
          key: '1-1',
          pId: 'p-1',
          showTitle: 'Parent1-Child1'
        }
      ]);

      expect(emptyOption).toEqual([
        {
          value: '1-',
          title: 'None',
          key: 'p-1-None',
          pId: 'p-1',
          showTitle: 'Parent1-None'
        }
      ]);
    });
  });

  describe('removeTickets', () => {
    it('removes ticket with given ticketId from entities', () => {
      const entities = [{ ticketId: 1 }, { ticketId: 2 }, { ticketId: 3 }];

      const ticketIdToRemove = 2;
      const filteredEntities = removeTickets(entities, ticketIdToRemove);

      expect(filteredEntities).toHaveLength(2);
      expect(
        filteredEntities.some(entity => entity.ticketId === ticketIdToRemove)
      ).toBeFalsy();
    });
  });

  describe('sortTickets', () => {
    it('correctly sorts entities based on criteria', () => {
      const entities = [
        { isPinned: true, lastCommentDate: '2024-07-16T10:00:00Z' },
        { isPinned: false, lastCommentDate: '2024-07-15T10:00:00Z' },
        { isPinned: true, lastCommentDate: '2024-07-14T10:00:00Z' }
      ];

      sortTickets(entities);

      const expectedOrder = [
        { isPinned: true, lastCommentDate: '2024-07-16T10:00:00Z' },
        { isPinned: true, lastCommentDate: '2024-07-14T10:00:00Z' },
        { isPinned: false, lastCommentDate: '2024-07-15T10:00:00Z' }
      ];

      expect(entities).toEqual(expectedOrder);
    });
  });

  describe('getDefaultDateRange', () => {
    it('returns default date range object', () => {
      const result = getDefaultDateRange();

      expect(result.fromDateDefault).toBeDefined();
      expect(result.toDateDefault).toBeDefined();
      expect(result.minDate).toBeDefined();
    });
  });
});
