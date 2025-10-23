import { selectTable, calculateRowHeight } from './OsLeagueSettingTableHelper';
import { DefaultTableStyle } from './TableSchema.json';
import { dynamicCell } from './ConfigNative';

describe('selectTable', () => {
  it('should create correct columnSchema with correct header names and widths', () => {
    const windowWidth = 880;
    const table = selectTable(windowWidth);

    const expectedColumnSchema = [
      {
        headerName: '#',
        columnKey: 'No',
        width: windowWidth * 0.02
      },
      {
        headerName: 'Category',
        columnKey: 'Category',
        width: windowWidth * 0.06
      },
      {
        headerName: 'League',
        columnKey: 'League',
        width: windowWidth * 0.21
      },
      {
        headerName: 'MaxBet HDP',
        columnKey: 'MaxBetHDP',
        width: windowWidth * 0.05
      },
      {
        headerName: 'MaxBet OU',
        columnKey: 'MaxBetOU',
        width: windowWidth * 0.05
      },
      {
        headerName: 'Spread HDP',
        columnKey: 'SpreadHDP',
        width: windowWidth * 0.05
      },
      {
        headerName: 'Spread OU',
        columnKey: 'SpreadOU',
        width: windowWidth * 0.05
      },
      {
        headerName: 'Round',
        columnKey: 'Round',
        width: windowWidth * 0.06
      },
      {
        headerName: 'Summary',
        columnKey: 'Summary',
        width: windowWidth * 0.41
      },
      {
        headerName: 'Action',
        columnKey: 'Action',
        width: windowWidth * 0.04
      }
    ];

    expect(table.columnSchema).toEqual(expectedColumnSchema);
  });

  it('should use minWidth when windowWidth is less than minWidth', () => {
    const windowWidth = 500; // Less than minWidth (880)
    const minWidth = DefaultTableStyle.minWidth;
    const table = selectTable(windowWidth);

    const expectedColumnSchema = [
      {
        headerName: '#',
        columnKey: 'No',
        width: minWidth * 0.02
      },
      {
        headerName: 'Category',
        columnKey: 'Category',
        width: minWidth * 0.06
      },
      {
        headerName: 'League',
        columnKey: 'League',
        width: minWidth * 0.21
      },
      {
        headerName: 'MaxBet HDP',
        columnKey: 'MaxBetHDP',
        width: minWidth * 0.05
      },
      {
        headerName: 'MaxBet OU',
        columnKey: 'MaxBetOU',
        width: minWidth * 0.05
      },
      {
        headerName: 'Spread HDP',
        columnKey: 'SpreadHDP',
        width: minWidth * 0.05
      },
      {
        headerName: 'Spread OU',
        columnKey: 'SpreadOU',
        width: minWidth * 0.05
      },
      {
        headerName: 'Round',
        columnKey: 'Round',
        width: minWidth * 0.06
      },
      {
        headerName: 'Summary',
        columnKey: 'Summary',
        width: minWidth * 0.41
      },
      {
        headerName: 'Action',
        columnKey: 'Action',
        width: minWidth * 0.04
      }
    ];

    expect(table.columnSchema).toEqual(expectedColumnSchema);
  });

  it('should return tableStyle from DefaultTableStyle', () => {
    const table = selectTable(880);
    expect(table.tableStyle).toEqual(DefaultTableStyle);
  });

  it('should return dynamicCell function', () => {
    const table = selectTable(880);
    expect(table.dynamicCell).toBe(dynamicCell);
  });

  it('should return sortFunc as undefined', () => {
    const table = selectTable(880);
    expect(table.sortFunc).toBeUndefined();
  });

  it('should return tableWidth function', () => {
    const table = selectTable(880);
    expect(typeof table.tableWidth).toBe('function');
  });

  it('should calculate table width correctly using tableWidth function', () => {
    const table = selectTable(880);
    const mockColumnSchema: any = [
      { headerName: 'Test1', columnKey: 'test1', width: 100 },
      { headerName: 'Test2', columnKey: 'test2', width: 200 }
    ];
    const result = table.tableWidth(mockColumnSchema);
    expect(typeof result).toBe('number');
    expect(result).toBeGreaterThan(0);
  });
});

