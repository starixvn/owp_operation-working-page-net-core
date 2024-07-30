import DynamicCell from './DynamicCell';
import { useDispatch, useSelector } from 'react-redux';

jest.mock('../../../app/store');
jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('react', () => ({
  ...jest.requireActual('react'),
  useState: jest.fn(),
  createRef: jest.fn()
}));

const mockDispatch = jest.fn();

const setup = () => {
  const state = {
    copy: {
      isCopying: false,
      copiedValue: {
        HDP: '',
        OU: ''
      },
      highlightedCell: {
        PreMatchLeagueId: 1,
        rowId: 1,
        isTakeOver: false
      }
    }
  };
  useDispatch.mockReturnValue(mockDispatch);
  useSelector.mockImplementation(selectorFn => selectorFn({ ...state }));
};
const createProps = columnKey => {
  const dataSource = [{}];
  const rowIndex = 0;
  return { dataSource, rowIndex, columnKey, isEditing: true };
};
describe('DynamicCell under test', () => {
  afterEach(() => {
    jest.clearAllMocks();
  });
  it('When columnKey: `No`, should render NoCell', () => {
    setup();
    const props = createProps('No');
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('NoCell');
  });
  it('When columnKey: `LeagueName`, should render LeagueCellFactory', () => {
    setup();
    const props = createProps('LeagueName');
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('LeagueCellFactory');
  });
  it('When columnKey: `Message`, should render IssuesCell', () => {
    setup();
    const props = createProps('Message');
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('IssuesCell');
  });

  it('When columnKey: `Blacklist`, should render BlacklistCell', () => {
    setup();
    const props = createProps('Blacklist');
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('BlacklistCell');
  });
  it('When columnKey: `HDP`, should render AssignmentCellFactory', () => {
    setup();
    const props = createProps('HDP');
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('AssignmentCellFactory');
  });
  it('When columnKey: `Remark`, should render RemarkCellFacory', () => {
    setup();
    const props = createProps('Remark');
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('RemarkCellFactory');
  });
  it('When columnKey: `Action`, should render ActionCell', () => {
    setup();
    const props = createProps('Action');
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('ActionCell');
  });
});
