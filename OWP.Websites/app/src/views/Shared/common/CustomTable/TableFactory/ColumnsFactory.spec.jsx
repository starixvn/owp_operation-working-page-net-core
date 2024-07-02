import ColumnsFactory from './ColumnsFactory';

describe('ColumnFactory under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('should ', () => {
    const props = {
      sortState: {
        columnKey: 'LeagueName'
      },
      columnSchema: [
        {
          order: 1,
          headerName: 'League/Match Name',
          columnKey: 'LeagueName',
          width: 550,
          isCanSort: true,
          sortState: {}
        },
        {
          headerName: 'Comment',
          columnKey: 'IsRead',
          width: 120,
          isCanSort: true,
          sortState: {}
        },
        {
          order: 2,
          headerName: 'Last Comment',
          columnKey: 'CreatedDate',
          width: 650,
          isCanSort: true,
          sortState: {}
        },
        {
          headerName: 'Status',
          columnKey: 'IsDone',
          width: 120,
          sortState: {}
        }
      ]
    };
    const columns = ColumnsFactory(props);
    const leagueNameColumn = columns.filter(e => e.key === 'League/Match Name')
      .length;
    const issuesColumn = columns.filter(e => e.key === 'Comment').length;
    const lastCmtColumn = columns.filter(e => e.key === 'Last Comment').length;
    const isDoneColumn = columns.filter(e => e.key === 'Status').length;
    expect(leagueNameColumn).toEqual(1);
    expect(issuesColumn).toEqual(1);
    expect(lastCmtColumn).toEqual(1);
    expect(isDoneColumn).toEqual(1);
  });
});
