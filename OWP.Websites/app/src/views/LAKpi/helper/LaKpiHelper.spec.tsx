import {
  sortData,
  filterData,
  buildOrderSelection,
  buildSectionInString
} from './LaKpiHelper'; // Adjust the import path as necessary
import { asc, desc } from '../TableConst.json';

describe('tableUtils', () => {
  describe('sortData', () => {
    const dataSource = [
      { userName: 'Alice', totalGameInfo: { score: 10 }, level: 'A' },
      { userName: 'Bob', totalGameInfo: { score: 20 }, level: 'B' },
      { userName: 'Charlie', totalGameInfo: { score: 15 }, level: 'C' }
    ];

    it('should sort data by level order', () => {
      const sortedData = sortData(dataSource, 'level', 'level', asc);
      expect(sortedData).toEqual([
        { userName: 'Alice', totalGameInfo: { score: 10 }, level: 'A' },
        { userName: 'Bob', totalGameInfo: { score: 20 }, level: 'B' },
        { userName: 'Charlie', totalGameInfo: { score: 15 }, level: 'C' }
      ]);
    });
  });

  describe('filterData', () => {
    const dataSource = [
      { userName: 'Alice', totalGameInfo: { score: 10 }, level: 'A' },
      { userName: 'Bob', totalGameInfo: { score: 20 }, level: 'B' },
      { userName: 'Charlie', totalGameInfo: { score: 15 }, level: 'C' }
    ];

    it('should filter data by userName', () => {
      const filteredData = filterData(dataSource, 'Alice');
      expect(filteredData).toEqual([
        { userName: 'Alice', totalGameInfo: { score: 10 }, level: 'A' }
      ]);
    });

    it('should filter data by partial userName', () => {
      const filteredData = filterData(dataSource, 'li');
      expect(filteredData).toEqual([
        { userName: 'Alice', totalGameInfo: { score: 10 }, level: 'A' },
        { userName: 'Charlie', totalGameInfo: { score: 15 }, level: 'C' }
      ]);
    });

    it('should return empty array if no match found', () => {
      const filteredData = filterData(dataSource, 'David');
      expect(filteredData).toEqual([]);
    });
  });

  describe('buildOrderSelection', () => {
    const sortState = {
      columnKeySelected: 'score',
      orderType: asc,
      propertySelected: 'totalGameInfo'
    };
    const props = { property: 'totalGameInfo', columnKey: 'score' };

    it('should toggle orderType if property and columnKey match', () => {
      const orderType = buildOrderSelection(sortState, props);
      expect(orderType).toBe(desc);
    });

    it('should return desc if property and columnKey do not match', () => {
      const newProps = { property: 'totalGameInfo', columnKey: 'level' };
      const orderType = buildOrderSelection(sortState, newProps);
      expect(orderType).toBe(desc);
    });
  });

  describe('buildSectionInString', () => {
    const sections = [
      { value: 'section1' },
      { value: 'section2' },
      { value: 'section3' }
    ];

    it('should build a comma-separated string from section values', () => {
      const sectionString = buildSectionInString(sections);
      expect(sectionString).toBe('section1,section2,section3');
    });

    it('should return an empty string if sections array is empty', () => {
      const sectionString = buildSectionInString([]);
      expect(sectionString).toBe('');
    });
  });
});
