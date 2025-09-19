import {
  buildTimeRangeForFromDate,
  buildTimeRangeForToDate,
  buildFromDateToDateDefaultWithServerTime,
  buildTimeRangeLast30Day,
  convertFrom24hTo12h,
  getHourAndMinutes,
  addDays,
  buildDateString
} from './dateTimeHelper';

describe('dateTimeHelper - Core Functions', () => {
  describe('buildTimeRangeForFromDate', () => {
    it('should build time range with 365 days before from date', () => {
      // Arrange
      const fromDate = new Date('2024-01-15');
      
      // Act
      const result = buildTimeRangeForFromDate(fromDate);
      
      // Assert
      expect(result.minFromDate).toEqual(new Date('2023-01-15'));
      expect(result.maxFromDate).toEqual(new Date('2024-01-15'));
    });

    it('should handle leap year correctly', () => {
      // Arrange
      const fromDate = new Date('2024-03-01');
      
      // Act
      const result = buildTimeRangeForFromDate(fromDate);
      
      // Assert
      expect(result.minFromDate).toEqual(new Date('2023-03-02')); // 2024 is leap year
      expect(result.maxFromDate).toEqual(new Date('2024-03-01'));
    });
  });

  describe('buildTimeRangeForToDate', () => {
    it('should build time range from fromDate to serverTime', () => {
      // Arrange
      const fromDate = new Date('2024-01-10');
      const serverTime = new Date('2024-01-15T10:30:00Z');
      
      // Act
      const result = buildTimeRangeForToDate(fromDate, serverTime);
      
      // Assert
      expect(result.minToDate).toEqual(fromDate);
      expect(result.maxToDate).toEqual(serverTime);
    });
  });

  describe('buildFromDateToDateDefaultWithServerTime', () => {
    it('should build default 30-day range from server time', () => {
      // Arrange
      const serverTime = new Date('2024-01-15T10:30:00Z');
      
      // Act
      const result = buildFromDateToDateDefaultWithServerTime(serverTime);
      
      // Assert
      expect(result.fromDate).toEqual(new Date('2023-12-16T10:30:00Z'));
      expect(result.toDate).toEqual(serverTime);
    });
  });

  describe('buildTimeRangeLast30Day', () => {
    it('should return date 30 days before given time', () => {
      // Arrange
      const time = new Date('2024-01-15T10:30:00Z');
      
      // Act
      const result = buildTimeRangeLast30Day(time);
      
      // Assert
      expect(result).toEqual(new Date('2023-12-16T10:30:00Z'));
    });
  });

  describe('convertFrom24hTo12h', () => {
    it('should convert 24-hour format to 12-hour AM format', () => {
      // Arrange
      const time24 = '09:30:00';
      
      // Act
      const result = convertFrom24hTo12h(time24);
      
      // Assert
      expect(result).toBe('09:30 AM');
    });

    it('should convert 24-hour format to 12-hour PM format', () => {
      // Arrange
      const time24 = '15:45:00';
      
      // Act
      const result = convertFrom24hTo12h(time24);
      
      // Assert
      expect(result).toBe('03:45 PM');
    });

    it('should handle midnight correctly', () => {
      // Arrange
      const time24 = '00:30:00';
      
      // Act
      const result = convertFrom24hTo12h(time24);
      
      // Assert
      expect(result).toBe('00:30 AM');
    });

    it('should handle noon correctly', () => {
      // Arrange
      const time24 = '12:00:00';
      
      // Act
      const result = convertFrom24hTo12h(time24);
      
      // Assert
      expect(result).toBe('00:00 PM');
    });

    it('should include seconds when isDisplaySecond is true', () => {
      // Arrange
      const time24 = '14:30:45';
      
      // Act
      const result = convertFrom24hTo12h(time24, true);
      
      // Assert
      expect(result).toBe('02:30:45 PM');
    });

    it('should exclude seconds when isDisplaySecond is false', () => {
      // Arrange
      const time24 = '14:30:45';
      
      // Act
      const result = convertFrom24hTo12h(time24, false);
      
      // Assert
      expect(result).toBe('02:30:45 PM');
    });

    it('should handle invalid time format', () => {
      // Arrange
      const invalidTime = 'invalid';
      
      // Act
      const result = convertFrom24hTo12h(invalidTime);
      
      // Assert
      expect(result).toBe('invalid');
    });
  });

  describe('getHourAndMinutes', () => {
    it('should return first 5 characters of time string', () => {
      // Arrange
      const time = '14:30:45';
      
      // Act
      const result = getHourAndMinutes(time);
      
      // Assert
      expect(result).toBe('14:30');
    });

    it('should return undefined for null/undefined input', () => {
      // Act & Assert
      expect(getHourAndMinutes(null)).toBeUndefined();
      expect(getHourAndMinutes(undefined)).toBeUndefined();
    });

    it('should handle short time strings', () => {
      // Arrange
      const time = '14:30';
      
      // Act
      const result = getHourAndMinutes(time);
      
      // Assert
      expect(result).toBe('14:30');
    });
  });

  describe('addDays', () => {
    it('should add positive days to date', () => {
      // Arrange
      const date = new Date('2024-01-15');
      const days = 5;
      
      // Act
      const result = addDays(date, days);
      
      // Assert
      expect(result).toEqual(new Date('2024-01-20'));
    });

    it('should subtract days when negative number provided', () => {
      // Arrange
      const date = new Date('2024-01-15');
      const days = -5;
      
      // Act
      const result = addDays(date, days);
      
      // Assert
      expect(result).toEqual(new Date('2024-01-10'));
    });

    it('should not modify original date', () => {
      // Arrange
      const originalDate = new Date('2024-01-15');
      const dateCopy = new Date(originalDate);
      
      // Act
      addDays(dateCopy, 5);
      
      // Assert
      expect(originalDate).toEqual(new Date('2024-01-15'));
    });
  });

  describe('buildDateString', () => {
    it('should build M-D-YYYY format string', () => {
      // Arrange
      const date = new Date('2024-01-15');
      
      // Act
      const result = buildDateString(date);
      
      // Assert
      expect(result).toBe('1-15-2024');
    });

    it('should handle single digit month and day', () => {
      // Arrange
      const date = new Date('2024-03-05');
      
      // Act
      const result = buildDateString(date);
      
      // Assert
      expect(result).toBe('3-5-2024');
    });
  });

});