describe('calculateRowHeight', () => {
  it('should return rowHeightPlus when summaryInfo text length exceeds MaximumLengthOfText', () => {
    const leagueOsWithLongInfo = {
      categoryName: 'Major',
      isActive: false,
      isActiveMaxBet: true,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'ABC',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [
        {
          text: 'We then used a global variable,dispatch2, to point to this dispatch function,we can ask the Title component to perform both dispatches,From the preceding setup, after the user clicks on count, it performs the dispatch from Title and performs dispatch2 from Greeting',
          style: 'Liam'
        }
      ],
      summaryRemark: []
    };

    const result = calculateRowHeight(leagueOsWithLongInfo);
    expect(result).toEqual(120);
  });

  it('should return rowHeightPlus when summaryRemark text length exceeds MaximumLengthOfText', () => {
    const leagueOsWithLongRemark = {
      categoryName: 'Major',
      isActive: false,
      isActiveMaxBet: true,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'ABC',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [],
      summaryRemark: [
        {
          text: 'This is a very long remark text that exceeds the maximum length of text allowed in the table. It should trigger the rowHeightPlus to be returned instead of the normal rowHeight value.',
          style: 'Default'
        }
      ]
    };

    const result = calculateRowHeight(leagueOsWithLongRemark);
    expect(result).toEqual(120);
  });

  it('should return rowHeightPlus when both summaryInfo and summaryRemark have long text', () => {
    const leagueOsWithBothLong = {
      categoryName: 'Major',
      isActive: false,
      isActiveMaxBet: true,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'ABC',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [
        {
          text: 'Very long summary info text that exceeds the maximum length allowed by the table configuration. This text is intentionally made very long to test the height calculation properly and ensure it works correctly.',
          style: 'Bold'
        }
      ],
      summaryRemark: [
        {
          text: 'Very long summary remark text that exceeds the maximum length allowed by the table configuration. This text is intentionally made very long to test the height calculation properly and ensure it works correctly.',
          style: 'Italic'
        }
      ]
    };

    const result = calculateRowHeight(leagueOsWithBothLong);
    expect(result).toEqual(120);
  });

  it('should return rowHeight when summaryInfo and summaryRemark text length is within MaximumLengthOfText', () => {
    const leagueOsWithShortText = {
      categoryName: 'Major',
      isActive: false,
      isActiveMaxBet: true,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'ABC',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [{ text: 'Short text', style: 'Normal' }],
      summaryRemark: [{ text: 'Short remark', style: 'Normal' }]
    };

    const result = calculateRowHeight(leagueOsWithShortText);
    expect(result).toEqual(80);
  });

  it('should return rowHeight when both summaryInfo and summaryRemark are empty arrays', () => {
    const leagueOsWithEmptyArrays = {
      categoryName: 'Major',
      isActive: false,
      isActiveMaxBet: true,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'ABC',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [],
      summaryRemark: []
    };

    const result = calculateRowHeight(leagueOsWithEmptyArrays);
    expect(result).toEqual(80);
  });

  it('should return 0 when leagueOs is null', () => {
    const result = calculateRowHeight(null as any);
    expect(result).toEqual(0);
  });

  it('should return 0 when leagueOs is undefined', () => {
    const result = calculateRowHeight(undefined as any);
    expect(result).toEqual(0);
  });

  it('should return rowHeight when text length is exactly at MaximumLengthOfText boundary', () => {
    const exactText = 'a'.repeat(150); // Exactly 150 characters
    const leagueOsAtBoundary = {
      categoryName: 'Major',
      isActive: false,
      isActiveMaxBet: true,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'ABC',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [{ text: exactText, style: 'Normal' }],
      summaryRemark: []
    };

    const result = calculateRowHeight(leagueOsAtBoundary);
    expect(result).toEqual(80);
  });

  it('should return rowHeightPlus when text length is just over MaximumLengthOfText boundary', () => {
    const overBoundaryText = 'a'.repeat(151); // 151 characters
    const leagueOsOverBoundary = {
      categoryName: 'Major',
      isActive: false,
      isActiveMaxBet: true,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'ABC',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [{ text: overBoundaryText, style: 'Normal' }],
      summaryRemark: []
    };

    const result = calculateRowHeight(leagueOsOverBoundary);
    expect(result).toEqual(120);
  });

  it('should correctly join multiple summaryInfo texts when calculating length', () => {
    const leagueOsWithMultipleInfo = {
      categoryName: 'Major',
      isActive: false,
      isActiveMaxBet: true,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'ABC',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [
        { text: 'a'.repeat(80), style: 'Bold' },
        { text: 'b'.repeat(80), style: 'Normal' }
      ],
      summaryRemark: []
    };

    const result = calculateRowHeight(leagueOsWithMultipleInfo);
    // Total length is 160 + 1 space = 161, which exceeds 150
    expect(result).toEqual(120);
  });

  it('should correctly join multiple summaryRemark texts when calculating length', () => {
    const leagueOsWithMultipleRemark = {
      categoryName: 'Major',
      isActive: false,
      isActiveMaxBet: true,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'ABC',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [],
      summaryRemark: [
        { text: 'a'.repeat(80), style: 'Bold' },
        { text: 'b'.repeat(80), style: 'Normal' }
      ]
    };

    const result = calculateRowHeight(leagueOsWithMultipleRemark);
    // Total length is 160 + 1 space = 161, which exceeds 150
    expect(result).toEqual(120);
  });
});
