import { useSelector, useDispatch, Provider } from 'react-redux';
import configureStore from 'redux-mock-store';
import PropTypes from 'prop-types';
import { render } from '@testing-library/react';
import CustomTable from './CustomTable';
import Spinner from '../../../components/Spinner';
import { noop } from 'lodash';
import { calculateCustomTableWidth } from './Helper';

const middleWares = [];
const mockStore = configureStore(middleWares);

const initialState = {};
const store = mockStore(initialState);

const Wrapper = ({ children }) => <Provider store={store}>{children}</Provider>;

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
jest.mock('../../../components/Spinner');
Spinner.show.mockImplementation(() => noop());
Spinner.hide.mockImplementation(() => noop());

const mockedDispatch = jest.fn();

useDispatch.mockReturnValue(mockedDispatch);
const setUp = () => {
  useSelector.mockImplementation(selectorFn =>
    selectorFn({
      eventChecking: {}
    })
  );
};
const bonusWidth = columnSchema => calculateCustomTableWidth(columnSchema, 16);

const config = {
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
  ],
  tableStyle: {
    rowHeight: 100,
    width: 1000,
    defaultHeight: 700,
    height: 750,
    maxHeight: 750,
    headerHeight: 38
  },
  tableWidth: bonusWidth
};
const { ResizeObserver } = window;

describe('CustomTable under test', () => {
  beforeEach(() => {
    //@ts-ignore
    delete window.ResizeObserver;
    window.ResizeObserver = jest.fn().mockImplementation(() => ({
      observe: jest.fn(),
      unobserve: jest.fn(),
      disconnect: jest.fn()
    }));
  });
  afterEach(() => {
    window.ResizeObserver = ResizeObserver;
    jest.restoreAllMocks();
  });

  it('should render table, not loading', () => {
    setUp();
    const props = {
      entities: [],
      sortState: {
        columnKey: 'league'
      },
      loading: 'idle'
    };
    const { container } = render(<CustomTable model={props} config={config} />);
    const table = container.querySelector('.table');
    const loading = container.querySelector('.loading-process');
    expect(table).toBeInTheDocument();
    expect(loading).not.toBeInTheDocument();
  });
  it('status is pending, should render Loading', () => {
    setUp();
    const props = {
      entities: [],
      sortState: {
        columnKey: 'league'
      },
      loading: 'pending'
    };
    const { container } = render(<CustomTable model={props} config={config} />);
    const loading = container.querySelector('.loading-process');
    expect(loading).toBeInTheDocument();
  });
});

const propTypes = {
  children: PropTypes.node
};

Wrapper.propTypes = propTypes;
