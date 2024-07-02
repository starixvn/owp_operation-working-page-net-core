import {
  filterIssueByUnreadMessageValue,
  formatTicketTypeOption
} from './eventSliceHelper';
describe('eventSliceHelper underTest', () => {
  describe('filterIssueByUnreadMessageValue underTest', () => {
    it('should handle when isShowUnreadMessOnly == true', () => {
      const state = {
        entitiesOrigin: [
          { id: 1, isRead: true },
          { id: 2, isRead: false },
          { id: 3, isRead: true }
        ],
        isShowUnreadMessOnly: true
      };

      const expectResult = [{ id: 2, isRead: false }];

      const result = filterIssueByUnreadMessageValue(state);

      expect(result).toEqual(expectResult);
    });
    it('should handle when isShowUnreadMessOnly == false', () => {
      const state = {
        entitiesOrigin: [
          { id: 1, isRead: true },
          { id: 2, isRead: false }
        ],
        isShowUnreadMessOnly: false
      };
      const expectResult = [
        { id: 1, isRead: true },
        { id: 2, isRead: false }
      ];

      const result = filterIssueByUnreadMessageValue(state);

      expect(result).toEqual(expectResult);
    });
  });
  describe('formatTicketTypeOption underTest', () => {
    it('should handle formatTicketTypeOption', () => {
      const ticketTypes = [
        { id: 1, name: 'hihi' },
        { id: 2, name: 'haha' }
      ];

      const expectResult = [
        { value: 1, label: 'hihi' },
        { value: 2, label: 'haha' }
      ];

      const result = formatTicketTypeOption(ticketTypes);

      expect(result).toEqual(expectResult);
    });
  });
});
