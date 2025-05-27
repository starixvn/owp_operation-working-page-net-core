import {
  getKeyValue,
  buildRequestURL,
  validDateRange,
  toDateDefault,
  fromDateDefault,
} from './helper'; // Điều chỉnh đường dẫn nếu cần

// Giả lập các giá trị từ Const.json nếu bạn muốn mock
jest.mock('../Const.json', () => ({
  dateRangeDefault: 7,
  dateRangeValid: 30,
  time: 86400000, // 1 ngày = 24 * 60 * 60 * 1000
}));

describe('Utility functions', () => {
  describe('getKeyValue', () => {
    it('should return correct value for given key', () => {
      const obj = { name: 'John', age: 30 };
      expect(getKeyValue(obj, 'name')).toBe('John');
      expect(getKeyValue(obj, 'age')).toBe(30);
    });
  });

  describe('buildRequestURL', () => {
    it('should build correct URL', () => {
      const key = 'daily';
      expect(buildRequestURL(key)).toBe('/api/ProviderStatistics/daily');
    });
  });

  describe('validDateRange', () => {
    it('should return true if range exceeds valid limit', () => {
      const fromDate = new Date('2024-01-01');
      const toDate = new Date('2024-02-15'); // > 30 ngày
      expect(validDateRange(fromDate, toDate)).toBe(true);
    });

    it('should return false if range is within valid limit', () => {
      const fromDate = new Date('2024-01-01');
      const toDate = new Date('2024-01-10'); // < 30 ngày
      expect(validDateRange(fromDate, toDate)).toBe(false);
    });
  });

  describe('Default Dates', () => {
    it('should have fromDateDefault earlier than toDateDefault', () => {
      expect(fromDateDefault.getTime()).toBeLessThan(toDateDefault.getTime());
    });

    it('should have correct range between default dates', () => {
      const diffInDays =
        (toDateDefault.getTime() - fromDateDefault.getTime()) / 86400000;
      expect(diffInDays).toBeCloseTo(7, 0); // 7 là dateRangeDefault mock
    });
  });
});
