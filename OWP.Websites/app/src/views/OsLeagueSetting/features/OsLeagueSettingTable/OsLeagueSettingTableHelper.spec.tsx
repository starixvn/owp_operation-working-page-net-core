import { selectTable, calculateRowHeight } from './OsLeagueSettingTableHelper';

describe('selectTable under unit test', () => {
  it('create correct columnSchema', () => {
    const columnSchema = [
      {
        headerName: "#",
        columnKey: "No",
        width: 17.6,
      },
      {
        headerName: "Category",
        columnKey: "Category",
        width: 52.8,
      },
      {
        headerName: "League",
        columnKey: "League",
        width: 184.79999999999998,
      },
      {
        headerName: "MaxBet",
        columnKey: "MaxBet",
        width: 70.4,
      },
      {
        headerName: "Spread",
        columnKey: "Spread",
        width: 52.8,
      },
      {
        headerName: "Round",
        columnKey: "Round",
        width: 52.8,
      },
      {
        headerName: "Summary",
        columnKey: "Summary",
        width: 413.59999999999997,
      },
      {
        headerName: "Action",
        columnKey: "Action",
        width: 35.2,
      },
    ];
    const table = selectTable(800);

    expect(table.columnSchema).toEqual(columnSchema);
  });
  it('CalculateRowHeight test', () => {
    const leagueOsWithLargeRow = {
      categoryName: 'Major',
      isActive: false,
      leagueSettingId: 1,
      maxBetHDP: 1,
      maxBetOU: 1,
      osLeagueName: 'ABC',
      roundName: 'Pre Season',
      spreadHDP: 1,
      spreadOU: 1,
      summaryInfo: [
        {
          text:
            'We then used a global variable,dispatch2, to point to this dispatch function,we can ask the Title component to perform both dispatches,From the preceding setup, after the user clicks on count, it performs the dispatch from Title and performs dispatch2 from Greeting',
          style: 'Liam'
        }
      ],
      summaryRemark: []
    };
    const result1 = calculateRowHeight(leagueOsWithLargeRow);
    const leagueOsWithSmallRow = {
      categoryName: 'Major',
      isActive: false,
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
    const result2 = calculateRowHeight(leagueOsWithSmallRow);
    expect(120).toEqual(result1);
    expect(80).toEqual(result2);
  });
});
