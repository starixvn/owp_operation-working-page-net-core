import createColumn from './createColumn';

describe('createColumn under test', () => {
  it('createColumn should render Column ', () => {
    const columnModel = {
      order: 1,
      headerName: 'League/Match Name',
      columnKey: 'LeagueName',
      width: 550,
      isCanSort: true,
      sortState: {}
    };

    const props = {
      dataSource: [],
      columnModel
    }

    const result = createColumn(props);
    expect(result.key).toEqual(columnModel.headerName);
  });
});
