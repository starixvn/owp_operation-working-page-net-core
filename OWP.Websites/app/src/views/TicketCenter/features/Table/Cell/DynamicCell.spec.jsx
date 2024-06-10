import DynamicCell from './DynamicCell';
import { useDispatch, useSelector } from 'react-redux';
import { ColumnModels } from '../ConfigNative';
const {
  LeagueColumn,
  LastCommentColumn,
  TicketIdColumn,
  TicketTypeColumn,
  StatusColumn,
  ActionColumn
} = ColumnModels;

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
  it('When columnKey: `LeagueColumn.columnKey`, should render LeagueCellFactory component', () => {
    setup();
    const props = createProps(LeagueColumn.columnKey);
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('ConversationCell');
  });
  it('When columnKey: `LastCommentColumn.columnKey`, should render LastCommentCell component', () => {
    setup();
    const props = createProps(LastCommentColumn.columnKey);
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('LastCommentCell');
  });
  it('When columnKey: `TicketIdColumn.columnKey`, should render TicketIdCell component', () => {
    setup();
    const props = createProps(TicketIdColumn.columnKey);
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('TicketIdCell');
  });
  it('When columnKey: `TicketTypeColumn.columnKey`, should render TicketTypeCell component', () => {
    setup();
    const props = createProps(TicketTypeColumn.columnKey);
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('TicketTypeCell');
  });
  it('When columnKey: `TicketTypeColumn.columnKey`, should render TicketTypeCell component', () => {
    setup();
    const props = createProps(TicketTypeColumn.columnKey);
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('TicketTypeCell');
  });
  it('When columnKey: `StatusColumn.columnKey`, should render TicketTypeCell component', () => {
    setup();
    const props = createProps(StatusColumn.columnKey);
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('StatusCell');
  });
  it('When columnKey: `ActionColumn.columnKey`, should render TicketTypeCell component', () => {
    setup();
    const props = createProps(ActionColumn.columnKey);
    const result = DynamicCell(0, props);
    expect(result.type.name).toEqual('ActionCell');
  });
  it('When columnKey not in schemaConfig, should not render component', () => {
    setup();
    const props = createProps('test');
    const result = DynamicCell(0, props);
    expect(result).toEqual(null);
  });
});
