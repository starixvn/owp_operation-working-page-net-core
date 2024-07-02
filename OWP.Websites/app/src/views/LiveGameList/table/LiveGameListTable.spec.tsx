import LiveGameListTable from './LiveGameListTable';
import { useSelector, useDispatch } from 'react-redux';
import { fakeEntities } from './tableSlice/tableSliceConst';
import { render, screen, waitFor } from '@testing-library/react';
import { KOTime } from './TableSchema.json';

jest.mock('react-redux', () => ({
  useSelector: jest.fn(),
  useDispatch: jest.fn()
}));
const mockedDispatch = jest.fn();

(useSelector as jest.Mock).mockImplementation(selectorFn =>
  selectorFn({
    events: {
      entities: fakeEntities,
      filterState: [],
      sortState: {
        columnKey: KOTime.columnKey,
        orderType: '',
      },
    },
    pagePolicy: {
      permission: {
        HasWrite: true
      }
    }
  })
);
(useDispatch as jest.Mock).mockReturnValue(mockedDispatch);

const { ResizeObserver } = window;

describe('test LiveGameListTable', () => {
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

  it('should render table content, ReactTable', async () => {
    const { container } = render(<LiveGameListTable />);
    await waitFor(() => {
      expect(container.querySelector('.table-content')).toBeInTheDocument;
    });
  });
});
