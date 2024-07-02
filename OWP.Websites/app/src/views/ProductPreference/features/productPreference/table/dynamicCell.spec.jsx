import dynamicCell from './dynamicCell';

const defaultProps = {
  rowIndex: 0,
  cellModel: {
    dataSource: [{
      assignmentRemark: 'carlos testxzxzxzvsdvsdvsdê2e2e2e2eevsdvsdvsdvsvsdvdsvsdvdvsdvsdvvsdvxztrttrtrtrhfgggehrthbthrththttrhrthrthrtrthp',
      betTypePreferencesId: 0,
      levelId: 1,
      multipleGamesId: 2,
      roleId: 2,
      userId: 2439,
      username: 'CLGCHAI'
    }],
    columnKey: 'role'
  }
};

describe('dynamicCell under test', () => {
  it('match snapshot', () => {
    const { rowIndex, cellModel } = defaultProps;
    const component = dynamicCell(rowIndex, cellModel);
    expect(component).toMatchSnapshot();
  });
});
